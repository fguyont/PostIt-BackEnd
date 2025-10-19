using PostIt.Database.EntityModels;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<SubjectModel> CreateSubject(SubjectModel subjectToAdd)
        {
            Subject subject = new Subject
            {
                Name = subjectToAdd.Name,
                Description = subjectToAdd.Description
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
