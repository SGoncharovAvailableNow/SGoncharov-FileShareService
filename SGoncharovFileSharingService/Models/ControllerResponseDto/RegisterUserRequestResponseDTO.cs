using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.ControllerDto
{
    public class RegisterUserRequestResponseDTO
    {
        [Length(3, 120, ErrorMessage = $"Invalid parameter length: {nameof(Name)}")]
        public string Name { get; set; }

        [Length(6, 120, ErrorMessage = $"Invalid parameter length: {nameof(Email)}")]
        public string Email { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; }
    }
}
