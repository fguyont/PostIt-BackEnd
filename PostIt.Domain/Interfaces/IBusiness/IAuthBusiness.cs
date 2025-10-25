using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface IAuthBusiness
    {
        public Task<UserLoginSuccess?> RegisterAsync(RegisterUpdateUserRequest registerUserRequest);

        public Task<UserLoginSuccess?> LoginAsync(LoginRequest loginRequest);
    }
}
