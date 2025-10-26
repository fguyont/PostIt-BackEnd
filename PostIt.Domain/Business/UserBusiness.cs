using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;

namespace PostIt.Domain.Business
{
    using BCrypt.Net;
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserManager _userManager;
        private readonly ISubjectManager _subjectManager;
        private readonly IPostManager _postManager;
        private readonly ICommentManager _commentManager;

        public UserBusiness(IUserManager userManager, ISubjectManager subjectManager, IPostManager postManager, ICommentManager commentManager)
        {
            _userManager = userManager;
            _subjectManager = subjectManager;
            _postManager = postManager;
            _commentManager = commentManager;
        }

        public async Task<UserModel?> GetUserByIdAsync(long id)
        {
            return await _userManager.GetUserByIdAsync(id) ?? null;
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return await _userManager.GetUserByEmailAsync(email) ?? null;
        }

        public async Task<UserModel?> UpdateConnectedUserAsync(RegisterUpdateUserRequest registerUpdateUserRequest, long id)
        {
            if (await DoesUserExistAsync(registerUpdateUserRequest) == true)
            {
                return null;
            }
            UserModel? userToUpdate = await GetUserByIdAsync(id);
            if (userToUpdate == null)
            {
                return null;
            }

            userToUpdate.Name = registerUpdateUserRequest.Name;
            userToUpdate.Email = registerUpdateUserRequest.Email;
            userToUpdate.Password = BCrypt.HashPassword(registerUpdateUserRequest.Password);
            userToUpdate.UpdatedAt = DateTime.UtcNow;
            UserModel? userUpdated = await _userManager.UpdateConnectedUserAsync(userToUpdate);

            return userUpdated ?? null;
        }

        public async Task<UserModel?> UnactivateConnectedUserAsync(long id)
        {
            UserModel? userToUnactivate = await GetUserByIdAsync(id);

            if (userToUnactivate == null)
            {
                return null;
            }

            foreach (long postId in userToUnactivate.PostIds)
            {
                await _postManager.UnactivatePostAsync(postId);
            }

            foreach (long commentId in userToUnactivate.CommentIds)
            {
                await _commentManager.UnactivateCommentAsync(commentId);
            }

            foreach (long subjectId in userToUnactivate.SubjectIds)
            {
                await _subjectManager.UnsubscribeAsync(subjectId, id);
            }

            return await _userManager.UnactivateConnectedUserAsync(id) ?? null;
        }

        public async Task<bool> DoesUserExistAsync(RegisterUpdateUserRequest registerUpdateUserRequest)
        {
            return await _userManager.DoesUserExistAsync(registerUpdateUserRequest.Name, registerUpdateUserRequest.Email);
        }
    }
}
