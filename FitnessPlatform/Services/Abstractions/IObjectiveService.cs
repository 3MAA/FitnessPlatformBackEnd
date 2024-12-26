using FitnessPlatform.Models.DTOs;

namespace FitnessPlatform.Services.Abstractions
{
    public interface IObjectiveService
    {
        Task<List<ObjectiveDto>> GetAllObjectives();
        Task<ObjectiveDto> GetObjectiveById(string id);
        Task<ObjectiveDto> GetObjectiveByType(string type);
        Task CreateObjective(ObjectiveDto objectiveDto);
        Task DeleteObjective(string id);
        Task DeleteObjectivePermanently(string id);
        Task<ObjectiveDto> UpdateObjective(ObjectiveDto objectiveDto, string id);
    }
}
