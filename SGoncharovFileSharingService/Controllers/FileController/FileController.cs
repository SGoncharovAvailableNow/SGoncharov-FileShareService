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

        private Guid GetUserId()
        {
            Guid.TryParse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, out Guid guidFromClaim);
            return guidFromClaim;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> UploadFileAsync([FromForm, Required] IFormFile file,
        [FromQuery, Required] string deletePassword)
        {

            var servicesResult = await _fileServices.UploadFileAsync(file, deletePassword, GetUserId());
            if (string.IsNullOrWhiteSpace(servicesResult) || string.IsNullOrEmpty(servicesResult))
            {
                return new ApiResponse<string>
                {
                    Data = servicesResult,
                    ErrorDetails = string.Empty,
                    StatusCode = 500
                };
            }
            return new ApiResponse<string>
            {
                Data = servicesResult,
                ErrorDetails = string.Empty,
                StatusCode = 201
            };
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteFileAsync([Required, FromRoute] string uuid,
        [FromQuery, Required] string deletePassword)
        {

            var servicesResult = await _fileServices.DeleteFileAsync(uuid, deletePassword);

            return servicesResult switch
            {
                "Invalid password!" => Forbid(servicesResult),
                "Already Deleted" => BadRequest(servicesResult),
                _ => new ApiResponse<string>
                {
                    Data = servicesResult,
                    StatusCode = StatusCodes.Status200OK,
                    ErrorDetails = string.Empty
                }
            };
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<string>> GetFileAsync([Required, FromRoute] string uuid)
        {

            var servicesResult = await _fileServices.GetFileAsync(uuid);

            return servicesResult switch
            {
                "File not exists!" => NotFound(servicesResult),
                _ => File(new FileStream(servicesResult,FileMode.Open),"file/file")
            };
        }
    }

}
