using NanoidDotNet;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGoncharovFileSharingService.Models.Entities.FileEntities
{
    public class FilesInfo
    {
        public string FileId { get; set; }
        public string FilePath { get; set; }
        public string DeletePassword { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User? UserEntity { get; set; }

    }
}
