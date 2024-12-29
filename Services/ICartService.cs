using System.Collections.Generic;
using System.Threading.Tasks;
using test.DTOs;

namespace test.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartDto>> GetAllCartsAsync();
        Task<CartDto> GetCartByIdAsync(int id);
        Task<CartDto> CreateCartAsync(CreateCartDto createCartDto, string accessToken);

        Task<bool> DeleteCartAsync(int id);
        Task ClearCartAsync();
        Task<CartResponseDto> PatchCartAsync(int id, PatchCartDto patchCartDto);


    }
}
