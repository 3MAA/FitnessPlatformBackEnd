using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FitnessPlatform.Models;
using System.Security.Cryptography;
using System.Text;
using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Context;

namespace FitnessPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FitnessDbContext _context;

        public AuthController(FitnessDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("Email already exists.");
            }

            var passwordHash = HashPassword(model.UserPassword);

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                UserPassword = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
