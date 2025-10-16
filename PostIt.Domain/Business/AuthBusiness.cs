using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using System.Text;

namespace PostIt.Domain.Business
{
    using BCrypt.Net;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using PostIt.Domain.Models.Responses;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    public class AuthBusiness : IAuthBusiness
    {
        public IAuthManager _authManager;
        private readonly IConfiguration _configuration;

        public AuthBusiness(IAuthManager authManager, IConfiguration configuration)
        {
            _authManager = authManager;
            _configuration = configuration;
        }

        public async Task<UserLoginSuccess?> Register(RegisterRequest registerRequest)
        {
            if (await _authManager.UserExists(registerRequest.Name, registerRequest.Email) == true)
            {
                return null;
            }

            UserModel userToRegister = new UserModel
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                Password = BCrypt.HashPassword(registerRequest.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (await _authManager.Register(userToRegister) == null)
            {
                return null;
            }
      
            return await Login(new LoginRequest { Email = registerRequest.Email, Password = registerRequest.Password });
        }

        public async Task<UserLoginSuccess?> Login(LoginRequest loginRequest)
        {
            UserModel? user = await _authManager.FindUserByEmail(loginRequest.Email);

            if (user == null || BCrypt.Verify(loginRequest.Password, user.Password) == false)
            {
                return null; //returning null intentionally to show that login was unsuccessful
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                IssuedAt = DateTime.UtcNow,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
   
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new UserLoginSuccess
            {
                Name = user.Name,
                Email = user.Email,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
