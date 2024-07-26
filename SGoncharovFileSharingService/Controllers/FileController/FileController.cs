using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Services.FileServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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
        private string GetUserId() => User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm,Required] IFormFile file,
         [FromQuery(Name = "deletePassword"),Required] string deletePassword) 
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Incorrect Data!");
            }

            var servicesResult = await _fileServices.UploadFileAsync(file, deletePassword, GetUserId());
            
            return servicesResult.StatusCode switch
            {
                500 => BadRequest(servicesResult.ErrorDetails),
                201 => Created(),
                _ => StatusCode(418)
            };
        }

        [HttpDelete("deletefile")]
        public async Task<IActionResult> DeleteFileAsync([FromQuery(Name = "uuid"),Required] string uuid, 
        [FromQuery(Name = "deletePassword"),Required]string deletePassword) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incorrect Data!");
            }

            var servicesResult = await _fileServices.DeleteFileAsync(uuid, deletePassword);
            
            return servicesResult.StatusCode switch
            {
                403 => Forbid(servicesResult.ErrorDetails),
                404 => BadRequest(servicesResult.ErrorDetails),
                200 => Ok(servicesResult.Data),
                _ => StatusCode(418)
            };
        }

        [HttpGet("getfile")]
        public async Task<IActionResult> GetFileAsync([FromQuery(Name = "uuid"),Required]string uuid) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Incorrect Data!");
            }

            var servicesResult = await _fileServices.GetFileAsync(uuid);
           
            return servicesResult.StatusCode switch 
            {
                400 => BadRequest(servicesResult.ErrorDetails),
                200 => await Task.FromResult(PhysicalFile(servicesResult.Data,"file/file")),
                _ => StatusCode(418)
            };
        }
    }

}
