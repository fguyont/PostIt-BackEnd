using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Business
{
    public class CommentBusiness : ICommentBusiness
    {
        private readonly ICommentManager _commentManager;
        private readonly IPostManager _postManager;
        private readonly IUserManager _userManager;

        public CommentBusiness(ICommentManager commentManager, IPostManager postManager, IUserManager userManager)
        {
            _commentManager = commentManager;
            _postManager = postManager;
            _userManager = userManager;
        }

        public async Task<List<CommentModel>> GetAllCommentsAsync(long postId)
        {
            return await _commentManager.GetAllCommentsAsync(postId);
        }

        public async Task<CommentModel?> GetCommentByIdAsync(long id)
        {
            return await _commentManager.GetCommentByIdAsync(id) ?? null;
        }

        public async Task<CommentModel?> CreateCommentAsync(CreateUpdateCommentRequest createCommentRequest, long postId, long userId)
        {
            PostModel? p = await _postManager.GetPostByIdAsync(postId);
            UserModel? u = await _userManager.GetUserByIdAsync(userId);

            if (p != null && u != null)
            {
                CommentModel commentToCreate = new CommentModel
                {
                    Text = createCommentRequest.Text,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    PostId = p.Id,
                    PostTitle = p.Title,
                    UserId = u.Id,
                    UserName = u.Name
                };
                return await _commentManager.CreateCommentAsync(commentToCreate, postId, userId);
            }
            return null;
        }

        public async Task<CommentModel?> UpdateCommentAsync(CreateUpdateCommentRequest updateCommentRequest, long commentId)
        {
            CommentModel? commentToUpdate = await GetCommentByIdAsync(commentId);
            if (commentToUpdate != null)
            {
                commentToUpdate.Text = updateCommentRequest.Text;
                commentToUpdate.UpdatedAt = DateTime.UtcNow;
                return await _commentManager.UpdateCommentAsync(commentToUpdate);
            }
            return null;
        }

        public async Task<CommentModel?> UnactivateCommentAsync(long id)
        {
            return await _commentManager.UnactivateCommentAsync(id) ?? null;
        }
    }
}
