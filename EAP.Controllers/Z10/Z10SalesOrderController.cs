using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using Zippy.Data.Collections;

namespace EAP.Controllers.Z10
{
    public class Z10SalesOrderController : EAP.Logic.__UserController
    {
        public ActionResult Index(int? PageIndex, int? PageSize, Int64? qCustomerIDStart, Int64? qCustomerIDEnd, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol, int? qOrderType)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            Session["ReturnUrl"] = "/Z10SalesOrder/Index/";

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='javascript:;' onclick='parent.go2(\"/Z10SalesOrder/Create/?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/") + "," + PageSize + "\");' class='btn img'><i class='icon i_create'></i>新建订单<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewBag.TopMenu = sbMenu.ToString();
            ViewBag.PageSize = PageSize ?? 10;

            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            int iOrderType = qOrderType ?? 0;

            Hashtable hs = new Hashtable();
            //if (iOrderType==0) qOrderType = (int)EAP.Logic.Z10.OrderTypes.Sale;
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);
            hs.Add("qOrderType", (int)EAP.Logic.Z10.OrderTypes.Sale);
            hs.Add("qIsSnap", 0);
            hs.Add("qDeleteFlag", (int)EAP.Logic.DeleteFlags.Normal);

            PaginatedList<Z10Order> result = Z10OrderHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;

            return View(result);
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="q"></param>
        /// <param name="brand"></param>
        /// <param name="cate"></param>
        /// <returns></returns>
        public ActionResult Create(string q, long? brand, long? cate)
        {
            //Session.Remove("Z10Order");

            var order = EAP.Logic.Z10.Order.CreateWithSession();
            order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.Sale;
            ViewBag._order = order;
            long iBrand = brand ?? 0;
            long iCate = cate ?? 0;

            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " ProductStatus=" + (int)EAP.Logic.Z01.ProductStatus.Normal;
            if (q.IsNotNullOrEmpty())
            {
                where += " and [Title] like @Title";
                dbParams.Add(db.CreateParameter("Title", "%" + q + "%"));
            }
            if (iBrand > 0)
            {
                where += " and [BrandID] = @BrandID";
                dbParams.Add(db.CreateParameter("BrandID", iBrand));
            }
            if (iCate > 0)
            {
                where += " and [ProductID] in (select [ProductID] from Z01ProductInCategory where [CategoryID]=@CategoryID)";
                dbParams.Add(db.CreateParameter("CategoryID", iCate));

            }

            List<Z01Product> products = db.Take<Z01Product>(where + " order by ViewCount desc", 20, dbParams.ToArray());

            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag._brands = brands;
            var currencies = db.Take<EAP.Bus.Entity.Currency>("1=1 order by DisplayOrder");
            ViewBag._currencies = currencies;
            var banks = db.Take<Z01Bank>("1=1 order by DisplayOrder");
            ViewBag._banks = banks;
            ViewBag._categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);

            return View(products);

        }

        public ActionResult SelectProduct_S1(string q, long? brand, long? cate)
        {
            long iBrand = brand ?? 0;
            long iCate = cate ?? 0;

            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " ProductStatus=" + (int)EAP.Logic.Z01.ProductStatus.Normal;
            if (q.IsNotNullOrEmpty())
            {
                where += " and ([Title] like @Title or [Code] like @Title)";
                dbParams.Add(db.CreateParameter("Title", "%" + q + "%"));
            }
            if (iBrand > 0)
            {
                where += " and [BrandID] = @BrandID";
                dbParams.Add(db.CreateParameter("BrandID", iBrand));
            }
            if (iCate > 0)
            {
                where += " and [ProductID] in (select [ProductID] from Z01ProductInCategory where [CategoryID]=@CategoryID)";
                dbParams.Add(db.CreateParameter("CategoryID", iCate));

            }

            List<Z01Product> products = db.Take<Z01Product>(where + " order by ViewCount desc", 20, dbParams.ToArray());
            System.Text.StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ul class='clearfix'>");
            foreach (var item in products)
            {
                string leftStock = "<br /><strong style='color:red;font-weight:bold'>无货</strong>";
                var xsum = Zippy.Data.StaticDB.DB.Take<Z10Cabbage.Entity.Z10DepotProduct>("ProductID=" + item.ProductID).Sum(s => s.StockSum) ?? 0;
                if (xsum > 0)
                {
                    leftStock = "<br /><span style='color:#009900'>库存：" + xsum.ToString("0.##") + "</span>";
                }
                var brandTitle = string.Empty;
                var entiBrand = db.FindUnique<Z01Brand>(item.BrandID ?? 0);
                if (entiBrand != null)
                    brandTitle = entiBrand.Title;
                var xcode = item.Code;
                if (xcode.IsNotNullOrEmpty()) xcode = "<br /><span>"+xcode+"</span>";
                sb.AppendLine("<li rel='" + item.ProductID + "'>" + brandTitle + " " + item.Title + " " + xcode + leftStock + "</li>");
            }
            sb.AppendLine("</ul>");
            return Content(sb.ToString());

        }
        public ActionResult SelectProduct_S2(long productID, string qTitle, int? xtype)
        {
            string sql = "ProductID=" + productID;
            List<System.Data.Common.DbParameter> paras = new List<System.Data.Common.DbParameter>();
            if (qTitle.IsNotNullOrEmpty())
            {
                sql += " and Title like @Title";
                paras.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
            }

            var result = db.Take<Z10Cabbage.Entity.Z10DepotProductDetail>(sql + " and StockSum>0", paras.ToArray());

            var _product = db.FindUnique<Z01Beetle.Entity.Z01Product>(productID);
            System.Text.StringBuilder sb = new StringBuilder();

            foreach (var item in result)
            {
                sb.AppendLine("<li rel='" + item.DepotProductID + "' pro='" + item.ProductID + "'>");
                sb.Append("<span class='title'>" + _product.Title + " " + item.ExtColor + " " + item.ExtSize + "</span>");
                sb.Append("<span class='count'>(" + item.StockSum.Value.ToString("0.##") + ")</span><br />");
                sb.Append("数量：<input type='text' value='1' class='text w50 ocount' />");
                sb.Append("价格：<input type='text' value='" + (xtype == (int)EAP.Logic.Z10.OrderTypes.SaleLowProfit ? _product.PriceStock.Value.ToString("0.##") :_product.PriceList.Value.ToString("0.##")) + "' class='text w50 oprice' />");
                sb.Append("<input type='button' value='加入订单' class='btn_addProduct' />");
                sb.Append("</li>");
            }
            return Content(sb.ToString());


        }


        public ActionResult SaveProduct_S3(decimal count, decimal? price, long pid, long dpid)
        {
            if (count <= 0)
            {
                return Content("数量必须大于0。");
            }
            var Price = price ?? 0;

            var order = EAP.Logic.Z10.Order.CreateWithSession();
            //order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.Sale;
            Z10Cabbage.Entity.Z10OrderItem item = new Z10OrderItem();
            var productDetail = db.FindUnique<Z10Cabbage.Entity.Z10DepotProductDetail>(dpid);
            var product = db.FindUnique<Z01Beetle.Entity.Z01Product>(pid);
            item.Title = product.Title;
            item.ProductID = pid;
            item.DepotProductDetailID = dpid;
            item.ExtColor = productDetail.ExtColor;
            item.ExtSize = productDetail.ExtSize;
            item.Price = Price;
            item.CountShould = count;
            var xtotal = count * Price;
            item.Total = xtotal;
            item.DepotID = productDetail.DepotID;
            db.ColAdd<Z01Product>("ViewCount", pid);

            order.Items.Add(item);

            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append("<td class='product_title'>");
            sb.Append("<input type='button' value='删' class='btn_del_item' /> ");
            sb.Append(product.Title);
            sb.Append(" ");
            sb.Append(productDetail.ExtColor);
            sb.Append(" ");
            sb.Append(productDetail.ExtSize);
            sb.Append("</td>");
            sb.Append("<td class='product_count tr'>");
            sb.Append(count.ToString("0.##"));
            sb.Append("</td>");
            sb.Append("<td class='product_count tr'>");
            sb.Append(Price.ToString("0.##"));
            sb.Append("</td>");
            sb.Append("<td class='product_total tr'>");
            sb.Append(xtotal.ToString("0.##"));
            sb.Append("</td>");
            sb.Append("</tr>");

            return Content(sb.ToString());
        }

        public ActionResult RemoveOrderItem(int? index)
        {
            var order = EAP.Logic.Z10.Order.CreateWithSession();
            int xindex = index ?? 0;
            if (xindex < 0)
            {
                return Content("错误的序号。");
            }
            else
            {
                order.Items.RemoveAt(xindex);
            }
            return Content("1");
        }

        public ActionResult SaveOrder(Z10Order xorder, bool? IsOutDepot, bool? IsPay, long? Bank)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            order.Z10Order.CustomerID = xorder.CustomerID;
            order.Z10Order.DateOrder = xorder.DateOrder;
            order.Z10Order.DateShip = xorder.DateOrder;
            order.Z10Order.Currency = xorder.Currency;
            order.Z10Order.Remark = xorder.Remark;
            order.Z10Order.FeeShip = xorder.FeeShip;
            order.Z10Order.FeeShould = xorder.FeeShould;

            order.Save(_tenant.TenantID.Value, db, null);

            db.ColAdd<Z01Customer>("ManageHot", xorder.CustomerID ?? 0);


            bool isout = IsOutDepot ?? false;
            bool ispay = IsPay ?? false;

            if (isout)
            {
                order.SaleOutDepot(_tenant.TenantID, _user.UserID);
            }
            if (ispay)
            {
                order.Pay(_tenant.TenantID, _user.UserID, xorder.FeeShould.Value, Bank);
            }
            Session.Remove("Z10Order");
            return Content("1");
        }

        #region 销售统计
        /// <summary>
        /// 总结指定时间的销售
        /// </summary>
        /// <param name="theDate"></param>
        /// <param name="fromDate"></param>
        /// <param name="category"></param>
        /// <param name="brand"></param>
        /// <param name="onlyPaid"></param>
        /// <returns></returns>
        public ActionResult DateSummary(DateTime? toDate, DateTime? fromDate, long? category, long? brand, bool? onlyPaid)
        {
            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var maxDate = toDate ?? today;
            maxDate = maxDate.AddDays(1);
            var minDate = fromDate ?? today;
            var diffDate = maxDate - minDate;
            if (diffDate.TotalDays > 366 || diffDate.TotalDays < 0)
            {
                return Content("不允许超过1年，且结束日期不能大于开始日期。");
            }

            long iCate = category ?? 0;
            long iBrand = brand ?? 0;
            bool iPaid = onlyPaid ?? false;

            StringBuilder sbOrder = new StringBuilder();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
            string sql = "TenantID=@TenantID and DateOrder>=@fromDate and DateOrder<@toDate and OrderType=@Sale";
            dbParams.Add(db.CreateParameter("TenantID", _tenant.TenantID));
            dbParams.Add(db.CreateParameter("fromDate", minDate));
            dbParams.Add(db.CreateParameter("toDate", maxDate));
            dbParams.Add(db.CreateParameter("Sale", (int)EAP.Logic.Z10.OrderTypes.Sale));
            sbOrder.Append("开始日期：" + minDate.ToString("yyyy-MM-dd"));
            sbOrder.Append(" 结束日期：" + maxDate.AddDays(-1).ToString("yyyy-MM-dd"));
            if (iPaid)
            {
                sql += " and (OrderStatus&@Paid)=@Paid";
                dbParams.Add(db.CreateParameter("Paid", (int)EAP.Logic.Z10.OrderStatus.Paid));
            }


            StringBuilder sbOrderItems = new StringBuilder();
            string sqlItems = "1=1";
            List<System.Data.Common.DbParameter> dbParamsItems = new List<System.Data.Common.DbParameter>();
            dbParamsItems.AddRange(dbParams);
            if (iBrand > 0)
            {
                var enBrand = db.FindUnique<Z01Brand>(iBrand);
                sqlItems += " and ProductID in (select ProductID from Z01Product where BrandID=@BrandID)";
                dbParamsItems.Add(db.CreateParameter("BrandID", iBrand));
                sbOrderItems.Append(" 商品品牌：" + enBrand.Title);
            }
            if (iCate > 0)
            {
                var enCate = db.FindUnique<Z01ProductCategory>(iCate);
                sqlItems += " and ProductID in (select ProductID from Z01ProductInCategory where CategoryID=@CategoryID)";
                dbParamsItems.Add(db.CreateParameter("CategoryID", iCate));
                sbOrderItems.Append(" 商品分类：" + enCate.Title);
            }

            var orders = db.Take<Z10Order>(sql, dbParams.ToArray());
            var items = db.Take<Z10OrderItem>(sqlItems + " and OrderID in ( select OrderID from Z10Order where " + sql + ")", dbParamsItems.ToArray());

            ViewBag.orders = orders;
            ViewBag.items = items;
            ViewBag.otitle = sbOrder.ToString();
            ViewBag.oititle = sbOrderItems.ToString();
            ViewBag._categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);
            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag._brands = brands;

            ViewBag.minDate = minDate;
            ViewBag.maxDate = maxDate;


            return View();
        }

        public ActionResult DateSummaryOrderItems(DateTime? toDate, DateTime? fromDate, long? category, long? brand, bool? onlyPaid, int? xorderby)
        {
            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var maxDate = toDate ?? today;
            maxDate = maxDate.AddDays(1);
            var minDate = fromDate ?? today;
            var diffDate = maxDate - minDate;
            if (diffDate.TotalDays > 366 || diffDate.TotalDays < 0)
            {
                return Content("不允许超过1年，且结束日期不能大于开始日期。");
            }

            long iCate = category ?? 0;
            long iBrand = brand ?? 0;
            bool iPaid = onlyPaid ?? false;

            StringBuilder sbOrder = new StringBuilder();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
            string sql = "TenantID=@TenantID and DateOrder>=@fromDate and DateOrder<@toDate and OrderType=@Sale";
            dbParams.Add(db.CreateParameter("TenantID", _tenant.TenantID));
            dbParams.Add(db.CreateParameter("fromDate", minDate));
            dbParams.Add(db.CreateParameter("toDate", maxDate));
            dbParams.Add(db.CreateParameter("Sale", (int)EAP.Logic.Z10.OrderTypes.Sale));
            sbOrder.Append("开始日期：" + minDate.ToString("yyyy-MM-dd"));
            sbOrder.Append(" 结束日期：" + maxDate.AddDays(-1).ToString("yyyy-MM-dd"));
            if (iPaid)
            {
                sql += " and (OrderStatus&@Paid)=@Paid";
                dbParams.Add(db.CreateParameter("Paid", (int)EAP.Logic.Z10.OrderStatus.Paid));
            }


            StringBuilder sbOrderItems = new StringBuilder();
            string sqlItems = "1=1";
            List<System.Data.Common.DbParameter> dbParamsItems = new List<System.Data.Common.DbParameter>();
            dbParamsItems.AddRange(dbParams);
            if (iBrand > 0)
            {
                var enBrand = db.FindUnique<Z01Brand>(iBrand);
                sqlItems += " and ProductID in (select ProductID from Z01Product where BrandID=@BrandID)";
                dbParamsItems.Add(db.CreateParameter("BrandID", iBrand));
                sbOrderItems.Append(" 商品品牌：" + enBrand.Title);
            }
            if (iCate > 0)
            {
                var enCate = db.FindUnique<Z01ProductCategory>(iCate);
                sqlItems += " and ProductID in (select ProductID from Z01ProductInCategory where CategoryID=@CategoryID)";
                dbParamsItems.Add(db.CreateParameter("CategoryID", iCate));
                sbOrderItems.Append(" 商品分类：" + enCate.Title);
            }

            var orders = db.Take<Z10Order>(sql, dbParams.ToArray());
            var items = db.Take<Z10OrderItem>(sqlItems + " and OrderID in ( select OrderID from Z10Order where " + sql + ")", dbParamsItems.ToArray());
            ViewBag.orders = orders;
            ViewBag.items = items;
            ViewBag.otitle = sbOrder.ToString();
            ViewBag.oititle = sbOrderItems.ToString();
            ViewBag._categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);
            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag._brands = brands;

            ViewBag.minDate = minDate;
            ViewBag.maxDate = maxDate;
            ViewBag.xorderby = xorderby ?? 1;
            return View();
        }

        #endregion


        public ActionResult S2_OrderInfo()
        {
            return View();
        }

        #region  调货订单（低利润）
        /// <summary>
        /// 创建调货订单（低利润）
        /// </summary>
        /// <param name="q"></param>
        /// <param name="brand"></param>
        /// <param name="cate"></param>
        /// <returns></returns>
        public ActionResult CreateLowProfit(string q, long? brand, long? cate)
        {
            var order = EAP.Logic.Z10.Order.CreateWithSession();
            order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.SaleLowProfit;
            ViewBag._order = order;
            long iBrand = brand ?? 0;
            long iCate = cate ?? 0;

            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " ProductStatus=" + (int)EAP.Logic.Z01.ProductStatus.Normal;
            if (q.IsNotNullOrEmpty())
            {
                where += " and [Title] like @Title";
                dbParams.Add(db.CreateParameter("Title", "%" + q + "%"));
            }
            if (iBrand > 0)
            {
                where += " and [BrandID] = @BrandID";
                dbParams.Add(db.CreateParameter("BrandID", iBrand));
            }
            if (iCate > 0)
            {
                where += " and [ProductID] in (select [ProductID] from Z01ProductInCategory where [CategoryID]=@CategoryID)";
                dbParams.Add(db.CreateParameter("CategoryID", iCate));

            }

            List<Z01Product> products = db.Take<Z01Product>(where + " order by ViewCount desc", 20, dbParams.ToArray());

            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag._brands = brands;
            var currencies = db.Take<EAP.Bus.Entity.Currency>("1=1 order by DisplayOrder");
            ViewBag._currencies = currencies;
            var banks = db.Take<Z01Bank>("1=1 order by DisplayOrder");
            ViewBag._banks = banks;
            ViewBag._categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);

            return View(products);
        }

        public ActionResult IndexLowProfit(int? PageIndex, int? PageSize, Int64? qCustomerIDStart, Int64? qCustomerIDEnd, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            Session["ReturnUrl"] = "/Z10SalesOrder/IndexLowProfit/";

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='javascript:;' onclick='parent.go2(\"/Z10SalesOrder/CreateLowProfit/?ReturnUrl=" + System.Web.HttpUtility.UrlEncode(Request.RawUrl) + "," + PageSize + "\");' class='btn img'><i class='icon i_create'></i>新建订单<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewBag.TopMenu = sbMenu.ToString();
            ViewBag.PageSize = PageSize ?? 10;

            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);
            hs.Add("qOrderType", (int)EAP.Logic.Z10.OrderTypes.SaleLowProfit);
            hs.Add("qIsSnap", 0);
            hs.Add("qDeleteFlag", (int)EAP.Logic.DeleteFlags.Normal);

            PaginatedList<Z10Order> result = Z10OrderHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;

            return View(result);
        }


        #endregion
    }
}
