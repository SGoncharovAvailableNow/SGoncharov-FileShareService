using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public interface IFileServices
    {
        Task<ResponseDto> GetFileAsync(string fileId);

        Task<ResponseDto> UploadFileAsync(IFormFile formFile, string deletePassword, Guid userId);

        Task<ResponseDto> DeleteFileAsync(string fileId, string deletePass);

    }
}
