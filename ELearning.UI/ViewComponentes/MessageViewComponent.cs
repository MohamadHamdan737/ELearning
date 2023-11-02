using ELearning.EF;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.UI.ViewComponentes
{
    public class MessageViewComponent:ViewComponent
    {
        private AppDbContext db;
        public MessageViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public IViewComponentResult Invoke()
        {
            return View(db.Messagess);
        }
    }
}
