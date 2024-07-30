using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class RegisterUserDto
    {
        [Length(3, 120, ErrorMessage = "Wrong Length!")]
        public string Name { get; set; }

        [Length(6, 120, ErrorMessage = "Wrong Length!")]
        public string Email { get; set; }

        [Length(8, 120, ErrorMessage = "Wrong Length!")]
        public string Password { get; set; }
    }
}
