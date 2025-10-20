using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Requests;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Business
{
    using BCrypt.Net;
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserManager _userManager;

        public UserBusiness(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserResponse?> GetUserById(long id)
        {
            UserModel? userModel = await _userManager.GetUserById(id);

            if (userModel == null)
            {
                return null;
            }
            return new GetUserResponse
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Email = userModel.Email,
                CreatedAt = userModel.CreatedAt,
                UpdatedAt = userModel.UpdatedAt
            };
        }

        public async Task<GetUserResponse?> GetUserByEmail(string email)
        {
            UserModel? userModel = await _userManager.GetUserByEmail(email);

            if (userModel == null)
            {
                return null;
            }
            return new GetUserResponse
            {
                Id = userModel.Id,
                Name = userModel.Name,
                Email = userModel.Email,
                CreatedAt = userModel.CreatedAt,
                UpdatedAt = userModel.UpdatedAt
            };
        }

        public async Task<GetUserResponse?> UpdateConnectedUser(RegisterRequest registerRequest, long id)
        {
            if (await _userManager.UserExists(registerRequest.Name, registerRequest.Email) == true)
            {
                return null;
            }
            GetUserResponse? getUserResponse = await GetUserById(id);
            if (getUserResponse == null)
            {
                return null;
            }
            UserModel userToUpdate = new UserModel
            {
                Id = getUserResponse.Id,
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                Password = BCrypt.HashPassword(registerRequest.Password),
                CreatedAt = getUserResponse.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            UserModel? userUpdated = await _userManager.UpdateConnectedUser(userToUpdate);

            if (userUpdated == null)
            {
                return null;
            }

            return new GetUserResponse
            {
                Id = userUpdated.Id,
                Name = userUpdated.Name,
                Email = userUpdated.Email,
                CreatedAt = userUpdated.CreatedAt,
                UpdatedAt = userUpdated.UpdatedAt
            };
        }

        public async Task<GetUserResponse?> UnactivateConnectedUser(long id)
        {
            UserModel? userModel = await _userManager.UnactivateConnectedUser(id);

            if (userModel == null)
            {
                return null;
            }

            return new GetUserResponse
            {
                Id = userModel.Id,
                Name= userModel.Name,
                Email = userModel.Email,
                CreatedAt = userModel.CreatedAt,
                UpdatedAt = userModel.UpdatedAt
            };
        }
    }
}
