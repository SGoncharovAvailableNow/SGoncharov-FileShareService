using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.UserServices
{
    public interface IUserServices
    {
        Task<LoginUserDto> RegisterUserAsync(RegisterUserDto regUserDto, CancellationToken cancellationToken);

        Task<LoginUserDto> LoginUserAsync(AuthUserDto authUserDto, CancellationToken cancellationToken);

        Task UpdateUserAsync(UserDto userDto, Guid id, CancellationToken cancellationToken);
    }
}
