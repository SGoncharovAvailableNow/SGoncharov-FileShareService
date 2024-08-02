using AutoMapper;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
namespace SGoncharovFileSharingService.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, LoginUserDto>()
            .ForMember(destinationMember => destinationMember.Email, opt => opt.MapFrom(sourceMember => sourceMember.Email))
            .ForMember(destinationMember => destinationMember.UserId, opt => opt.MapFrom(sourceMember => sourceMember.UserId))
            .ForMember(destinationMember => destinationMember.Name, opt => opt.MapFrom(sourceMember => sourceMember.Name));

            CreateMap<User, RegisterUserDto>()
            .ForMember(destinationMember => destinationMember.Email, opt => opt.MapFrom(sourceMember => sourceMember.Email))
            .ForMember(destinationMember => destinationMember.Name, opt => opt.MapFrom(sourceMember => sourceMember.Name));
        }
    }
}
