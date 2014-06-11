using System;
using System.Collections;
using System.Web.Mvc;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using Zippy.Data.Collections;

namespace EAP.Controllers.Z10
{
    public class Z10SaleReturnedController : EAP.Logic.__UserController
    {
        public ActionResult SaleReturnedList(int? PageIndex, int? PageSize, Int64? qCustomerIDStart, Int64? qCustomerIDEnd, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            Session["ReturnUrl"] = "/Z10SaleReturned/SaleReturnedList";

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='javascript:;' onclick='parent.go2(\"/" + _ContollerName + "/Create?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/") + "," + PageSize + "\");' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);
            int currentPageSize  = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);
            hs.Add("qOrderType", (int)EAP.Logic.Z10.OrderTypes.SaleReturn);
            hs.Add("qIsSnap", 0);
            hs.Add("qDeleteFlag", (int)EAP.Logic.DeleteFlags.Normal);

            PaginatedList<Z10Order> result = Z10OrderHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View("Index", result);
        }

        #region 创建退货单
        public ActionResult Create()
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");

            Session["Z10Order"] = null;
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            ViewData["MoneyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);

            return View(order);
        }

        public ActionResult doReturned(Z10Order xorder)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            order.Z10Order.CustomerID = xorder.CustomerID;
            order.Z10Order.DateOrder = xorder.DateOrder;
            order.Z10Order.DateShip = xorder.DateShip;
            order.Z10Order.Currency = xorder.Currency;
            order.Z10Order.Remark = xorder.Remark;
            order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.SaleReturn;

            order.Save(_tenant.TenantID.Value, db, null);

            Session.Remove("Z10Order");
            return Content("1");
        }
        #endregion

        #region 修改退货单
        /// <summary>
        /// 修改订单G:\LiWill\EAP\LiUI\Views\User\
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult Edit(long id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            ViewData["VTitle"] = "修改退货单";
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            Session["Z10Order"] = order;

            ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            ViewData["MoneyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);
            ViewData["db"] = db;
            return View(order);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSave(long id, Z10Order xorder)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order.Snap(id, _tenant.TenantID.Value, _user.UserID.Value, db); //制作快照

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();
            order.Z10Order.OrderID = null;
            order.Z10Order.OriID = id;
            order.Z10Order.CustomerID = xorder.CustomerID;
            order.Z10Order.DateOrder = xorder.DateOrder;
            order.Z10Order.DateShip = xorder.DateShip;
            order.Z10Order.Currency = xorder.Currency;
            order.Z10Order.Remark = xorder.Remark;

            foreach (Z10OrderItem item in order.Items)
            {
                item.ItemID = null;
            }
            order.Save(_tenant.TenantID.Value, db, id);
            Session.Remove("Z10Order");
            return Content("1");
        }

        #endregion

        #region 删除订单（非物理删除）
        public ActionResult Delete(long id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);

            var index = Z10OrderHelper.Delete(db, order.Z10Order);

            if (index > 0)
                return Content("1");
            else
                return Content("0");
        }


        /// <summary>
        /// 批量删除，仅适用id为整形的处理。
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteBatch(string ids)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");

            try
            {
                ids = ids.RemoveLastChar();
            }
            catch
            {
                return Content("您没有选择任何数据！");
            }

            var tempIds = ids.Split(',');

            foreach (var id in tempIds)
            {
                if (id.ToInt32() == 0)
                    return Content("提交的数据中有不安全的字符,已被系统拒绝！");
            }

            var xSql = "UPDATE [Z10Order] SET [DeleteFlag] = " + (int)EAP.Logic.DeleteFlags.Deleted + " WHERE [OrderID] in (" + ids + ",0)";

            if (db.ExecuteNonQuery(xSql) > 0)
                return Content("1");
            else
                return Content("0");
        }

        #endregion

        #region 条款附加

        public ActionResult Terms(long id)
        {
            Z10Order xorder = db.FindUnique<Z10Order>("OrderID=@OrderID and TenantID=@TenantID", db.CreateParameter("OrderID", id),
               db.CreateParameter("TenantID", _tenant.TenantID));
            ViewData["TempOptions"] = EAP.Logic.Z01.HtmlHelper.PaperTemplateSelectOptions(_tenant.TenantID.Value, db);

            return View(xorder);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Terms(long id, FormCollection cols)
        {
            Z10Order xorder = new Z10Order();
            xorder.OrderID = id;
            xorder.Terms = cols["Terms"];
            db.Update(xorder);
            return Return();
        }
        #endregion

        #region 出库单收款

        public ActionResult Pay(long id)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);

            ViewData["BankList"] = EAP.Logic.Z01.HtmlHelper.BankSelectOptions(_tenant.TenantID.Value);

            ViewData["db"] = db;

            Session["Z10Order"] = order;

            return View(order);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PayAction(long bank, decimal feePaid, int orderStatus)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            var xOrderStatus = (EAP.Logic.Z10.OrderStatus)orderStatus;
            if (order != null)
            {
                var result = order.Pay(_tenant.TenantID.Value, _user.UserID.Value, xOrderStatus, feePaid, bank);

                if (result)
                    return Content("1");
                else
                    return Content("系统内部错误，请联系管理员。");
            }
            else
            {
                return Content("不存在的出库单！");
            }
        }

        #endregion

        #region 出库

        public ActionResult PutIn(long id)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            Session["Z10Order"] = order;
            ViewData["db"] = db;

            return View(order);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SavePutIn(int orderStatus)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            Hashtable hsItem = new Hashtable();

            foreach (var item in order.Items)
            {
                hsItem.Add(item.ItemID, -item.CountHappend);
            }

            var oStatus = (EAP.Logic.Z10.OrderStatus)orderStatus;

            var result = order.InOutDepot(_tenant.TenantID.Value, _user.UserID.Value, oStatus, hsItem, db);

            if (result)
                return Content("1");
            else
                return Content("0");
        }

        #endregion

        #region 退货单详情

        public ActionResult Detail(long id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            Session["Z10Order"] = order;
            ViewData["db"] = db;

            return View("Show", order);
        }

        #endregion
    }
}
