using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface IUserBusiness
    {
        public Task<GetUserResponse?> GetUserById(long id);

        public Task<GetUserResponse?> GetUserByEmail(string email);
    }
}
