using SGoncharovFileSharingService.Models.Entities.UserEntities;

namespace SGoncharovFileSharingService.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(User userEntity, CancellationToken cancellationToken);

        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        Task UpdateUserAsync(string name, string email, Guid id, CancellationToken cancellationToken);
    }
}
