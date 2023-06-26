using Microsoft.AspNetCore.Mvc;
using Milestone.Models;
using Milestone.Services;
using NLog;

namespace Milestone.Controllers
{
    public class LoginController : Controller
    {
        private static Logger logger = LogManager.GetLogger("LoginAppRule");
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserModel userModel)
        {
            logger.Info("Login Attempt: ");
            logger.Info(userModel.LoginToString());

            SecurityService securityService = new SecurityService();

            if (securityService.IsValid(userModel))
            {
                logger.Info("Login Success");

                //remember who is logged in
                HttpContext.Session.SetString("username", userModel.UserName);
                return View("LoginSuccess", userModel);
            }
            else
            {
                logger.Info("Login Failure");

                //cancel any existing valid login
                HttpContext.Session.Remove("username");
                return View("LoginFailure", userModel);
            }
        }
    }
}
