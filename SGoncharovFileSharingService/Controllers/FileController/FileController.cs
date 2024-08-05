using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.ControllerResponseDto;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.ResponseDto;
using SGoncharovFileSharingService.Services.FileServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
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
        public async Task<ActionResult<ApiResponse<UploadFileRequestResponseDTO>>> UploadFileAsync([FromForm, Required] IFormFile file,
        [FromQuery, Required] string deletePassword, CancellationToken cancellationToken)
        {
            var fileServiceResponseDto = await _fileServices.UploadFileAsync(file, deletePassword,
            this.GetUserId(), cancellationToken);

            return new ApiResponse<UploadFileRequestResponseDTO>
            {
                Data = _mapper.Map<UploadFileRequestResponseDTO>(fileServiceResponseDto),
                ErrorDetails = null,
                StatusCode = 201
            };
        }

        [HttpDelete("{fileId}")]
        public async Task<ActionResult<ApiResponse<DeleteFileRequestResponseDTO>>> DeleteFileAsync(
            [Required, FromRoute] string fileId, [FromQuery, Required] string deletePassword, CancellationToken cancellationToken)
        {
            var fileServiceResponseDto = await _fileServices.DeleteFileAsync(fileId, deletePassword, cancellationToken);

            return new ApiResponse<DeleteFileRequestResponseDTO>
            {
                Data = _mapper.Map<DeleteFileRequestResponseDTO>(fileServiceResponseDto),
                ErrorDetails = null,
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFileAsync([Required, FromRoute] string fileId,
            CancellationToken cancellationToken)
        {
            var servicesResult = await _fileServices.GetFileAsync(fileId, cancellationToken);

            return File(servicesResult.FileData, MediaTypeNames.Application.Octet, servicesResult.FileName);
        }
    }

}
