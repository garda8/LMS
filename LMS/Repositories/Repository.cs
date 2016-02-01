//från AccountController:
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LMS.Models;


using System.Collections.Generic;
using System.Linq;
//using System.Web;
//using Microsoft.AspNet.Identity;
//using System.Web.Security;

using Microsoft.AspNet.Identity;
using System.Security.Claims;
using LMS.Controllers;

namespace LMS.Repositories
{
    public class Repository
    {
        //private ApplicationUserManager _userManager;

        
        ApplicationDbContext db = new ApplicationDbContext();

        public List<LMS.Models.Klass> getClasses() {
            List<LMS.Models.Klass> classes = new List<LMS.Models.Klass>();
            //object id = Membership.GetUser().ProviderUserKey;
            //var userID = User.Identity.GetUserId();
            string userId = ClaimsPrincipal.Current.Identity.GetUserId();
            
            //Här fråga mot databas efter klasser som är associerad till denna user.. :)
            return classes;
        }

        public string getUserId() { 
            string retval = ClaimsPrincipal.Current.Identity.GetUserId();
            return retval;
        }

        public string getUserRole()
        {
            string userId = ClaimsPrincipal.Current.Identity.GetUserId();

            string userRole = "";
            using (var context = new ApplicationDbContext())
            {
                var role  = context.Users.Where(u => u.Roles.Any(r => r.UserId == userId)).First();
                                    //.ToList();
            }

            ;            
            return userRole;
            /*var user = db.AspNetUsers
                .Where(u => u.AspNetRoles.Any(r => r.Name == "Customer") &&
                              u.IsActivated == true && u.IsClosed == false &&
                              u.IsPaused == false && u.IsSuspended == false)
                  .ToList();*/
            //string userId = ClaimsPrincipal.Current.Identity.g.GetUserId();
            
        }


        /*
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
        }*/
    }
}