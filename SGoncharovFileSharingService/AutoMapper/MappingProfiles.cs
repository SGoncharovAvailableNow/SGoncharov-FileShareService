using AutoMapper;
using SGoncharovFileSharingService.Models.ControllerDto;
using SGoncharovFileSharingService.Models.ControllerResponseDto;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.DTO.FileServiceDTO;
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

            CreateMap<RegisterUserRequestResponseDTO, LoginUserDto>()
                .ForMember(destinationMember => destinationMember.Name, opt => opt.MapFrom(sourceMember => sourceMember.Name))
                .ForMember(destinationMember => destinationMember.Email, opt => opt.MapFrom(sourceMember => sourceMember.Email))
                .ForMember(destinationMember => destinationMember.UserId, opt => opt.MapFrom(sourceMember => sourceMember.UserId))
                .ForMember(destinationMember => destinationMember.Token, opt => opt.MapFrom(sourceMember => sourceMember.Token));

            CreateMap<UploadFileRequestResponseDTO, UploadFileDTO>()
                .ForMember(destinationMember => destinationMember.UploadFileData, opt => opt.MapFrom(sourceMember => sourceMember.UploadFileData));

            CreateMap<DeleteFileRequestResponseDTO, DeleteFileDTO>()
                .ForMember(destinationMember => destinationMember.DeleteInfo, opt => opt.MapFrom(sourceMember => sourceMember.DeleteInfo));
        }
    }
}
