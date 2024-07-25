using Microsoft.EntityFrameworkCore;
using SGoncharovFileSharingService.FileSharingContext;
using SGoncharovFileSharingService.Models.Entities.FileEntities;

namespace SGoncharovFileSharingService.Repository.FileRepository
{
    public class FileRepository : IFileRepository
    {
        private FileShareContext _context;

        public FileRepository(FileShareContext context)
        {
            _context = context;
        }

        public async Task CreateFileInfo(FileEntity fileEntity)
        {
            await _context.FileEntities.AddAsync(fileEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFileInfoAsync(string uuid,string deletePass)
        {
            await _context.FileEntities
                .Where(file => file.Uuid == uuid && file.DeletePassword == deletePass)
                .ExecuteDeleteAsync();
        }

        public async Task<string> GetPasswordAsync(string uuid) 
        {
            return (await _context.FileEntities
                .FirstAsync(file => file.Uuid == uuid)).DeletePassword;
        }
    }
}
