using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface IAuthBusiness
    {
        public Task<UserLoginSuccess?> Register(RegisterRequest registerRequest);

        public Task<UserLoginSuccess?> Login(LoginRequest loginRequest);
    }
}
