using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Services.FileServices;
using System;
using System.ComponentModel.DataAnnotations;

namespace SGoncharovFileSharingService.Controllers.FileController
{
    [ApiController, Route("/file"),Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileServices _fileServices;
        public FileController(IFileServices fileServices)
        {
            _fileServices = fileServices;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm,Required] IFormFile file, [FromQuery,Required] string deletePassword) 
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Incorrect Data!");
            }

            var servicesResult = await _fileServices.UploadFileAsync(file, deletePassword);
            
            return servicesResult.StatusCode switch
            {
                500 => BadRequest(servicesResult.ErrorDetails),
                201 => Created(),
                _ => StatusCode(418)
            };
        }

        [HttpDelete("deletefile")]
        public async Task<IActionResult> DeleteFileAsync([FromRoute,Required] string uuid, [FromRoute,Required]string deletePass) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incorrect Data!");
            }

            var servicesResult = await _fileServices.DeleteFileAsync(uuid, deletePass);
            
            return servicesResult.StatusCode switch
            {
                403 => Forbid(servicesResult.ErrorDetails),
                404 => BadRequest(servicesResult.ErrorDetails),
                200 => Ok(servicesResult.Data),
                _ => StatusCode(418)
            };
        }

        [HttpGet("getfile")]
        public async Task<IActionResult> GetFileAsync([FromRoute,Required]string uuid) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incorrect Data!");
            }

            var servicesResult = _fileServices.GetFile(uuid);
           
            return servicesResult.StatusCode switch 
            {
                400 => BadRequest(servicesResult.ErrorDetails),
                200 => await Task.FromResult(PhysicalFile(servicesResult.Data,"file/file")),
                _ => StatusCode(418)
            };
        }
    }

}
