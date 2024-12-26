using FitnessPlatform.Models;

namespace FitnessPlatform.Repositories.Abstractions
{
    public interface IWorkoutRepository
    {
        Task<List<Workout>> GetAllWorkouts();
        Task<Workout> GetWorkoutById(string id);
        Task<Workout> GetWorkoutByType(string type);
        Task InsertWorkout(Workout workout);
        Task DeleteWorkout(string id);
        Task DeleteWorkoutPermanently(string id);
        Task UpdateWorkout(Workout workout, string id);
    }
}
