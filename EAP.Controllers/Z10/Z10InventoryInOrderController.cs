using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using Zippy.Data.Collections;

namespace EAP.Controllers.Z10
{
    public class Z10InventoryInOrderController : EAP.Logic.__UserController
    {
        public ActionResult Index(int? PageIndex, int? PageSize, Int64? qCustomerIDStart, Int64? qCustomerIDEnd, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='javascript:;' onclick='parent.go2(\"/Z10InventoryInOrder/Create?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/") + "," + PageSize + "\");' class='btn img'><i class='icon i_create'></i>新建订单<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewBag.PageSize = PageSize ?? 10;
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);
            hs.Add("qOrderType", (int)EAP.Logic.Z10.OrderTypes.InventoryIn);
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
            Session.Remove("Z10InventoryInOrder");

            var order = EAP.Logic.Z10.Order.CreateWithSession("Z10InventoryInOrder");
            ViewBag._order = order;
            long iBrand = brand ?? 0;
            long iCate = cate ?? 0;

            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = "1=1";
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
            var depots = db.Take<Z10Depot>();
            ViewBag._depots = depots;
            var units = db.Take<Z01Unit>();
            ViewBag._units = units;
            ViewBag._categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);


            return View(products);

        }
        public ActionResult SelectProduct_S2(long productID, string qTitle)
        {
            var _product = db.FindUnique<Z01Beetle.Entity.Z01Product>(productID);
            EAP.Logic.DictResponse res = new Logic.DictResponse();
            if (_product != null)
            {
                res._state = true;
                res._data.Add("price", _product.PriceStock.Value.ToString("0.##"));
            }
            else
            {
                res._state = false;
                res._message = "错误的商品。";
            }
            return Content(res.ToJson());
        }


        public ActionResult SaveProduct_S3(decimal count, decimal? price, long pid, long? did, string color, string size)
        {
            if (count <= 0)
            {
                return Content("数量必须大于0。");
            }
            var Price = price ?? 0;

            var order = EAP.Logic.Z10.Order.CreateWithSession("Z10InventoryInOrder");
            order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.InventoryIn;
            Z10Cabbage.Entity.Z10OrderItem item = new Z10OrderItem();
            var product = db.FindUnique<Z01Beetle.Entity.Z01Product>(pid);
            item.Title = product.Title;
            item.ProductID = pid;
            item.ExtColor = color;
            item.ExtSize = size;
            item.Price = Price;
            item.CountShould = count;
            var xtotal = count * Price;
            item.Total = xtotal;
            item.DepotID = did;
            db.ColAdd<Z01Product>("ViewCount", pid);
            order.Items.Add(item);

            string depotTitle = "";
            var depot = db.FindUnique<Z10Depot>(did);
            if (depot != null)
                depotTitle = depot.Title;
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append("<td class='product_title'>");
            sb.Append("<input type='button' value='删' class='btn_del_item' /> ");
            sb.Append(product.Title);
            sb.Append(" ");
            sb.Append(color);
            sb.Append(" ");
            sb.Append(size);
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append(depotTitle);
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
            var order = EAP.Logic.Z10.Order.CreateWithSession("Z10InventoryInOrder");
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


        public ActionResult SaveOrder(Z10Order xorder, bool? IsInDepot, bool? IsPay, long? Bank)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession("Z10InventoryInOrder");

            order.Z10Order.CustomerID = xorder.CustomerID;
            order.Z10Order.DateOrder = xorder.DateOrder;
            order.Z10Order.DateShip = xorder.DateOrder;
            order.Z10Order.Currency = xorder.Currency;
            order.Z10Order.Remark = xorder.Remark;
            order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.InventoryIn;
            order.Z10Order.FeeShip = xorder.FeeShip;
            order.Z10Order.FeeShould = xorder.FeeShould;

            order.Save(_tenant.TenantID.Value, db, null);
            Session.Remove("Z10InventoryInOrder");


            order.StockInDepot(_tenant.TenantID, _user.UserID);

            return Content("1");
        }


    }
}
