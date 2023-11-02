using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Bl.Models.ViewModels
{
    public class RegistorViewModel
    {
        public int id { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? Mopile { set; get; }
        public string? UserName { set; get; }
        [Required(ErrorMessage = "Please Enter Email")]
        [MaxLength(40, ErrorMessage = "max length 40 char")]
        [EmailAddress(ErrorMessage = "Example: user@gmail.com")]
        public string? Email { set; get; }
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string? Password { set; get; }
        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Miss Match")]
        public string? ConfirmPassword { set; get; }


    }
}
