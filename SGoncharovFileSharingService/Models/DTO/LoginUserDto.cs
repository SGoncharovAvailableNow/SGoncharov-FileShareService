using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class LoginUserDto
    {
        [Length(3, 120, ErrorMessage = "Wrong Length!")]
        public string Name { get; set; }

        [Length(6, 120, ErrorMessage = "Wrong Length!")]
        public string Email { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; }
    }
}
