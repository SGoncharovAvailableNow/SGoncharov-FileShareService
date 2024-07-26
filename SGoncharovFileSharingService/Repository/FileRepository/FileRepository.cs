using Microsoft.EntityFrameworkCore;
using SGoncharovFileSharingService.FileSharingContext;
using SGoncharovFileSharingService.Models.Entities.FileEntities;

namespace SGoncharovFileSharingService.Repository.FileRepository
{
    public class FileRepository : IFileRepository
    {
        private FileSharingContext.FileShareContext _context;

        public FileRepository(FileSharingContext.FileShareContext context)
        {
            _context = context;
        }

        public async Task CreateFileInfo(FileEntity fileEntity)
        {
            await _context.FileEntities.AddAsync(fileEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<FileEntity> GetFileInfoAsync(string uuid)
        {
            return await _context.FileEntities?
                .FirstOrDefaultAsync(file => file.Uuid == uuid);
        }

        public async Task DeleteFileInfoAsync(string uuid)
        {
            await _context.FileEntities
                .Where(file => file.Uuid == uuid )
                .ExecuteDeleteAsync();
        }

        public async Task DeleteFileInfoByPathAsync(string path)
        {
            await _context.FileEntities
            .Where(file => file.FilePath == path)
            .ExecuteDeleteAsync();    
        }
    }
}
