using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface IPostBusiness
    {
        public List<PostModel> GetAllPosts(long subjectId);

        public Task<PostModel> GetPostById(long id);

        public Task<PostModel?> CreatePost(CreatePostRequest createPostRequest, long subjectId, long userId);

        public Task<PostModel?> UpdatePost(CreatePostRequest createPostRequest, long postId);

        public Task<PostModel?> UnactivatePost(long id);
    }
}
