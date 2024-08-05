using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class RegisterUserDto
    {
        [Length(3, 120, ErrorMessage = $"Invalid parameter length: {nameof(Name)}")]
        public string Name { get; set; }

        [Length(6, 120, ErrorMessage = $"Invalid parameter length: {nameof(Email)}")]
        public string Email { get; set; }

        [Length(8, 120, ErrorMessage = $"Invalid parameter length: {nameof(Password)}")]
        public string Password { get; set; }
    }
}
