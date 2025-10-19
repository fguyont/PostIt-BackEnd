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
                UpdatedAt = userToRegister.UpdatedAt,
                IsActive = true
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
                UpdatedAt = user.UpdatedAt,
                IsActive = user.IsActive
            };

            return userRegistered;
        }
    }
}
