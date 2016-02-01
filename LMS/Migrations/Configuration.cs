namespace LMS.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LMS.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LMS.Models.ApplicationDbContext context)
        {

            /*context.Klasser.AddOrUpdate(
                new Klass { Name = "Kemi A-kurs", TeacherId = "fe63124e-882a-4e0a-a441-ca768bb0539d", startDate = DateTime.Now, sharedFolder="shared", submitFolder="submit" },
                new Klass { Name = "Kemi B-kurs", TeacherId = "fe63124e-882a-4e0a-a441-ca768bb0539d", startDate = DateTime.Now, sharedFolder = "shared", submitFolder = "submit" }
            );
            context.SaveChanges();
            */

            if (!context.Users.Any(u => u.UserName == "erik"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new User { UserName = "erik", Email = "erik.lovbom@studentconsulting.com" };
                manager.Create(user, "password");
            }

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
