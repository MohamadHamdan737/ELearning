using ELearning.EF;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.UI.ViewComponentes
{
    public class SliderViewComponent : ViewComponent
    {
        private AppDbContext db;
        public SliderViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public IViewComponentResult Invoke()
        {
            return View(db.Sliders);
        }
    }
}
