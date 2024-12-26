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
            var objective = await _context.Objectives.FindAsync(parsedId); 

            if (objective != null)
            {
                await _context.Entry(objective).Reference(o => o.UserIdNavigation).LoadAsync();
            }

            return objective;
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
            var objective = await _context.Objectives.FindAsync(parsedId);

            if (objective != null)
            {
                objective.IsDeleted = true; 
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteObjectivePermanently(string id)
        {
            int parsedId = int.Parse(id);
            var objective = await _context.Objectives.FindAsync(parsedId);

            if (objective != null)
            {
                _context.Objectives.Remove(objective);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateObjective(Objective objective, string id)
        {
            int parsedId = int.Parse(id);
            var existingObjective = await _context.Objectives.FindAsync(parsedId);

            if (existingObjective != null)
            {
                _context.Entry(existingObjective).CurrentValues.SetValues(objective);
                await _context.SaveChangesAsync();
            }
        }
    }
}
