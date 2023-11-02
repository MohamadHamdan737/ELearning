using ELearning.EF;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.UI.ViewComponentes
{
    public class InstructorViewComponent:ViewComponent
    {
        private AppDbContext db;
        public InstructorViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public IViewComponentResult Invoke(string term)
        {

           return View(db.Instructors);
        }
    }
}
