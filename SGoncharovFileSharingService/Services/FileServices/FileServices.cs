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

        public async Task<ApiResponse<string>> UploadFileAsync(IFormFile formFile, string deletePassword)
        {
            var fileEntity = new FileEntity();
            fileEntity.FilePath = Path.Combine(_webHostEnvironment.WebRootPath, fileEntity.Uuid);
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
            fileEntity.DeletePassword = Argon2.Hash(deletePassword);
            await _fileRepository.CreateFileInfo(fileEntity);
            return new ApiResponse<string>
            {
                Data = fileEntity.Uuid,
                StatusCode = 201,
                ErrorDetails = string.Empty
            };
        }

        public ApiResponse<string> GetFile(string uuid)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, uuid);
            if (!File.Exists(path))
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
                Data = path,
                StatusCode = StatusCodes.Status200OK,
                ErrorDetails = string.Empty
            };
        }

        public async Task<ApiResponse<string>> DeleteFileAsync(string uuid, string deletePass)
        {
            var hashedPass = await _fileRepository.GetPasswordAsync(uuid);
            if (!Argon2.Verify(hashedPass, deletePass)) 
            {
                return new ApiResponse<string>
                {
                    Data = string.Empty,
                    StatusCode = StatusCodes.Status403Forbidden,
                    ErrorDetails = "Invalid password!"
                };
            }
            await _fileRepository.DeleteFileInfoAsync(uuid, deletePass);
            var path = Path.Combine(_webHostEnvironment.WebRootPath, uuid);
            if (File.Exists(path))
            {
                new Task(() =>
                {
                    File.Delete(path);
                });
            }
            else 
            {
                return new ApiResponse<string>
                {
                    Data = string.Empty,
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorDetails = "Already Deleted"
                };
            }
            return new ApiResponse<string>
            {
                Data = "Deleted",
                StatusCode = StatusCodes.Status200OK,
                ErrorDetails = string.Empty
            };
        }
    }
}
