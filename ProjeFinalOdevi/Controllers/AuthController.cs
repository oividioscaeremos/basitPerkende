﻿using ProjeFinalOdevi.Models;
using ProjeFinalOdevi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static ProjeFinalOdevi.Models.UsersMap;

namespace ProjeFinalOdevi.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            return View();
        }

        

        [HttpPost]
        public ActionResult Login(AuthLogin form, string returnUrl)
        {
            
            var user = Database.Session.Query<User>().FirstOrDefault(p => p.Username == form.username);

            if (user == null || !user.CheckPassword(form.password))
            {
                ModelState.AddModelError("Username", "Kullanıcı adı ve(ya) Şifre hatalı.");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            FormsAuthentication.SetAuthCookie(form.username, true);

            if (user.Roles.Contains(Database.Session.Query<Models.Roles>().FirstOrDefault(r => r.name == "admin")))
            {
                return RedirectToAction("Index", "Users", new { area = "Admin" });
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToRoute("Home", user.Id);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        private void setUserRole(String roleName, IList<Models.Roles> roles)
        {
            foreach (var role in Database.Session.Query<Models.Roles>())
            {
                if (role.name == roleName)
                {
                    roles.Add(role);
                }
            }
        }
        
        [HttpPost]
        public ActionResult Register(AuthRegister form)
        {
            if (Database.Session.Query<User>().Any(u => u.Username == form.username))
                ModelState.AddModelError("username","Kullanıcı adı kullanımda.");

            if (!ModelState.IsValid)
                return View(form);

            var user = new User
            {
                Email = form.email,
                Username = form.username,
                adSoyad = form.adSoyad,
                addressMah = form.adresMahalle,
                addRessCadSk = form.adresCadde,
                addressIl = form.il,
                addressIlce = form.ilce
            };

            if(form.username == "admin")
            {
                setUserRole("admin", user.Roles);
            }
            else
            {
                setUserRole("user", user.Roles);
            }

            user.SetPassword(form.password);            

            Database.Session.Save(user);
            Database.Session.Flush();

            return RedirectToRoute("Login");
        }
    }
}