using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.UserServices
{
    public interface IUserServices
    {
        Task<LoginUserDto> RegisterUserAsync(RegisterUserDto regUserDto);

        Task<LoginUserDto> LoginUserAsync(AuthUserDto authUserDto);

        Task UpdateUserAsync(UserDto userDto, Guid id);
    }
}
