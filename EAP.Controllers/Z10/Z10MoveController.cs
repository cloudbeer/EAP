using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using Zippy.Data.Collections;
using System.Collections.Generic;

namespace EAP.Controllers.Z10
{
    public class Z10MoveController : EAP.Logic.__UserController
    {
        public ActionResult MoveList(int? PageIndex, int? PageSize, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

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
            hs.Add("qOrderType", (int)EAP.Logic.Z10.OrderTypes.Transfer);
            hs.Add("qIsSnap", 0);
            hs.Add("qDeleteFlag", (int)EAP.Logic.DeleteFlags.Normal);

            PaginatedList<Z10Order> result = Z10OrderHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View("Index", result);
        }

        #region 创建调拨单
        
        public ActionResult Create()
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");

            Session["Z10Order"] = null;
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);

            return View(order);
        }

        public ActionResult doCreate(Z10Order xorder)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            order.Z10Order.DateOrder = xorder.DateOrder;
            order.Z10Order.Remark = xorder.Remark;
            order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.Transfer;

            order.Save(_tenant.TenantID.Value, db, null);

            Session.Remove("Z10Order");
            return Content("1");
        }
        #endregion

        #region 订单详情

        public ActionResult Detail(long id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            Session["Z10Order"] = order;
            ViewData["db"] = db;

            return View("Show", order);
        }

        #endregion

        #region 修改订单
        /// <summary>
        /// 修改订单G:\LiWill\EAP\LiUI\Views\User\
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ActionResult Edit(long id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            Session["Z10Order"] = order;


            string returnUrl = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty())
                returnUrl = "/" + _ContollerName;

            ViewData["ReturnUrl"] = returnUrl;
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

        #region 选取库存商品
        public ActionResult SelectProduct(long id, int? PageIndex, int? PageSize, long? qProductID, string qProductTitle, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");



            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);

            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qDepotID", id);
            hs.Add("qProductID", qProductID);
            hs.Add("orderCol", orderCol);
            hs.Add("qProductTitle", qProductTitle);

            PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> result = Z10DepotProductHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;

            return View(result);

        }
        #endregion

        #region 删除订单
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

        #region 调拨入库

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
                hsItem.Add(item.ItemID, item.CountHappend);
            }

            var oStatus = (EAP.Logic.Z10.OrderStatus)orderStatus;

            var result = order.InOutDepot(_tenant.TenantID.Value, _user.UserID.Value, oStatus, hsItem, db);

            if (result)
                return Content("1");
            else
                return Content("0");
        }

        #endregion

        #region 调拨出库

        public ActionResult PutOut(long id)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            Session["Z10Order"] = order;
            ViewData["db"] = db;

            return View(order);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SavePutOut(int orderStatus)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            Hashtable hsItem = new Hashtable();

            foreach (var item in order.Items)
            {
                hsItem.Add(item.ItemID, -item.CountHappend2);
            }

            var oStatus = (EAP.Logic.Z10.OrderStatus)orderStatus;

            var result = order.InOutDepot(_tenant.TenantID.Value, _user.UserID.Value, oStatus, hsItem, db);

            if (result)
                return Content("1");
            else
                return Content("0");
        }

        #endregion
    }
}
