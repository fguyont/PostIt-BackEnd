using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface ISubjectBusiness
    {
        public List<SubjectModel> GetAllSubjects();

        public Task<SubjectModel> CreateSubject(AddSubjectRequest addSubjectRequest);
    }
}
