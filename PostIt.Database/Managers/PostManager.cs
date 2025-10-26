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

        public async Task<List<PostModel>> GetPostsBySubjectIdAsync(long subjectId)
        {
            List<Post> posts = await _postItDbContext.Posts
                .Where(u => u.SubjectId == subjectId && u.IsActive == true)
                .Include(p => p.Subject)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ToListAsync();

            List<PostModel> postModels = new List<PostModel>();

            foreach (Post post in posts)
            {
                postModels.Add(FromPostToPostModel(post));
            }
            return postModels;
        }

        public async Task<PostModel?> GetPostByIdAsync(long id)
        {
            Post? postToGet = await _postItDbContext.Posts
                .Include(p => p.Subject)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive == true);

            return (postToGet != null) ? FromPostToPostModel(postToGet) : null;
        }

        public async Task<PostModel?> CreatePostAsync(PostModel postToCreate, long subjectId, long userId)
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
                IsActive = postToCreate.IsActive,
                SubjectId = subjectId,
                Subject = subjectProperty,
                UserId = userId,
                User = userProperty
            };
            _postItDbContext.Posts.Add(postToInsert);
            await _postItDbContext.SaveChangesAsync();

            return FromPostToPostModel(postToInsert);
        }
        public async Task<PostModel?> UpdatePostAsync(PostModel postDataToUpdate)
        {
            Post? postToUpdate = await _postItDbContext.Posts
                .Include(p => p.Subject)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postDataToUpdate.Id && p.IsActive == true);

            if (postToUpdate == null)
            {
                return null;
            }

            postToUpdate.Title = postDataToUpdate.Title;
            postToUpdate.Text = postDataToUpdate.Text;
            postToUpdate.UpdatedAt = postDataToUpdate.UpdatedAt;

            _postItDbContext.Posts.Update(postToUpdate);
            await _postItDbContext.SaveChangesAsync();

            return FromPostToPostModel(postToUpdate);
        }

        public async Task<PostModel?> UnactivatePostAsync(long id)
        {
            Post? postToUnactivate = await _postItDbContext.Posts
                .Include(p => p.Subject)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive == true);

            if (postToUnactivate == null)
            {
                return null;
            }

            postToUnactivate.IsActive = false;
            postToUnactivate.UpdatedAt = DateTime.UtcNow;

            _postItDbContext.Posts.Update(postToUnactivate);
            await _postItDbContext.SaveChangesAsync();

            return FromPostToPostModel(postToUnactivate);
        }

        private static PostModel FromPostToPostModel(Post post)
        {
            List<long> commentsFromPostIds = new List<long>();

            foreach (Comment comment in post.Comments)
            {
                commentsFromPostIds.Add(comment.Id);
            }

            return new PostModel
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                IsActive = post.IsActive,
                SubjectId = post.SubjectId,
                SubjectName = post.Subject.Name,
                UserId = post.User.Id,
                UserName = post.User.Name,
                CommentIds = commentsFromPostIds
            };
        }
    }
}
