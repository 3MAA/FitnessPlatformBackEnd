using AutoMapper;

using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;
using FitnessPlatform.Services.Abstractions;

namespace FitnessPlatform.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        private readonly IMapper _mapper;

        public WorkoutService(IWorkoutRepository workoutRepository, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _mapper = mapper;
        }

        public async Task<List<WorkoutDto>> GetAllWorkouts()
        {
            var workouts = await _workoutRepository.GetAllWorkouts();
            return _mapper.Map<List<WorkoutDto>>(workouts);
        }

        public async Task<WorkoutDto> GetWorkoutById(string id)
        {
            var workout = await _workoutRepository.GetWorkoutById(id);
            return _mapper.Map<WorkoutDto>(workout);
        }

        public async Task<WorkoutDto> GetWorkoutByType(string type)
        {
            var workout = await _workoutRepository.GetWorkoutByType(type);
            return _mapper.Map<WorkoutDto>(workout);
        }

        public async Task CreateWorkout(WorkoutDto workoutDto)
        {
            var workout = _mapper.Map<Workout>(workoutDto); // Mapping WorkoutDto to Workout
            await _workoutRepository.InsertWorkout(workout);
        }

        public async Task DeleteWorkout(string id)
        {
            var workout = await _workoutRepository.GetWorkoutById(id);
            if(workout == null)
            {
                throw new ArgumentException($"Workout with ID {id} not found.");
            }
            await _workoutRepository.DeleteWorkout(id);
        }

        public async Task DeleteWorkoutPermanently(string id)
        {
            var workout = await _workoutRepository.GetWorkoutById(id);
            if (workout == null)
            {
                throw new ArgumentException($"Workout with ID {id} not found.");
            }
            await _workoutRepository.DeleteWorkoutPermanently(id);
        }

        public async Task<WorkoutDto> UpdateWorkout(WorkoutDto workoutDto, string id)
        {
            var foundWorkout = await _workoutRepository.GetWorkoutById(id);
            if (foundWorkout == null)
            {
                throw new ArgumentException($"Workout with ID {id} not found.");
            }

            var updatedWorkout = _mapper.Map<Workout>(workoutDto);
            await _workoutRepository.UpdateWorkout(updatedWorkout, id);
            return await GetWorkoutById(id);
        }
    }
}
