using AutoMapper;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
namespace SGoncharovFileSharingService.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<User,LoginUserDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
        }
    }
}
