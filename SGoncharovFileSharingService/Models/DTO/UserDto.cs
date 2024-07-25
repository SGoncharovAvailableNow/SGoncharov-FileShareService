using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class UserDto
    {
        [Length(3,120,ErrorMessage = "Wrong Length!")]
        public string Name { get; set; }
        [Length(6, 120, ErrorMessage = "Wrong Length!")]
        public string Email { get; set; }

    }
}
