using FitnessPlatform.Models.DTOs;

namespace FitnessPlatform.Services.Abstractions
{
    public interface IWorkoutService
    {
        Task<List<WorkoutDto>> GetAllWorkouts();
        Task<WorkoutDto> GetWorkoutById(string id);
        Task<WorkoutDto> GetWorkoutByType(string type);
        Task CreateWorkout(WorkoutDto workoutDto);
        Task DeleteWorkout(string id);
        Task DeleteWorkoutPermanently(string id);
        Task<WorkoutDto> UpdateWorkout(WorkoutDto workoutDto, string id);
    }
}
