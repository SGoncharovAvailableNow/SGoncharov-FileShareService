using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class LoginUserDto : UserDto
    {
        public Guid UserId { get; set; }
        public string Token {  get; set; }
    }
}
