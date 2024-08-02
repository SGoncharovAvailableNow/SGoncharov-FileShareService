using SGoncharovFileSharingService.Models.Entities.FileEntities;

namespace SGoncharovFileSharingService.Repository.FileRepository
{
    public interface IFileRepository
    {
        Task CreateFileInfo(Models.Entities.FileEntities.FilesInfo fileEntity);

        Task DeleteFileInfoAsync(string fileId);
        
        Task<Models.Entities.FileEntities.FilesInfo> GetFileInfoAsync(string fileId);

        Task DeleteFileInfoByPathAsync(string fileName);
        
    }
}
