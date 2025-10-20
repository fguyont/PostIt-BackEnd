using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface IAuthManager
    {
        public Task<UserModel> Register(UserModel userToRegister);
    }
}
