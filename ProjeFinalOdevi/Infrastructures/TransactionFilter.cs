﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeFinalOdevi.Infrastructures
{
    public class TransactionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
                Database.Session.Transaction.Commit();
            else
                Database.Session.Transaction.Rollback();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Database.Session.BeginTransaction();

        }
    }
}