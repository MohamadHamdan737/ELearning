using ELearning.Bl.IRepositories;
using ELearning.Bl.Models;
using ELearning.EF;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ELearning.UI.Controllers
{
    public class SliderController : Controller
    {
        private AppDbContext db;
        public IBaseRepository<Slider> SliderRepository;
        private IWebHostEnvironment hosting;
        private IToastNotification toastNotification;

        public SliderController( IToastNotification toastNotification,AppDbContext _db, IWebHostEnvironment _hosting, IBaseRepository<Slider> _SliderRepository)
        {
            db = _db;
            SliderRepository = _SliderRepository;
            hosting = _hosting;
            this.toastNotification = toastNotification;
        }
     
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {

            if (slider.File != null)
            {
                string ImageFolder = Path.Combine(hosting.WebRootPath, "SliderImage");
                string ImagePath = Path.Combine(ImageFolder, slider.File.FileName);
                slider.File.CopyTo(new FileStream(ImagePath, FileMode.Create));
                slider.Image = slider.File.FileName;
            }




                SliderRepository.Add(slider);
            toastNotification.AddSuccessToastMessage("Created Successful");
            return RedirectToAction("Index","Home");
        }
        [HttpGet] 
        public IActionResult Edite(int id)
        {
            var result=SliderRepository.GetById(id);
            return View(result);
        }
        [HttpPost]
        public IActionResult Edite(int id,Slider slider)
        {
            if (slider==null)
            {
                return NotFound();
            }
            var result=SliderRepository.GetById(id);
            result.Subject=slider.Subject;
            result.MainTitale=slider.MainTitale;
            if (slider.File != null)
            {
                string ImageFolder = Path.Combine(hosting.WebRootPath, "SliderImage");
                string ImagePath = Path.Combine(ImageFolder, slider.File.FileName);
                slider.File.CopyTo(new FileStream(ImagePath, FileMode.Create));
                slider.Image = slider.File.FileName;
            }

            SliderRepository.Update(result);
            toastNotification.AddSuccessToastMessage("Edite Successful");
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = SliderRepository.GetById(id);
            return View(result);
        }   
        [HttpPost]
        public IActionResult Delete(int id,Slider slider)
        {
            if (slider!=null)
            {
                var result = SliderRepository.GetById(id);
                SliderRepository.Delete(result.SliderId);
                toastNotification.AddErrorToastMessage("Deleted Successful");

                return RedirectToAction("Index", "Home");
            }
            

            return View(slider);
        }
    }
}
