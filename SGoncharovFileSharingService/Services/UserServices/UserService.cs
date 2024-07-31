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

        public async Task<User> LoginUserAsync(AuthUserDto authUserDto)
        {
            return await _userRepository.GetUserByEmailAsync(authUserDto.Email);
        }

        public async Task UpdateUserAsync(UserDto userDto, Guid id)
        {
            
            await _userRepository.UpdateUserAsync(userDto.Name, userDto.Email, id);

        }

    }
}
