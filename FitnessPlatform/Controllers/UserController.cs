using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Services.Abstractions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string PostSuccessMessage = "Successfully registered!";

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetUserById(string id)
        {
            return await _userService.GetUserById(id);
        }

        [HttpGet("name/{name}")]
        public async Task<UserDto> GetUserByName(string name)
        {
            return await _userService.GetUserByName(name);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto userDto)
        {
            await _userService.CreateUser(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, PostSuccessMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound(new { Message = $"User with ID {id} not found." });

            await _userService.DeleteUser(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] UserDto userDto, string id)
        {
            return Ok(await _userService.UpdateUser(userDto, id));
        }
    }
}
