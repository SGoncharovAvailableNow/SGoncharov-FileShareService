using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public interface IFileServices
    {
        ApiResponse<string> GetFile(string uuid);
        Task<ApiResponse<string>> UploadFileAsync(IFormFile formFile, string deletePassword);
        Task<ApiResponse<string>> DeleteFileAsync(string uuid, string deletePass);
    }
}
