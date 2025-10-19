using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.API.Controllers
{
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectBusiness _subjectBusiness;

        public SubjectController(ISubjectBusiness subjectBusiness)
        {
            _subjectBusiness = subjectBusiness;
        }

        [Authorize]
        [HttpGet]
        [Route("[controller]/all")]
        public IActionResult GetAllSubjects()
        {
            List<SubjectModel> subjects = new List<SubjectModel>();
            subjects = _subjectBusiness.GetAllSubjects();

            return Ok(subjects);
        }

        [Authorize]
        [HttpPost]
        [Route("[controller]/new")]
        public async Task<IActionResult> CreateSubject([FromBody] AddSubjectRequest addSubjectRequest)
        {
            SubjectModel subjectAdded = await _subjectBusiness.CreateSubject(addSubjectRequest);

            if (subjectAdded != null)
            {
                return Ok(subjectAdded);
            }
            return BadRequest(new { message = "Subject creation failed."});
        }
    }
}
