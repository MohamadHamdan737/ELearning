using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int CourseId { get; set; }


         public string? CoursesName { get; set; }
        public int Hours { get; set; }
        public string? Description { get; set; }
        public string? InstructorName { get; set; }


        public string? ImageFileName { get; set; }
        public byte[]? FileDataImage { get; set; }
      


        public bool IsDeleted { get; set; }
      

    }
}
