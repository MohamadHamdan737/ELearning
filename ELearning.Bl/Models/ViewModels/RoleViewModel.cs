using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string? RoleName { get; set; }
    }
}
