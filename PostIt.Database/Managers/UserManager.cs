using Microsoft.EntityFrameworkCore;
using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;

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
            User? userToFind = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsActive && true);

            if (userToFind == null)
            {
                return null;
            }

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

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            User? userToFind = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive == true);

            if (userToFind == null)
            {
                return null;
            }

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

        public async Task<UserModel?> UpdateConnectedUser(UserModel userDataToUpdate)
        {
            User? userToUpdate = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Id == userDataToUpdate.Id && u.IsActive == true);

            if (userToUpdate == null)
            {
                return null;
            }

            userToUpdate.Name = userDataToUpdate.Name;
            userToUpdate.Email = userDataToUpdate.Email;
            userToUpdate.Password = userDataToUpdate.Password;
            userToUpdate.UpdatedAt = userDataToUpdate.UpdatedAt;

            _postItDbContext.Users.Update(userToUpdate);
            await _postItDbContext.SaveChangesAsync();

            UserModel userUpdated = new UserModel
            {
                Id = userToUpdate.Id,
                Name = userToUpdate.Name,
                Email = userToUpdate.Email,
                Password = userToUpdate.Password,
                CreatedAt = userToUpdate.CreatedAt,
                UpdatedAt = userToUpdate.UpdatedAt,
                IsActive = userToUpdate.IsActive
            };

            return userUpdated;
        }

        public async Task<UserModel?> UnactivateConnectedUser(long id)
        {
            User? userToUnactivate = await _postItDbContext.Users.Include(u => u.Subjects).FirstOrDefaultAsync(u => u.Id == id && u.IsActive == true);

            if (userToUnactivate == null)
            {
                return null;
            }

            userToUnactivate.Subjects.Clear();

            List<Post> postsToUnactivate = _postItDbContext.Posts.Where(p => p.UserId == id && p.IsActive == true).ToList();

            foreach (Post post in postsToUnactivate)
            {
                post.IsActive = false;
            }

            List<Comment> commentsToUnactivate = _postItDbContext.Comments.Where(c => c.PostId == id && c.IsActive == true).ToList();

            foreach (Comment comment in commentsToUnactivate)
            {
                comment.IsActive = false;
            }

            userToUnactivate.IsActive = false;
            userToUnactivate.UpdatedAt = DateTime.UtcNow;

            _postItDbContext.Users.Update(userToUnactivate);
            await _postItDbContext.SaveChangesAsync();

            UserModel userUnactivated = new UserModel
            {
                Id = userToUnactivate.Id,
                Name = userToUnactivate.Name,
                Email = userToUnactivate.Email,
                Password = userToUnactivate.Password,
                CreatedAt = userToUnactivate.CreatedAt,
                UpdatedAt = userToUnactivate.UpdatedAt,
                IsActive = userToUnactivate.IsActive
            };

            return userUnactivated;
        }

        public async Task<bool> UserExists(string name, string email)
        {
            User? userToFind = await _postItDbContext.Users.FirstOrDefaultAsync(u => (u.Name.ToLower().Equals(name.ToLower()) || u.Email.Equals(email)) && u.IsActive == true);

            return userToFind != null;
        }
    }
}
