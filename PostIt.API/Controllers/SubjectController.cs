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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubjectController(ISubjectBusiness subjectBusiness, IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor)
        {
            _subjectBusiness = subjectBusiness;
            _userBusiness = userBusiness;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/All")]
        public async Task<IActionResult> GetAllSubjectsAsync()
        {
            List<SubjectModel> subjects = new List<SubjectModel>();
            subjects = await _subjectBusiness.GetAllSubjectsAsync();

            return Ok(subjects);
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<ActionResult> GetPostByIdAsync(long id)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectByIdAsync(id);

            if (subject != null)
            {
                return Ok(subject); ;
            }

            return BadRequest(new { message = "Subject not found." });
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/New")]
        public async Task<IActionResult> CreateSubjectAsync([FromBody] CreateSubjectRequest createSubjectRequest)
        {
            SubjectModel? subjectCreated = await _subjectBusiness.CreateSubjectAsync(createSubjectRequest);

            if (subjectCreated != null)
            {
                return Ok(subjectCreated);
            }
            return BadRequest(new { message = "Subject creation failed." });
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/{subjectId}/Subscribe")]
        public async Task<IActionResult> SubscribeAsync(long subjectId)
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

            if (await _subjectBusiness.IsItOkToSubscribe(subjectId, userModel.Id) == false)
            {
                return BadRequest(new { message = "User is already subscribed. Subscription failed." });
            }

            SubUnsubSuccess? subSuccess = await _subjectBusiness.SubscribeAsync(subjectId, userModel.Id);
            return (subSuccess != null) ? Ok(subSuccess) : BadRequest(new { message = "Subscription failed." });
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/{subjectId}/Unsubscribe")]
        public async Task<IActionResult> UnsubscribeAsync(long subjectId)
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

            if (await _subjectBusiness.IsItOkToUnsubscribe(subjectId, userModel.Id) == false)
            {
                return BadRequest(new { message = "User is not subscribed. Unsubscription failed." });
            }

            SubUnsubSuccess? unsubSuccess = await _subjectBusiness.UnsubscribeAsync(subjectId, userModel.Id);
            return (unsubSuccess != null) ? Ok(unsubSuccess) : BadRequest(new { message = "Unsubscription failed." });
        }
    }
}
