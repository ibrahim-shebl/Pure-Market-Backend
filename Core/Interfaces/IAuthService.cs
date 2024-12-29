using System.Security.Claims;
using test.Core.Dtos;

namespace test.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> SeedRolesAsync();
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto);
        Task<AuthServiceResponseDto> GetUserInfoAsync(ClaimsPrincipal userClaims);
        Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
        Task<bool> DeleteUserByIdAsync(string id);
        Task<AuthServiceResponseDto> LogoutAsync(string token);
    }

}