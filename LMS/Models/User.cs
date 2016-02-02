using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class User : ApplicationUser
    {
        
        public String Name { get; set; }

        public virtual ICollection<Klass> Klasser { get; set; }
    }
}