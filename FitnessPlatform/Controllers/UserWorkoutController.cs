using FitnessPlatform.Context;
using FitnessPlatform.Models;
using FitnessPlatform.Models.DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWorkoutController : ControllerBase
    {
        private readonly FitnessDbContext _context;

        public UserWorkoutController(FitnessDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-workout/{userWorkoutId}")]
        public async Task<IActionResult> GetUserWorkout(int userWorkoutId)
        {
            var userWorkout = await _context.UserWorkouts
                .Include(uw => uw.Workout)  // Include detalii despre workout
                .FirstOrDefaultAsync(uw => uw.UserWorkoutId == userWorkoutId);

            if (userWorkout == null)
            {
                return NotFound("User workout not found.");
            }

            return Ok(new
            {
                userWorkout.UserId,
                userWorkout.CompletionDate,
                workoutDescription = userWorkout.Workout.WorkoutDescription  // Acceseza descrierea workout-ului
            });
        }


        [HttpPost("save-workout")]
        public async Task<IActionResult> SaveWorkout([FromBody] UserWorkoutDto workoutDto)
        {
            try
            {
                var userWorkout = new UserWorkout
                {
                    UserId = workoutDto.UserId,
                    WorkoutId = workoutDto.WorkoutId,
                    CompletionDate = workoutDto.CompletionDate
                };

                _context.UserWorkouts.Add(userWorkout);
                await _context.SaveChangesAsync();

                return Ok("Workout saved successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving workout: {ex.Message}");
            }
        }
    }
}
