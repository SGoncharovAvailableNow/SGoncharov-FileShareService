using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class UserDto
    {
        [Length(3, 120, ErrorMessage = $"Invalid parameter length: {nameof(Name)}")]
        public string Name { get; set; }
        [Length(6, 120, ErrorMessage = $"Invalid parameter length: {nameof(Email)}")]
        public string Email { get; set; }

    }
}
