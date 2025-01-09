using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Services.Abstractions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private const string PostSuccessMessage = "Successfully registered!";

        private readonly IWorkoutService _workoutService;
        private readonly ILogger<WorkoutController> _logger;

        public WorkoutController(IWorkoutService workoutService, ILogger<WorkoutController> logger)
        {
            _workoutService = workoutService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkouts()
        {
            var workouts = await _workoutService.GetAllWorkouts();
            _logger.LogInformation("Fetched {Count} workouts.", workouts.Count());
            return Ok(workouts);
        }

        [HttpGet("{id}")]
        public async Task<WorkoutDto> GetWorkoutById(string id)
        {
            _logger.LogInformation("Fetching workout with ID {Id}.", id);
            return await _workoutService.GetWorkoutById(id);
        }

        [HttpGet("type/{type}")]
        public async Task<WorkoutDto> GetWorkoutByType(string type)
        {
            _logger.LogInformation("Fetching workout with type {Type}.", type);
            return await _workoutService.GetWorkoutByType(type); ;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WorkoutDto workoutDto)
        {
            await _workoutService.CreateWorkout(workoutDto);
            _logger.LogInformation("Workout with ID {Id} created successfully.", workoutDto.WorkoutId);
            return CreatedAtAction(nameof(GetWorkoutById), new { id = workoutDto.WorkoutId }, PostSuccessMessage);
        }

        [HttpDelete("logical/{id}")]
        public async Task<IActionResult> DeleteLogical(string id)
        {
            var workout = await _workoutService.GetWorkoutById(id);
            if (workout == null)
            {
                _logger.LogWarning("Workout with ID {Id} not found.", id);
                return NotFound(new { Message = $"Workout with ID {id} not found." });
            }

            await _workoutService.DeleteWorkout(id);
            _logger.LogInformation("Workout with ID {Id} logically deleted.", id);
            return NoContent();
        }

        [HttpDelete("physical/{id}")]
        public async Task<IActionResult> DeletePhysical(string id)
        {
            var workout = await _workoutService.GetWorkoutById(id);
            if (workout == null)
            {
                _logger.LogWarning("Workout with ID {Id} not found.", id);
                return NotFound(new { Message = $"Workout with ID {id} not found." });
            }

            await _workoutService.DeleteWorkoutPermanently(id);
            _logger.LogInformation("Workout with ID {Id} physically deleted.", id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] WorkoutDto workoutDto, string id)
        {
            var result = await _workoutService.UpdateWorkout(workoutDto, id);
            _logger.LogInformation("Workout with ID {Id} updated successfully.", id);
            return Ok(result);
        }
    }
}
