using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models
{
    public class Videos
    {
        public int VideosId { get; set; }
        public int CourseId { get; set; }
        public string? VideoFileName { get; set; }
        public byte[]? VideoFileData { get; set; }
        [NotMapped]
        [Required]
        public IFormFile? VideoFile { get; set; }
        public string? Titel { get; set; }
        public string? Description { get; set; }
    }
}
