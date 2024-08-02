using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.FileProviders;
using NanoidDotNet;
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

        private readonly IPasswordHasher<FilesInfo> _passwordHasher;

        private const int ID_SIZE = 10;

        public FileServices(IFileRepository fileRepository, IWebHostEnvironment webHostEnvironment, 
        IPasswordHasher<FilesInfo> passwordHasher)
        {
            _fileRepository = fileRepository;
            _webHostEnvironment = webHostEnvironment;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseDto> UploadFileAsync(IFormFile formFile, string deletePassword, 
        Guid userId, CancellationToken cancellationToken)
        {
            var fileEntity = new FilesInfo();
            fileEntity.FilePath = $"{fileEntity.FileId}_{formFile.FileName}";

            using (var fileStream = new FileStream(Path
            .Combine(_webHostEnvironment.WebRootPath, fileEntity.FilePath), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            };

            fileEntity.DeletePassword = _passwordHasher.HashPassword(fileEntity, deletePassword);
            fileEntity.UserId = userId;
            fileEntity.FileId = await Nanoid.GenerateAsync(size: ID_SIZE);

            await _fileRepository.CreateFileInfoAsync(fileEntity,cancellationToken);

            return new ResponseDto
            {
                ResponseData = fileEntity.FileId
            };
        }

        public async Task<ResponseDto> GetFileAsync(string fileId, CancellationToken cancellationToken)
        {
            var fileEntity = await _fileRepository.GetFileInfoAsync(fileId, cancellationToken);

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

        public async Task<ResponseDto> DeleteFileAsync(string fileId, string deletePass, CancellationToken cancellationToken)
        {
            var fileEntity = await _fileRepository.GetFileInfoAsync(fileId, cancellationToken);

            var verifyResult = _passwordHasher.VerifyHashedPassword(fileEntity, fileEntity.DeletePassword, deletePass);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return new ResponseDto
                {
                    ResponseData = "Invalid password!"
                };
            }

            await _fileRepository.DeleteFileInfoAsync(fileId,cancellationToken);

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
