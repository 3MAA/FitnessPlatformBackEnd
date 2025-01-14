using FitnessPlatform.Models;
using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Services;
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
        private readonly GoalService _goalService;
        private readonly ILogger<ObjectiveController> _logger;

        public ObjectiveController(IObjectiveService objectiveService, GoalService goalService, ILogger<ObjectiveController> logger)
        {
            _objectiveService = objectiveService;
            _goalService = goalService;
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

        [HttpPost("save-draft")]
        public async Task<IActionResult> SaveDraft([FromBody] ObjectiveDto objective)
        {
            if (objective == null)
            {
                _logger.LogWarning("Attempted to save a null draft.");
                return BadRequest(new { message = "Objective data cannot be null." });
            }

            if (objective.ObjectiveId <= 0) // Daca este un obiectiv nou
            {
                objective.IsDraft = true;
                await _objectiveService.CreateObjective(objective);
                _logger.LogInformation("Draft created successfully for a new objective.");
                return Ok(new { message = "Draft created successfully." });
            }

            // Cauta obiectivul existent
            var existingObjective = await _objectiveService.GetObjectiveById(objective.ObjectiveId.ToString());

            if (existingObjective == null)
            {
                _logger.LogWarning("Objective with ID {Id} not found for update.", objective.ObjectiveId);
                return NotFound(new { message = $"Objective with ID {objective.ObjectiveId} not found." });
            }

            // Actualizeaza campurile obiectivului
            existingObjective.ObjectiveType = objective.ObjectiveType;
            existingObjective.TargetValue = objective.TargetValue;
            existingObjective.Progress = objective.Progress;
            existingObjective.StartDate = objective.StartDate;
            existingObjective.Deadline = objective.Deadline;
            existingObjective.IsDraft = true;

            // Actualizeaza obiectivul, furnizand ID-ul ca al doilea parametru
            await _objectiveService.UpdateObjective(existingObjective, existingObjective.ObjectiveId.ToString());
            _logger.LogInformation("Draft updated successfully for objective with ID {Id}.", objective.ObjectiveId);

            return Ok(new { message = "Draft updated successfully." });
        }

        [HttpPost("save-final")]
        public async Task<IActionResult> SaveFinal([FromQuery] int userId, [FromQuery] int objectiveId)
        {
            if (userId <= 0 || objectiveId <= 0)
            {
                _logger.LogWarning("Invalid userId or objectiveId received.");
                return BadRequest(new { message = "Invalid user ID or objective ID." });
            }

            var objective = await _objectiveService.GetObjectiveById(objectiveId.ToString());
            if (objective == null)
            {
                _logger.LogWarning("Objective with ID {Id} not found.", objectiveId);
                return NotFound(new { message = $"Objective with ID {objectiveId} not found." });
            }

            // Verifica daca obiectivul este in draft
            if (objective.IsDraft == false)
            {
                _logger.LogWarning("Objective with ID {Id} is already final.", objectiveId);
                return BadRequest(new { message = "This objective is already finalized." });
            }

            // Verifica daca obiectivul este valid pentru a deveni final (de exemplu, progres complet, deadline valid)
            if (objective.Progress < objective.TargetValue || (objective.Deadline.HasValue && objective.Deadline.Value < DateOnly.FromDateTime(DateTime.Now)))
            {
                _logger.LogWarning("Objective with ID {Id} does not meet the conditions to be finalized.", objectiveId);
                return BadRequest(new { message = "Objective conditions not met." });
            }

            // Actualizeaza obiectivul, setand `IsDraft` la `false`
            objective.IsDraft = false;
            await _objectiveService.UpdateObjective(objective, objectiveId.ToString());

            // Verifica daca utilizatorul este eligibil pentru un discount
            var isEligibleForDiscount = await _goalService.CheckAndGrantDiscountAsync(userId, objectiveId);
            if (isEligibleForDiscount)
            {
                _logger.LogInformation("Discount granted for objective with ID {Id}.", objectiveId);
                return Ok(new { message = "Discount granted!" });
            }

            _logger.LogInformation("Objective with ID {Id} was successfully saved as final.", objectiveId);
            return Ok(new { message = "Objective successfully saved as final." });
        }
    }
}
