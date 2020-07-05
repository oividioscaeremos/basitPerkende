using ProjeFinalOdevi.Infrastructures;
using ProjeFinalOdevi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace ProjeFinalOdevi.Controllers
{
    [Authorize(Roles = "user")]
    public class StockController : Controller
    {
        private const int itemsPerPage = 20;
        // GET: Stock
        public ActionResult Index(int id, int page = 1)
        {
            var ourStocks = Database.Session.Query<Models.Stock>()
                .Where(stock => stock.Belongstouser.Id == id)
                .OrderByDescending(stc => stc.StockId)
                .ToList();

            return View(new ViewModels.StockIndex
            {
                stockInfo = new PagedData<Models.Stock>(ourStocks, ourStocks.Count(), page, itemsPerPage)
            });
        }

        [HttpGet]
        public ActionResult StockViewOne(int id)
        {
            var stockInfo = Database.Session.Query<Models.Stock>()
                .Where(s => s.StockId == id)
                .FirstOrDefault();

            return View(new ViewModels.StockViewOne { 
                stock = stockInfo
            });
        }

        [HttpPost]
        public ActionResult StockViewOne(int id, ProjeFinalOdevi.ViewModels.StockViewOne form)
        {
            var ourForm = form;
            
            int userid = Database.Session.Query<Models.User>()
                    .Where(u => u.Username == HttpContext.User.Identity.Name)
                    .FirstOrDefault().Id;

            var ourStock = Database.Session.Query<Models.Stock>()
                   .Where(s => s.StockId == id)
                   .Where(s => s.Belongstouser.Id == userid)
                   .FirstOrDefault();
            try
            {
                ourStock.Description = form.stock.Description;
                ourStock.Quantityinstock = form.stock.Quantityinstock;
                ourStock.Unitprice = form.stock.Unitprice;

                Database.Session.Update(ourStock);
                Database.Session.Flush();

            }
            catch (Exception ex)
            {
                string message = string.Format("<b>Message:</b> {0}<br /><br />", ex.Message);
                message += string.Format("<b>StackTrace:</b> {0}<br /><br />", ex.StackTrace.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>Source:</b> {0}<br /><br />", ex.Source.Replace(Environment.NewLine, string.Empty));
                message += string.Format("<b>TargetSite:</b> {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, string.Empty));
                ModelState.AddModelError(string.Empty, message);

                return View(new ViewModels.StockViewOne { 
                    stock = ourStock
                });
            }
            finally
            {

            }
            
            return RedirectToAction("Index", new { id = userid });
        }

        [HttpGet]
        public ActionResult NewStock()
        {
            return View(new ViewModels.StockNew
            {
                user_id = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .FirstOrDefault().Id
            });
        }

        [HttpPost]
        public ActionResult NewStock(ViewModels.StockNew form)
        {
            var ourUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            if(form.unit_price <= 0)
            {
                ModelState.AddModelError("unit_price", "Birim Fiyat 0'dan büyük olmalıdır.");
            }
            if (form.stock_desc.IsEmpty())
            {
                ModelState.AddModelError("stock_desc", "Stok açıklaması boş bırakılamaz.");
            }
            if (form.stock_number <= 0)
            {
                ModelState.AddModelError("stock_number", "Stok adedi 0'dan büyük olmalıdır.");
            }

            if (!ModelState.IsValid)
            {
                form.user_id = ourUser.Id;
                return View(form);
            }

            Models.Stock yeniUrun = new Models.Stock
            {
                Belongstouser = ourUser,
                Description = form.stock_desc,
                Quantityinstock = form.stock_number,
                Unitprice = form.unit_price
            };

            try
            {
                Database.Session.Save(yeniUrun);
            }catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.ToString());
                return View();
            }
            Database.Session.Flush();

            return RedirectToAction("Index", new { id = ourUser.Id });
        }

        [HttpPost]
        public ActionResult DeleteStock(int id)
        {
            var currUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            var stock = Database.Session.Query<Models.Stock>()
                .Where(u => u.StockId == id)
                .FirstOrDefault();

            try
            {
                if(currUser.Id == stock.Belongstouser.Id)
                {
                    Database.Session.Delete(stock);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message.ToString());
                return View();
            }
            Database.Session.Flush();

            return RedirectToAction("Index", new { id = stock.Belongstouser.Id });
        }
    }
}