using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.ResponseDto;
using SGoncharovFileSharingService.Services.FileServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SGoncharovFileSharingService.Controllers.FileController
{
    [ApiController]
    [Route("api/v1/files")]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileServices _fileServices;

        public FileController(IFileServices fileServices)
        {
            _fileServices = fileServices;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ResponseDto>>> UploadFileAsync([FromForm, Required] IFormFile file,
        [FromQuery, Required] string deletePassword)
        {

            var servicesResult = await _fileServices.UploadFileAsync(file, deletePassword, ControllerExtension.GetUserId(this));

            if (string.IsNullOrWhiteSpace(servicesResult.ResponseData) || string.IsNullOrEmpty(servicesResult.ResponseData))
            {
                throw new NullReferenceException();
            }

            return new ApiResponse<ResponseDto>
            {
                Data = servicesResult,
                ErrorDetails = string.Empty,
                StatusCode = 201
            };
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult<ApiResponse<ResponseDto>>> DeleteFileAsync([Required, FromRoute] string fileId,
        [FromQuery, Required] string deletePassword)
        {

            var servicesResult = await _fileServices.DeleteFileAsync(fileId, deletePassword);

            return servicesResult.ResponseData switch
            {
                "Invalid password!" => Forbid(servicesResult.ResponseData),
                "Already Deleted" => BadRequest(servicesResult),
                _ => new ApiResponse<ResponseDto>
                {
                    Data = servicesResult,
                    StatusCode = StatusCodes.Status200OK,
                    ErrorDetails = string.Empty
                }
            };
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<ResponseDto>> GetFileAsync([Required, FromRoute] string fileId)
        {

            var servicesResult = await _fileServices.GetFileAsync(fileId);

            return servicesResult.ResponseData switch
            {
                "File not exists!" => NotFound(servicesResult),
                _ => File(new FileStream(servicesResult.ResponseData, FileMode.Open), "file/file")
            };
        }
    }

}
