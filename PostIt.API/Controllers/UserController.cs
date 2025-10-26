using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using System.Security.Claims;

namespace PostIt.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _userBusiness = userBusiness;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<ActionResult> GetUserByIdAsync(long id)
        {
            UserModel? userModel = await _userBusiness.GetUserByIdAsync(id);
            return (userModel != null) ? Ok(userModel) : BadRequest(new { message = "User not found." });
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/Me")]
        public async Task<ActionResult> GetConnectedUserAsync()
        {
            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user email not found." });
            }
            UserModel? userModel = await _userBusiness.GetUserByEmailAsync(userEmail);
            return (userModel != null) ? Ok(userModel) : BadRequest(new { message = "Connected user not found." });
        }

        [Authorize]
        [HttpPut]
        [Route("[controller]/Me/Edit")]
        public async Task<ActionResult> UpdateConnectedUserAsync(RegisterUpdateUserRequest updateUserRequest)
        {
            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user email not found." });
            }

            UserModel? userModel = await _userBusiness.GetUserByEmailAsync(userEmail);

            if (userModel == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            if (await _userBusiness.DoesUserExistAsync(updateUserRequest) == true)
            {
                return BadRequest(new { message = "User name and/or email is already taken." });
            }

            UserModel? userUpdated = await _userBusiness.UpdateConnectedUserAsync(updateUserRequest, userModel.Id);
            return (userUpdated != null) ? Ok(userUpdated) : BadRequest(new { message = "Connected user update failed." });
        }

        [Authorize]
        [HttpPut]
        [Route("[controller]/Me/Remove")]
        public async Task<ActionResult> UnactivateConnectedUserAsync()
        {
            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return BadRequest(new { message = "Connected user email not found." });
            }

            UserModel? userModel = await _userBusiness.GetUserByEmailAsync(userEmail);
            if (userModel == null)
            {
                return BadRequest(new { message = "Connected user not found." });
            }

            UserModel? userUnactivated = await _userBusiness.UnactivateConnectedUserAsync(userModel.Id);
            return (userUnactivated != null) ? Ok(userUnactivated) : BadRequest(new { message = "Connected user unactivation failed." });
        }
    }
}
