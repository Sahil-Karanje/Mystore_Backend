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
    public class YourOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public YourOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrderlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderlist = await _context.YourOrders
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return Ok(orderlist);
        }

        [HttpPost("add/{productId}")]
        public async Task<IActionResult> AddToOrderlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exists = await _context.YourOrders
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (exists != null)
                return BadRequest("Product already in orderList.");

            var orderlistItem = new YourOrders
            {
                ProductId = productId,
                UserId = userId
            };

            _context.YourOrders.Add(orderlistItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Added to orderList" });
        }
    }
}
