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
        private readonly IUserManager _userManager;

        public PostBusiness(IPostManager postManager, ISubjectManager subjectManager, IUserManager userManager)
        {
            _postManager = postManager;
            _subjectManager = subjectManager;
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
            SubjectModel? s = await _subjectManager.GetSubjectByIdAsync(subjectId);
            UserModel? u = await _userManager.GetUserByIdAsync(userId);

            if (s != null && u != null)
            {
                PostModel postToCreate = new PostModel
                {
                    Title = createPostRequest.Title,
                    Text = createPostRequest.Text,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = false,
                    SubjectId = subjectId,
                    SubjectName = s.Name,
                    UserId = userId,
                    UserName = u.Name
                };
                return await _postManager.CreatePostAsync(postToCreate, subjectId, userId);
            }
            return null;
        }

        public async Task<PostModel?> UpdatePostAsync(CreateUpdatePostRequest createPostRequest, long postId)
        {
            PostModel? postToUpdate = await GetPostByIdAsync(postId);
            if (postToUpdate != null)
            {
                postToUpdate.Title = createPostRequest.Title;
                postToUpdate.Text = createPostRequest.Text;
                postToUpdate.UpdatedAt = DateTime.UtcNow;
                return await _postManager.UpdatePostAsync(postToUpdate);
            }
            return null;
        }

        public async Task<PostModel?> UnactivatePostAsync(long id)
        {
            return await _postManager.UnactivatePostAsync(id) ?? null;
        }
    }
}
