using SGoncharovFileSharingService.Models.Entities.UserEntities;

namespace SGoncharovFileSharingService.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(User userEntity);

        Task<User?> GetUserByEmailAsync(string email);

        Task UpdateUserAsync(string name, string email, Guid id);
    }
}
