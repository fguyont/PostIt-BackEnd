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
        public async Task<IActionResult> GetAllPosts(long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            List<PostModel> posts = new List<PostModel>();
            posts = _postBusiness.GetAllPosts(subjectId);

            return Ok(posts);
        }

        [Authorize]
        [HttpGet]
        [Route("Subject/{subjectId}/[controller]/{postId}")]
        public async Task<ActionResult> GetPostById(long postId, long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? postModel = await _postBusiness.GetPostById(postId);

            if (postModel != null)
            {
                return Ok(postModel);
            }
            return BadRequest(new { message = "Post not found." });
        }

        [Authorize]
        [HttpPost]
        [Route("Subject/{subjectId}/[controller]/New")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest createPostRequest, long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
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

            PostModel? postCreated = await _postBusiness.CreatePost(createPostRequest, subjectId, getUserResponse.Id);

            if (postCreated != null)
            {
                return Ok(postCreated);
            }

            return BadRequest(new { message = "Post creation failed." });
        }

        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/[controller]/{postId}/Edit")]
        public async Task<IActionResult> UpdatePost([FromBody] CreatePostRequest createPostRequest, long postId, long subjectId)
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

            if (postToUpdate.UserId != getUserResponse.Id)
            {
                return BadRequest(new { message = "User not allowed to update this post." });
            }

            PostModel? postUpdated = await _postBusiness.UpdatePost(createPostRequest, postId);

            if (postUpdated != null)
            {
                return Ok(postUpdated);
            }

            return BadRequest(new { message = "Post update failed." });
        }

        [Authorize]
        [HttpPut]
        [Route("Subject/{subjectId}/[controller]/{postId}/Remove")]
        public async Task<IActionResult> UnactivatePost(long postId, long subjectId)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(subjectId);

            if (subject == null)
            {
                return BadRequest(new { message = "Subject not found." });
            }

            PostModel? postToUnactivate = await _postBusiness.GetPostById(postId);

            if (postToUnactivate == null)
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

            if (postToUnactivate.UserId != getUserResponse.Id)
            {
                return BadRequest(new { message = "User not allowed to unactivate this post." });
            }

            PostModel? postUnactivated = await _postBusiness.UnactivatePost(postId);

            if (postUnactivated != null)
            {
                return Ok(postUnactivated);
            }

            return BadRequest(new { message = "Post unactivation failed." });
        }
    }
}
