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

        public async Task<List<SubjectModel>> GetAllSubjectsAsync()
        {
            List<Subject> subjects = await _postItDbContext.Subjects
                .Include(s => s.Posts)
                .Include(s => s.Users)
                .ToListAsync();

            List<SubjectModel> subjectModels = new List<SubjectModel>();

            foreach (Subject subject in subjects)
            {
                subjectModels.Add(FromSubjectToSubjectModel(subject));
            }
            return subjectModels;
        }

        public async Task<SubjectModel?> GetSubjectByIdAsync(long id)
        {
            Subject? subjectToGet = await _postItDbContext.Subjects
                .Include(s => s.Posts)
                .Include(s => s.Users)
                .FirstOrDefaultAsync(s => s.Id == id);

            return (subjectToGet != null) ? FromSubjectToSubjectModel(subjectToGet) : null;
        }

        public async Task<SubjectModel?> CreateSubjectAsync(SubjectModel subjectToCreate)
        {
            Subject subject = new Subject
            {
                Name = subjectToCreate.Name,
                Description = subjectToCreate.Description
            };
            _postItDbContext.Subjects.Add(subject);
            await _postItDbContext.SaveChangesAsync();

            return FromSubjectToSubjectModel(subject);
        }

        public async Task<bool> SubscribeAsync(long subjectId, long UserId)
        {
            Subject? subjectToSub = await _postItDbContext.Subjects
                .Include(s => s.Posts)
                .Include(s => s.Users)
                .FirstOrDefaultAsync(s => s.Id == subjectId);
            User? userToSub = await _postItDbContext.Users
                .Include(u => u.Subjects)
                .Include(s => s.Posts)
                .Include(s => s.Comments)
                .FirstOrDefaultAsync(u => u.Id == UserId && u.IsActive == true);

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

        public async Task<bool> UnsubscribeAsync(long subjectId, long UserId)
        {
            Subject? subjectToUnsub = await _postItDbContext.Subjects
                .Include(s => s.Posts)
                .Include(s => s.Users)
                .FirstOrDefaultAsync(s => s.Id == subjectId);
            User? userToUnSub = await _postItDbContext.Users
                .Include(u => u.Subjects)
                .Include(s => s.Posts)
                .Include(s => s.Comments)
                .FirstOrDefaultAsync(u => u.Id == UserId && u.IsActive == true);

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
        private static SubjectModel FromSubjectToSubjectModel(Subject subject)
        {
            List<long> postsFromSubjectIds = new List<long>();

            foreach (Post post in subject.Posts)
            {
                postsFromSubjectIds.Add(post.Id);
            }

            List<long> usersFromSubjectIds = new List<long>();

            foreach (User user in subject.Users)
            {
                usersFromSubjectIds.Add(user.Id);
            }

            return new SubjectModel
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description,
                PostIds = postsFromSubjectIds,
                UserIds = usersFromSubjectIds        
            };
        }
    }
}
