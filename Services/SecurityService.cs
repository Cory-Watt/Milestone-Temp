using Milestone.Models;
using Milestone.Services;

namespace Milestone.Services
{
    public class SecurityService
    {
        UsersDAO usersDAO = new UsersDAO();

        public SecurityService() { }

        public bool IsAdded(UserModel user)
        {
            return usersDAO.AddUser(user);
        }

        public bool IsValid(UserModel user)
        {
            return usersDAO.FindUserByNameAndPassword(user);
        }
    }
}