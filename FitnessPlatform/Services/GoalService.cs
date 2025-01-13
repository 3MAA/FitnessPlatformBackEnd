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

            // 3. Verifica numarul de discounturi acordate in acest an
            var currentYear = DateOnly.FromDateTime(DateTime.Now).Year;
            var startOfYear = new DateOnly(currentYear, 1, 1);
            var endOfYear = new DateOnly(currentYear, 12, 31);

            var discountCountThisYear = await _context.Discounts
                .Where(d => d.UserId == userId && d.GrantDate >= startOfYear && d.GrantDate <= endOfYear)
                .CountAsync();

            if (discountCountThisYear >= 2)
            {
                // Discounturile au fost deja acordate de doua ori in acest an
                return false;
            }

            // 4. Acorda discount de 10% pentru utilizator
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
