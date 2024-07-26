using SGoncharovFileSharingService.Models.Entities.FileEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGoncharovFileSharingService.Models.Entities.UserEntities
{
    public class UserEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<FileEntity> Files { get; set; }
    }
}
