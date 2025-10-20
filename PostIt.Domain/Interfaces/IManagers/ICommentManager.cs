using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface ICommentManager
    {
        public List<CommentModel> GetAllComments(long postId);

        public Task<CommentModel?> GetCommentById(long id);

        public Task<CommentModel?> CreateComment(CommentModel commentToCreate, long postId, long userId);

        public Task<CommentModel?> UpdateComment(CommentModel commentToUpdate);

        public Task<CommentModel?> UnactivateComment(long id);
    }
}
