using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface IPostBusiness
    {
        public Task<List<PostModel>> GetPostsBySubjectIdAsync(long subjectId);

        public Task<PostModel?> GetPostByIdAsync(long id);

        public Task<PostModel?> CreatePostAsync(CreateUpdatePostRequest createPostRequest, long subjectId, long userId);

        public Task<PostModel?> UpdatePostAsync(CreateUpdatePostRequest updatePostRequest, long postId);

        public Task<PostModel?> UnactivatePostAsync(long id);
    }
}
