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

            if (subjectToGet == null)
            {
                return null;
            }
            return new SubjectModel { Id = subjectToGet.Id, Name = subjectToGet.Name, Description = subjectToGet.Description };
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

        public async Task<bool> Subscribe(long subjectId, long UserId)
        {
            Subject? subjectToSub = await _postItDbContext.Subjects.Include(s => s.Users).FirstOrDefaultAsync(s => s.Id == subjectId);
            User? userToSub = await _postItDbContext.Users.Include(u => u.Subjects).FirstOrDefaultAsync(u => u.Id == UserId);

            if (subjectToSub == null || userToSub == null)
            {
                return false;
            }

            User? userAlreadySubbed = subjectToSub.Users.FirstOrDefault(u => u.Id == UserId);
            if (userAlreadySubbed != null)
            {
                return false;
            }

            subjectToSub.Users.Add(userToSub);
            await _postItDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Unsubscribe(long subjectId, long UserId)
        {
            Subject? subjectToUnsub = await _postItDbContext.Subjects.Include(s => s.Users).FirstOrDefaultAsync(s => s.Id == subjectId);
            User? userToUnSub = await _postItDbContext.Users.Include(u => u.Subjects).FirstOrDefaultAsync(u => u.Id == UserId);

            if (subjectToUnsub == null || userToUnSub == null)
            {
                return false;
            }

            User? userAlreadyUnsubbed = subjectToUnsub.Users.FirstOrDefault(u => u.Id == UserId);
            if (userAlreadyUnsubbed == null)
            {
                return false;
            }

            subjectToUnsub.Users.Remove(userToUnSub);
            await _postItDbContext.SaveChangesAsync();
            return true;
        }
    }
}
