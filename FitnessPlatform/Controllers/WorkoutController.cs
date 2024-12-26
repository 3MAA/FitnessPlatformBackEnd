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
        private const string PostSuccessMessage = "Successfully registered";

        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkouts()
        {
            return Ok(await _workoutService.GetAllWorkouts());
        }

        [HttpGet("{id}")]
        public async Task<WorkoutDto> GetWorkoutById(string id)
        {
            return await _workoutService.GetWorkoutById(id);
        }

        [HttpGet("type/{type}")]
        public async Task<WorkoutDto> GetWorkoutByType(string type)
        {
            return await _workoutService.GetWorkoutByType(type); ;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WorkoutDto workoutDto)
        {
            await _workoutService.CreateWorkout(workoutDto);
            return CreatedAtAction(nameof(GetWorkoutById), new { id = workoutDto.WorkoutId }, PostSuccessMessage);
        }

        [HttpDelete("logical/{id}")]
        public async Task<IActionResult> DeleteLogical(string id)
        {
            var workout = await _workoutService.GetWorkoutById(id);
            if (workout == null)
                return NotFound(new { Message = $"Workout with ID {id} not found." });

            await _workoutService.DeleteWorkout(id);
            return NoContent();
        }

        [HttpDelete("physical/{id}")]
        public async Task<IActionResult> DeletePhysical(string id)
        {
            var workout = await _workoutService.GetWorkoutById(id);
            if (workout == null)
                return NotFound(new { Message = $"Workout with ID {id} not found." });

            await _workoutService.DeleteWorkoutPermanently(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] WorkoutDto workoutDto, string id)
        {
            return Ok(await _workoutService.UpdateWorkout(workoutDto, id));
        }
    }
}
