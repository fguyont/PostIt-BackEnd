using Microsoft.EntityFrameworkCore;
using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;

namespace PostIt.Database.Managers
{
    public class PostManager : IPostManager
    {
        private readonly PostItDbContext _postItDbContext;

        public PostManager(PostItDbContext postItDbContext)
        {
            _postItDbContext = postItDbContext;
        }

        public List<PostModel> GetAllPosts(long subjectId)
        {
            List<Post> posts = _postItDbContext.Posts.Where(u => u.SubjectId == subjectId && u.IsActive == true).Include(p => p.Subject).Include(p => p.User).ToList();

            List<PostModel> postModels = new List<PostModel>();

            foreach (var post in posts)
            {
                Subject? subjectProperty = _postItDbContext.Subjects.FirstOrDefault(s => s.Id == post.SubjectId);
                User? userProperty = _postItDbContext.Users.FirstOrDefault(u => u.Id == post.User.Id);
                if (subjectProperty != null && userProperty != null)
                {
                    postModels.Add(new PostModel
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Text = post.Text,
                        CreatedAt = post.CreatedAt,
                        UpdatedAt = post.UpdatedAt,
                        SubjectId = post.SubjectId,
                        SubjectName = subjectProperty.Name,
                        UserId = post.UserId,
                        UserName = userProperty.Name
                    });
                }
            }
            return postModels;
        }

        public async Task<PostModel?> GetPostById(long id)
        {
            Post? postToGet = await _postItDbContext.Posts.FirstOrDefaultAsync(p => p.Id == id && p.IsActive == true);

            if (postToGet == null)
            {
                return null;
            }

            Subject? subjectProperty = await _postItDbContext.Subjects.FirstOrDefaultAsync(s => s.Id == postToGet.SubjectId);
            User? userProperty = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Id == postToGet.UserId && u.IsActive == true);

            if (subjectProperty == null || userProperty == null)
            {
                return null;
            }
            return new PostModel
            {
                Id = postToGet.Id,
                Title = postToGet.Title,
                Text = postToGet.Text,
                CreatedAt = postToGet.CreatedAt,
                UpdatedAt = postToGet.UpdatedAt,
                IsActive = postToGet.IsActive,
                SubjectId = subjectProperty.Id,
                SubjectName = subjectProperty.Name,
                UserId = userProperty.Id,
                UserName = userProperty.Name
            };
        }

        public async Task<PostModel?> CreatePost(PostModel postToCreate, long subjectId, long userId)
        {
            Subject? subjectProperty = await _postItDbContext.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            User? userProperty = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive == true);

            if (subjectProperty == null || userProperty == null)
            {
                return null;
            }

            Post postToInsert = new Post
            {
                Title = postToCreate.Title,
                Text = postToCreate.Text,
                CreatedAt = postToCreate.CreatedAt,
                UpdatedAt = postToCreate.UpdatedAt,
                IsActive = true,
                SubjectId = subjectId,
                Subject = subjectProperty,
                UserId = userId,
                User = userProperty
            };
            _postItDbContext.Posts.Add(postToInsert);
            await _postItDbContext.SaveChangesAsync();

            PostModel postCreated = new PostModel
            {
                Id = postToInsert.Id,
                Title = postToInsert.Title,
                Text = postToInsert.Text,
                CreatedAt = postToInsert.CreatedAt,
                UpdatedAt = postToInsert.UpdatedAt,
                IsActive = postToInsert.IsActive,
                SubjectId = postToInsert.SubjectId,
                SubjectName = postToInsert.Subject.Name,
                UserId = postToInsert.UserId,
                UserName = postToInsert.User.Name
            };

            return postCreated;
        }
        public async Task<PostModel?> UpdatePost(PostModel postDataToUpdate)
        {
            Post? postToUpdate = await _postItDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postDataToUpdate.Id && p.IsActive == true);

            if (postToUpdate == null)
            {
                return null;
            }

            postToUpdate.Title = postDataToUpdate.Title;
            postToUpdate.Text = postDataToUpdate.Text;
            postToUpdate.UpdatedAt = postDataToUpdate.UpdatedAt;

            _postItDbContext.Posts.Update(postToUpdate);
            await _postItDbContext.SaveChangesAsync();

            PostModel postUpdated = new PostModel
            {
                Id = postToUpdate.Id,
                Title = postToUpdate.Title,
                Text = postToUpdate.Text,
                CreatedAt = postToUpdate.CreatedAt,
                UpdatedAt = postToUpdate.UpdatedAt,
                IsActive = postToUpdate.IsActive,
                SubjectId = postDataToUpdate.SubjectId,
                SubjectName = postDataToUpdate.SubjectName,
                UserId = postDataToUpdate.UserId,
                UserName = postDataToUpdate.UserName
            };

            return postUpdated;
        }

        public async Task<PostModel?> UnactivatePost(long id)
        {
            Post? postToUnactivate = await _postItDbContext.Posts.FirstOrDefaultAsync(p => p.Id == id && p.IsActive == true);

            if (postToUnactivate == null)
            {
                return null;
            }

            List<Comment> commentsToUnactivate = _postItDbContext.Comments.Where(c => c.PostId == id && c.IsActive == true).ToList();

            foreach (Comment comment in commentsToUnactivate)
            {
                comment.IsActive = false;
            }

            postToUnactivate.IsActive = false;
            postToUnactivate.UpdatedAt = DateTime.UtcNow;

            _postItDbContext.Posts.Update(postToUnactivate);
            await _postItDbContext.SaveChangesAsync();

            PostModel postUnactivated = new PostModel
            {
                Id = postToUnactivate.Id,
                Title = postToUnactivate.Title,
                Text = postToUnactivate.Text,
                CreatedAt = postToUnactivate.CreatedAt,
                UpdatedAt = postToUnactivate.UpdatedAt,
                IsActive = postToUnactivate.IsActive,
                SubjectId = postToUnactivate.SubjectId,
                SubjectName = postToUnactivate.Subject.Name,
                UserId = postToUnactivate.UserId,
                UserName = postToUnactivate.User.Name
            };

            return postUnactivated;
        }
    }
}
