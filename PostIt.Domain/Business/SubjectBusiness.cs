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
        public async Task<List<SubjectModel>> GetAllSubjectsAsync()
        {
            return await _subjectManager.GetAllSubjectsAsync();
        }

        public async Task<SubjectModel?> GetSubjectByIdAsync(long id)
        {
            return await _subjectManager.GetSubjectByIdAsync(id) ?? null;
        }

        public async Task<SubjectModel?> CreateSubjectAsync(CreateSubjectRequest createSubjectRequest)
        {
            return await _subjectManager.CreateSubjectAsync(new SubjectModel
            {
                Name = createSubjectRequest.Name,
                Description = createSubjectRequest.Description,
                PostIds = new List<long>(),
                UserIds = new List<long>()
            }
            ) ?? null;
        }

        public async Task<SubUnsubSuccess?> SubscribeAsync(long subjectId, long userId)
        {
            if (await IsItOkToSubscribe(subjectId, userId) == false)
            {
                return null;
            }
            if (await _subjectManager.SubscribeAsync(subjectId, userId) == false)
            {
                return null;
            }
            return await MakeSubUnsubSuccessAsync(subjectId, userId);
        }

        public async Task<SubUnsubSuccess?> UnsubscribeAsync(long subjectId, long userId)
        {
            if (await IsItOkToUnsubscribe(subjectId, userId) == false)
            {
                return null;
            }
            if (await _subjectManager.UnsubscribeAsync(subjectId, userId) == false)
            {
                return null;
            }
            return await MakeSubUnsubSuccessAsync(subjectId, userId);
        }

        public async Task<bool> IsItOkToSubscribe(long subjectId, long userId)
        {
            SubjectModel? subjectModel = await _subjectManager.GetSubjectByIdAsync(subjectId);
            UserModel? userModel = await _userManager.GetUserByIdAsync(userId);

            if (subjectModel == null || userModel == null)
            {
                return false;
            }

            bool IsUserInSubject = subjectModel.UserIds.Contains(userId);
            bool IsSubjectInUser = userModel.SubjectIds.Contains(subjectId);
            return !IsUserInSubject && !IsSubjectInUser;
        }

        public async Task<bool> IsItOkToUnsubscribe(long subjectId, long userId)
        {
            SubjectModel? subjectModel = await _subjectManager.GetSubjectByIdAsync(subjectId);
            UserModel? userModel = await _userManager.GetUserByIdAsync(userId);

            if (subjectModel == null || userModel == null)
            {
                return false;
            }

            bool IsUserInSubject = subjectModel.UserIds.Contains(userId);
            bool IsSubjectInUser = userModel.SubjectIds.Contains(subjectId);
            return IsUserInSubject && IsSubjectInUser;
        }

        private async Task<SubUnsubSuccess?> MakeSubUnsubSuccessAsync(long subjectId, long userId)
        {
            SubjectModel? subjectModel = await _subjectManager.GetSubjectByIdAsync(subjectId);
            UserModel? userModel = await _userManager.GetUserByIdAsync(userId);
            return (userModel == null || subjectModel == null) ? null : new SubUnsubSuccess
            {
                UserId = userModel.Id,
                UserName = userModel.Name,
                SubjectId = subjectModel.Id,
                SubjectName = subjectModel.Name
            };
        }
    }
}
