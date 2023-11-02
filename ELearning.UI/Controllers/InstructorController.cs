 using ELearning.Bl.IRepositories;
using ELearning.Bl.Models;
using ELearning.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ELearning.UI.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        public IBaseRepository<Instructor> InstructorRepository;
        private AppDbContext db;
        private SignInManager<IdentityUser> _signInManager;
        private IWebHostEnvironment hosting;
		private UserManager<IdentityUser> userManager;
		public InstructorController(UserManager<IdentityUser> userManager,IWebHostEnvironment _hosting,IBaseRepository<Instructor> _InstructorRepository, AppDbContext _db, SignInManager<IdentityUser> signInManager)
        {
            InstructorRepository = _InstructorRepository;
            db = _db;
            _signInManager = signInManager;
            hosting = _hosting;
            this.userManager=userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.isDeleted = db.Instructors.Where(e => e.IsDeleted == true).Count();
            return View(db.Instructors);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(string term)
        {
            if (term == null)
            {
                return View(InstructorRepository.GetAll());
            }

            return View(db.Instructors.Where(x => x.InstructorName!.Contains(term!) || x.Majoring!.Contains(term)));


        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (instructor.File != null)
            {
      
                if (instructor.File.Length> 1250000)
                {
                    ModelState.AddModelError("File", "Instructor image cannot be more then 10 MB!");
                    return View(instructor);
                }
       
                string ImageFolder = Path.Combine(hosting.WebRootPath, "Upload");
                string ImagePath = Path.Combine(ImageFolder, instructor.File.FileName);
                instructor.File.CopyTo(new FileStream(ImagePath, FileMode.Create));
                instructor.Images = instructor.File.FileName;

            }
			var currentUser = await userManager.GetUserAsync(User);
            if (currentUser!=null)
            {
				instructor.CreatedBy = currentUser.UserName;
			}
            
			InstructorRepository.Add(instructor);
            return RedirectToAction("Instructors", "Admin");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = InstructorRepository.GetById(id);
            return View(result);
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Instructor instructor, int id)
        {
            if (instructor == null)
            {
                return NotFound();
            }
            var result = InstructorRepository.GetById(id);
            result.InstructorName = instructor.InstructorName;
            result.City = instructor.City;
            result.Email = instructor.Email;
            result.CreationDate = instructor.CreationDate;
            result.IsDeleted = instructor.IsDeleted;
            result.Majoring = instructor.Majoring;
            result.FacebookLink=instructor.FacebookLink;
            result.InstagramLink=instructor.InstagramLink;
            result.TwitterLink=instructor.TwitterLink;
            if (instructor.File != null)
            {
                string ImageFolder = Path.Combine(hosting.WebRootPath, "Upload");
                string ImagePath = Path.Combine(ImageFolder, instructor.File.FileName);
                instructor.File.CopyTo(new FileStream(ImagePath, FileMode.Create));
                instructor.Images = instructor.File.FileName;
                result.Images= instructor.Images;
            }
           
            InstructorRepository.Update(result);
            return RedirectToAction("Instructors", "Admin");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = InstructorRepository.GetById(id);
            return View(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Instructor instructor, int id)
        {
            var result = InstructorRepository.GetById(id);
            result.IsDeleted = true;
            InstructorRepository.Update(result);
            return RedirectToAction("Instructors", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Soft()
        {
            var result = db.Instructors.Where(x => x.IsDeleted == true);
            return View(result);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Restor(int id)
        {
            var result = InstructorRepository.GetById(id);
            result.IsDeleted = false;
            InstructorRepository.Update(result);
            return RedirectToAction("Instructors","Admin");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            return View(InstructorRepository.GetById(id));
        }

        public IActionResult CoursesInstrouctor(int id)
        {
            var userid=InstructorRepository.GetById(id);
            var course = db.courses
            .Where(fi => fi.InstructorName == userid.InstructorName)
            .ToList();
            return View(course);
        }



        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
