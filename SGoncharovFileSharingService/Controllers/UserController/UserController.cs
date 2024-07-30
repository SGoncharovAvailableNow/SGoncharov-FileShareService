using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Services.UserServices;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SGoncharovFileSharingService.Controllers.UserController
{
    [ApiController,Route("/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync([FromBody, Required] RegisterUserDto userDto)
        {
            var servicesResponse = await _userServices.AddUserAsync(userDto);

            return servicesResponse.StatusCode switch
            {
                StatusCodes.Status400BadRequest => BadRequest(servicesResponse.ErrorDetails),
                StatusCodes.Status200OK => Ok(servicesResponse.Data),
                _ => StatusCode(418)
            };
                
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUserAsync([FromBody, Required] AuthUserDto authUserDto)
        {
            var servicesResponse = await _userServices.LoginUserAsync(authUserDto);

            return servicesResponse.StatusCode switch 
            {
                StatusCodes.Status400BadRequest => BadRequest(servicesResponse.ErrorDetails),
                StatusCodes.Status200OK => Ok(servicesResponse?.Data),
                StatusCodes.Status401Unauthorized => Unauthorized(servicesResponse?.ErrorDetails),
                _ => StatusCode(418)
            };
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserInfoAsync([Required, FromBody] UserDto userDto)
        {
            var responseUpdate = await _userServices.UpdateUserAsync(userDto, User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return responseUpdate.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(),
                StatusCodes.Status400BadRequest => BadRequest(responseUpdate.ErrorDetails),
                _ => StatusCode(418)
            };
        }
    }
}
