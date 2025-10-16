using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface IAuthManager
    {
        public Task<UserModel> Register(UserModel userModel);

        public Task<UserModel?> FindUserByEmail(string email);

        public Task<bool> UserExists(string name, string email);
    }
}
