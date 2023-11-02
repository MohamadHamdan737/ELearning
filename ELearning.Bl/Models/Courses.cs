using ELearning.Bl.Models.SharedProp;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models
{
    public class Courses:CommonProp
    {
        public int CoursesId { get; set; }
        [Required]
        public string? CoursesName { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? InstructorName { get; set; }

        //Image course
        
        public string? ImageFileName { get; set; }
        public byte[]? FileDataImage { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? ImageFile { get; set; }
        //Certificate Image
       
		public string? CertificateFileName { get; set; }
        public byte[]? FileDataCertificate { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? CertificateFile { get; set; }
        public string? CreatedBy { get; set; }






    }
}