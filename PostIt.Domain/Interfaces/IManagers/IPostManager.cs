using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface IPostManager
    {
        public Task<List<PostModel>> GetPostsBySubjectIdAsync(long subjectId);

        public Task<PostModel?> GetPostByIdAsync(long id);

        public Task<PostModel?> CreatePostAsync(PostModel postToCreate, long subjectId, long userId);

        public Task<PostModel?> UpdatePostAsync(PostModel postToUpdate);

        public Task<PostModel?> UnactivatePostAsync(long id);
    }
}
