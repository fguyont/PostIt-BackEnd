using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;
using System.Security.Claims;

namespace PostIt.API.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostBusiness _postBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostController(IPostBusiness postBusiness, IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _postBusiness = postBusiness;
            _userBusiness = userBusiness;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        [Route("Subject/{subjectId}/[controller]/All")]
        public IActionResult GetAllPosts(long subjectId)
        {
            List<PostModel> posts = new List<PostModel>();
            posts = _postBusiness.GetAllPosts(subjectId);

            return Ok(posts);
        }

        [Authorize]
        [HttpPost]
        [Route("Subject/{subjectId}/[controller]/New")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest createPostRequest, long subjectId)
        {
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
        [Route("Subject/{subjectId}/[controller]/{postId}/Update")]
        public async Task<IActionResult> UpdatePost([FromBody] CreatePostRequest createPostRequest, long postId)
        {
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
        [Route("Subject/{subjectId}/[controller]/{postId}/Unactivate")]
        public async Task<IActionResult> UnactivatePost(long postId)
        {
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
