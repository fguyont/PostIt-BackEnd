using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface IPostManager
    {
        public List<PostModel> GetAllPosts(long subjectId);

        public Task<PostModel?> GetPostById(long id);

        public Task<PostModel?> CreatePost(PostModel postToCreate, long subjectId, long userId);

        public Task<PostModel?> UpdatePost(PostModel postToUpdate);

        public Task<PostModel?> UnactivatePost(long id);
    }
}
