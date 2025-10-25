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
        public async Task<IActionResult> GetAllCommentsAsync(long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostByIdAsync(postId);

            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            List<CommentModel> comments = new List<CommentModel>();
            comments = await _commentBusiness.GetAllCommentsAsync(postId);

            return Ok(comments);
        }

        [Authorize]
        [HttpGet]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/{commentId}")]
        public async Task<ActionResult> GetCommentByIdAsync(long commentId, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostByIdAsync(postId);

            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            CommentModel? commentModel = await _commentBusiness.GetCommentByIdAsync(commentId);

            if (commentModel != null)
            {
                return Ok(commentModel);
            }
            return BadRequest(new { message = "Comment not found." });
        }

        [Authorize]
        [HttpPost]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/New")]
        public async Task<IActionResult> CreateCommentAsync([FromBody] CreateUpdateCommentRequest createCommentRequest, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);
            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostByIdAsync(postId);
            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            UserModel? userModel = await _userBusiness.GetUserByEmailAsync(userEmail);
            if (userModel == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            CommentModel? commentCreated = await _commentBusiness.CreateCommentAsync(createCommentRequest, postId, userModel.Id);
            return (commentCreated != null) ? Ok(commentCreated) : BadRequest(new { message = "Comment creation failed." });
        }

        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/{commentId}/Edit")]
        public async Task<IActionResult> UpdateCommentAsync([FromBody] CreateUpdateCommentRequest createCommentRequest, long commentId, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);
            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? postToUpdate = await _postBusiness.GetPostByIdAsync(postId);
            if (postToUpdate == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            CommentModel? commentToUpdate = await _commentBusiness.GetCommentByIdAsync(commentId);
            if (commentToUpdate == null)
            {
                return BadRequest(new { message = "Comment not found." });
            }

            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            UserModel? userModel = await _userBusiness.GetUserByEmailAsync(userEmail);
            if (userModel == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }
            if (commentToUpdate.UserId != userModel.Id)
            {
                return BadRequest(new { message = "User not allowed to update this comment." });
            }

            CommentModel? commentUpdated = await _commentBusiness.UpdateCommentAsync(createCommentRequest, commentId);
            return (commentUpdated != null) ? Ok(commentUpdated) : BadRequest(new { message = "Comment update failed." });
        }


        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/Post/{postId}/[Controller]/{commentId}/Remove")]
        public async Task<IActionResult> UnactivateCommentAsync(long commentId, long subjectId, long postId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);
            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? post = await _postBusiness.GetPostByIdAsync(postId);
            if (post == null)
            {
                return BadRequest(new { message = "Post not found." });
            }

            CommentModel? commentToUnactivate = await _commentBusiness.GetCommentByIdAsync(commentId);
            if (commentToUnactivate == null)
            {
                return BadRequest(new { message = "Comment not found." });
            }

            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            UserModel? userModel = await _userBusiness.GetUserByEmailAsync(userEmail);
            if (userModel == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }
            if (commentToUnactivate.UserId != userModel.Id)
            {
                return BadRequest(new { message = "User not allowed to unactivate this comment." });
            }

            CommentModel? commentUnactivated = await _commentBusiness.UnactivateCommentAsync(commentId);
            return (commentUnactivated != null) ? Ok(commentUnactivated) : BadRequest(new { message = "Comment unactivation failed." });
        }
    }
}
