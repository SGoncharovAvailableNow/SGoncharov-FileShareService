using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class AuthUserDto
    {
        [Length(8, 120, ErrorMessage = "Wrong Length!")]
        public string Password { get; set; }
        
        [Length(6, 120, ErrorMessage = "Wrong Length!")]
        public string Email { get; set; }
    }
}
