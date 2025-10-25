using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Business
{
    using BCrypt.Net;
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserManager _userManager;

        public UserBusiness(IUserManager userManager)
        {
            _userManager = userManager;
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
            return await _userManager.UnactivateConnectedUserAsync(id) ?? null;
        }

        public async Task<bool> DoesUserExistAsync(RegisterUpdateUserRequest registerUpdateUserRequest)
        {
            return await _userManager.DoesUserExistAsync(registerUpdateUserRequest.Name, registerUpdateUserRequest.Email);
        }
    }
}
