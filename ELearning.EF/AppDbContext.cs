using ELearning.Bl.Models;
using ELearning.Bl.Models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.EF
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Courses> courses { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AboutModel> AboutModels { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Message> Messagess { get; set; }
        public DbSet<Videos> videos { get; set; }
    }
}
