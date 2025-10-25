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
            List<Comment> comments = _postItDbContext.Comments.Where(c => c.PostId == postId && c.IsActive == true).Include(c => c.Post).Include(c => c.User).ToList();
            List<CommentModel> commentModels = new List<CommentModel>();

            foreach (Comment comment in comments)
            {
                commentModels.Add(new CommentModel
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    CreatedAt = comment.CreatedAt,
                    UpdatedAt = comment.UpdatedAt,
                    IsActive = comment.IsActive,
                    PostId = comment.Post.Id,
                    PostTitle = comment.Post.Title,
                    UserId = comment.User.Id,
                    UserName = comment.User.Name
                });
            }
            return await Task.FromResult(commentModels);
        }

        public async Task<CommentModel?> GetCommentByIdAsync(long id)
        {
            Comment? commentToGet = await _postItDbContext.Comments.FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);

            if (commentToGet == null)
            {
                return null;
            }

            Post? postProperty = await _postItDbContext.Posts.FirstOrDefaultAsync(p => p.Id == commentToGet.PostId && p.IsActive == true);
            User? userProperty = await _postItDbContext.Users.FirstOrDefaultAsync(u => u.Id == commentToGet.UserId && u.IsActive == true);

            if (postProperty == null || userProperty == null)
            {
                return null;
            }

            return new CommentModel
            {
                Id = commentToGet.Id,
                Text = commentToGet.Text,
                CreatedAt = commentToGet.CreatedAt,
                UpdatedAt = commentToGet.UpdatedAt,
                IsActive = commentToGet.IsActive,
                PostId = postProperty.Id,
                PostTitle = postProperty.Title,
                UserId = userProperty.Id,
                UserName = userProperty.Name
            };
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
                IsActive = true,
                PostId = postProperty.Id,
                Post = postProperty,
                UserId = userProperty.Id,
                User = userProperty
            };
            _postItDbContext.Comments.Add(commentToInsert);
            await _postItDbContext.SaveChangesAsync();

            CommentModel commentCreated = new CommentModel
            {
                Id = commentToInsert.Id,
                Text = commentToInsert.Text,
                CreatedAt = commentToInsert.CreatedAt,
                UpdatedAt = commentToInsert.UpdatedAt,
                IsActive = commentToInsert.IsActive,
                PostId = commentToInsert.PostId,
                PostTitle = commentToInsert.Post.Title,
                UserId = commentToInsert.UserId,
                UserName = commentToInsert.User.Name
            };

            return commentCreated;
        }

        public async Task<CommentModel?> UpdateCommentAsync(CommentModel commentDataToUpdate)
        {
            Comment? commentToUpdate = await _postItDbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentDataToUpdate.Id && c.IsActive == true);

            if (commentToUpdate == null)
            {
                return null;
            }

            commentToUpdate.Text = commentDataToUpdate.Text;
            commentToUpdate.UpdatedAt = commentDataToUpdate.UpdatedAt;

            _postItDbContext.Comments.Update(commentToUpdate);
            await _postItDbContext.SaveChangesAsync();

            CommentModel commentUpdated = new CommentModel
            {
                Id = commentToUpdate.Id,
                Text = commentToUpdate.Text,
                CreatedAt = commentToUpdate.CreatedAt,
                UpdatedAt = commentToUpdate.UpdatedAt,
                IsActive = commentToUpdate.IsActive,
                PostId = commentToUpdate.Post.Id,
                PostTitle = commentToUpdate.Post.Title,
                UserId = commentToUpdate.UserId,
                UserName = commentToUpdate.User.Name
            };

            return commentUpdated;
        }

        public async Task<CommentModel?> UnactivateCommentAsync(long id)
        {
            Comment? commentToUnactivate = await _postItDbContext.Comments.FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);

            if (commentToUnactivate == null)
            {
                return null;
            }

            commentToUnactivate.IsActive = false;
            commentToUnactivate.UpdatedAt = DateTime.UtcNow;

            _postItDbContext.Comments.Update(commentToUnactivate);
            await _postItDbContext.SaveChangesAsync();

            CommentModel commentUnactivated = new CommentModel
            {
                Id = commentToUnactivate.Id,
                Text = commentToUnactivate.Text,
                CreatedAt = commentToUnactivate.CreatedAt,
                UpdatedAt = commentToUnactivate.UpdatedAt,
                IsActive = commentToUnactivate.IsActive,
                PostId = commentToUnactivate.Post.Id,
                PostTitle = commentToUnactivate.Post.Title,
                UserId = commentToUnactivate.UserId,
                UserName = commentToUnactivate.User.Name
            };

            return commentUnactivated;
        }
    }
}
