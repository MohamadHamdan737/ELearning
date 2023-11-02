using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models
{
    public class Slider
    {
        public int SliderId { get; set; }
        public string? Image { get; set; }
        public string? MainTitale { get; set; }
        public string? Subject { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
