using AutoMapper;

using FitnessPlatform.Models.DTOs;
using FitnessPlatform.Models;

namespace FitnessPlatform.Configs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Objective, ObjectiveDto>().ReverseMap();
            CreateMap<Workout, WorkoutDto>().ReverseMap();
        }
    }
}
