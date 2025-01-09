using FitnessPlatform.Context;
using FitnessPlatform.Models;

using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Services
{
    public class GoalService
    {
        private readonly FitnessDbContext _context;

        public GoalService(FitnessDbContext context)
        { 
            _context = context;
        }

        public async Task<bool> CheckAndGrantDiscountAsync(int userId, int objectiveId)
        {
            // 1. Verifica obiectivul
            var objective = await _context.Objectives
                .Where(o => o.UserId == userId && o.ObjectiveId == objectiveId)
                .FirstOrDefaultAsync();

            if (objective == null || objective.Progress < objective.TargetValue ||
                objective.Deadline.HasValue && objective.Deadline.Value < DateOnly.FromDateTime(DateTime.Now))
            {
                // Obiectivul nu este complet sau a depasit data limita
                return false;
            }

            // 2. Numara antrenamentele efectuate de utilizator
            var workoutCount = await _context.UserWorkouts
                .Where(uw => uw.UserId == userId && uw.CompletionDate >= objective.StartDate && uw.CompletionDate <= objective.Deadline)
                .CountAsync();

            if (workoutCount < 5)
            {
                // Nu sunt suficiente antrenamente
                return false;
            }

            // 3. Acorda discount de 10% pentru utilizator
            var discount = new Discount
            {
                UserId = userId,
                DiscountPercent = 10,
                GrantDate = DateOnly.FromDateTime(DateTime.Now),
                ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)) // Discountul expira intr-o lună
            };

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
