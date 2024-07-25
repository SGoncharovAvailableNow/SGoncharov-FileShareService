using Microsoft.EntityFrameworkCore;
using SGoncharovFileSharingService.Models.Entities.FileEntities;
using SGoncharovFileSharingService.Models.Entities.UserEntities;

namespace SGoncharovFileSharingService.FileSharingContext
{
    public class FileSharingContext : DbContext
    {
        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<FileEntity> FileEntities { get; set; }

        public FileSharingContext(DbContextOptions<FileSharingContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
