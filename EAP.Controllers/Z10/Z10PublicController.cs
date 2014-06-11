using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z01Beetle.Entity.Helper;
using Zippy.Data.Collections;
using System.Linq;

namespace EAP.Controllers.Z10
{
    public class Z10PublicController : Controller
    {
        public ActionResult QueryDepot(int? PageIndex, int? PageSize, string qTitle, long? qBrandID, long? qCateID)
        {


            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            var db = Zippy.Data.StaticDB.DB;
            var tenant = Zippy.SaaS.Helper.TenantHelper.Get("TouchBike", db);

            ViewData.Add("PageSize", PageSize ?? 20);
            int currentPageSize = PageSize ?? 20;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qCateID", qCateID);
            hs.Add("qBrandID", qBrandID);


            PaginatedList<Z01Product> rtn = new PaginatedList<Z01Product>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " ProductStatus=" + (int)EAP.Logic.Z01.ProductStatus.Normal;


            if (qTitle.IsNotNullOrEmpty())
            {
                where += " and ([Title] like @Title or [Code] like @Title)";
                dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
            }
            if (qBrandID > 0)
            {
                where += " and [BrandID] = @BrandID";
                dbParams.Add(db.CreateParameter("BrandID", qBrandID));
            }
            if (qCateID > 0)
            {
                where += " and [ProductID] in (select [ProductID] from Z01ProductInCategory where [CategoryID]=@CategoryID)";
                dbParams.Add(db.CreateParameter("CategoryID", qCateID));
            }


            int RecordCount = db.Count<Z01Product>(where, dbParams.ToArray());
            int PageCount = 0;
            if (RecordCount % PageSize == 0)
            {
                PageCount = RecordCount / currentPageSize;
            }
            else
            {
                PageCount = RecordCount / currentPageSize + 1;
            }
            if (PageIndex > PageCount)
                PageIndex = PageCount;
            if (PageIndex < 1)
                PageIndex = 1;


            List<Z01Product> records = db.Take<Z01Product>(where + " order by PriceStock", currentPageSize, currentPageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = currentPageIndex;
            rtn.PageSize = currentPageSize;
            rtn.TotalCount = RecordCount;
            rtn.QueryParameters = hs;


            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag.brands = brands;
            ViewBag.categories = EAP.Logic.Z01.Helper.GetProductCategories(tenant.TenantID.Value);

            return View(rtn);
        }
        public ActionResult DepotDetail(long? id)
        {
            var xid = id ?? 0;
            var db = Zippy.Data.StaticDB.DB;
            //  
            //return Content(xid.ToString());
            var xdetail = db.Take<Z10Cabbage.Entity.Z10DepotProductDetail>("ProductID=@ProductID and StockSum>0 order by ExtColor,DepotProductID",
                db.CreateParameter("ProductID", xid));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var depots = db.Take<Z10Cabbage.Entity.Z10Depot>();
            //sb.Append("<ul>");
            foreach (var xd in xdetail)
            {
                var depotTitle = string.Empty;
                var depot1 = depots.Where(s => s.DepotID == xd.DepotID).FirstOrDefault();
                if (depot1 != null)
                    depotTitle = depot1.Title;
                sb.AppendLine("<li>" + EAP.Logic.Helper.ColorHtml(xd.ExtColor) + " " + xd.ExtSize + " " + depotTitle + " " + xd.ExtColor + "  (" + (xd.StockSum ?? 0).ToString("0.##") + ") " + "</li>");
                //var pro = db.FindUnique<Z01Beetle.Entity.Z01Product>(xd.ProductID);
                //decimal oriPrice = pro.PriceStock ?? 0;
                //sb.AppendLine("<li>" + xd.ExtColor + " " + xd.ExtSize + "  (" + (xd.StockSum ?? 0).ToString("0.##") + ") 进货价：" + (xd.PriceStock ?? 0).ToString("0.##") + "<input value='" + oriPrice.ToString("0.##") + "' /><input type='button' class='savesprice' rel='" + xd.DepotProductID + "' value='save' /> " + depotTitle + "<li>");
            }
            //sb.Append("</ul>");
            return Content(sb.ToString());
        }
    }
}
