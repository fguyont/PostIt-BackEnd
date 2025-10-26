using Microsoft.EntityFrameworkCore;
using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;

namespace PostIt.Database.Managers
{
    public class CommentManager : ICommentManager
    {
        private readonly PostItDbContext _postItDbContext;

        public CommentManager(PostItDbContext postItDbContext)
        {
            _postItDbContext = postItDbContext;
        }

        public async Task<List<CommentModel>> GetAllCommentsAsync(long postId)
        {
            List<Comment> comments = await _postItDbContext.Comments
                .Where(c => c.PostId == postId && c.IsActive == true)
                .Include(c => c.Post)
                .Include(c => c.User)
                .ToListAsync();
            List<CommentModel> commentModels = new List<CommentModel>();

            foreach (Comment comment in comments)
            {
                commentModels.Add(FromCommentToCommentModel(comment));
            }
            return commentModels;
        }

        public async Task<CommentModel?> GetCommentByIdAsync(long id)
        {
            Comment? commentToGet = await _postItDbContext.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);

            return (commentToGet != null) ? FromCommentToCommentModel(commentToGet) : null;
        }

        public async Task<CommentModel?> CreateCommentAsync(CommentModel commentToCreate, long postId, long userId)
        {
            Post? postProperty = await _postItDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.IsActive == true);
            User? userProperty = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive == true);

            if (postProperty == null || userProperty == null)
            {
                return null;
            }

            Comment commentToInsert = new Comment
            {
                Text = commentToCreate.Text,
                CreatedAt = commentToCreate.CreatedAt,
                UpdatedAt = commentToCreate.UpdatedAt,
                IsActive = commentToCreate.IsActive,
                PostId = postProperty.Id,
                Post = postProperty,
                UserId = userProperty.Id,
                User = userProperty
            };
            _postItDbContext.Comments.Add(commentToInsert);
            await _postItDbContext.SaveChangesAsync();

            return FromCommentToCommentModel(commentToInsert);
        }

        public async Task<CommentModel?> UpdateCommentAsync(CommentModel commentDataToUpdate)
        {
            Comment? commentToUpdate = await _postItDbContext.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentDataToUpdate.Id && c.IsActive == true);

            if (commentToUpdate == null)
            {
                return null;
            }

            commentToUpdate.Text = commentDataToUpdate.Text;
            commentToUpdate.UpdatedAt = commentDataToUpdate.UpdatedAt;

            _postItDbContext.Comments.Update(commentToUpdate);
            await _postItDbContext.SaveChangesAsync();

            return FromCommentToCommentModel(commentToUpdate);
        }

        public async Task<CommentModel?> UnactivateCommentAsync(long id)
        {
            Comment? commentToUnactivate = await _postItDbContext.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);

            if (commentToUnactivate == null)
            {
                return null;
            }

            commentToUnactivate.IsActive = false;
            commentToUnactivate.UpdatedAt = DateTime.UtcNow;

            _postItDbContext.Comments.Update(commentToUnactivate);
            await _postItDbContext.SaveChangesAsync();

            return FromCommentToCommentModel(commentToUnactivate);
        }

        private static CommentModel FromCommentToCommentModel(Comment comment)
        {
            return new CommentModel
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                IsActive = comment.IsActive,
                PostId = comment.Post.Id,
                PostTitle = comment.Post.Title,
                UserId = comment.UserId,
                UserName = comment.User.Name
            };
        }
    }
}
