using FitnessPlatform.Context;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FitnessDbContext _context;

        public UserRepository(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            //return await _context.Users.ToListAsync();
            return await _context.Users
                .Include(u => u.Objectives) // Eager loading for related Objectives
                .Include(u => u.Subscriptions) // Eager loading for related Subscriptions
                .Include(u => u.Discounts) // Eager loading for related Discounts
                .ToListAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            int parsedId = int.Parse(id);
            return await _context.Users.FindAsync(parsedId);
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _context.Users
                .Include(u => u.Objectives)
                .Include(u => u.Subscriptions)
                .Include(u => u.Discounts)
                .FirstOrDefaultAsync(u => u.UserName == name);
        }

        public async Task InsertUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(string id)
        {
            int parsedId = int.Parse(id);
            var user = await _context.Users.FindAsync(parsedId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(User user, string id)
        {
            int parsedId = int.Parse(id);
            var existingUser = await _context.Users.FindAsync(parsedId);

            if (existingUser != null)
            {
                // Update fields
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.UserPassword = user.UserPassword;
                existingUser.Gender = user.Gender;
                existingUser.Age = user.Age;
                existingUser.UserWeight = user.UserWeight;
                existingUser.UserHeight = user.UserHeight;

                // Save changes
                await _context.SaveChangesAsync();
            }
        }
    }
}
