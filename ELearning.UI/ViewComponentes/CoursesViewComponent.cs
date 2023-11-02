using ELearning.EF;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.UI.ViewComponentes
{
    public class CoursesViewComponent:ViewComponent
    {
        private AppDbContext db;
       public CoursesViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public IViewComponentResult Invoke()
        {
            return View (db.courses);
        }
    }
}
