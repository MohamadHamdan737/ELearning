using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models
{
    public class AboutModel
    {
        public int AboutModelId { get; set; }
        [Required]
        public string? MainTitale { get; set; }
        [Required]
        public string? Subject { get; set; }
    }
}
