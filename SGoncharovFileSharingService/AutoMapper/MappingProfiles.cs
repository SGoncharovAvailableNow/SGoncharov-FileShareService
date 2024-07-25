using AutoMapper;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
namespace SGoncharovFileSharingService.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<UserEntity,LoginUserDto>().ReverseMap();
            CreateMap<UserEntity, RegisterUserDto>().ReverseMap();
        }
    }
}
