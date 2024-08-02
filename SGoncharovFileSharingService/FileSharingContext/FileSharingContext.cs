using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilesInfo>().Property(p => p.FileId).HasValueGenerator(typeof(Nanoid));
            modelBuilder.Entity<FilesInfo>().HasKey(p => p.FileId);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
