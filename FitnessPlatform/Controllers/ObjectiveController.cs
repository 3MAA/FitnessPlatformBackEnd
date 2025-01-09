using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Services.Abstractions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectiveController : ControllerBase
    {
        private const string PostSuccessMessage = "Successfully registered!";

        private readonly IObjectiveService _objectiveService;
        private readonly ILogger<ObjectiveController> _logger;

        public ObjectiveController(IObjectiveService objectiveService, ILogger<ObjectiveController> logger)
        {
            _objectiveService = objectiveService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllObjectives()
        {
            var objectives = await _objectiveService.GetAllObjectives();
            _logger.LogInformation("Fetched {Count} objectives.", objectives.Count());
            return Ok(objectives);
        }

        [HttpGet("{id}")]
        public async Task<ObjectiveDto> GetObjectiveById(string id)
        {
            _logger.LogInformation("Fetching objective with ID {Id}.", id);
            return await _objectiveService.GetObjectiveById(id);
        }

        [HttpGet("type/{type}")]
        public async Task<ObjectiveDto> GetObjectiveByType(string type)
        {
            _logger.LogInformation("Fetching objective with type {Type}.", type);
            return await _objectiveService.GetObjectiveByType(type);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ObjectiveDto objectiveDto)
        {
            await _objectiveService.CreateObjective(objectiveDto);
            _logger.LogInformation("Objective with ID {Id} created successfully.", objectiveDto.ObjectiveId);
            return CreatedAtAction(nameof(GetObjectiveById), new { id = objectiveDto.ObjectiveId }, PostSuccessMessage);
        }

        [HttpDelete("logical/{id}")]
        public async Task<IActionResult> DeleteLogical(string id)
        {
            var objective = await _objectiveService.GetObjectiveById(id);
            if (objective == null)
            {
                _logger.LogWarning("Objective with ID {Id} not found.", id);
                return NotFound(new { Message = $"Objective with ID {id} not found." });
            }

            await _objectiveService.DeleteObjective(id);
            _logger.LogInformation("Objective with ID {Id} logically deleted.", id);
            return NoContent();
        }

        [HttpDelete("physical/{id}")]
        public async Task<IActionResult> DeletePhysical(string id)
        {
            var objective = await _objectiveService.GetObjectiveById(id);
            if (objective == null)
            {
                _logger.LogWarning("Objective with ID {Id} not found.", id);
                return NotFound(new { Message = $"Objective with ID {id} not found." });
            }

            await _objectiveService.DeleteObjectivePermanently(id);
            _logger.LogInformation("Objective with ID {Id} physically deleted.", id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] ObjectiveDto objectiveDto, string id)
        {
            var result = await _objectiveService.UpdateObjective(objectiveDto, id);
            _logger.LogInformation("Objective with ID {Id} updated successfully.", id);
            return Ok(result);
        }
    }
}
