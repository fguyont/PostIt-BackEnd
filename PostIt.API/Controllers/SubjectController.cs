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
            public IActionResult GetAllSubjects()
        {
            List<SubjectModel> subjects = new List<SubjectModel>();
            subjects = _subjectBusiness.GetAllSubjects();

            return Ok(subjects);
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<ActionResult> GetPostById(long id)
        {
            SubjectModel? subject = await _subjectBusiness.GetSubjectById(id);

            if (subject != null)
            {
                return Ok(subject); ;
            }

            return BadRequest(new { message = "Subject not found." });
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/New")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest createSubjectRequest)
        {
            SubjectModel subjectCreated = await _subjectBusiness.CreateSubject(createSubjectRequest);

            if (subjectCreated != null)
            {
                return Ok(subjectCreated);
            }
            return BadRequest(new { message = "Subject creation failed."});
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/{subjectId}/Subscribe")]
        public async Task<IActionResult> Subscribe(long subjectId)
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

            UserSubjectSuccess subSuccess = await _subjectBusiness.Subscribe(subjectId, getUserResponse.Id);

            if (subSuccess != null)
            {
                return Ok(subSuccess);
            }

            return BadRequest(new { message = "Subscription failed." });
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/{subjectId}/Unsubscribe")]
        public async Task<IActionResult> Unsubscribe(long subjectId)
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

            UserSubjectSuccess unsubSuccess = await _subjectBusiness.Unsubscribe(subjectId, getUserResponse.Id);

            if (unsubSuccess != null)
            {
                return Ok(unsubSuccess);
            }

            return BadRequest(new { message = "Unsubscription failed." });
        }
    }
}
