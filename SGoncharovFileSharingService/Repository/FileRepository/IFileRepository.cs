using SGoncharovFileSharingService.Models.Entities.FileEntities;

namespace SGoncharovFileSharingService.Repository.FileRepository
{
    public interface IFileRepository
    {
        Task CreateFileInfo(FileEntity fileEntity);
        Task DeleteFileInfoAsync(string uuid, string deletePass);
        Task<string> GetPasswordAsync(string uuid);
    }
}
