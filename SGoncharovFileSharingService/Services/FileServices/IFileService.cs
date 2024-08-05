using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.DTO.FileServiceDTO;
using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public interface IFileServices
    {
        Task<GetFileDTO> GetFileAsync(string fileId, CancellationToken cancellationToken);

        Task<UploadFileDTO> UploadFileAsync(IFormFile formFile, string deletePassword, Guid userId, CancellationToken cancellationToken);

        Task<DeleteFileDTO> DeleteFileAsync(string fileId, string deletePass, CancellationToken cancellationToken);

    }
}
