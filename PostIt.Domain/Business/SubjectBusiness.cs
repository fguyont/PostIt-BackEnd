using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Business
{
    public class SubjectBusiness : ISubjectBusiness
    {
        private readonly ISubjectManager _subjectManager;

        public SubjectBusiness(ISubjectManager subjectManager)
        {
            _subjectManager = subjectManager;
        }
        public List<SubjectModel> GetAllSubjects()
        {
            return _subjectManager.GetAllSubjects();
        }

        public async Task<SubjectModel?> GetSubjectById(long id)
        {
            return await _subjectManager.GetSubjectById(id) ?? null;
        }

        public async Task<SubjectModel?> CreateSubject(CreateSubjectRequest createSubjectRequest)
        {
            return await _subjectManager.CreateSubject(new SubjectModel 
            { Name = createSubjectRequest.Name, 
                Description = createSubjectRequest.Description }
            ) ?? null;
        }
    }
}
