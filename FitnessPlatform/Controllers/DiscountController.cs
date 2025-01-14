using FitnessPlatform.Context;
using FitnessPlatform.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly FitnessDbContext _context;

        public DiscountController(FitnessDbContext context)
        {
            _context = context;
        }

        // GET: api/discount
        [HttpGet]
        public async Task<IActionResult> GetDiscounts()
        {
            var discounts = await _context.Discounts.ToListAsync(); 
            return Ok(discounts);
        }

        // GET: api/discount/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscount(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            return Ok(discount);
        }

        // POST: api/discount
        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] Discount discount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDiscount), new { id = discount.DiscountId }, discount);
        }

        // DELETE: api/discount/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
