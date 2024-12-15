using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using test.Data;
using test.DTOs;
using test.Models;
using test.Repositories;

namespace test.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ApplicationDbContext _context;  

        public CartService(ICartRepository cartRepository, ApplicationDbContext context)
        {
            _cartRepository = cartRepository;
            _context = context;
        }

        public async Task<IEnumerable<CartDto>> GetAllCartsAsync()
        {
            var carts = await _cartRepository.GetAllCartsAsync();

            var productIds = carts.SelectMany(cart => cart.CartItems.Select(ci => ci.ProductId)).Distinct();
            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            return carts.Select(cart => new CartDto
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = products.FirstOrDefault(p => p.Id == ci.ProductId)?.ProductName ?? "Unknown Product",
                    Price = products.FirstOrDefault(p => p.Id == ci.ProductId)?.Price ?? 0,  
                    Quantity = ci.Quantity
                }).ToList()
            });
        }

        public async Task<CartDto> GetCartByIdAsync(int id)
        {
            var cart = await _cartRepository.GetCartByIdAsync(id);
            if (cart == null)
            {
                return null;
            }

            return new CartDto
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product != null ? ci.Product.ProductName : "Unknown Product",
                    Quantity = ci.Quantity
                }).ToList()
            };
        }

        public async Task<CartDto> CreateCartAsync(CreateCartDto createCartDto)
        {
            var cart = new Cart
            {
                CartItems = new List<CartItem>()
            };

            foreach (var item in createCartDto.CartItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);  
                if (product != null)
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        
                    });
                }
            }

            cart = await _cartRepository.CreateCartAsync(cart);

            return new CartDto
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown Product",
                    Quantity = ci.Quantity,
                    Price = ci.Product?.Price ?? 0  
                }).ToList()
            };
        }

        public async Task<bool> DeleteCartAsync(int id)
        {
            await _cartRepository.DeleteCartAsync(id);
            return true;
        }

        public async Task ClearCartAsync()  
        {
            var carts = await _context.Carts.ToListAsync();  
            _context.Carts.RemoveRange(carts);  
            await _context.SaveChangesAsync();  
        }
        public async Task<CartResponseDto> PatchCartAsync(int id, PatchCartDto patchCartDto)
        {
            var cart = await _cartRepository.GetCartByIdAsync(id);
            if (cart == null) return null;

            cart.CartItems.Clear(); 
            foreach (var item in patchCartDto.CartItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
            }

            await _cartRepository.UpdateCartAsync(cart);   

            return new CartResponseDto
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown Product",
                    Price = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity
                }).ToList()
            };
        }

    }
}
