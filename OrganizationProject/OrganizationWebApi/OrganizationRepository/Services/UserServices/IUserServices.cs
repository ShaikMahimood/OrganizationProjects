using OrganizationRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationRepository.Services.UserServices
{
    public interface IUserServices
    {
        bool UserLogin(Credentials login);
        bool AddNewUser(UserRegistration user);
        IEnumerable<UserData> GetUsers();

        IEnumerable<UserData> GetUserByUsernameOrEmailId(string UsernameOrEmailId);

        bool UpdateUser(UserRegistration user);
        bool DeleteUserByEmailId(string EmailId);

    }
}
