using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ResponseDto> UploadFileAsync(IFormFile formFile, string deletePassword, Guid userId)
        {
            var passwordHasher = new PasswordHasher<FilesInfo>();

            var fileEntity = new FilesInfo();
            fileEntity.FilePath = $"{fileEntity.FileId}_{formFile.FileName}";

            using (var fileStream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, fileEntity.FilePath), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            };

            fileEntity.DeletePassword = passwordHasher.HashPassword(fileEntity, deletePassword);
            fileEntity.UserId = userId;

            await _fileRepository.CreateFileInfo(fileEntity);

            return new ResponseDto
            {
                ResponseData = fileEntity.FileId
            };
        }

        public async Task<ResponseDto> GetFileAsync(string fileId)
        {
            var fileEntity = await _fileRepository.GetFileInfoAsync(fileId);

            if (fileEntity == null)
            {
                return new ResponseDto
                {
                    ResponseData = "File not exists!"
                };
            }

            return new ResponseDto
            {
                ResponseData = Path.Combine(_webHostEnvironment.WebRootPath, fileEntity.FilePath)
            };
        }

        public async Task<ResponseDto> DeleteFileAsync(string fileId, string deletePass)
        {
            var passwordHasher = new PasswordHasher<FilesInfo>();

            var fileEntity = await _fileRepository.GetFileInfoAsync(fileId);

            var verifyResult = passwordHasher.VerifyHashedPassword(fileEntity, fileEntity.DeletePassword, deletePass);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return new ResponseDto
                {
                    ResponseData = "Invalid password!"
                };
            }

            await _fileRepository.DeleteFileInfoAsync(fileId);

            if (!File.Exists(fileEntity.FilePath))
            {
                return new ResponseDto
                {
                    ResponseData = "Already Deleted"
                };
            }

            File.Delete(fileEntity.FilePath);

            return new ResponseDto
            {
                ResponseData = "Deleted"
            };
        }
    }
}
