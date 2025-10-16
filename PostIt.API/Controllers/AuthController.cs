using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBusiness _authBusiness;

        public AuthController(IAuthBusiness authBusiness)
        {
            _authBusiness = authBusiness;
        }

        // POST: auth/register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Name))
            {
                return BadRequest(new { message = "Name needs to be entered" });
            }
            else if (string.IsNullOrEmpty(registerRequest.Email))
            {
                return BadRequest(new { message = "Email needs to be entered" });
            }
            else if (string.IsNullOrEmpty(registerRequest.Password))
            {
                return BadRequest(new { message = "Password needs to be entered" });
            }

            UserLoginSuccess? userRegisteredAndLogged = await _authBusiness.Register(registerRequest);

            if (userRegisteredAndLogged != null)
            {
                return Ok(userRegisteredAndLogged);
            }

            return BadRequest(new { message = "User registration unsuccessful. User name or email may already exist." });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email))
            {
                return BadRequest(new { message = "Email address needs to be entered" });
            }
            else if (string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Password needs to be entered" });
            }

            UserLoginSuccess? userLoginSuccess = await _authBusiness.Login(loginRequest);

            if (userLoginSuccess != null)
            {
                return Ok(userLoginSuccess);
            }

            return BadRequest(new { message = "User login unsuccessful" });
        }
    }
}
