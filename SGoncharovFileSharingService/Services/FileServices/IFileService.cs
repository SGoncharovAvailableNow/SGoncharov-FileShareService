using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public interface IFileServices
    {
        Task<ResponseDto> GetFileAsync(string fileId, CancellationToken cancellationToken);

        Task<ResponseDto> UploadFileAsync(IFormFile formFile, string deletePassword, Guid userId, CancellationToken cancellationToken);

        Task<ResponseDto> DeleteFileAsync(string fileId, string deletePass, CancellationToken cancellationToken);

    }
}
