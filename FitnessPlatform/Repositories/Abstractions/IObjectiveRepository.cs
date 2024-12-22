using FitnessPlatform.Models;

namespace FitnessPlatform.Repositories.Abstractions
{
    public interface IObjectiveRepository
    {
        Task<List<Objective>> GetAllObjectives();
        Task<Objective> GetObjectiveById(string id);
        Task<Objective> GetObjectiveByType(string type);
        Task InsertObjective(Objective objective);
        Task DeleteObjective(string id);
        Task UpdateObjective(Objective objective, string id);
    }
}
