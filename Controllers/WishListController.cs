using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models.Entities;
using System;
using System.Security.Claims;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  
    public class WishListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserWishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var wishlist = await _context.WishList
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return Ok(wishlist);
        }

        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exists = await _context.WishList
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (exists != null)
                return BadRequest("Product already in wishlist.");

            var wishlistItem = new WishList
            {
                ProductId = productId,
                UserId = userId
            };

            _context.WishList.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Added to wishlist" });
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlistItem = await _context.WishList
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (wishlistItem == null)
                return NotFound("Product not found in wishlist.");

            _context.WishList.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Removed from wishlist" });
        }
    }
}
