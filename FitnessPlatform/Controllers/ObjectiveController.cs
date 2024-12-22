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
        private const string PostSuccessMessage = "Successfully registered";

        private readonly IObjectiveService _objectiveService;

        public ObjectiveController(IObjectiveService objectiveService)
        {
            _objectiveService = objectiveService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllObjectives()
        {
            return Ok(await _objectiveService.GetAllObjectives());
        }

        [HttpGet("{id}")]
        public async Task<ObjectiveDto> GetObjectiveById(string id)
        {
            return await _objectiveService.GetObjectiveById(id);
        }

        [HttpGet("type/{type}")]
        public async Task<ObjectiveDto> GetObjectiveByType(string type)
        {
            return await _objectiveService.GetObjectiveByType(type);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ObjectiveDto objectiveDto)
        {
            await _objectiveService.CreateObjective(objectiveDto);
            return CreatedAtAction(nameof(GetObjectiveById), new { id = objectiveDto.ObjectiveId }, PostSuccessMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var objective = await _objectiveService.GetObjectiveById(id);
            if (objective == null)
                return NotFound(new { Message = $"Objective with ID {id} not found." });

            await _objectiveService.DeleteObjective(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] ObjectiveDto objectiveDto, string id)
        {
            return Ok(await _objectiveService.UpdateObjective(objectiveDto, id));
        }
    }
}
