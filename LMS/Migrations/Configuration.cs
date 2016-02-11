namespace LMS.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LMS.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;


    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        bool AddUserAndRole(LMS.Models.ApplicationDbContext context)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //ir = rm.Create(new IdentityRole("canEdit"));
            
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            var user = new User { Name = "Rolf",  UserName = "rolf", Email = "rolf@gmail.com" };
            
            ir = um.Create(user, "password");
            
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "student");
            return ir.Succeeded;
        }
        
        protected override void Seed(LMS.Models.ApplicationDbContext context)
        {

            /*
            context.Klasser.AddOrUpdate(
                new Klass { Name = "Kemi A-kurs", startDate = DateTime.Now, sharedFolder="shared", submitFolder="submit" },
                new Klass { Name = "Kemi B-kurs", startDate = DateTime.Now, sharedFolder = "shared", submitFolder = "submit" }
            );
            context.SaveChanges();
            */

            IdentityResult ir;
            //var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //ir = rm.Create(new IdentityRole("canEdit"));

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            /*
            var user = new User { Name = "Erik", UserName = "erik", Email = "lovbom@gmail.com" };
            ir = um.Create(user, "password");
            if (ir.Succeeded == true)
                ir = um.AddToRole(user.Id, "admin");

            user = new User { Name = "Rolf", UserName = "rolf", Email = "rolf@gmail.com" };
            ir = um.Create(user, "password");
            if (ir.Succeeded == true)
                ir = um.AddToRole(user.Id, "teacher");

            user = new User { Name = "Anna", UserName = "anna", Email = "anna@gmail.com" };
            ir = um.Create(user, "password");
            if (ir.Succeeded == true)
                ir = um.AddToRole(user.Id, "teacher");

            user = new User { Name = "Lisa", UserName = "lisa", Email = "lisa@gmail.com" };
            ir = um.Create(user, "password");
            if (ir.Succeeded == true)
                ir = um.AddToRole(user.Id, "student");
            */

            Klass myklass = context.Klasser.Find(1); //Kemi A-kurs..

            /*var user = new User { Name = "Lisbet", UserName = "lisbet", Email = "lisbet@gmail.com"};
            ir = um.Create(user, "password");
            if (ir.Succeeded == true)
                ir = um.AddToRole(user.Id, "student");*/
            
            User myuser = context.Users.Find("93537f68-4678-4071-9761-b161274d79ab");  //lars
            if ((myklass != null) && (myuser != null))
            { 
                myklass.Students.Add(myuser);
                myuser.Klasser.Add(myklass);
            }
            

            myuser = context.Users.Find("dd258d0a-e4c7-4406-967e-9a12d967c07d");  //leif
            if ((myklass != null) && (myuser != null))
            { 
                myklass.Students.Add(myuser);
                myuser.Klasser.Add(myklass);
            }
            context.SaveChanges(); 

            /*var user = new User { Name = "Leif", UserName = "leif", Email = "leif@gmail.com" };
            ir = um.Create(user, "password");
            if (ir.Succeeded == true)
                ir = um.AddToRole(user.Id, "student");

            user = new User { Name = "Lars", UserName = "lars", Email = "lars@gmail.com" };
            ir = um.Create(user, "password");
            if (ir.Succeeded == true)
                ir = um.AddToRole(user.Id, "student");
            */
           
            //AddUserAndRole(context);
            
            /*
            if (!context.Users.Any(u => u.UserName == "anna"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new User { UserName = "anna", Email = "anna@gmail.com" };
                manager.Create(user, "password");
            }*/



            /*if (!context.Users.Any(u => u.UserName == "erik.lovbom"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "erik.lovbom", Email = "lovbom@gmail.com" };
                manager.Create(user, "password");
            }*/
            
            /*context.Klasser.AddOrUpdate(
                new Klass { Name = "Kemi A-kurs", TeacherId = "547c6b31-a5c6-4579-9d70-b3e170529aa5", startDate = DateTime.Now },
                new Klass { Name = "Kemi B-kurs", TeacherId = "547c6b31-a5c6-4579-9d70-b3e170529aa5", startDate = DateTime.Now }
            );
            context.SaveChanges();

            if (!context.Users.Any(u => u.UserName == "anna.andersson"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "anna.andersson", Email = "anna.andersson@gmail.com" };
                manager.Create(user, "password");



            }*/


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
