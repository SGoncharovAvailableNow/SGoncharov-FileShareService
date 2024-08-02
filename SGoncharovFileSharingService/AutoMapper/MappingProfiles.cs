using AutoMapper;
using SGoncharovFileSharingService.Models.ControllerDto;
using SGoncharovFileSharingService.Models.ControllerResponseDto;
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

            CreateMap<UsersControllerLoginDto, LoginUserDto>()
                .ForMember(destinationMember => destinationMember.Name, opt => opt.MapFrom(sourceMember => sourceMember.Name))
                .ForMember(destinationMember => destinationMember.Email, opt => opt.MapFrom(sourceMember => sourceMember.Email))
                .ForMember(destinationMember => destinationMember.UserId, opt => opt.MapFrom(sourceMember => sourceMember.UserId))
                .ForMember(destinationMember => destinationMember.Token, opt => opt.MapFrom(sourceMember => sourceMember.Token));

            CreateMap<FileControllerResponseDto, FileServiceResponseDto>()
                .ForMember(destinationMember => destinationMember.ResponseData, opt => opt.MapFrom(sourceMember => sourceMember.Data));


        }
    }
}
