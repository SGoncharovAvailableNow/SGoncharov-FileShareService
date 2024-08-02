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
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserServices(IUserRepository userRepository, IJwtTokenProvider jwtTokenProvider, 
        IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginUserDto> RegisterUserAsync(RegisterUserDto regUserDto, CancellationToken cancellationToken)
        {
            User userEntity = _mapper.Map<User>(regUserDto);

            userEntity.Password = _passwordHasher.HashPassword(userEntity, userEntity.Password);

            await _userRepository.AddUserAsync(userEntity, cancellationToken);

            var loginUserDto = _mapper.Map<LoginUserDto>(userEntity);

            loginUserDto.Token = _jwtTokenProvider.GetJwtToken(userEntity.UserId, userEntity.Name);

            return loginUserDto;
        }

        public async Task<LoginUserDto> LoginUserAsync(AuthUserDto authUserDto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(authUserDto.Email, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException($"User with {authUserDto.Email} not found!");
            }

            var verifyResult = _passwordHasher
            .VerifyHashedPassword(user, user.Password, authUserDto.Password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                throw new WrongPasswordException("Invalid password!");
            }

            var logDto = _mapper.Map<LoginUserDto>(user);
            logDto.Token = _jwtTokenProvider.GetJwtToken(user.UserId, user.Name);

            return logDto;

        }

        public async Task UpdateUserAsync(UserDto userDto, Guid id, CancellationToken cancellationToken)
        {

            await _userRepository.UpdateUserAsync(userDto.Name, userDto.Email, id, cancellationToken);

        }

    }
}
