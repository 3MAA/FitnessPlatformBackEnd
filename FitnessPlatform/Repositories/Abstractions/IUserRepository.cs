using FitnessPlatform.Models;

namespace FitnessPlatform.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<User> GetUserByName(string name);
        Task InsertUser(User user);
        Task DeleteUser(string id);
        Task UpdateUser(User user, string id);
    }
}
