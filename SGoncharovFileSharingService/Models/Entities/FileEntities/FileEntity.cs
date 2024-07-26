﻿using NanoidDotNet;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGoncharovFileSharingService.Models.Entities.FileEntities
{
    public class FileEntity
    {
        [Key]
        public string Uuid { get; set; } = Nanoid.Generate();
        public string FilePath { get; set; }
        public string DeletePassword { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity? UserEntity { get; set; }

    }
}
