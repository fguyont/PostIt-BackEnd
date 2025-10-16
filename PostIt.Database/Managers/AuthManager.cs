using Microsoft.EntityFrameworkCore;
using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;

namespace PostIt.Database.Managers
{
    public class AuthManager : IAuthManager
    {
        private readonly PostItDbContext _postItDbContext;

        public AuthManager(PostItDbContext postItDbContext)
        {
            _postItDbContext = postItDbContext;
        }

        public async Task<UserModel> Register(UserModel userToRegister)
        {
            var user = new User
            {
                Name = userToRegister.Name,
                Email = userToRegister.Email,
                Password = userToRegister.Password,
                CreatedAt = userToRegister.CreatedAt,
                UpdatedAt = userToRegister.UpdatedAt
            };
            _postItDbContext.Users.Add(user);
            await _postItDbContext.SaveChangesAsync();

            var userRegistered = new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return userRegistered;
        }

        public async Task<UserModel?> FindUserByEmail(string email)
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
                    UpdatedAt = userToFind.UpdatedAt
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
