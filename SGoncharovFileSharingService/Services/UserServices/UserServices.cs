using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SGoncharovFileSharingService.JwtTokenProvider;
using SGoncharovFileSharingService.Models.DTO;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
using SGoncharovFileSharingService.Models.ResponseDto;
using SGoncharovFileSharingService.Repository.UserRepository;

namespace SGoncharovFileSharingService.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IMapper _mapper;

        public UserServices(IUserRepository userRepository,IJwtTokenProvider jwtTokenProvider, IMapper mapper) 
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _mapper = mapper;
        }

        public async Task<ApiResponse<LoginUserDto>> AddUserAsync(RegisterUserDto regUserDto) 
        {
            var passwordHasher = new PasswordHasher<UserEntity>();
            UserEntity userEntity = _mapper.Map<UserEntity>(regUserDto);
            userEntity.Password = passwordHasher.HashPassword(userEntity, userEntity.Password); 
            var repositoryResult =  await _userRepository.AddUserAsync(userEntity);
            if(repositoryResult == false) 
            {
                return new ApiResponse<LoginUserDto>
                {
                    Data = null,
                    ErrorDetails = $"User with {regUserDto.Email} already exists!",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var loginUserDto = _mapper.Map<LoginUserDto>(userEntity);
            loginUserDto.Token = _jwtTokenProvider.GetJwtToken(userEntity.Id, userEntity.Name);

            return new ApiResponse<LoginUserDto>
            {
                Data = loginUserDto,
                ErrorDetails = string.Empty,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<ApiResponse<LoginUserDto>> LoginUserAsync(AuthUserDto authUserDto)
        {
            var passHash = new PasswordHasher<UserEntity>();
            var user = await _userRepository.GetUserByEmailAsync(authUserDto.Email);
            if (user is null)
            {
                return new ApiResponse<LoginUserDto>
                {
                    Data = null,
                    ErrorDetails = $"User with {authUserDto.Email} not exists!",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var verifyResult = passHash.VerifyHashedPassword(user,user.Password,authUserDto.Password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return new ApiResponse<LoginUserDto>
                {
                    Data = null,
                    ErrorDetails = $"Invalid Password!",
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            var loginUserDto = _mapper.Map<LoginUserDto>(user);
            loginUserDto.Token = _jwtTokenProvider.GetJwtToken(user.Id, user.Name);

            return new ApiResponse<LoginUserDto>
            {
                Data = loginUserDto,
                ErrorDetails = string.Empty,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<ApiResponse<string?>> UpdateUserAsync(UserDto userDto, string id)
        {
            Guid guid;
            Guid.TryParse(id, out guid);
            var repositoryResponse = await _userRepository.UpdateUserAsync(userDto.Name, userDto.Email, guid);

            if(repositoryResponse == false)
            {
                return new ApiResponse<string?>
                {
                    Data = string.Empty,
                    ErrorDetails = $"User with {userDto.Email} exists!",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            return new ApiResponse<string?>
            {
                Data = string.Empty,
                ErrorDetails = string.Empty,
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
