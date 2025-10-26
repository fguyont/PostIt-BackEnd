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
        public IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public AuthBusiness(IAuthManager authManager, IUserManager userManager, IConfiguration configuration)
        {
            _authManager = authManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserLoginSuccess?> RegisterAsync(RegisterUpdateUserRequest registerUserRequest)
        {
            UserModel userToRegister = new UserModel
            {
                Name = registerUserRequest.Name,
                Email = registerUserRequest.Email,
                Password = BCrypt.HashPassword(registerUserRequest.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                PostIds = new List<long>(),
                CommentIds = new List<long>(),
                SubjectIds = new List<long>()
            };

            if (await _authManager.RegisterAsync(userToRegister) == null)
            {
                return null;
            }
      
            return await LoginAsync(new LoginRequest { Email = registerUserRequest.Email, Password = registerUserRequest.Password });
        }

        public async Task<UserLoginSuccess?> LoginAsync(LoginRequest loginRequest)
        {
            UserModel? userModel = await _userManager.GetUserByEmailAsync(loginRequest.Email);

            if (userModel == null || BCrypt.Verify(loginRequest.Password, userModel.Password) == false || userModel.IsActive == false)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userModel.Name),
                    new Claim(ClaimTypes.Email, userModel.Email)
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
                Name = userModel.Name,
                Email = userModel.Email,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
