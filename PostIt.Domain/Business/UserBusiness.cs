using PostIt.Domain.Interfaces.IBusiness;
using PostIt.Domain.Interfaces.IManagers;
using PostIt.Domain.Models;
using PostIt.Domain.Models.Responses;

namespace PostIt.Domain.Business
{
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

            if (userModel != null)
            {
                return new GetUserResponse
                {
                    Id = userModel.Id,
                    Name = userModel.Name,
                    Email = userModel.Email,
                    CreatedAt = userModel.CreatedAt,
                    UpdatedAt = userModel.UpdatedAt
                };
            }
            return null;
        }

        public async Task<GetUserResponse?> GetUserByEmail(string email)
        {
            UserModel? userModel = await _userManager.GetUserByEmail(email);

            if (userModel != null)
            {
                return new GetUserResponse
                {
                    Id = userModel.Id,
                    Name = userModel.Name,
                    Email = userModel.Email,
                    CreatedAt = userModel.CreatedAt,
                    UpdatedAt = userModel.UpdatedAt
                };
            }
            return null;
        }
    }
}
