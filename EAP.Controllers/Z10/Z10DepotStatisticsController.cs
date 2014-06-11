using System;
using System.Collections;
using System.Web.Mvc;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using System.Linq;
using Zippy.Data.Collections;

namespace EAP.Controllers.Z10
{
    public class Z10DepotStatisticsController : EAP.Logic.__UserController
    {
        #region 查询
        public ActionResult FlowList(int? PageIndex, int? PageSize, Int64? qDepotID, long? qProductID, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qDepotID", qDepotID);
            hs.Add("qProductID", qProductID);
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);

            PaginatedList<EAP.Logic.Z10.View.V_DepotFlow> result = Z10DepotFlowHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            return View(result);
        }
        #endregion

        #region DepotProducts
        public ActionResult DepotProducts(int? PageIndex, int? PageSize, long? qProductID, long? qDepotID, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);

            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qDepotID", qDepotID);
            hs.Add("qProductID", qProductID);
            hs.Add("orderCol", orderCol);

            PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> result = Z10DepotProductHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;

            ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            return View(result);
        }

        public ActionResult ProductDetail(long? id)
        {
            var xid = id ?? 0;
            //  
            //return Content(xid.ToString());
            var xdetail = Zippy.Data.StaticDB.DB.Take<Z10DepotProductDetail>("ProductID=@ProductID and StockSum>0",
                db.CreateParameter("ProductID", xid));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var depots = db.Take<Z10Depot>();
            foreach (var xd in xdetail)
            {
                var depotTitle = string.Empty;
                var depot1 = depots.Where(s => s.DepotID == xd.DepotID).FirstOrDefault();
                if (depot1 != null)
                    depotTitle = depot1.Title;
                sb.AppendLine("<li>" + depotTitle + " " + xd.ExtColor + " " + xd.ExtSize + "  (" + (xd.StockSum ?? 0).ToString("0.##") + ") " + "<li>");
                //var pro = db.FindUnique<Z01Beetle.Entity.Z01Product>(xd.ProductID);
                //decimal oriPrice = pro.PriceStock ?? 0;
                //sb.AppendLine("<li>" + xd.ExtColor + " " + xd.ExtSize + "  (" + (xd.StockSum ?? 0).ToString("0.##") + ") 进货价：" + (xd.PriceStock ?? 0).ToString("0.##") + "<input value='" + oriPrice.ToString("0.##") + "' /><input type='button' class='savesprice' rel='" + xd.DepotProductID + "' value='save' /> " + depotTitle + "<li>");
            }
            return Content(sb.ToString());
        }
        #endregion

        public ActionResult UpdateStockPrice(long xid, decimal xprice)
        {
            Z10DepotProductDetail dd = new Z10DepotProductDetail();
            dd.DepotProductID = xid;
            dd.PriceStock = xprice;
            db.Update(dd);
            return Content("1");

        }
    }
}
