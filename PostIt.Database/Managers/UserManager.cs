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

        public async Task<UserModel?> GetUserByIdAsync(long id)
        {
            User? userToGet = await _postItDbContext.Users
                .Include(u => u.Posts.Where(p => p.IsActive == true))
                .Include(u => u.Comments.Where(c => c.IsActive == true))
                .Include(u => u.Subjects)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive && true);

            return (userToGet != null) ? FromUserToUserModel(userToGet) : null;
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            User? userToGet = await _postItDbContext.Users
                .Include(u => u.Posts.Where(p => p.IsActive == true))
                .Include(u => u.Comments.Where(c => c.IsActive == true))
                .Include(u => u.Subjects)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive == true);

            return (userToGet != null) ? FromUserToUserModel(userToGet) : null;
        }

        public async Task<UserModel?> UpdateConnectedUserAsync(UserModel userDataToUpdate)
        {
            User? userToUpdate = await _postItDbContext.Users
                .Include(u => u.Posts.Where(p => p.IsActive == true))
                .Include(u => u.Comments.Where(c => c.IsActive == true))
                .Include(u => u.Subjects)
                .FirstOrDefaultAsync(u => u.Id == userDataToUpdate.Id && u.IsActive == true);

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

            return FromUserToUserModel(userToUpdate);
        }

        public async Task<UserModel?> UnactivateConnectedUserAsync(long id)
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

            return FromUserToUserModel(userToUnactivate);
        }

        private static UserModel FromUserToUserModel (User user)
        {
            List<long> postsFromUserIds = new List<long>();

            foreach (Post post in user.Posts)
            {
                postsFromUserIds.Add(post.Id);
            }

            List<long> commentsFromUserIds = new List<long>();

            foreach (Comment comment in user.Comments)
            {
                commentsFromUserIds.Add(comment.Id);
            }

            List<long> subjectsFromUserIds = new List<long>();

            foreach (Subject subject in user.Subjects)
            {
                subjectsFromUserIds.Add(subject.Id);
            }

            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsActive = user.IsActive,
                PostIds = postsFromUserIds,
                CommentIds = commentsFromUserIds,
                SubjectIds = subjectsFromUserIds
            };
        }

        public async Task<bool> DoesUserExistAsync(string name, string email)
        {
            User? userToFind = await _postItDbContext.Users.FirstOrDefaultAsync(u => (u.Name.ToLower().Equals(name.ToLower()) || u.Email.Equals(email)) && u.IsActive == true);

            return userToFind != null;
        }
    }
}
