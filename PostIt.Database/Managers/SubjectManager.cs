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
            List<Subject> subjects = await _postItDbContext.Subjects.ToListAsync();

            List<SubjectModel> subjectModels = new List<SubjectModel>();

            foreach (var subject in subjects)
            {
                List<Post> postsFromSubject = _postItDbContext.Posts.Where(p => p.Id == subject.Id && p.IsActive == true).ToList();
                List<long> postsFromSubjectIds = new List<long>();

                foreach (Post post in postsFromSubject)
                {
                    postsFromSubjectIds.Add(post.Id);
                }

                List<User> usersFromSubject = subject.Users.ToList();
                List<long> usersFromSubjectIds = new List<long>();

                foreach (User user in usersFromSubject)
                {
                    usersFromSubjectIds.Add(user.Id);
                }

                subjectModels.Add(new SubjectModel
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    Description = subject.Description,
                    PostIds = postsFromSubjectIds,
                    UserIds = usersFromSubjectIds
                });
            }
            return subjectModels;
        }

        public async Task<SubjectModel?> GetSubjectByIdAsync(long id)
        {
            Subject? subjectToGet = await _postItDbContext.Subjects.FirstOrDefaultAsync(s => s.Id == id);

            if (subjectToGet == null)
            {
                return null;
            }

            List<Post> postsFromSubject = _postItDbContext.Posts.Where(p => p.SubjectId == id && p.IsActive == true).ToList();
            List<long> postsFromSubjectIds = new List<long>();

            foreach (Post post in postsFromSubject)
            {
                postsFromSubjectIds.Add(post.Id);
            }

            List<User> usersFromSubject = subjectToGet.Users.ToList();
            List<long> usersFromSubjectIds = new List<long>();

            foreach (User user in usersFromSubject)
            {
                usersFromSubjectIds.Add(user.Id);
            }

            return new SubjectModel
            {
                Id = subjectToGet.Id,
                Name = subjectToGet.Name,
                Description = subjectToGet.Description,
                PostIds = postsFromSubjectIds,
                UserIds = usersFromSubjectIds
            };
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

            SubjectModel subjectAdded = new SubjectModel
            {
                Id = subject.Id,
                Name = subject.Name,
                Description = subject.Description
            };

            return subjectAdded;
        }

        public async Task<bool> SubscribeAsync(long subjectId, long UserId)
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

        public async Task<bool> UnsubscribeAsync(long subjectId, long UserId)
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
