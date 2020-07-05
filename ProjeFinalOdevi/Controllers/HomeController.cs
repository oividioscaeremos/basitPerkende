using Microsoft.Ajax.Utilities;
using NHibernate.Criterion;
using ProjeFinalOdevi.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeFinalOdevi.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        [Authorize(Roles = "user")]
        public ActionResult Index()
        {

            var currentUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);

            ViewModels.PrintReportViewModel sales = new ViewModels.PrintReportViewModel();
            sales.allSales = Database.Session.Query<Models.Sales>()
                .Where(s => s.belongsToUser.Id == currentUser.Id);
            sales.currUser = currentUser;
            return View(sales);

        }

        public ActionResult ViewUser(int id)
        {
            var user = Database.Session.Load<Models.User>(id);
            return View(new ViewModels.CurrentUserView
            {
                id = user.Id,
                username = user.Username,
                adsoyad = user.adSoyad,
                email = user.Email,
                adresMah = user.addressMah,
                adresCadSk = user.addRessCadSk,
                adresIlce = user.addressIlce,
                adresIl = user.addressIl,
                balance = user.Balance
            });
        }

        [HttpPost]
        public ActionResult ViewUser(int id, ViewModels.CurrentUserView form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            if (Database.Session.Query<User>().Any(u => u.Username == form.username && u.Id != id))
                ModelState.AddModelError("Username", "Kullanıcı adı zaten kullanımda.");

            if (!ModelState.IsValid)
                return View(form);

            user.adSoyad = form.adsoyad;
            user.Email = form.email;
            user.addressMah = form.adresMah;
            user.addRessCadSk = form.adresCadSk;
            user.addressIlce = form.adresIlce;
            user.addressIl = form.adresIl;

            Database.Session.Update(user);
            Database.Session.Flush();
            return RedirectToRoute("Home");
        }
        
        [HttpPost]
        public ActionResult PrintPartialViewToPdf(ViewModels.PrintReportViewModel form)
        {
            if (form.reportFrom.CompareTo(form.reportTo) > 0 || form.reportFrom.CompareTo(DateTime.Now) > 0 || form.reportTo.CompareTo(DateTime.Now) > 0)
                return RedirectToAction("Index"); 

            var currentUser = Database.Session.Query<Models.User>().FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            var toDate = form.reportTo;
            toDate = toDate.AddSeconds(86399);

            ViewModels.PrintReportViewModel rapor = new ViewModels.PrintReportViewModel();

            rapor.allSales = Database.Session.Query<Models.Sales>()
                .Where(s => s.belongsToUser.Id == currentUser.Id)
                .Where(s => s.date.CompareTo(form.reportFrom) >= 0)
                .Where(s => s.date.CompareTo(toDate) <= 0);

            rapor.currUser = currentUser;
            rapor.reportFrom = form.reportFrom;
            rapor.reportTo = form.reportTo;
            var report = new PartialViewAsPdf("~/Views/Shared/_PrintReport.cshtml", rapor);
            return report;
        }
    }
}