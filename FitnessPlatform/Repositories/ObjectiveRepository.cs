using FitnessPlatform.Context;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Repositories
{
    public class ObjectiveRepository : IObjectiveRepository
    {
        private readonly FitnessDbContext _context;

        public ObjectiveRepository(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<List<Objective>> GetAllObjectives()
        {
            return await _context.Objectives
                .Include(o => o.UserIdNavigation) // Eager loading for related User
                .ToListAsync(); ;
        }

        public async Task<Objective> GetObjectiveById(string id)
        {
            int parsedId = int.Parse(id);
            return await _context.Objectives
                .Include(o => o.UserIdNavigation)
                .FirstOrDefaultAsync(o => o.ObjectiveId == parsedId);
        }

        public async Task<Objective> GetObjectiveByType(string type)
        {
            return await _context.Objectives
                .Include(o => o.UserIdNavigation)
                .FirstOrDefaultAsync(o => o.ObjectiveType == type);
        }

        public async Task InsertObjective(Objective objective)
        {
            await _context.Objectives.AddAsync(objective);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteObjective(string id)
        {
            int parsedId = int.Parse(id);
            var objective = await _context.Objectives.FirstOrDefaultAsync(o => o.ObjectiveId == parsedId);
            if (objective != null)
            {
                _context.Objectives.Remove(objective);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateObjective(Objective objective, string id)
        {
            int parsedId = int.Parse(id);
            var existingObjective = await _context.Objectives.FirstOrDefaultAsync(o => o.ObjectiveId == parsedId);

            if (existingObjective != null)
            {
                // Update fields
                existingObjective.UserId = objective.UserId;
                existingObjective.ObjectiveType = objective.ObjectiveType;
                existingObjective.TargetValue = objective.TargetValue;
                existingObjective.Progress = objective.Progress;
                existingObjective.StartDate = objective.StartDate;
                existingObjective.Deadline = objective.Deadline;

                // Save changes
                await _context.SaveChangesAsync();
            }
        }
    }
}
