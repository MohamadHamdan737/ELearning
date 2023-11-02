using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models.ViewModels
{
    public class EditeRoleViewModel
    {
        public EditeRoleViewModel()
        {
            Users = new List<string>();
        }
        public string? RoleId { get; set; }
        [Required(ErrorMessage = "Enter Role Name")]
        [MinLength(3)]
        [MaxLength(25)]
        public string? RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}

