using AutoMapper;

using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Models;
using FitnessPlatform.Repositories.Abstractions;
using FitnessPlatform.Services.Abstractions;

namespace FitnessPlatform.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var user = await _userRepository.GetUserById(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByName(string name)
        {
            var user = await _userRepository.GetUserByName(name);
            return _mapper.Map<UserDto>(user);
        }

        public async Task CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);  // Mapping UserDto to User
            await _userRepository.InsertUser(user);
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }
            await _userRepository.DeleteUser(id);
        }

        public async Task<UserDto> UpdateUser(UserDto userDto, string id)
        {
            var foundUser = await _userRepository.GetUserById(id);
            if (foundUser == null)
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }

            var updatedUser = _mapper.Map<User>(userDto);
            await _userRepository.UpdateUser(updatedUser, id);
            return await GetUserById(id);
        }
    }
}
