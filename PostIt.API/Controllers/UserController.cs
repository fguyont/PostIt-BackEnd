using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Models.Responses;
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
        public async Task<ActionResult> GetUserById(long id)
        {
            GetUserResponse? getUserResponse = await _userBusiness.GetUserById(id);

            if (getUserResponse != null)
            {
                return Ok(getUserResponse);
            }
            return BadRequest(new { message = "User not found." });
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/Me")]
        public async Task<ActionResult> GetConnectedUser()
        {
            string? userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (userEmail != null)
            {
                GetUserResponse? getUserResponse = await _userBusiness.GetUserByEmail(userEmail);
                if (getUserResponse != null)
                {
                    return Ok(getUserResponse);
                }
                return BadRequest(new { message = "Connected user not found." });
            }
            return BadRequest(new { message = "Connected user not found." });
        }
    }
}
