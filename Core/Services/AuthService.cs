using test.Core.Dtos;
using test.Core.Entities;
using test.Core.Interfaces;
using test.Core.OtherObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using test.Data;

namespace test.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings , RoleManager<IdentityRole> roleManager, IConfiguration configuration , ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }


        public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim("JWTID", Guid.NewGuid().ToString()),
        new Claim("FirstName", user.FirstName),
        new Claim("LastName", user.LastName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = token
            };
        }

        public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByEmailAsync(updatePermissionDto.Email);

            if (user is null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Email Address"
                };
            }

            // إضافة المستخدم إلى دور ADMIN
            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

            // توليد التوكن
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, StaticUserRoles.ADMIN),
                new Claim("UserID", user.Id.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = tokenString
            };
        }
    

    public async Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.Email);

            if (user is null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid User name!!!!!!!!"
                };

            await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User is now an OWNER"
            };
        }

        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExistsUser != null)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "UserName Already Exists"
                };

            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

             
            var authClaims = new List<Claim>
    {
            new Claim(ClaimTypes.Name, newUser.UserName),
            new Claim(ClaimTypes.NameIdentifier, newUser.Id),
            new Claim("JWTID", Guid.NewGuid().ToString()),
            new Claim("FirstName", newUser.FirstName),
            new Claim("LastName", newUser.LastName),
    };

            var userRoles = await _userManager.GetRolesAsync(newUser);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = token  
            };
        }

        public async Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = true,
                    Message = "Roles Seeding is Already Done"
                };

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
          
            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Role Seeding Done Successfully"
            };
        }


        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }

        public async Task<AuthServiceResponseDto> GetUserInfoAsync(ClaimsPrincipal userClaims)
        {
            var userName = userClaims.FindFirstValue(ClaimTypes.Name);
            var email = userClaims.FindFirstValue(ClaimTypes.Email);
            var token = userClaims.FindFirst("JWTID")?.Value;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email))
            {
                throw new Exception("User is not authenticated");
            }

            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "User information retrieved successfully",
                Data = new
                {
                    UserName = userName,
                    Email = email,
                    AccessToken = token
                }
            };
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();  
            var usersInfo = users.Select(user => new UserInfoDto
            {
                Id = user.Id,  
                UserName = user.UserName,
                Email = user.Email
            }).ToList();

            return await Task.FromResult(usersInfo);
        }
        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;  
            }

            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;  
        }
        public async Task<AuthServiceResponseDto> LogoutAsync(string token)
        {
            // تحقق من أن التوكن ليس فارغًا
            if (string.IsNullOrEmpty(token))
            {
                return new AuthServiceResponseDto
                {
                    IsSucceed = false,
                    Message = "Token is required"
                };
            }

            // يمكنك حفظ التوكن في قائمة سوداء (Blacklist) لتجنب استخدامه مجددًا
            // هنا يمكنك استخدام تخزين مثل Redis أو قاعدة بيانات
            // await _cacheService.AddToBlacklistAsync(token);

            // الرد الناجح
            return new AuthServiceResponseDto
            {
                IsSucceed = true,
                Message = "Logged out successfully"
            };
        }

    }
}
