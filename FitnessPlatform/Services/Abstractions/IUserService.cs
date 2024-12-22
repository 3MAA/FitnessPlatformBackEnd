using FitnessPlatform.Models.DTOs;

namespace FitnessPlatform.Services.Abstractions
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(string id);
        Task<UserDto> GetUserByName(string name);
        Task CreateUser(UserDto user);
        Task DeleteUser(string id);
        Task<UserDto> UpdateUser(UserDto userDto, string id);
    }
}
