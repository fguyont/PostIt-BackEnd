using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface ICommentBusiness
    {
        public Task<List<CommentModel>> GetAllCommentsAsync(long postId);

        public Task<CommentModel?> GetCommentByIdAsync(long id);

        public Task<CommentModel?> CreateCommentAsync(CreateUpdateCommentRequest createCommentRequest, long postId, long userId);

        public Task<CommentModel?> UpdateCommentAsync(CreateUpdateCommentRequest createCommentRequest, long commentId);

        public Task<CommentModel?> UnactivateCommentAsync(long id);
    }
}
        