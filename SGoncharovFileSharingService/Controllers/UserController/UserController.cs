using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SGoncharovFileSharingService.JwtTokenProvider;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
using SGoncharovFileSharingService.Models.ResponseDto;
using SGoncharovFileSharingService.Services.UserServices;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SGoncharovFileSharingService.Controllers.UserController
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        private readonly IJwtTokenProvider _jwtTokenProvider;

        public UserController(IUserServices userServices, IMapper mapper, IJwtTokenProvider jwtTokenProvider)
        {
            _userServices = userServices;
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LoginUserDto>>> RegisterUserAsync(
            [FromBody, Required] RegisterUserDto userDto, CancellationToken cancellationToken)
        {
            var logDto = await _userServices.RegisterUserAsync(userDto, cancellationToken);

            return logDto switch
            {
                null => throw new NullReferenceException(),
                _ => new ApiResponse<LoginUserDto>
                {
                    Data = logDto,
                    ErrorDetails = "",
                    StatusCode = 200
                }
            };

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LoginUserDto>>> LoginUserAsync(
            [FromBody, Required] AuthUserDto authUserDto, CancellationToken cancellationToken)
        {
            var logDto = await _userServices.LoginUserAsync(authUserDto, cancellationToken);

            if (logDto is null)
            {
                throw new NullReferenceException();
            }

            return new ApiResponse<LoginUserDto>
            {
                Data = logDto,
                ErrorDetails = string.Empty,
                StatusCode = StatusCodes.Status200OK
            };

        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserInfoAsync([Required, FromBody] UserDto userDto,
            CancellationToken cancellationToken)
        {
            await _userServices.UpdateUserAsync(userDto, ControllerExtension.GetUserId(this), cancellationToken);

            return Ok();
        }

    }
}
