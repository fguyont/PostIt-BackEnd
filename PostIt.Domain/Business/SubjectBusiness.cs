using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Business
{
    public class SubjectBusiness : ISubjectBusiness
    {
        private readonly ISubjectManager _subjectManager;
        private readonly IUserManager _userManager;

        public SubjectBusiness(ISubjectManager subjectManager, IUserManager userManager)
        {
            _subjectManager = subjectManager;
            _userManager = userManager;
        }
        public List<SubjectModel> GetAllSubjects()
        {
            return _subjectManager.GetAllSubjects();
        }

        public async Task<SubjectModel?> GetSubjectById(long id)
        {
            return await _subjectManager.GetSubjectById(id) ?? null;
        }

        public async Task<SubjectModel?> CreateSubject(CreateSubjectRequest createSubjectRequest)
        {
            return await _subjectManager.CreateSubject(new SubjectModel
            {
                Name = createSubjectRequest.Name,
                Description = createSubjectRequest.Description
            }
            ) ?? null;
        }

        public async Task<UserSubjectSuccess?> Subscribe(long subjectId, long userId)
        {
            if (await _subjectManager.Subscribe(subjectId, userId) == true)
            {
                UserModel? userModel = await _userManager.GetUserById(userId);
                SubjectModel? subjectModel = await _subjectManager.GetSubjectById(subjectId);
                return (userModel == null || subjectModel == null) ? null : new UserSubjectSuccess
                {
                    UserId = userModel.Id,
                    UserName = userModel.Name,
                    SubjectId = subjectModel.Id,
                    SubjectName = subjectModel.Name
                };
            }
            return null;
        }

        public async Task<UserSubjectSuccess?> Unsubscribe(long subjectId, long userId)
        {
            if (await _subjectManager.Unsubscribe(subjectId, userId) == true)
            {
                UserModel? userModel = await _userManager.GetUserById(userId);
                SubjectModel? subjectModel = await _subjectManager.GetSubjectById(subjectId);
                return (userModel == null || subjectModel == null) ? null : new UserSubjectSuccess
                {
                    UserId = userModel.Id,
                    UserName = userModel.Name,
                    SubjectId = subjectModel.Id,
                    SubjectName = subjectModel.Name
                };
            }
            return null;
        }
    }
}
