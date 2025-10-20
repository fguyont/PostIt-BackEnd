using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface ISubjectBusiness
    {
        public List<SubjectModel> GetAllSubjects();

        public Task<SubjectModel> GetSubjectById(long id);

        public Task<SubjectModel> CreateSubject(CreateSubjectRequest createSubjectRequest);

        public Task<UserSubjectSuccess?> Subscribe(long subjectId, long userId);

        public Task<UserSubjectSuccess?> Unsubscribe(long subjectId, long userId);
    }
}
