using Microsoft.EntityFrameworkCore;
using SGoncharovFileSharingService.FileSharingContext;
using SGoncharovFileSharingService.Models.Entities.UserEntities;
namespace SGoncharovFileSharingService.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private FileShareContext _context;

        public UserRepository(FileShareContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserAsync(UserEntity userEntity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var emailCheck = await _context.UserEntities
                        .FirstAsync(entity => entity.Email == userEntity.Email);
                    if (emailCheck != null)
                    {
                        return false;
                    }
                    await _context.UserEntities.AddAsync(userEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> UpdateUserAsync(string name, string email, Guid id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var userCheckEmail = await GetUserByEmailAsync(email);
                try
                {
                    if (userCheckEmail is not null)
                    {
                        if (userCheckEmail.Id != id)
                        {
                            return false;
                        }
                        else
                        {
                            await _context.UserEntities.Where(entity => entity.Id == id)
                             .ExecuteUpdateAsync(entity => entity
                             .SetProperty(property => property.Name, name));
                        }
                    }
                    await _context.UserEntities.Where(entity => entity.Id == id)
                        .ExecuteUpdateAsync(entity => entity
                        .SetProperty(property => property.Email, email)
                        .SetProperty(property => property.Name, name));
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return true;
        }

        public async Task<UserEntity?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.UserEntities.FirstAsync(entity => entity.Id == id);
            return user;
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            var user = await _context.UserEntities
                        .FirstAsync(entity => entity.Email == email);
            return user;
        }
    }
}
