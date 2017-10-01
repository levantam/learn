using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityCore.Models;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace IdentityCore.Controllers
{
    public class TestController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public TestController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateAccount()
        {
            string email = "vantamck2@gmail.com";
            var password = "12345678A@a";
            var result = await this.userManager.CreateAsync(new ApplicationUser { Email = email, UserName = email }, password);
            if (result.Succeeded)
            {
                return Json("Success");
            }
            else
            {
                string message = "";
                foreach(var error in result.Errors)
                {
                    message += ", " + error.Description;
                }
                return Json(message);
            }
        }

        public async Task<IActionResult> Login()
        {
            string email = "vantamck2@gmail.com";
            var password = "12345678A@a";
            var result = await signInManager.PasswordSignInAsync(email, password, false, true);
            string message = string.Empty;
            if (result.Succeeded)
            {
                message = "Logged in successfully";
            }
            else if(result.IsLockedOut)
            {
                message = "Account have been locked";
            }

            return Json(message);
        }

        public async Task<ActionResult> AddRoles()
        {
            string email = "vantamck2@gmail.com";
            var password = "12345678A@a";
            var user = new ApplicationUser() { UserName = email, Email = email };

            //create roles
            if(!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await userManager.AddToRoleAsync(user, "User");
            await userManager.AddToRoleAsync(user, "Admin");

            //add claim
            await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("technology", "technology"));

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> CreateAccountWithConfirm()
        {
            string email = "vantam@gmail.com";
            var password = "12345678A@a";
            var user = new ApplicationUser { Email = email, UserName = email };
            var result = await this.userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                string code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                //SEND email
                return Json(Url.Action("Confirm", new { userId = user.Id, code = code }));
            }
            else
            {
                return Json("create account failed");
            }

        }
        public async Task<IActionResult> Confirm(string userId, string code)
        {
            string message = string.Empty;
            var user = await this.userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var result = await this.userManager.ConfirmEmailAsync(user, code);
                message = string.Format("Confirm email {0}", result.Succeeded);
            }
            return Json(message);
        }
    }
}