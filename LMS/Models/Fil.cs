using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Fil
    {
        [Key]
        public int Id { get; set; }  //auto generate.

        //Teacher FK:
        [Required]
        public int schemaOccasionId { get; set; }
        [ForeignKey("schemaOccasionId")]
        public virtual schemaOccasion schemaId { get; set; }

        public bool isShared { get; set; }

        [MaxLength(255)]
        public string fileName { get; set; }

        [MaxLength(255)]
        public string filePath { get; set; }

        public string owner { get; set; }

        public DateTime date { get; set; }

        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string FileType { get; set; }

        public string Grade { get; set; }
        public string TeacherFeedback { get; set; }

    }
}