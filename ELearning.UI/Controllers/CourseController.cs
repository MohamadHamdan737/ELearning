using ELearning.Bl.IRepositories;
using ELearning.Bl.Models;
using ELearning.Bl.Models.ViewModels;
using ELearning.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace ELearning.UI.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        public IBaseRepository<Courses> CoursesRepository;
        public IBaseRepository<Instructor> InstructorRepository;
        public IBaseRepository<Favorite> FavoriteRepository;
        private AppDbContext db;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> userManager;
        private IWebHostEnvironment hosting;
        private IToastNotification toastNotification;
        public CourseController(IToastNotification toastNotification,UserManager<IdentityUser> _userManager,IWebHostEnvironment _hosting, IBaseRepository<Favorite> _FavoriteRepository, IBaseRepository<Instructor> _InstructorRepository, IBaseRepository<Courses> _CoursesRepository, AppDbContext _db, SignInManager<IdentityUser> signInManager)
        {
            CoursesRepository = _CoursesRepository;
            db = _db;
            _signInManager = signInManager;
            hosting = _hosting;
            InstructorRepository = _InstructorRepository;
            userManager = _userManager;
            FavoriteRepository= _FavoriteRepository;
            this.toastNotification = toastNotification;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.isDeleted = db.courses.Where(e => e.IsDeleted == true).Count();
            return View(db.courses);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(string term)
        {
            if (term == null)
            {
                return View(CoursesRepository.GetAll());
            }
      
                var name = db.courses.Where(e => e.CoursesName!.Contains(term) || e.Description!.Contains(term));
                return View(name);
            
         
        }
      
     

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
       


        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Courses courses)
        {
            if (courses.ImageFile != null && courses.CertificateFile != null)
                {
                if (courses.ImageFile.Length > 1250000)
                {
                    ModelState.AddModelError("ImageFile", "Course image cannot be more then 10 MB!");
                    return View(courses);
                }
                using (var memoryStream = new MemoryStream())
                    {
                    await courses.ImageFile.CopyToAsync(memoryStream);
                        var fileData = memoryStream.ToArray();
                        courses.ImageFileName = courses.ImageFile.FileName;
                 
                    courses.FileDataImage = fileData;

                    }
                
           

                   if (courses.CertificateFile.Length > 1250000)
              {
                  ModelState.AddModelError("CertificateFile", "Certificate cannot be more then 10 MB!");
                  return View(courses);
              }
                using (var memoryStream = new MemoryStream())
                {
                  await courses.CertificateFile.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    courses.CertificateFileName = courses.CertificateFile.FileName;
           
                    courses.FileDataCertificate = imageBytes;
                }


			



				var currentUser = await userManager.GetUserAsync(User);
				if (currentUser != null)
				{
					courses.CreatedBy = currentUser.UserName;
				}
				CoursesRepository.Add(courses);
                toastNotification.AddSuccessToastMessage("Created Successful");
                return RedirectToAction("Course","Admin");
            }

            return View(courses);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult VideoSettings(int id)
        {
            ViewBag.Id = id;
            var result = db.videos.Where(x => x.CourseId == id).ToList();
            if (result!=null)
            {
                toastNotification.AddSuccessToastMessage("Created Successful");
                return View(result);

            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddVideos()
        {
            
            return View();
        } 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddVideos(int id ,Videos model)
        {
            var result = CoursesRepository.GetById(id);
            if (result!=null)
            {

                model.CourseId = result.CoursesId;

               
                if (model.VideoFile != null)
                {
                    if (model.VideoFile.Length > 40000000)
                    {
                        ModelState.AddModelError("VideoFile", "Videos cannot be more then 40 MB!");
                        return View(model);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.VideoFile.CopyToAsync(memoryStream);
                        var fileData = memoryStream.ToArray();
                        model.VideoFileName = model.VideoFile.FileName;
                        model.VideoFileData = fileData;
                    }
                }
                db.videos.Add(model);
                db.SaveChanges();
                return RedirectToAction("Course", "Admin");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditeVideo(int id)
        {
            var result = db.videos.Find(id);
            return View(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditeVideo(int id,Videos videos)
        {
           
            var result = db.videos.Find(id);
            result.Description = videos.Description;
            result.Titel=videos.Titel;
            if (videos.VideoFile != null)
            {
                if (videos.VideoFile.Length > 40000000)
                {
                    ModelState.AddModelError("VideoFile", "Videos cannot be more then 40 MB!");
                    return View(videos);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await videos.VideoFile.CopyToAsync(memoryStream);
                    var fileData = memoryStream.ToArray();
                    videos.VideoFileName = videos.VideoFile.FileName;
                    videos.VideoFileData = fileData;
                    result.VideoFileName = videos.VideoFile.FileName;
                    result.VideoFileData = fileData;
                }
            }
            db.Update(result);
            db.SaveChanges ();
            return RedirectToAction("Course", "Admin");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVideos(int id)
        {
            var result=db.videos.Find(id);
            return View(result);
        } 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVideos(int id,Videos videos)
        {
            var result=db.videos.Find(id);
            db.videos.Remove(result!);
            db.SaveChanges();
            return RedirectToAction("Course", "Admin");
            
        }




        public IActionResult DownloadImage(int id)
        {
            var course = db.courses.Find(id);
            if (course != null && course.FileDataCertificate != null)
            {
                string contentType = "application/octet-stream";
                string fileName = "certificate.png"; 

                Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName,
                    Size = course.FileDataCertificate.Length
                }.ToString();

                return File(course.FileDataCertificate, contentType);
            }
            return NotFound();
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var result = CoursesRepository.GetById(id);
            if (result!=null)
            {
                ViewBag.VideoCourseId = id;
            }
            
            return View(result);
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Courses courses, int id)
        {
            if (courses == null)
            {
                return NotFound();
            }
            var result = CoursesRepository.GetById(id);
           
            result.InstructorName = courses.InstructorName;
            result.CoursesName = courses.CoursesName;
            result.CreationDate = courses.CreationDate;
            result.Description = courses.Description;
            result.Hours= courses.Hours;

            result.IsDeleted = courses.IsDeleted;
            if (courses.ImageFile != null)
            {
                if (courses.ImageFile.Length > 1250000)
                {
                    ModelState.AddModelError("ImageFile", "Course image cannot be more then 10 MB!");
                    return View(courses);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await courses.ImageFile.CopyToAsync(memoryStream);
                    var fileData = memoryStream.ToArray();
                    courses.ImageFileName = courses.ImageFile.FileName;
                    courses.FileDataImage = fileData;
                    result.ImageFileName = courses.ImageFile.FileName;
                    result.FileDataImage = fileData;

                }

            }
            if (courses.CertificateFile != null)
            {

                if (courses.CertificateFile.Length > 1250000)
                {
                    ModelState.AddModelError("CertificateFile", "Certificate cannot be more then 10 MB!");
                    return View(courses);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await courses.CertificateFile.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    courses.CertificateFileName = courses.CertificateFile.FileName;
                    courses.FileDataCertificate = imageBytes;
                    result.CertificateFileName = courses.CertificateFile.FileName;
                    result.FileDataCertificate = imageBytes;


                }
            }
          

            CoursesRepository.Update(result);
            return RedirectToAction("Course", "Admin");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var result = CoursesRepository.GetById(id);
            return View(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Favorite favorite, int id)
        {
            var result = CoursesRepository.GetById(id);
  
                result.IsDeleted = true;
                CoursesRepository.Update(result);
            foreach (var item in db.Favorites.ToList())
            {
                if (item.CourseId==id)
                {
                    db.Favorites.Remove(item); 
                    db.SaveChanges();
                }
            }


            toastNotification.AddSuccessToastMessage("Created Successful");
            return RedirectToAction("Course", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Soft()
        {
            var result = db.courses.Where(x => x.IsDeleted == true);
            return View(result);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Restor(int id)
        {
            var result = CoursesRepository.GetById(id);
            result.IsDeleted = false;
            CoursesRepository.Update(result);
            return RedirectToAction("Course", "Admin");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var details = CoursesRepository.GetById(id);
            return View(details);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Index");
        }

        [HttpGet]
        public IActionResult MyFavorite()
        {
 
 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            var favoriteItems = db.Favorites
                .Where(fi => fi.UserId == userId)
                .ToList();

            return View(favoriteItems);
        }

 
        public async Task<IActionResult> AddToFavorite(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var result = CoursesRepository.GetById(id);
            if (!db.Favorites.Any(fi => fi.UserId == currentUser!.Id && fi.CourseId == id))
            {
                Favorite favorite = new Favorite
                {
                    CourseId = id,
                    UserId = currentUser!.Id,
                    CoursesName = result.CoursesName,
                    Hours = result.Hours,
                    Description = result.Description,
                    InstructorName = result.InstructorName,
                    FileDataImage = result.FileDataImage,
                    ImageFileName = result.ImageFileName,
                };
                FavoriteRepository.Add(favorite);
                toastNotification.AddSuccessToastMessage("Add to favorite");

                return RedirectToAction("Index", "Home");
            }

           
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var favoriteItem = await db.Favorites
                .FirstOrDefaultAsync(fi => fi.UserId == user!.Id && fi.CourseId == id);

            if (favoriteItem != null)
            {
                db.Favorites.Remove(favoriteItem);
                await db.SaveChangesAsync();
            }
            toastNotification.AddSuccessToastMessage("Removed from favorite");
            return RedirectToAction("Index"); 
        }


        public IActionResult CourseVideo(int id)
        {
            var result = db.courses.Find(id);
            if (result != null)
            {
               var courseVideo = db.videos
               .Where(fi => fi.CourseId == result.CoursesId)
               .ToList();
                ViewBag.courseId = result.CoursesId;
                return View(courseVideo);
            }
            

            return View();

        }



        public IActionResult StartCourse(int id)
        {
            var co = CoursesRepository.GetById(id);
            ViewBag.Lectures = db.videos.Where(e => e.CourseId == co.CoursesId).Count();
            ViewBag.IsFavorite = db.Favorites.Where(x => x.CourseId == co.CoursesId).Count();

            return View(co);
            
            
        } 
       
        //public IActionResult Comment()
        //{
        //    return View(db.Messagess);
        //}





    }
}
