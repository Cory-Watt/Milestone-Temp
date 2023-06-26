using Microsoft.AspNetCore.Mvc;
using Milestone.Models;
using Milestone.Services;
using NLog;

namespace Milestone.Controllers
{
    public class RegistrationController : Controller
    {
        private static Logger logger = LogManager.GetLogger("UserRegistrationAppRule");
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProcessRegistration(UserModel user)
        {
            logger.Info("New User Registration: ");
            logger.Info(user.RegistrationToString());

            SecurityService securityService = new SecurityService();
            UsersDAO usersDAO = new UsersDAO();
            if (usersDAO.AddUser(user))
            {
                return View("RegistrationSuccess", user);
            }
            else
            {
                return View("RegistrationFailure", user);
            }
        }
    }
}
