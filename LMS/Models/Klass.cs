using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Klass
    {
        [Key]
        public int Id { get; set; }  //auto generate.

        //Teacher FK:
        //[Required]
        [MaxLength(128)]
        public string TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual ApplicationUser teacher { get; set; }


        
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string sharedFolder { get; set; }

        [MaxLength(100)]
        public string submitFolder { get; set; }

        public DateTime startDate { get; set; }

        public virtual ICollection<User> Students { get; set; }
        
    }
}