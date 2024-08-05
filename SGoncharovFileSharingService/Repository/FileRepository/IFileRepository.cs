using SGoncharovFileSharingService.Models.Entities.FileEntities;

namespace SGoncharovFileSharingService.Repository.FileRepository
{
    public interface IFileRepository
    {
        Task CreateFileInfoAsync(FilesInfo fileEntity, CancellationToken cancellationToken);

        Task DeleteFileInfoAsync(string fileId, CancellationToken cancellationToken);
        
        Task<FilesInfo> GetFileInfoAsync(string fileId, CancellationToken cancellationToken);

        Task DeleteFileInfoByPathAsync(string fileName, CancellationToken cancellationToken);
        
    }
}
