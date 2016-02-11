using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMS.Models;

namespace LMS.ViewModels
{
    public class StudentsTaskList
    {
        public string id { get; set; }
        public string Name { get; set; }
        public List<Fil> filer { get; set; }

        public StudentsTaskList() {
            filer = new List<Fil>(); 
        }
        
    }
}