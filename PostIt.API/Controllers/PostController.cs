using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PostIt.API.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostBusiness _postBusiness;
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostController(IPostBusiness postBusiness, ISubjectBusiness subjectBusiness, IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _postBusiness = postBusiness;
            _subjectBusiness = subjectBusiness;
            _userBusiness = userBusiness;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        [Route("Subject/{subjectId}/[controller]/All")]
        public async Task<IActionResult> GetPostsBySubjectIdAsync(long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            List<PostModel> posts = new List<PostModel>();
            posts = await _postBusiness.GetPostsBySubjectIdAsync(subjectId);

            return Ok(posts);
        }

        [Authorize]
        [HttpGet]
        [Route("Subject/{subjectId}/[controller]/{postId}")]
        public async Task<ActionResult> GetPostByIdAsync(long postId, long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? postModel = await _postBusiness.GetPostByIdAsync(postId);

            if (postModel != null)
            {
                return Ok(postModel);
            }
            return BadRequest(new { message = "Post not found." });
        }

        [Authorize]
        [HttpPost]
        [Route("Subject/{subjectId}/[controller]/New")]
        public async Task<IActionResult> CreatePostAsync([FromBody] CreateUpdatePostRequest createPostRequest, long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);
            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
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

            PostModel? postCreated = await _postBusiness.CreatePostAsync(createPostRequest, subjectId, userModel.Id);
            return (postCreated != null) ? Ok(postCreated) : BadRequest(new { message = "Post creation failed." });
        }

        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/[controller]/{postId}/Edit")]
        public async Task<IActionResult> UpdatePostAsync([FromBody] CreateUpdatePostRequest createPostRequest, long postId, long subjectId)
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
            if (postToUpdate.UserId != userModel.Id)
            {
                return BadRequest(new { message = "User not allowed to update this post." });
            }

            PostModel? postUpdated = await _postBusiness.UpdatePostAsync(createPostRequest, postId);
            return (postUpdated != null) ? Ok(postUpdated) : BadRequest(new { message = "Post update failed." });
        }

        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/[controller]/{postId}/Remove")]
        public async Task<IActionResult> UnactivatePostAsync(long postId, long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(subjectId);
            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? postToUnactivate = await _postBusiness.GetPostByIdAsync(postId);
            if (postToUnactivate == null)
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
            if (postToUnactivate.UserId != userModel.Id)
            {
                return BadRequest(new { message = "User not allowed to unactivate this post." });
            }

            PostModel? postUnactivated = await _postBusiness.UnactivatePostAsync(postId);
            return (postUnactivated != null) ? Ok(postUnactivated) : BadRequest(new { message = "Post unactivation failed." });
        }
    }
}
