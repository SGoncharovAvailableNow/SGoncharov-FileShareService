using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.ControllerResponseDto;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.ResponseDto;
using SGoncharovFileSharingService.Services.FileServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;

namespace SGoncharovFileSharingService.Controllers.FileController
{
    [ApiController]
    [Route("api/v1/files")]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileServices _fileServices;

        private readonly IMapper _mapper;

        public FileController(IFileServices fileServices, IMapper mapper)
        {
            _fileServices = fileServices;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<FileControllerResponseDto>>> UploadFileAsync([FromForm, Required] IFormFile file,
        [FromQuery, Required] string deletePassword, CancellationToken cancellationToken)
        {

            var fileServiceResponseDto = await _fileServices.UploadFileAsync(file, deletePassword,
            this.GetUserId(), cancellationToken);

            if (string.IsNullOrWhiteSpace(fileServiceResponseDto.ResponseData) || string.IsNullOrEmpty(fileServiceResponseDto.ResponseData))
            {
                throw new NullReferenceException();
            }

            return new ApiResponse<FileControllerResponseDto>
            {
                Data = _mapper.Map<FileControllerResponseDto>(fileServiceResponseDto),
                ErrorDetails = string.Empty,
                StatusCode = 201
            };
        }

        [HttpDelete("{fileId}")]
        public async Task<ActionResult<ApiResponse<FileControllerResponseDto>>> DeleteFileAsync(
            [Required, FromRoute] string fileId,[FromQuery, Required] string deletePassword, CancellationToken cancellationToken)
        {
            var fileServiceResponseDto = await _fileServices.DeleteFileAsync(fileId, deletePassword, cancellationToken);

            return new ApiResponse<FileControllerResponseDto>
            {
                Data = _mapper.Map<FileControllerResponseDto>(fileServiceResponseDto),
                StatusCode = StatusCodes.Status200OK,
                ErrorDetails = string.Empty
            };
        }

        [HttpGet("{fileId}")]
        public async Task<ActionResult<FileControllerResponseDto>> GetFileAsync([Required, FromRoute] string fileId, 
            CancellationToken cancellationToken)
        {

            var servicesResult = await _fileServices.GetFileAsync(fileId, cancellationToken);

            return File(new FileStream(servicesResult.ResponseData, FileMode.Open), "file/file");
        }
    }

}
