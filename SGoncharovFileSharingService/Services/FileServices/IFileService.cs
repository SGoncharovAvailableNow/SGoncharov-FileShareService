using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public interface IFileServices
    {
        Task<string> GetFileAsync(string uuid);

        Task<string> UploadFileAsync(IFormFile formFile, string deletePassword, Guid userId);

        Task<string> DeleteFileAsync(string uuid, string deletePass);

    }
}
