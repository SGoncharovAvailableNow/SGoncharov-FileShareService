using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;
using SGoncharovFileSharingService.Models.Entities.FileEntities;
using SGoncharovFileSharingService.Models.ResponseDto;
using SGoncharovFileSharingService.Repository.FileRepository;
using System.Diagnostics;
using System.IO;

namespace SGoncharovFileSharingService.Services.FileServices
{
    public class FileServices : IFileServices
    {
        private readonly IFileRepository _fileRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileServices(IFileRepository fileRepository, IWebHostEnvironment webHostEnvironment)
        {
            _fileRepository = fileRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ApiResponse<string>> UploadFileAsync(IFormFile formFile, string deletePassword, string userId)
        {
            var fileEntity = new FileEntity();
            fileEntity.FilePath = Path.Combine(_webHostEnvironment.WebRootPath, $"{fileEntity.Uuid}_{formFile.FileName}");
            try
            {
                using (var fileStream = new FileStream(fileEntity.FilePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Data = string.Empty,
                    StatusCode = 500,
                    ErrorDetails = ex.Message
                };
            }
            Guid userGuid;
            Guid.TryParse(userId, out userGuid);
            fileEntity.DeletePassword = Argon2.Hash(deletePassword);
            fileEntity.UserId = userGuid;
            await _fileRepository.CreateFileInfo(fileEntity);
            return new ApiResponse<string>
            {
                Data = fileEntity.Uuid,
                StatusCode = 201,
                ErrorDetails = string.Empty
            };
        }

        public async Task<ApiResponse<string>> GetFileAsync(string uuid)
        {
            var fileEntity = await _fileRepository.GetFileInfoAsync(uuid);
            if (!File.Exists(fileEntity.FilePath))
            {
                return new ApiResponse<string>
                {
                    Data = string.Empty,
                    StatusCode = 400,
                    ErrorDetails = $"File {uuid} does not exists!"
                };
            }
            return new ApiResponse<string>
            {
                Data = fileEntity.FilePath,
                StatusCode = StatusCodes.Status200OK,
                ErrorDetails = string.Empty
            };
        }

        public async Task<ApiResponse<string>> DeleteFileAsync(string uuid, string deletePass)
        {
            var fileEntity = await _fileRepository.GetFileInfoAsync(uuid);
            if (!Argon2.Verify(fileEntity.DeletePassword, deletePass))
            {
                return new ApiResponse<string>
                {
                    Data = string.Empty,
                    StatusCode = StatusCodes.Status403Forbidden,
                    ErrorDetails = "Invalid password!"
                };
            }
            await _fileRepository.DeleteFileInfoAsync(uuid);
            if (!File.Exists(fileEntity.FilePath))
            {
                return new ApiResponse<string>
                {
                    Data = string.Empty,
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorDetails = "Already Deleted"
                };

            }
            File.Delete(fileEntity.FilePath);
            return new ApiResponse<string>
            {
                Data = "Deleted",
                StatusCode = StatusCodes.Status200OK,
                ErrorDetails = string.Empty
            };
        }
    }
}
