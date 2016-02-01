using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LMS.DataAccessLayer
{
    public class LMSContext: IdentityDbContext
    {
        public LMSContext() : base("DefaultConnection")  { }

        public DbSet<User> Users {get; set;}
        public DbSet<Klass> Klasser {get; set;}
        public DbSet<schemaOccasion> occasions {get; set;}

    }
}