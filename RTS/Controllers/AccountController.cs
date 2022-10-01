using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RTS.BusinessLogic;
using RTS.DataAccess.Models;
using RTS.Models;
using RTS.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RTS.Controllers
{
    public class AccountController :Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly EmployeesBS _employeesBS;
        

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            EmployeesBS employeesBS)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _employeesBS = employeesBS;           
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                    //EmailConfirmed = true
                };
                
                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                
                if (result.Succeeded)
                {
                    /*await signInManager.SignInAsync(user, isPersistent: false);
                    Employee employee = new Employee();
                    employee.Email = model.Email;
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    return RedirectToAction("index", "home");*/

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var link = Url.Action(nameof(VerifyEmail), "Account",
                    new { userId = user.Id, token }, 
                    Request.Scheme,Request.Host.ToString());

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("my email");
                    mailMessage.To.Add(model.Email);
                    mailMessage.Subject = "Email Verfication";
                    mailMessage.Body = $"<a href=\"{link}\">Confirm your Email</a>";
                    mailMessage.IsBodyHtml = true;

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential("my email", "my password");
                    smtpClient.Send(mailMessage);

                    /*await _emailService.SendAsync("xx@gmail.com", "Email Verification",
                        $"<a href=\"{link}\">Verify your Email</a>",true);*/

                    Employee employee = new Employee();
                    employee.EmployeeName = model.FullName;
                    employee.Email = model.Email;                    
                    _employeesBS.CreateEmployee(employee);
                    return RedirectToAction(nameof(VerifyEmail));
                }

                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {           
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
            
        }

        public IActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
                await signInManager.SignOutAsync();
                return RedirectToAction("index", "home");
        }
            

            
        [HttpGet]
        public IActionResult Login()
            {
                return View();
            }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
                if (ModelState.IsValid)
                {
                    var result = await signInManager.PasswordSignInAsync(
                        model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "Items");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }

        return View(model);
        }

        
    }
}
