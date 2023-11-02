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
    public class Instructor:CommonProp
    {
        public int InstructorId { get; set; }
        [Required]
        public string? InstructorName { get; set; }
        [Required]
        public string? City { get; set; }
        [EmailAddress]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        public string? Majoring { get; set; }
        
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? Images { get; set; }
       
        [NotMapped]
        public IFormFile? File { get; set; }
		public string? CreatedBy { get; set; }

	}
}
