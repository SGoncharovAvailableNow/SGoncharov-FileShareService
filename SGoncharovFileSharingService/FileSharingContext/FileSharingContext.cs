using Microsoft.EntityFrameworkCore;
using SGoncharovFileSharingService.Models.Entities.FileEntities;
using SGoncharovFileSharingService.Models.Entities.UserEntities;

namespace SGoncharovFileSharingService.FileSharingContext
{
    public class FileShareContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FilesInfo> Files { get; set; }

        public FileShareContext(DbContextOptions<FileShareContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
