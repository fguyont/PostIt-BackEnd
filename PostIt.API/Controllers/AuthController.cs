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
        private readonly IUserBusiness _userBusiness;

        public AuthController(IAuthBusiness authBusiness, IUserBusiness userBusiness)
        {
            _authBusiness = authBusiness;
            _userBusiness = userBusiness;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUpdateUserRequest registerUserRequest)
        {
            if (string.IsNullOrEmpty(registerUserRequest.Name))
            {
                return BadRequest(new { message = "Name needs to be entered" });
            }
            if (string.IsNullOrEmpty(registerUserRequest.Email))
            {
                return BadRequest(new { message = "Email needs to be entered" });
            }
            if (string.IsNullOrEmpty(registerUserRequest.Password))
            {
                return BadRequest(new { message = "Password needs to be entered" });
            }
            if (await _userBusiness.DoesUserExistAsync(registerUserRequest) == true)
            {
                return BadRequest(new { message = "User name and/or email is already taken." });
            }

            UserLoginSuccess? userRegisteredAndLogged = await _authBusiness.RegisterAsync(registerUserRequest);
            return (userRegisteredAndLogged != null) ? Ok(userRegisteredAndLogged) : BadRequest(new { message = "User registration unsuccessful." });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email))
            {
                return BadRequest(new { message = "Email address needs to be entered" });
            }
            if (string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Password needs to be entered" });
            }

            UserLoginSuccess? userLoginSuccess = await _authBusiness.LoginAsync(loginRequest);
            return (userLoginSuccess != null) ? Ok(userLoginSuccess) : BadRequest(new { message = "User login unsuccessful" });
        }
    }
}
