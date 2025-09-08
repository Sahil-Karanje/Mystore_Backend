using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models.Entities;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("addNewProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product data is required");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _context.Products
                .Where(p => p.Category.ToLower() == category.ToLower() && p.IsActive)
                .ToListAsync();

            if (products == null || !products.Any())
                return NotFound("No products found in this category");

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest("Keyword is required");

            var keywords = keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var query = _context.Products.AsQueryable();

            foreach (var k in keywords)
            {
                var temp = k; 
                query = query.Where(p =>
                    p.Name.Contains(temp) ||
                    p.ShortDescription.Contains(temp) ||
                    p.LongDescription.Contains(temp) ||
                    p.Brand.Contains(temp) ||
                    p.Category.Contains(temp)
                );
            }

            var products = await query.ToListAsync();

            if (!products.Any())
                return NotFound("No products found matching the keyword(s)");

            return Ok(products);
        }
    }
}
