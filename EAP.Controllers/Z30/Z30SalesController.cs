using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Z30CRM.Entity;
using Z10Cabbage.Entity;

namespace EAP.Controllers.Z30
{
    public class Z30SalesController : EAP.Logic.__UserController
    {
        public ActionResult List()
        {
            //角色为销售的用户
            var salesTable = db.ExecuteDataSet("select * from [V_UserRole] where [Title]=@Title", db.CreateParameter("Title", "销售")).Tables[0];
            ViewBag.db = db;
            return View(salesTable);
        }
    }
}
