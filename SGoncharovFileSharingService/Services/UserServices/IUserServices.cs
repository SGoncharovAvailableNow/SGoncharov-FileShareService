using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.UserServices
{
    public interface IUserServices
    {
        Task<ApiResponse<LoginUserDto>> AddUserAsync(RegisterUserDto regUserDto);

        Task<ApiResponse<LoginUserDto>> LoginUserAsync(AuthUserDto authUserDto);

        Task<ApiResponse<string?>> UpdateUserAsync(UserDto userDto,string id);
    }
}
