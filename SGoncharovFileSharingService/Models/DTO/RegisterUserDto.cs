using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class RegisterUserDto : UserDto
    {
        [Length(8, 120, ErrorMessage = "Wrong Length!")]
        public string Password { get; set; }
    }
}
