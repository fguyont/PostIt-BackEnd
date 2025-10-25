using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface IUserManager
    {
        public Task<UserModel?> GetUserByIdAsync(long id);

        public Task<UserModel?> GetUserByEmailAsync(string email);

        public Task<UserModel?> UpdateConnectedUserAsync(UserModel userToUpdate);

        public Task<UserModel?> UnactivateConnectedUserAsync(long id);

        public Task<bool> DoesUserExistAsync(string name, string email);
    }
}
