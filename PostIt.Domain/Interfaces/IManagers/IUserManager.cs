using PostIt.Domain.Models;
using PostIt.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostIt.Domain.Interfaces.IManagers
{
    public interface IUserManager
    {
        public Task<UserModel?> GetUserById(long id);

        public Task<UserModel?> GetUserByEmail(string email);

        public Task<bool> UserExists(string name, string email);
    }
}
