using ELearning.Bl.IRepositories;
using ELearning.Bl.Models;
using ELearning.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.UI.Controllers
{
    public class AboutController : Controller
    {
        private AppDbContext db;
        public IBaseRepository<AboutModel> AboutRepository;
        public AboutController(AppDbContext _db, IBaseRepository<AboutModel> _AboutRepository)
        {
            db = _db;
            AboutRepository = _AboutRepository;
        }
        public IActionResult Index()
        {
            return View(db.AboutModels);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        } 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(AboutModel about)
        {
            if (ModelState.IsValid)
            {
                if (about!=null)
                {
                    AboutRepository.Add(about);
                    return RedirectToAction("About", "Admin");
                }
                
                
            }

            return View();
        }
        [HttpGet]
        public IActionResult Edite(int id) 
        { 
       var resoult= AboutRepository.GetById(id);    
            return View(resoult);
        } 
        [HttpPost]
        public IActionResult Edite(int id,AboutModel about) 
        { 
       var resoult= AboutRepository.GetById(id);
            if (about==null)
            {
                return NotFound();
            }
            resoult.MainTitale=about.MainTitale;
            resoult.Subject=about.Subject;
            AboutRepository.Update(resoult);
            return RedirectToAction("About","Admin");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var resoult = AboutRepository.GetById(id);
            return View(resoult);
        }
        [HttpPost]
        public IActionResult Delete(int id ,AboutModel about)
        {
            var resoult = AboutRepository.GetById(id);
            AboutRepository.Delete(resoult.AboutModelId);
            return RedirectToAction("About", "Admin");
        }
    }
}
