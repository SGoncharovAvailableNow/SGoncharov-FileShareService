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

        public async Task CreateFileInfo(Models.Entities.FileEntities.FilesInfo fileEntity)
        {
            await _context.Files.AddAsync(fileEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Entities.FileEntities.FilesInfo> GetFileInfoAsync(string fileId)
        {
            return await _context.Files?
                .FirstOrDefaultAsync(file => file.FileId == fileId);
        }

        public async Task DeleteFileInfoAsync(string fileId)
        {
            await _context.Files
                .Where(file => file.FileId == fileId )
                .ExecuteDeleteAsync();
        }

        public async Task DeleteFileInfoByPathAsync(string path)
        {
            await _context.Files
            .Where(file => file.FilePath == path)
            .ExecuteDeleteAsync();    
        }
    }
}
