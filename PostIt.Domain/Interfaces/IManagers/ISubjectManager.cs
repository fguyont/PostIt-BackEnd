using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface ISubjectManager
    {
        List<SubjectModel> GetAllSubjects();

        Task<SubjectModel?> GetSubjectById(long id);

        Task<SubjectModel> CreateSubject(SubjectModel subjectToCreate);
    }
}
