using AutoMapper;

using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;
using FitnessPlatform.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FitnessPlatform.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        private readonly IDistributedCache _distributedCache;

        private readonly IMapper _mapper;

        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

        public WorkoutService(IWorkoutRepository workoutRepository, IDistributedCache distributedCache, IMapper mapper)
        {
            _workoutRepository = workoutRepository;
            _distributedCache = distributedCache;
            _mapper = mapper;
        }

        public async Task<List<WorkoutDto>> GetAllWorkouts()
        {
            var cachedWorkouts = await _distributedCache.GetStringAsync("all_workouts");
            if (cachedWorkouts != null)
            {
                return JsonSerializer.Deserialize<List<WorkoutDto>>(cachedWorkouts);
            }

            var workouts = await _workoutRepository.GetAllWorkouts();
            var workoutsDto = _mapper.Map<List<WorkoutDto>>(workouts);

            await _distributedCache.SetStringAsync("all_workouts", JsonSerializer.Serialize(workoutsDto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return workoutsDto;
        }

        public async Task<WorkoutDto> GetWorkoutById(string id)
        {
            var cachedWorkout = await _distributedCache.GetStringAsync($"workout_{id}");
            if (cachedWorkout != null)
            {
                return JsonSerializer.Deserialize<WorkoutDto>(cachedWorkout);
            }

            var workout = await _workoutRepository.GetWorkoutById(id);
            var workoutDto = _mapper.Map<WorkoutDto>(workout);

            await _distributedCache.SetStringAsync($"workout_{id}", JsonSerializer.Serialize(workoutDto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return workoutDto;
        }

        public async Task<WorkoutDto> GetWorkoutByType(string type)
        {
            var cachedWorkout = await _distributedCache.GetStringAsync($"workout_type_{type}");
            if (cachedWorkout != null)
            {
                return JsonSerializer.Deserialize<WorkoutDto>(cachedWorkout);
            }

            var workout = await _workoutRepository.GetWorkoutByType(type);
            var workoutDto = _mapper.Map<WorkoutDto>(workout);

            await _distributedCache.SetStringAsync($"workout_type_{type}", JsonSerializer.Serialize(workoutDto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return workoutDto;
        }

        public async Task CreateWorkout(WorkoutDto workoutDto)
        {
            var workout = _mapper.Map<Workout>(workoutDto); // Mapping WorkoutDto to Workout
            await _workoutRepository.InsertWorkout(workout);

            await _distributedCache.RemoveAsync("all_workouts");
        }

        public async Task DeleteWorkout(string id)
        {
            var workout = await _workoutRepository.GetWorkoutById(id);
            if(workout == null)
            {
                throw new ArgumentException($"Workout with ID {id} not found.");
            }
            await _workoutRepository.DeleteWorkout(id);

            await _distributedCache.RemoveAsync($"workout_{id}");
            await _distributedCache.RemoveAsync("all_workouts");
        }

        public async Task DeleteWorkoutPermanently(string id)
        {
            var workout = await _workoutRepository.GetWorkoutById(id);
            if (workout == null)
            {
                throw new ArgumentException($"Workout with ID {id} not found.");
            }
            await _workoutRepository.DeleteWorkoutPermanently(id);

            await _distributedCache.RemoveAsync($"workout_{id}");
            await _distributedCache.RemoveAsync("all_workouts");
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

            await _distributedCache.RemoveAsync($"workout_{id}");
            await _distributedCache.RemoveAsync("all_workouts");

            return await GetWorkoutById(id);
        }
    }
}
