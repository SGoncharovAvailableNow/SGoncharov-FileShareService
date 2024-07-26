using SGoncharovFileSharingService.Models.Entities.FileEntities;

namespace SGoncharovFileSharingService.Repository.FileRepository
{
    public interface IFileRepository
    {
        Task CreateFileInfo(FileEntity fileEntity);
        Task DeleteFileInfoAsync(string uuid);
        Task<FileEntity> GetFileInfoAsync(string uuid);
        Task DeleteFileInfoByPathAsync(string fileName);
    }
}
