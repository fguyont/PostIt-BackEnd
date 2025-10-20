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
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest createSubjectRequest)
        {
            SubjectModel subjectCreated = await _subjectBusiness.CreateSubject(createSubjectRequest);

            if (subjectCreated != null)
            {
                return Ok(subjectCreated);
            }
            return BadRequest(new { message = "Subject creation failed."});
        }
    }
}
