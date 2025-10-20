using PostIt.Domain.Models;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface IUserManager
    {
        public Task<UserModel?> GetUserById(long id);

        public Task<UserModel?> GetUserByEmail(string email);

        public Task<bool> UserExists(string name, string email);
    }
}
