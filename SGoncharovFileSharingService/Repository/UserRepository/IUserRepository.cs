using SGoncharovFileSharingService.Models.Entities.UserEntities;

namespace SGoncharovFileSharingService.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(UserEntity userEntity);
        Task<UserEntity?> GetUserByEmailAsync(string email);

        Task<UserEntity?> GetUserByIdAsync(Guid id);

        Task<bool> UpdateUserAsync(string name, string email, Guid id);
    }
}
