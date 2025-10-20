using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Domain.Business;
using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;
using System.Security.Claims;

namespace PostIt.API.Controllers
{
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentBusiness _commentBusiness;
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IPostBusiness _postBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentController(ICommentBusiness commentBusiness, ISubjectBusiness subjectBusiness, IPostBusiness postBusiness, IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _commentBusiness = commentBusiness;
            _subjectBusiness = subjectBusiness;
            _postBusiness = postBusiness;
            _userBusiness = userBusiness;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/All")]
        public async Task<IActionResult> GetAllComments(long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostById(postId);

            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            List<CommentModel> comments = new List<CommentModel>();
            comments = _commentBusiness.GetAllComments(postId);

            return Ok(comments);
        }

        [Authorize]
        [HttpGet]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/{commentId}")]
        public async Task<ActionResult> GetCommentById(long commentId, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostById(postId);

            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            CommentModel? commentModel = await _commentBusiness.GetCommentById(commentId);

            if (commentModel != null)
            {
                return Ok(commentModel);
            }
            return BadRequest(new { message = "Comment not found." });
        }

        [Authorize]
        [HttpPost]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/New")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest createCommentRequest, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostById(postId);

            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            GetUserResponse? getUserResponse = await _userBusiness.GetUserByEmail(userEmail);

            if (getUserResponse == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            CommentModel? commentCreated = await _commentBusiness.CreateComment(createCommentRequest, postId, getUserResponse.Id);

            if (commentCreated != null)
            {
                return Ok(commentCreated);
            }

            return BadRequest(new { message = "Comment creation failed." });
        }

        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/{commentId}/Edit")]
        public async Task<IActionResult> UpdateComment([FromBody] CreateCommentRequest createCommentRequest, long commentId, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? postToUpdate = await _postBusiness.GetPostById(postId);

            if (postToUpdate == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            CommentModel? commentToUpdate = await _commentBusiness.GetCommentById(commentId);

            if (commentToUpdate == null)
            {
                return BadRequest(new { message = "Comment not found." });
            }

            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            GetUserResponse? getUserResponse = await _userBusiness.GetUserByEmail(userEmail);

            if (getUserResponse == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            if (commentToUpdate.UserId != getUserResponse.Id)
            {
                return BadRequest(new { message = "User not allowed to update this comment." });
            }

            CommentModel? commentUpdated = await _commentBusiness.UpdateComment(createCommentRequest, commentId);

            if (commentUpdated != null)
            {
                return Ok(commentUpdated);
            }

            return BadRequest(new { message = "Comment update failed." });
        }


        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/{commentId}/Remove")]
        public async Task<IActionResult> UnactivateComment(long commentId, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostById(postId);

            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            CommentModel? commentToUnactivate = await _commentBusiness.GetCommentById(commentId);

            if (commentToUnactivate == null)
            {
                return BadRequest(new { message = "Comment not found." });
            }

            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            GetUserResponse? getUserResponse = await _userBusiness.GetUserByEmail(userEmail);

            if (getUserResponse == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            if (commentToUnactivate.UserId != getUserResponse.Id)
            {
                return BadRequest(new { message = "User not allowed to unactivate this comment." });
            }

            CommentModel? commentUnactivated = await _commentBusiness.UnactivateComment(commentId);

            if (commentUnactivated != null)
            {
                return Ok(commentUnactivated);
            }

            return BadRequest(new { message = "Comment unactivation failed." });
        }
    }
}
