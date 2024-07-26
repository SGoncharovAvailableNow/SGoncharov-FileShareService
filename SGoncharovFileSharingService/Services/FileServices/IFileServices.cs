using SGoncharovFileSharingService.Models.ResponseDto;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public interface IFileServices
    {
        Task<ApiResponse<string>> GetFileAsync(string uuid);
        Task<ApiResponse<string>> UploadFileAsync(IFormFile formFile, string deletePassword, string userId);
        Task<ApiResponse<string>> DeleteFileAsync(string uuid, string deletePass);
    }
}
