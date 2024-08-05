using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class AuthUserDto
    {
        [Length(8, 120, ErrorMessage = $"Invalid parameter length: {nameof(Password)}")]
        public string Password { get; set; }
        
        [Length(6, 120, ErrorMessage = $"Invalid parameter length: {nameof(Email)}")]
        public string Email { get; set; }
    }
}
