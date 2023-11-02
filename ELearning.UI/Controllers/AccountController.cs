using ELearning.Bl.IRepositories;
using ELearning.Bl.Models;
using ELearning.Bl.Models.ViewModels;
using ELearning.EF;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using ELearning.UI.SendEmails;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NToastNotify;

namespace ELearning.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Configration
        public IBaseRepository<Instructor> InstructorRepository;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private AppDbContext db;
        private IConfiguration Configuration;
        private IToastNotification toastNotification;
        //private IEmailSender emailSender;



        public AccountController(IToastNotification toastNotification,IConfiguration _Configuration, AppDbContext _db,IBaseRepository<Instructor> _InstructorRepository,SignInManager<IdentityUser> _signInManager, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            roleManager = _roleManager;
            InstructorRepository= _InstructorRepository;
            db = _db;
            Configuration = _Configuration;
            this.toastNotification= toastNotification;
        }
        #endregion


        #region User
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistorViewModel model)
        {
               
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.Mopile
                };
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { email = user.Email ,code}, protocol: Request.Scheme);

                    var sendEmail = new SendEmail(Configuration); 
                    bool isSendEmail = sendEmail.EmailSend(user.Email, "Confirmation email link", $"Please Confirme your email by clicking <a href=\"{confirmationLink}\">here</a>", true);
                    await userManager.AddToRoleAsync(user, "User");


                    if (isSendEmail)
                    {
                        return RedirectToAction("SuccessRegistration", "Account");
                    }
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                    return View(model);
                }
            }

            return View(model);
        }
     

        [HttpGet,HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail( string email, string code)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
               return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }
        [HttpGet, HttpPost]
        [AllowAnonymous]
        public IActionResult SuccessRegistration()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByEmailAsync(model.Email);
                if (user==null)
                {
                    ModelState.AddModelError("", "this email is not exist");
                    return View(model);
                }
                if ( !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError("", "please Confirme your email before login");
                    return View(model);
                }



                    var resoult = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);
                if (resoult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid user email or password");
                return View(model);
            }
            return View(model);

        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {

                    ModelState.AddModelError("", "This Email doesn't exist");
                    return View("Login");

                }

                string code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                var sendEmail = new SendEmail(Configuration); 
                bool isSendEmail = sendEmail.EmailSend(model.Email, "Reset Your Password", $"Please reset your password by clicking <a href=\"{callbackUrl}\">here</a>", true);

                if (isSendEmail)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }

            return View("Login");
        }


        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }


            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var currentUser = await userManager.GetUserAsync(User);
            
        
            if (currentUser != null)
            {
                string email = currentUser.Email!;
                string username = currentUser.UserName!;
                string mopile = currentUser.PhoneNumber!;
              //  var role = await userManager.GetRolesAsync(currentUser);
               
               
            }

         

            return View(currentUser);
        }

        [HttpGet]
        [Authorize]

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user != null)
                {
                    var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (changePasswordResult.Succeeded)
                    {
                        toastNotification.AddSuccessToastMessage("Changed Successful");
                        return RedirectToAction("MyAccount");
                    }
                    toastNotification.AddErrorToastMessage("Old Password is incorrect");
                    return View(model);
                }
               
            }
            return View(model);
        }




        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
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
                if (await userManager.IsInRoleAsync(user, role.Name!))
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
            if (result==null)
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
    }
}
