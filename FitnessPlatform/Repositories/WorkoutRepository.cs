using FitnessPlatform.Context;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly FitnessDbContext _context;

        public WorkoutRepository(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<List<Workout>> GetAllWorkouts()
        {
            return await _context.Workouts.ToListAsync();
        }

        public async Task<Workout> GetWorkoutById(string id)
        {
            int parsedId = int.Parse(id);
            return await _context.Workouts.FirstOrDefaultAsync(w => w.WorkoutId == parsedId);
        }

        public async Task<Workout> GetWorkoutByType(string type)
        {
            return await _context.Workouts.FirstOrDefaultAsync(w => w.WorkoutType == type);
        }

        public async Task InsertWorkout(Workout workout)
        {
            await _context.Workouts.AddAsync(workout);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkout(string id)
        {
            int parsedId = int.Parse(id);
            var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.WorkoutId == parsedId);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateWorkout(Workout workout, string id)
        {
            int parsedId = int.Parse(id);
            var existingWorkout = await _context.Workouts.FirstOrDefaultAsync(w => w.WorkoutId == parsedId);

            if (existingWorkout != null)
            {
                // Update fields
                existingWorkout.WorkoutDescription = workout.WorkoutDescription;
                existingWorkout.WorkoutType = workout.WorkoutType;
                existingWorkout.DifficultyLevel = workout.DifficultyLevel;
                existingWorkout.WorkoutDuration = workout.WorkoutDuration;
                existingWorkout.CaloriesBurned = workout.CaloriesBurned;
                existingWorkout.ContentPath = workout.ContentPath;

                // Save changes
                await _context.SaveChangesAsync();
            }
        }
    }
}
