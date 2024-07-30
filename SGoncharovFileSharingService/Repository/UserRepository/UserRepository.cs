using Microsoft.EntityFrameworkCore;
using SGoncharovFileSharingService.FileSharingContext;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
namespace SGoncharovFileSharingService.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private FileSharingContext.FileShareContext _context;

        public UserRepository(FileSharingContext.FileShareContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User userEntity)
        {
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(string name, string email, Guid id)
        {
            await _context.Users.Where(entity => entity.UserId == id)
                .ExecuteUpdateAsync(entity => entity
                .SetProperty(property => property.Email, email)
                .SetProperty(property => property.Name, name));
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstAsync(entity => entity.UserId == id);

            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                        .FirstOrDefaultAsync(entity => entity.Email == email);

            return user;
        }
    }
}
