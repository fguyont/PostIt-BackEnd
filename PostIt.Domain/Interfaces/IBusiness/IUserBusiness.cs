using PostIt.Domain.Models;
using PostIt.Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostIt.Domain.Interfaces.IBusiness
{
    public interface IUserBusiness
    {
        public Task<GetUserResponse?> GetUserById(long id);

        public Task<GetUserResponse?> GetUserByEmail(string email);
    }
}
