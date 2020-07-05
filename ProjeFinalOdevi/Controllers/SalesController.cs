using NHibernate.Criterion;
using ProjeFinalOdevi.Infrastructures;
using ProjeFinalOdevi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeFinalOdevi.Controllers
{
    public class SalesController : Controller
    {
        private const int itemsPerPage = 20;

        // GET: Sales
        public ActionResult Index(int id, int page = 1)
        {
            var allSales = Database.Session.Query<Models.Sales>()
                .Where(s => s.belongsToUser.Id == id)
                .ToList();

            return View(new ViewModels.SalesIndex { 
                salesInfo = new PagedData<Models.Sales>(allSales, allSales.Count(), page, itemsPerPage)
            });
        }

        public ActionResult NewSale()
        {
            var currUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            IEnumerable<Models.Stock> allStock = Database.Session.Query<Models.Stock>()
                .Where(c => c.Belongstouser.Id == currUser.Id)
                .OrderBy(c => c.Description);

            return View(new ViewModels.SalesNew { 
                stocks = allStock
            });
        }

        [HttpPost]
        public ActionResult NewSale(ViewModels.SalesNew form)
        {
            var currUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            Models.Stock stockToSell = Database.Session.Query<Models.Stock>()
                .Where(c => c.StockId == form.selectedStockID)
                .OrderBy(c => c.Description)
                .FirstOrDefault();

            if (form.selectedStockID == 0)
            {
                ModelState.AddModelError("stocks", "Ürün Seçimi Yapılmalıdır.");
            }

            if(form.quantity <= 0)
            {
                ModelState.AddModelError("quantity", "Satış Adedi 0 veya 0'dan Küçük Olamaz.");
            }

            if(stockToSell != null)
            {
                if(form.quantity > stockToSell.Quantityinstock)
                {
                    ModelState.AddModelError("quantity", "Stoktaki Ürün Miktarından Fazla Satış Yapılamaz.\nStok Ürün Miktarı: " + stockToSell.Quantityinstock.ToString());
                }
            }

            if (!ModelState.IsValid)
            {
                form.stocks = Database.Session.Query<Models.Stock>()
                                .Where(c => c.Belongstouser.Id == currUser.Id)
                                .OrderBy(c => c.Description)
                                .ToList();
                form.user_id = currUser.Id;
                return View(form);
            }


            var newSale = new Sales
            {
                belongsToUser = currUser,
                quantity = form.quantity,
                date = DateTime.Now,
                stock_id = stockToSell,
                sale_sum = stockToSell.Unitprice * form.quantity
            };

            stockToSell.Quantityinstock -= form.quantity;
            newSale.belongsToUser.Balance += stockToSell.Unitprice * form.quantity;

            try {
                Database.Session.Save(newSale);
                Database.Session.Update(stockToSell);
                Database.Session.Update(newSale.belongsToUser);
            }catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View(form);
            }

            Database.Session.Flush();
            return RedirectToAction("Index", new { id = currUser.Id});
        }


        [HttpPost]
        public ActionResult DeleteSale(int id)
        {
            var currUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            var sale = Database.Session.Query<Models.Sales>()
                .Where(u => u.sales_id == id)
                .FirstOrDefault();

            var stock = Database.Session.Query<Models.Stock>()
                .Where(s => s == sale.stock_id)
                .FirstOrDefault();
                

            try
            {
                if (currUser.Id == sale.belongsToUser.Id)
                {
                    currUser.Balance -= sale.sale_sum;
                    stock.Quantityinstock += sale.quantity;

                    Database.Session.Update(stock);
                    Database.Session.Update(currUser);
                    Database.Session.Delete(sale);
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

        [HttpGet]
        public ActionResult EditSale(int id)
        {
            Models.Sales saleToEdit = Database.Session.Query<Models.Sales>()
                .Where(c => c.sales_id == id)
                .FirstOrDefault();

            return View(new ViewModels.SalesEdit
            {
                sale_id = id,
                quantity = saleToEdit.quantity,
                user_id = saleToEdit.belongsToUser.Id
            }) ;
        }


        [HttpPost]
        public ActionResult EditSale(ViewModels.SalesEdit form)
        {
            var currUser = Database.Session.Query<Models.User>()
                .Where(u => u.Username == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            Models.Sales saleToEdit = Database.Session.Query<Models.Sales>()
                .Where(c => c.sales_id == form.sale_id)
                .FirstOrDefault();

            if (form.quantity <= 0)
            {
                ModelState.AddModelError("quantity", "Satış Adedi 0 veya 0'dan Küçük Olamaz.");
            }

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var stockForSale = Database.Session.Query<Stock>()
                .Where(s => s.StockId == saleToEdit.stock_id.StockId)
                .FirstOrDefault();

            int beforeQuantity = saleToEdit.quantity;
            int afterQuantity = form.quantity;

            if (afterQuantity - beforeQuantity >= 0)
            {
                saleToEdit.quantity += Math.Abs(afterQuantity - beforeQuantity);
                saleToEdit.sale_sum += Math.Abs(afterQuantity - beforeQuantity) * stockForSale.Unitprice;
                currUser.Balance += Math.Abs(afterQuantity - beforeQuantity) * stockForSale.Unitprice;
                stockForSale.Quantityinstock -= Math.Abs(afterQuantity - beforeQuantity);
            } else
            {
                saleToEdit.quantity -= Math.Abs(afterQuantity - beforeQuantity);
                saleToEdit.sale_sum -= Math.Abs(afterQuantity - beforeQuantity) * stockForSale.Unitprice;
                currUser.Balance -= Math.Abs(afterQuantity - beforeQuantity) * stockForSale.Unitprice;
                stockForSale.Quantityinstock += Math.Abs(afterQuantity - beforeQuantity);
            }

            try
            {
                Database.Session.Update(saleToEdit);
                Database.Session.Update(currUser);
                Database.Session.Update(stockForSale);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return View(form);
            }

            Database.Session.Flush();
            return RedirectToAction("Index", new { id = currUser.Id });
        }
    }

}