﻿using Microsoft.AspNetCore.Identity;
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

        public async Task<string> UploadFileAsync(IFormFile formFile, string deletePassword, Guid userId)
        {
            var passwordHasher = new PasswordHasher<FilesInfo>();

            var fileEntity = new FilesInfo();
            fileEntity.FilePath = $"{fileEntity.Uuid}_{formFile.FileName}";

            using (var fileStream = new FileStream(Path.Combine(_webHostEnvironment.WebRootPath, fileEntity.FilePath), FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            };

            fileEntity.DeletePassword = passwordHasher.HashPassword(fileEntity,deletePassword);
            fileEntity.UserId = userId;

            await _fileRepository.CreateFileInfo(fileEntity);

            return fileEntity.Uuid;
        }

        public async Task<string> GetFileAsync(string uuid)
        {
            var fileEntity = await _fileRepository.GetFileInfoAsync(uuid);

            if (fileEntity == null)
            {
                return "File not exists!";
            }

            return Path.Combine(_webHostEnvironment.WebRootPath, fileEntity.FilePath);
        }

        public async Task<string> DeleteFileAsync(string uuid, string deletePass)
        {
            var passwordHasher = new PasswordHasher<FilesInfo>();

            var fileEntity = await _fileRepository.GetFileInfoAsync(uuid);

            var verifyResult = passwordHasher.VerifyHashedPassword(fileEntity, fileEntity.DeletePassword, deletePass);
            
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return "Invalid password!";
            }

            await _fileRepository.DeleteFileInfoAsync(uuid);

            if (!File.Exists(fileEntity.FilePath))
            {
                return "Already Deleted";
            }

            File.Delete(fileEntity.FilePath);

            return "Deleted";
        }
    }
}
