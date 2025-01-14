using FitnessPlatform.Context;
using FitnessPlatform.Models;
using FitnessPlatform.Models.DTOs;

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

        // Creare obiectiv nou
        public async Task<Objective> CreateObjectiveAsync(Objective objective)
        {
            await _context.Objectives.AddAsync(objective);
            await _context.SaveChangesAsync();
            return objective;
        }

        // Actualizare obiectiv
        public async Task<bool> UpdateObjectiveAsync(ObjectiveDto objectiveDto)
        {
            var existingObjective = await _context.Objectives
                .FirstOrDefaultAsync(o => o.ObjectiveId == objectiveDto.ObjectiveId);

            if (existingObjective == null)
                return false;

            existingObjective.ObjectiveType = objectiveDto.ObjectiveType;
            existingObjective.TargetValue = objectiveDto.TargetValue;
            existingObjective.Progress = objectiveDto.Progress;
            existingObjective.StartDate = objectiveDto.StartDate;
            existingObjective.Deadline = objectiveDto.Deadline;
            existingObjective.IsDraft = objectiveDto.IsDraft; // Se actualizeaza statusul draft

            _context.Objectives.Update(existingObjective);
            await _context.SaveChangesAsync();
            return true;
        }

        // Obtine obiectivele unui utilizator
        public async Task<List<Objective>> GetUserObjectivesAsync(int userId)
        {
            return await _context.Objectives
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        // Sterge obiectiv
        public async Task<bool> DeleteObjectiveAsync(int objectiveId)
        {
            var objective = await _context.Objectives.FirstOrDefaultAsync(o => o.ObjectiveId == objectiveId);
            if (objective == null)
                return false;

            _context.Objectives.Remove(objective);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckAndGrantDiscountAsync(int userId, int objectiveId)
        {
            // 1. Verifica obiectivul
            var objective = await _context.Objectives
                .Where(o => o.UserId == userId && o.ObjectiveId == objectiveId)
                .FirstOrDefaultAsync();

            if (objective == null || objective.Progress < objective.TargetValue ||
    (objective.Deadline.HasValue && objective.Deadline.Value < DateOnly.FromDateTime(DateTime.Now)))
            {
                // Obiectivul nu este complet sau a depasit data limita
                return false;
            }

            // 2. Verifica numarul de discounturi acordate in acest an
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

            // 3. Acorda discount de 10% pentru utilizator
            var discount = new Discount
            {
                UserId = userId,
                DiscountPercent = 10,
                GrantDate = DateOnly.FromDateTime(DateTime.Now),
                ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)) // Discountul expira intr-o luna
            };

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
