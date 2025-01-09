using FitnessPlatform.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FitnessPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly GoalService _goalService;
        private readonly ILogger<GoalController> _logger;

        public GoalController(GoalService goalService, ILogger<GoalController> logger)
        {
            _goalService = goalService;
            _logger = logger;
        }

        [HttpPost("check-discount")]
        public async Task<IActionResult> CheckDiscount(int userId, int objectiveId)
        {
            _logger.LogInformation("CheckDiscount endpoint called for UserId: {UserId} and ObjectiveId: {ObjectiveId}", userId, objectiveId);
            var isEligible = await _goalService.CheckAndGrantDiscountAsync(userId, objectiveId);

            if (isEligible)
            {
                _logger.LogInformation("Discount granted for UserId: {UserId} and ObjectiveId: {ObjectiveId}", userId, objectiveId);
                return Ok(new { Message = "Discount granted successfully." });
            }
            else
            {
                _logger.LogWarning("Conditions not met for discount for UserId: {UserId} and ObjectiveId: {ObjectiveId}", userId, objectiveId);
                return BadRequest(new { Message = "Conditions not met for discount." });
            }
        }
    }
}
