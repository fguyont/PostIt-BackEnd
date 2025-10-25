using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface ISubjectManager
    {
        public Task<List<SubjectModel>> GetAllSubjectsAsync();

        public Task<SubjectModel?> GetSubjectByIdAsync(long id);

        public Task<SubjectModel?> CreateSubjectAsync(SubjectModel subjectToCreate);

        public Task<bool> SubscribeAsync(long subjectId, long UserId);

        public Task<bool> UnsubscribeAsync(long subjectId, long UserId);
    }
}
