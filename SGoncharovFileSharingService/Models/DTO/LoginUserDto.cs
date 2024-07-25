using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Models.DTO
{
    public class LoginUserDto : UserDto
    {
        public Guid Id { get; set; }
        public string Token {  get; set; }
    }
}
