﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class schemaOccasion
    {
        [Key]
        public int Id { get; set; }  //auto generate.

        //Klass FK:
        [Required]
        public int KlassId { get; set; }
        [ForeignKey("KlassId")]
        public virtual Klass klass { get; set; }


        public string name_for_schemaoccasion { get; set; }
        public string description { get; set; }
        public string path_to_inlamningsuppgift { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}