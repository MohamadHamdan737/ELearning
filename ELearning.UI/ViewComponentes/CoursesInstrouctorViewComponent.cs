using ELearning.EF;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.UI.ViewComponentes
{
    public class CoursesInstrouctorViewComponent:ViewComponent
    {
      public  AppDbContext db;
       public CoursesInstrouctorViewComponent(AppDbContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke(int id)
        {
            var userid = db.Instructors.Find(id);
            var course = db.courses
            .Where(fi => fi.InstructorName == userid!.InstructorName)
            .ToList();
            return View(course);
        }
    }
}
