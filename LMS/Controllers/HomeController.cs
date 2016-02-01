using LMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationUserManager _userManager;
        
        public ActionResult Index()
        {
            
            return View();
        }

        
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //object id = Membership.GetUser().ProviderUserKey;
            String userId = new Repository().getUserId();

            ViewBag.userId = userId;
            ViewBag.userRole = "";
            if (UserManager.IsInRole(userId, "admin"))
                ViewBag.userRole = "admin";
            else if (UserManager.IsInRole(userId, "student"))
                ViewBag.userRole = "student";
            else if (UserManager.IsInRole(userId, "teacher"))
                ViewBag.userRole = "teacher";

            //ViewBag.userId = ClaimsPrincipal.Current.Identity.GetUserId();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}