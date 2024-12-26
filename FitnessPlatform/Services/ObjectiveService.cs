using AutoMapper;

using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;
using FitnessPlatform.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FitnessPlatform.Services
{
    public class ObjectiveService : IObjectiveService
    {
        private readonly IObjectiveRepository _objectiveRepository;

        private readonly IDistributedCache _distributedCache;

        private readonly IMapper _mapper;

        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

        public ObjectiveService(IObjectiveRepository objectiveRepository, IDistributedCache distributedCache, IMapper mapper)
        {
            _objectiveRepository = objectiveRepository;
            _distributedCache = distributedCache;
            _mapper = mapper;
        }

        public async Task<List<ObjectiveDto>> GetAllObjectives()
        {
            var cachedObjectives = await _distributedCache.GetStringAsync("all_objectives");
            if (cachedObjectives != null)
            {
                return JsonSerializer.Deserialize<List<ObjectiveDto>>(cachedObjectives);
            }

            var objectives = await _objectiveRepository.GetAllObjectives();
            var objectivesDto = _mapper.Map<List<ObjectiveDto>>(objectives);

            await _distributedCache.SetStringAsync("all_objectives", JsonSerializer.Serialize(objectivesDto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return objectivesDto;
        }

        public async Task<ObjectiveDto> GetObjectiveById(string id)
        {
            var cachedObjective = await _distributedCache.GetStringAsync($"objective_{id}");
            if (cachedObjective != null)
            {
                return JsonSerializer.Deserialize<ObjectiveDto>(cachedObjective);
            }

            var objective = await _objectiveRepository.GetObjectiveById(id);
            var objectiveDto = _mapper.Map<ObjectiveDto>(objective);

            await _distributedCache.SetStringAsync($"objective_{id}", JsonSerializer.Serialize(objectiveDto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return objectiveDto;
        }

        public async Task<ObjectiveDto> GetObjectiveByType(string type)
        {
            var cachedObjective = await _distributedCache.GetStringAsync($"objective_type_{type}");
            if (cachedObjective != null)
            {
                return JsonSerializer.Deserialize<ObjectiveDto>(cachedObjective);
            }

            var objective = await _objectiveRepository.GetObjectiveByType(type);
            var objectiveDto = _mapper.Map<ObjectiveDto>(objective);

            await _distributedCache.SetStringAsync($"objective_type_{type}", JsonSerializer.Serialize(objectiveDto), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return objectiveDto;
        }

        public async Task CreateObjective(ObjectiveDto objectiveDto)
        {
            var objective = _mapper.Map<Objective>(objectiveDto); // Mapping Objective1Dto to Objective
            await _objectiveRepository.InsertObjective(objective);

            await _distributedCache.RemoveAsync("all_objectives");
        }

        public async Task DeleteObjective(string id)
        {
            var foundObjective = await _objectiveRepository.GetObjectiveById(id);
            if (foundObjective == null)
            {
                throw new ArgumentException($"Objective with ID {id} not found.");
            }

            await _objectiveRepository.DeleteObjective(id);

            await _distributedCache.RemoveAsync($"objective_{id}");
            await _distributedCache.RemoveAsync("all_objectives");
        }

        public async Task DeleteObjectivePermanently(string id)
        {
            var foundObjective = await _objectiveRepository.GetObjectiveById(id);
            if (foundObjective == null)
            {
                throw new ArgumentException($"Objective with ID {id} not found.");
            }

            await _objectiveRepository.DeleteObjectivePermanently(id);

            await _distributedCache.RemoveAsync($"objective_{id}");
            await _distributedCache.RemoveAsync("all_objectives");
        }

        public async Task<ObjectiveDto> UpdateObjective(ObjectiveDto objectiveDto, string id)
        {
            var foundObjective = await _objectiveRepository.GetObjectiveById(id);
            if (foundObjective == null)
            {
                throw new ArgumentException($"Objective with ID {id} not found.");
            }

            var updatedObjective = _mapper.Map<Objective>(objectiveDto);
            await _objectiveRepository.UpdateObjective(updatedObjective, id);

            await _distributedCache.RemoveAsync($"objective_{id}");
            await _distributedCache.RemoveAsync("all_objectives");

            return await GetObjectiveById(id);
        }
    }
}
