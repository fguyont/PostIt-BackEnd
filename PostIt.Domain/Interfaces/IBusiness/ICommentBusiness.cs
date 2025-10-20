using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface ICommentBusiness
    {
        public List<CommentModel> GetAllComments(long postId);

        public Task<CommentModel> GetCommentById(long id);

        public Task<CommentModel> CreateComment(CreateCommentRequest createCommentRequest, long postId, long userId);

        public Task<CommentModel?> UpdateComment(CreateCommentRequest createCommentRequest, long commentId);

        public Task<CommentModel?> UnactivateComment(long id);
    }
}
        