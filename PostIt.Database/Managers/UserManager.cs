using Microsoft.EntityFrameworkCore;
using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Responses;

namespace PostIt.Database.Managers
{
    public class UserManager : IUserManager
    {
        private readonly PostItDbContext _postItDbContext;

        public UserManager(PostItDbContext postItDbContext)
        {
            _postItDbContext = postItDbContext;
        }

        public async Task<UserModel?> GetUserById(long id)
        {
            User? userToFind = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userToFind != null)
            {
                return new UserModel
                {
                    Id = userToFind.Id,
                    Name = userToFind.Name,
                    Email = userToFind.Email,
                    Password = userToFind.Password,
                    CreatedAt = userToFind.CreatedAt,
                    UpdatedAt = userToFind.UpdatedAt,
                    IsActive = userToFind.IsActive
                };
            }
            return null;
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            User? userToFind = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (userToFind != null)
            {
                return new UserModel
                {
                    Id = userToFind.Id,
                    Name = userToFind.Name,
                    Email = userToFind.Email,
                    Password = userToFind.Password,
                    CreatedAt = userToFind.CreatedAt,
                    UpdatedAt = userToFind.UpdatedAt,
                    IsActive = userToFind.IsActive
                };
            }
            return null;
        }

        public async Task<bool> UserExists(string name, string email)
        {
            User? userToFind = await _postItDbContext.Users.FirstOrDefaultAsync(u => (u.Name.ToLower().Equals(name.ToLower())) || (u.Email.Equals(email)));

            if (userToFind != null)
            {
                return true;
            }

            return false;
        }
    }
}
