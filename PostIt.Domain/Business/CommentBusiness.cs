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

        public List<CommentModel> GetAllComments(long postId)
        {
            return _commentManager.GetAllComments(postId);
        }

        public async Task<CommentModel?> GetCommentById(long id)
        {
            return await _commentManager.GetCommentById(id) ?? null;
        }

        public async Task<CommentModel?> CreateComment(CreateCommentRequest createCommentRequest, long postId, long userId)
        {
            PostModel? p = await _postManager.GetPostById(postId);
            UserModel? u = await _userManager.GetUserById(userId);

            if (p != null && u != null)
            {
                CommentModel commentToCreate = new CommentModel
                {
                    Text = createCommentRequest.Text,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = false,
                    PostId = p.Id,
                    PostTitle = p.Title,
                    UserId = u.Id,
                    UserName = u.Name
                };
                return await _commentManager.CreateComment(commentToCreate, postId, userId);
            }
            return null;
        }

        public async Task<CommentModel?> UpdateComment(CreateCommentRequest createCommentRequest, long commentId)
        {
            CommentModel? commentToUpdate = await GetCommentById(commentId);
            if (commentToUpdate != null)
            {
                commentToUpdate.Text = createCommentRequest.Text;
                commentToUpdate.UpdatedAt = DateTime.UtcNow;
                return await _commentManager.UpdateComment(commentToUpdate);
            }
            return null;
        }

        public async Task<CommentModel?> UnactivateComment(long id)
        {
            return await _commentManager.UnactivateComment(id) ?? null;
        }
    }
}
