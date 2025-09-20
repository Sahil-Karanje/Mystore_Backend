using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using System.Security.Claims;
using server.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context) { 
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartList() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartList = await _context.carts
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return Ok(cartList);
        }

        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exists = await _context.carts
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (exists != null)
                return BadRequest("Product already in cart.");

            var cartlistItem = new Cart
            {
                ProductId = productId,
                UserId = userId
            };

            _context.carts.Add(cartlistItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Added to cart" });
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItem = await _context.carts
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (cartItem == null)
                return NotFound("Product not found in wishlist.");

            _context.carts.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Removed from Cart" });
        }
    }
}
