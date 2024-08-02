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

        public async Task CreateFileInfoAsync(FilesInfo fileEntity, CancellationToken cancellationToken)
        {
            _context.Files.Add(fileEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<FilesInfo> GetFileInfoAsync(string fileId, CancellationToken cancellationToken)
        {
            return await _context.Files?
                .FirstOrDefaultAsync(file => file.FileId == fileId, cancellationToken);
        }

        public async Task DeleteFileInfoAsync(string fileId, CancellationToken cancellationToken)
        {
            await _context.Files
                .Where(file => file.FileId == fileId)
                .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task DeleteFileInfoByPathAsync(string path, CancellationToken cancellationToken)
        {
            await _context.Files
            .Where(file => file.FilePath == path)
            .ExecuteDeleteAsync(cancellationToken);    
        }
    }
}
