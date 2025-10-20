using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface ISubjectManager
    {
        List<SubjectModel> GetAllSubjects();

        Task<SubjectModel?> GetSubjectById(long id);

        Task<SubjectModel> CreateSubject(SubjectModel subjectToCreate);

        Task<bool> Subscribe(long subjectId, long UserId);

        Task<bool> Unsubscribe(long subjectId, long UserId);
    }
}
