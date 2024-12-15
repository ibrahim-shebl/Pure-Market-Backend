using Microsoft.AspNetCore.Mvc;
using test.DTOs;
using test.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCartsAsync()
        {
            var carts = await _cartService.GetAllCartsAsync();
            return Ok(carts);
        }

        [HttpGet("{id}", Name = "GetCartById")]
        public async Task<ActionResult<CartDto>> GetCartAsync(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> CreateCartAsync(CreateCartDto createCartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cart = await _cartService.CreateCartAsync(createCartDto);
            if (cart == null || cart.Id == 0)
            {
                return BadRequest("Unable to create cart.");
            }

            return CreatedAtRoute("GetCartById", new { id = cart.Id }, cart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartAsync(int id)
        {
            var result = await _cartService.DeleteCartAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("clear")]  
        public async Task<IActionResult> ClearCartAsync()
        {
            await _cartService.ClearCartAsync();
            return NoContent();  
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<CartResponseDto>> PatchCartAsync(int id, PatchCartDto patchCartDto)
        {
            var updatedCart = await _cartService.PatchCartAsync(id, patchCartDto);
            if (updatedCart == null)
            {
                return NotFound();
            }

            return Ok(updatedCart);
        }

    }
}
