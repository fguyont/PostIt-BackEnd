using Microsoft.EntityFrameworkCore;
using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;

namespace PostIt.Database.Managers
{
    public class SubjectManager : ISubjectManager
    {
        private readonly PostItDbContext _postItDbContext;


        public SubjectManager(PostItDbContext postItDbContext)
        {
            _postItDbContext = postItDbContext;
        }

        public List<SubjectModel> GetAllSubjects()
        {
            List<Subject> subjects = _postItDbContext.Subjects.ToList();

            List<SubjectModel> subjectModels = new List<SubjectModel>();

            foreach (var subject in subjects)
            {
                subjectModels.Add(new SubjectModel { Id = subject.Id, Name = subject.Name, Description = subject.Description });
            }
            return subjectModels;
        }

        public async Task<SubjectModel?> GetSubjectById(long id)
        {
            Subject? subjectToGet = await _postItDbContext.Subjects.FirstOrDefaultAsync(s => s.Id == id);

            if (subjectToGet != null)
            {
                return new SubjectModel { Id = subjectToGet.Id, Name = subjectToGet.Name, Description = subjectToGet.Description };
            }
            return null;
        }

        public async Task<SubjectModel> CreateSubject(SubjectModel subjectToCreate)
        {
            Subject subject = new Subject
            {
                Name = subjectToCreate.Name,
                Description = subjectToCreate.Description
            };
            _postItDbContext.Subjects.Add(subject);
            await _postItDbContext.SaveChangesAsync();

            SubjectModel subjectAdded = new SubjectModel
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description
            };

            return subjectAdded;
        }
    }
}
