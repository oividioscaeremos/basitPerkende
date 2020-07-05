using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjeFinalOdevi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "", 
                new { controller = "Home", action = "Index" }
                );
            routes.MapRoute("ViewUser", "view-user", 
                new { controller = "Home", action = "ViewUser" }
                );
            routes.MapRoute("Partial", "partial",
                new { controller = "Home", action = "PrintReport" }
                );
            routes.MapRoute("Stocks", "urunler",
               new { controller = "Stock", action = "Index" }
               );
            routes.MapRoute("UrunView", "urun",
                new { controller = "Stock", action = "StockViewOne" }
                );
            routes.MapRoute("UrunNew", "yeniUrun",
                new { controller = "Stock", action = "NewStock" }
                );
            routes.MapRoute("Sales", "satislar",
              new { controller = "Sales", action = "Index" }
              );
            routes.MapRoute("EditSale", "urunDuzenle",
                new { controller = "Sales", action = "EditSale" }
                );
            routes.MapRoute("SatisNew", "yeniSatis",
                new { controller = "Sales", action = "NewSale" }
                );
            routes.MapRoute("DeleteStock", "stokSil",
                new { controller = "Stock", action = "DeleteStock" }
                );
            routes.MapRoute("DeleteSale", "satisSil",
                new { controller = "Sales", action = "DeleteSale" }
                );
            /*routes.MapRoute("UrunView", "urun",
                new { controller = "Stock", action = "StockViewOne" }
                );
            routes.MapRoute("UrunNew", "yeniUrun",
                new { controller = "Stock", action = "NewStock" }
                );*/
            routes.MapRoute("Login", "login", 
                new { controller = "Auth", action = "Login" }
                );
            routes.MapRoute("Logout", "logout", 
                new { controller = "Auth", action = "Logout" }
                );
            routes.MapRoute("Register", "register",
                new { controller = "Auth", action = "Register" }
                );
            routes.MapRoute("PrintPartialViewToPdf", "print-report",
                new { controller = "Home", action = "PrintPartialViewToPdf" }
                );


        }
    }
}
