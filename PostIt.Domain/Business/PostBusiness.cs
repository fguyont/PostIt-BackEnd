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

        public List<PostModel> GetAllPosts(long subjectId)
        {
            return _postManager.GetAllPosts(subjectId);
        }

        public async Task<PostModel?> GetPostById(long id)
        {
            return await _postManager.GetPostById(id) ?? null;
        }

        public async Task<PostModel?> CreatePost(CreatePostRequest createPostRequest, long subjectId, long userId)
        {
            SubjectModel? s = await _subjectManager.GetSubjectById(subjectId);
            UserModel? u = await _userManager.GetUserById(userId);

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
                return await _postManager.CreatePost(postToCreate, subjectId, userId);
            }
            return null;
        }

        public async Task<PostModel?> UpdatePost(CreatePostRequest createPostRequest, long postId)
        {
            PostModel? postToUpdate = await GetPostById(postId);
            if (postToUpdate != null)
            {
                postToUpdate.Title = createPostRequest.Title;
                postToUpdate.Text = createPostRequest.Text;
                postToUpdate.UpdatedAt = DateTime.UtcNow;
                return await _postManager.UpdatePost(postToUpdate);
            }
            return null;
        }

        public async Task<PostModel?> UnactivatePost(long id)
        {
            return await _postManager.UnactivatePost(id) ?? null;
        }
    }
}
