using SGoncharovFileSharingService.Models.Entities.UserEntities;

namespace SGoncharovFileSharingService.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User userEntity);
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByIdAsync(Guid id);

        Task<bool> UpdateUserAsync(string name, string email, Guid id);
    }
}
