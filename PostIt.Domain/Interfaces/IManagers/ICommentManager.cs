using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface ICommentManager
    {
        public Task<List<CommentModel>> GetAllCommentsAsync(long postId);

        public Task<CommentModel?> GetCommentByIdAsync(long id);

        public Task<CommentModel?> CreateCommentAsync(CommentModel commentToCreate, long postId, long userId);

        public Task<CommentModel?> UpdateCommentAsync(CommentModel commentToUpdate);

        public Task<CommentModel?> UnactivateCommentAsync(long id);
    }
}
