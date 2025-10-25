using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface ISubjectBusiness
    {
        public Task<List<SubjectModel>> GetAllSubjectsAsync();

        public Task<SubjectModel?> GetSubjectByIdAsync(long id);

        public Task<SubjectModel?> CreateSubjectAsync(CreateSubjectRequest createSubjectRequest);

        public Task<SubUnsubSuccess?> SubscribeAsync(long subjectId, long userId);

        public Task<SubUnsubSuccess?> UnsubscribeAsync(long subjectId, long userId);
    }
}
