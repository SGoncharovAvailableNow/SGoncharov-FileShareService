using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public interface IFileServices
    {
        Task<FileServiceResponseDto> GetFileAsync(string fileId, CancellationToken cancellationToken);

        Task<FileServiceResponseDto> UploadFileAsync(IFormFile formFile, string deletePassword, Guid userId, CancellationToken cancellationToken);

        Task<FileServiceResponseDto> DeleteFileAsync(string fileId, string deletePass, CancellationToken cancellationToken);

    }
}
