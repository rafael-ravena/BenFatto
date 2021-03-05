using BenFatto.App.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BenFatto.App.Controllers.Account
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("~/");
            }
            return View(new BenFattoUser());
        }
        [HttpPost]
        public IActionResult Login([FromForm] BenFattoUser user)
        {
            using (BenFattoAppContext context = new BenFattoAppContext())
            {
                BenFattoUser loggingInUser = context.Users.FirstOrDefault(u =>
                    u.Email.ToLower() == user.Email.ToLower() &&
                    u.Password == user.Password
                );
                if (null == loggingInUser)
                    return Forbid();
                SignUserIn(loggingInUser);
            }
            return Redirect("~/");
        }
        public IActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("~/");
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Signout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Account/Logout");
        }
        [Authorize]
        public ActionResult<IEnumerable<Model.BenFattoUser>> Index()
        {
            List<Model.BenFattoUser> users;
            using (BenFattoAppContext context = new BenFattoAppContext())
            {
                users = context.Users.ToList();
            }
            return View(users);
        }
        [Authorize]
        public ActionResult<DTO.User> Edit(int id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "Id").Value != id.ToString())
                return Forbid();
            using (BenFattoAppContext context = new BenFattoAppContext())
            {
                BenFattoUser loggedUser = context.Users.FirstOrDefault(u => u.Id == id);
                if (null == loggedUser)
                    return NotFound();
                return View(new DTO.User
                {
                    Id = loggedUser.Id,
                    Name = loggedUser.Name,
                    Email = loggedUser.Email,
                    Password = loggedUser.Password
                });
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult<Model.BenFattoUser> Edit([FromForm] DTO.User user)
        {
            using (BenFattoAppContext context = new BenFattoAppContext())
            {
                BenFattoUser loggedUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (null == loggedUser)
                    return NotFound();
                if (loggedUser.Password != user.Password)
                    return Forbid();
                loggedUser.Password = user.NewPassword;
                loggedUser.Name = user.Name;
                loggedUser.Email = user.Email;
                context.Entry(loggedUser).State = EntityState.Modified;
                context.SaveChanges();
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                SignUserIn(loggedUser);
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize]
        public ActionResult<Model.BenFattoUser> Create()
        {
            return View(new Model.BenFattoUser());
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create([FromForm] BenFattoUser user)
        {
            using (BenFattoAppContext context = new BenFattoAppContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        private void SignUserIn(BenFattoUser user)
        {
            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Id", user.Id.ToString())
                };
            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.Now.AddDays(5) });
        }
    }
}
