using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface IUserBusiness
    {
        public Task<UserModel?> GetUserByIdAsync(long id);

        public Task<UserModel?> GetUserByEmailAsync(string email);

        public Task<UserModel?> UpdateConnectedUserAsync(RegisterUpdateUserRequest updateUserRequest, long id);

        public Task<UserModel?> UnactivateConnectedUserAsync(long id);

        public Task<bool> DoesUserExistAsync(RegisterUpdateUserRequest registerUpdateUserRequest);
    }
}
