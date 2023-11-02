using ELearning.Bl.IRepositories;
using ELearning.Bl.Models;
using ELearning.Bl.Models.ViewModels;
using ELearning.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearning.UI.Controllers
{
    public class AdminController : Controller
    {
        public IBaseRepository<Instructor> InstructorRepository;
        public IBaseRepository<Message> MessageRepository;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private AppDbContext db;
        //private IEmailSender emailSender;



        public AdminController(AppDbContext _db, IBaseRepository<Message> _MessageRepository, IBaseRepository<Instructor> _InstructorRepository, SignInManager<IdentityUser> _signInManager, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            roleManager = _roleManager;
            InstructorRepository = _InstructorRepository;
            db = _db;
            MessageRepository = _MessageRepository;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UserList()
        {
       
            return View (userManager.Users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string id)
        {
       
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id,IdentityUser identity)
        {
            if (id==null)
            {
                return NotFound();
            }
            var user=await userManager.FindByIdAsync(id);
            var result= await userManager.DeleteAsync(user!);
            if (result.Succeeded)
            {
                return RedirectToAction("UserList");
            }
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }


        #region Role
        [Authorize(Roles = "Admin")]
        public IActionResult RolesList()
        {
            return View(roleManager.Roles);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList");
                }
                ModelState.AddModelError("", "Not Created");
                return View(model);
            }

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditeRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            EditeRoleViewModel model = new EditeRoleViewModel
            {
                RoleName = role!.Name,
                RoleId = role.Id
            };

            foreach (var users in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(users, role.Name!))
                {
                    model.Users.Add(users.Email!);
                }
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditeRole(EditeRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.RoleId!);
                role!.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(RolesList));
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }


            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await roleManager.FindByIdAsync(id);
            return View(role);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id, IdentityUser r)
        {
            if (id == null) { return NotFound(); }
            var role = await roleManager.FindByIdAsync(id);
            var result = await roleManager.DeleteAsync(role!);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(RolesList));
            }
            return View(role);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await roleManager.FindByIdAsync(id);
            List<UserRoleViewModel> userRoleViewModels = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users.ToList())
            {
                UserRoleViewModel model = new UserRoleViewModel
                {

                    UserName = user.UserName,
                    UserId = user.Id,
                };
                if (await userManager.IsInRoleAsync(user, role.Name!))//Role Name
                {
                    model.IsSelected = true;
                }
                else
                {
                    model.IsSelected = false;
                }
                userRoleViewModels.Add(model);


            }
            return View(userRoleViewModels);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserRole(string id, List<UserRoleViewModel> modela)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            IdentityResult result = null!;
            for (int i = 0; i < modela.Count; i++)
            {
                var user = await userManager.FindByIdAsync(modela[i].UserId!);
                if (modela[i].IsSelected && !(await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.AddToRoleAsync(user!, role.Name!);
                }
                else if (!(modela[i].IsSelected) && await userManager.IsInRoleAsync(user!, role.Name!))
                {
                    result = await userManager.RemoveFromRoleAsync(user!, role.Name!);
                }
            }
            if (result == null)
            {
                return RedirectToAction(nameof(EditeRole));
            }
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(EditeRole), new { id = id });
            }
            return View(modela);
        }

        #endregion
        public IActionResult Course()
        {
            ViewBag.isDeleted = db.courses.Where(e => e.IsDeleted == true).Count();
            return View(db.courses);
        }
        public IActionResult Instructors()
        {
            ViewBag.isDeleted = db.Instructors.Where(e => e.IsDeleted == true).Count();
            return View(db.Instructors);
        }
        public IActionResult About()
        {
            return View(db.AboutModels);
        } 
        public IActionResult Slider()
        {
            return View(db.Sliders);
        }
     
    }
}
