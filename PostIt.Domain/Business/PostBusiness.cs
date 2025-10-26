using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Business
{
    public class PostBusiness : IPostBusiness
    {
        private readonly IPostManager _postManager;
        private readonly ISubjectManager _subjectManager;
        private readonly ICommentManager _commentManager;
        private readonly IUserManager _userManager;

        public PostBusiness(IPostManager postManager, ISubjectManager subjectManager, ICommentManager commentManager, IUserManager userManager)
        {
            _postManager = postManager;
            _subjectManager = subjectManager;
            _commentManager = commentManager;
            _userManager = userManager;
        }

        public async Task<List<PostModel>> GetPostsBySubjectIdAsync(long subjectId)
        {
            return await _postManager.GetPostsBySubjectIdAsync(subjectId);
        }

        public async Task<PostModel?> GetPostByIdAsync(long id)
        {
            return await _postManager.GetPostByIdAsync(id) ?? null;
        }

        public async Task<PostModel?> CreatePostAsync(CreateUpdatePostRequest createPostRequest, long subjectId, long userId)
        {
            SubjectModel? subjectProperty = await _subjectManager.GetSubjectByIdAsync(subjectId);
            UserModel? userProperty = await _userManager.GetUserByIdAsync(userId);

            if (subjectProperty == null || userProperty == null)
            {
                return null;
            }
            PostModel postToCreate = new PostModel
            {
                Title = createPostRequest.Title,
                Text = createPostRequest.Text,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                SubjectId = subjectId,
                SubjectName = subjectProperty.Name,
                UserId = userId,
                UserName = userProperty.Name,
            };
            return await _postManager.CreatePostAsync(postToCreate, subjectId, userId);
        }

        public async Task<PostModel?> UpdatePostAsync(CreateUpdatePostRequest updatePostRequest, long postId)
        {
            PostModel? postToUpdate = await GetPostByIdAsync(postId);
            if (postToUpdate != null)
            {
                postToUpdate.Title = updatePostRequest.Title;
                postToUpdate.Text = updatePostRequest.Text;
                postToUpdate.UpdatedAt = DateTime.UtcNow;
                return await _postManager.UpdatePostAsync(postToUpdate);
            }
            return null;
        }

        public async Task<PostModel?> UnactivatePostAsync(long id)
        {
            PostModel? postToUnactivate = await GetPostByIdAsync(id);

            if (postToUnactivate == null)
            {
                return null;
            }

            foreach (long commentId in postToUnactivate.CommentIds)
            {
                await _commentManager.UnactivateCommentAsync(commentId);
            }

            return await _postManager.UnactivatePostAsync(id) ?? null;
        }
    }
}
