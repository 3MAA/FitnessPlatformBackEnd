using AutoMapper;

using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;
using FitnessPlatform.Services.Abstractions;

namespace FitnessPlatform.Services
{
    public class ObjectiveService : IObjectiveService
    {
        private readonly IObjectiveRepository _objectiveRepository;

        private readonly IMapper _mapper;

        public ObjectiveService(IObjectiveRepository objectiveRepository, IMapper mapper)
        {
            _objectiveRepository = objectiveRepository;
            _mapper = mapper;
        }

        public async Task<List<ObjectiveDto>> GetAllObjectives()
        {
            var objectives = await _objectiveRepository.GetAllObjectives();
            return _mapper.Map<List<ObjectiveDto>>(objectives);
        }

        public async Task<ObjectiveDto> GetObjectiveById(string id)
        {
            var objective = await _objectiveRepository.GetObjectiveById(id);
            return _mapper.Map<ObjectiveDto>(objective);
        }

        public async Task<ObjectiveDto> GetObjectiveByType(string type)
        {
            var objective = await _objectiveRepository.GetObjectiveByType(type);
            return _mapper.Map<ObjectiveDto>(objective);
        }

        public async Task CreateObjective(ObjectiveDto objectiveDto)
        {
            var objective = _mapper.Map<Objective>(objectiveDto); // Mapping Objective1Dto to Objective
            await _objectiveRepository.InsertObjective(objective);
        }

        public async Task DeleteObjective(string id)
        {
            var foundObjective = await _objectiveRepository.GetObjectiveById(id);
            if (foundObjective == null)
            {
                throw new ArgumentException($"Objective with ID {id} not found.");
            }

            await _objectiveRepository.DeleteObjective(id);
        }

        public async Task DeleteObjectivePermanently(string id)
        {
            var foundObjective = await _objectiveRepository.GetObjectiveById(id);
            if (foundObjective == null)
            {
                throw new ArgumentException($"Objective with ID {id} not found.");
            }

            await _objectiveRepository.DeleteObjectivePermanently(id);
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
            return await GetObjectiveById(id);
        }
    }
}
