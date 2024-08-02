using System.ComponentModel.DataAnnotations;
using System.Security;
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

        public UserServices(IUserRepository userRepository, IJwtTokenProvider jwtTokenProvider, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _mapper = mapper;
        }

        public async Task<LoginUserDto> RegisterUserAsync(RegisterUserDto regUserDto)
        {
            var passwordHasher = new PasswordHasher<User>();

            User userEntity = _mapper.Map<User>(regUserDto);

            userEntity.Password = passwordHasher.HashPassword(userEntity, userEntity.Password);

            await _userRepository.AddUserAsync(userEntity);

            var loginUserDto = _mapper.Map<LoginUserDto>(userEntity);

            loginUserDto.Token = _jwtTokenProvider.GetJwtToken(userEntity.UserId, userEntity.Name);

            return loginUserDto;
        }

        public async Task<LoginUserDto> LoginUserAsync(AuthUserDto authUserDto)
        {
            var passHash = new PasswordHasher<User>();

            var user = await _userRepository.GetUserByEmailAsync(authUserDto.Email);
            
            if (user == null)
            {
                throw new UserNotFoundException($"User with {authUserDto.Email} not found!");
            }

            var verifyResult = passHash
            .VerifyHashedPassword(user, user.Password, authUserDto.Password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                throw new WrongPasswordException("Invalid password!");
            }

            


        }

        public async Task UpdateUserAsync(UserDto userDto, Guid id)
        {

            await _userRepository.UpdateUserAsync(userDto.Name, userDto.Email, id);

        }

    }
}
