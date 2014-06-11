using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using Zippy.Data.Collections;
using System.Collections.Generic;
using EAP.Logic.Z10;

namespace EAP.Controllers.Z10
{
    public class Z10OrderController : EAP.Logic.__UserController
    {
        public ActionResult PurchaseList(int? PageIndex, int? PageSize, Int64? qCustomerIDStart, Int64? qCustomerIDEnd, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='javascript:;' onclick='parent.go2(\"/Z10StockOrder/Create?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/") + "," + PageSize + "\");' class='btn img'><i class='icon i_create'></i>新建订单<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);
            hs.Add("qOrderType", (int)EAP.Logic.Z10.OrderTypes.Purchase);
            hs.Add("qIsSnap", 0);
            hs.Add("qDeleteFlag", (int)EAP.Logic.DeleteFlags.Normal);

            PaginatedList<Z10Order> result = Z10OrderHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }

        #region 订单项编辑
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ModiItem2Session(int? index, Z10OrderItem item)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            //if (order.Items.Where(s => s.ProductID == item.ProductID && s.DepotID == item.DepotID && s.ExtColor == item.ExtColor && s.ExtSize == item.ExtSize).Count() > 0)
            //{
            //    return Content("同一件商品同一个规格不允许多次加入同一个仓库。");
            //}

            int xindex = index ?? 0;
            if (xindex > 0)
            {
                var currentItem = order.Items[xindex - 1]; //order.Items.Where(s => s.ItemID == item.ItemID).FirstOrDefault();

                if (currentItem != null)
                {
                    currentItem.Price = item.Price;
                    currentItem.DepotID = item.DepotID;
                    currentItem.DepotID2 = item.DepotID2;
                    currentItem.CountShould = item.CountShould;
                    currentItem.CountChecked = item.CountChecked;
                    currentItem.CountHappend = item.CountHappend;
                    currentItem.CountHappend2 = item.CountHappend2;
                    currentItem.Total = item.Price * item.CountShould;
                    currentItem.UpdateDate = DateTime.Now;
                    //if (item.CountHappend2 != null)
                    //    currentItem.CountHappend2 = item.CountHappend2;
                    //else if (item.CountHappend != null)
                    //    currentItem.CountHappend = item.CountHappend;
                }
                else
                {
                    return Content("错误的库存商品！");
                }
            }
            else
            {

                return Content("错误的库存商品！");
            }


            if (item.ItemID.HasValue)
            {
                Z10OrderItem mItem = order.Items.Where(s => s.ItemID == item.ItemID).FirstOrDefault();

                if (mItem != null)
                {
                    mItem.CountShould = item.CountShould;
                    mItem.Price = item.Price;
                    mItem.ProductID = item.ProductID;
                    mItem.Total = item.Total;
                    mItem.DepotID = item.DepotID;
                    mItem.UpdateDate = DateTime.Now;
                }
                else
                {
                    return Content("错误的库存商品。");
                }

            }
            else
            {
                //if (order.Items.Where(s => s.ItemStatus != index && s.ProductID == item.ProductID && s.DepotID == item.DepotID).Count() > 0)
                //{
                //    return Content("同一件商品不允许多次加入同一个仓库。");
                //}
                Z10OrderItem mItem = order.Items.Where(s => s.ItemStatus == index).FirstOrDefault();

                if (mItem != null)
                {
                    mItem.CountShould = item.CountShould;
                    mItem.Price = item.Price;
                    mItem.ProductID = item.ProductID;
                    mItem.Total = item.Total;
                    mItem.UpdateDate = DateTime.Now;
                }
                else
                {
                    return Content("错误的库存商品。");
                }

            }

            return Content("1");
        }

        /// <summary>
        /// 修改库存数量
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ModiItemCountHappend2Session(int? index, Z10OrderItem item)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();
            int xindex = index ?? 0;
            if (xindex > 0)
            {
                var currentItem = order.Items[xindex - 1]; //order.Items.Where(s => s.ItemID == item.ItemID).FirstOrDefault();

                if (currentItem != null)
                {
                    currentItem.Price = item.Price;
                    currentItem.DepotID = item.DepotID;
                    currentItem.DepotID2 = item.DepotID2;
                    currentItem.CountShould = item.CountShould;
                    currentItem.CountChecked = item.CountChecked;
                    currentItem.CountHappend = item.CountHappend;
                    currentItem.CountHappend2 = item.CountHappend2;
                    //if (item.CountHappend2 != null)
                    //    currentItem.CountHappend2 = item.CountHappend2;
                    //else if (item.CountHappend != null)
                    //    currentItem.CountHappend = item.CountHappend;
                }
                else
                {
                    return Content("错误的库存商品！");
                }
            }
            else
            {

                return Content("错误的库存商品！");
            }

            return Content("1");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveItem2Session(int? index, Z10OrderItem item)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();
            if (item.ItemID.HasValue && item.ItemID > 0)
            {
                Z10OrderItem mItem = order.Items.Where(s => s.ItemID == item.ItemID).FirstOrDefault();
                if (mItem != null)
                {
                    mItem.ExtColor = item.ExtColor;
                    mItem.ExtSize = item.ExtSize;
                    mItem.CountShould = item.CountShould;
                    mItem.Price = item.Price;
                    mItem.ProductID = item.ProductID;
                    mItem.Total = item.Total;
                    mItem.UpdateDate = DateTime.Now;
                }
                else
                {
                    return Content("错误的库存商品。");
                }
            }
            else
            {
                if (order.Items.Where(s => s.ProductID == item.ProductID && s.DepotID == item.DepotID && s.ExtColor == item.ExtColor && s.ExtSize == item.ExtSize).Count() > 0)
                {
                    return Content("同一件商品不允许多次加入同一个仓库。");
                }
                item.ItemStatus = index;
                item.Total = item.Price * item.CountShould;
                order.Items.Add(item);

                //return Content("商品数量" + order.Items.Count);
            }
            //Session["Z10Order"] = order;
            return Content("1");
        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveItem2Session(int? productid)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            if (productid.Value > 0)
            {
                EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

                var item = order.Items.Where<Z10OrderItem>(s => s.ProductID == productid).FirstOrDefault();

                //order.Items.RemoveAt()
                if (item != null)
                    order.Items.Remove(item);

                //Session["Z10Order"] = order;
            }

            return Content("1");
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
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            Session["Z10Order"] = order;
            return Redirect("/z10order/Purchase/");

            //if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");

            //ViewData["VTitle"] = "修改采购单";


            //string returnUrl = Request["ReturnUrl"];
            //if (returnUrl.IsNullOrEmpty())
            //    returnUrl = "/" + _ContollerName;

            //ViewData["ReturnUrl"] = returnUrl;
            //ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            //ViewData["MoneyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);
            //ViewData["db"] = db;
            //return View(order);
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

        #region 订单详情

        public ActionResult Detail(long id)
        {

            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            if (order.Z10Order == null)
            {
                return Content("订单不存在。");
            }

            EAP.Logic.Z10.OrderTypes otype = (EAP.Logic.Z10.OrderTypes)(order.Z10Order.OrderType ?? 0);
            if (otype.BitIs(EAP.Logic.Z10.OrderTypes.Purchase))
                ViewData["VTitle"] = "采购单";
            else if (otype.BitIs(EAP.Logic.Z10.OrderTypes.Sale))
                ViewData["VTitle"] = "销售单";
            else if (otype.BitIs(EAP.Logic.Z10.OrderTypes.Transfer))
                ViewData["VTitle"] = "调库单";
            else
                ViewData["VTitle"] = "订单";
            string rUrl = Request["ReturnUrl"];
            if (rUrl.IsNullOrEmpty())
                rUrl = "/Z10Order/PurchaseList";
            ViewData["ReturnUrl"] = rUrl;

            //Session["Z10Order"] = order;
            ViewBag._user = _user;
            ViewData["db"] = db;

            return View(order);
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

        #region 创建采购单
        public ActionResult Purchase()
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");

            //Session["Z10Order"] = null;
            ViewData["VTitle"] = "新建采购单";
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            ViewData["CurrentDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            ViewData["MoneyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(order.Z10Order.Currency, db);

            return View(order);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PurchaseSave(Z10Order xorder)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.CreateWithSession();

            order.Z10Order.CustomerID = xorder.CustomerID;
            order.Z10Order.DateOrder = xorder.DateOrder;
            order.Z10Order.DateShip = xorder.DateShip;
            order.Z10Order.Currency = xorder.Currency;
            order.Z10Order.Remark = xorder.Remark;
            order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.Purchase;
            order.Z10Order.FeeShip = xorder.FeeShip;
            order.Z10Order.FeeShould = xorder.FeeShould;

            order.Save(_tenant.TenantID.Value, db, null);
            Session.Remove("Z10Order");
            return Content("1");
        }
        #endregion

        /// <summary>
        /// 计算并保存订单费用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        public ActionResult calfee(long id, decimal? ship)
        {
            if (id == 0)
            {
                var order0 = EAP.Logic.Z10.Order.CreateWithSession();
                var sum = order0.Items.Sum(s => s.Total) ?? 0;
                return Content(sum.ToString("0.##"));
            }
            decimal xship = ship ?? 0;
            //decimal xfeeshould = feeshould ?? 0;
            Z10Order order = new Z10Order();
            order.OrderID = id;
            order.FeeShip = ship;
            var sumTotal = db.ExecuteScalar("update Z10OrderItem set Total=Price*CountShould where OrderID=@orderid;  select sum(Total) from Z10OrderItem where OrderID=@orderid", db.CreateParameter("orderid", id)).ToDecimal();
            order.Total = sumTotal;
            db.Update(order);
            return Content(sumTotal.ToString());
        }
        public ActionResult savefee(long id, decimal? ship, decimal? feeShould)
        {
            decimal xship = ship ?? 0;
            decimal xfeeshould = feeShould ?? 0;
            Z10Order order = new Z10Order();
            order.OrderID = id;
            order.FeeShip = ship;
            order.FeeShould = feeShould;

            db.Update(order);
            return Content("1");
        }

        #region 采购入库
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">对应数据列：ItemID</param>
        /// <param name="count">入库的数量</param>
        /// <returns></returns>
        public ActionResult PutItemIn(long id, int count)
        {
            var item = db.FindUnique<Z10OrderItem>(id);
            if (item != null)
            {
                EAP.Logic.Z10.Order.PutInOut(_tenant.TenantID, _user.UserID, item, count);
                return Content("1");
            }
            else
            {
                return Content("错误的订单项。");
            }
        }

        public ActionResult PutIn(long id)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            //Session["Z10Order"] = order;
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

        public ActionResult ClearSessionOrder(string bUrl)
        {
            Session.Remove("Z10Order");
            return Redirect(bUrl);
        }

        public ActionResult SaveOrderStatus(long id, int? orderStatus)
        {
            OrderStatus oStatus = (OrderStatus)(orderStatus ?? 0);
            Z10Order xorder = db.FindUnique<Z10Order>(id);

            if (oStatus == OrderStatus.Inned && ((xorder.OrderStatus ?? 0) & (int)OrderStatus.InnedSome) == (int)OrderStatus.InnedSome)
            {
                oStatus = oStatus & (~OrderStatus.InnedSome);
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) & (~(int)OrderStatus.InnedSome);
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) | (int)oStatus;
            }
            else if (oStatus == OrderStatus.Outted && ((xorder.OrderStatus ?? 0) & (int)OrderStatus.OuttedSome) == (int)OrderStatus.OuttedSome)
            {
                oStatus = oStatus & (~OrderStatus.OuttedSome);
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) & (~(int)OrderStatus.OuttedSome);
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) | (int)oStatus;
            }
            else
            {
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) | (int)oStatus;
            }
            Z10Order xorder1 = new Z10Order();
            xorder1.OrderID = id;
            xorder1.OrderStatus = xorder.OrderStatus;

            db.Update(xorder1);
            return Content("1");
        }

        #region 采购单付款

        public ActionResult Pay(long id)
        {
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);

            ViewBag.banks = db.Take<Z01Beetle.Entity.Z01Bank>("TenantID=@TenantID order by DisplayOrder", db.CreateParameter("TenantID", _tenant.TenantID));
            ViewBag.customer = db.FindUnique<Z01Beetle.Entity.Z01Customer>(order.Z10Order.CustomerID ?? 0);
            ViewBag.currency = order.Z10Order.Currency;


            return View(order);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Pay(long id, long bank, decimal feePaid, int orderStatus, string returnUrl)
        {
            decimal absPaid = Math.Abs(feePaid);
            EAP.Logic.Z10.Order order = EAP.Logic.Z10.Order.LoadFromDB(id, _tenant.TenantID.Value, db);
            if (order.Z10Order.OrderType.BitIs(EAP.Logic.Z10.OrderTypes.Purchase) || order.Z10Order.OrderType.BitIs(EAP.Logic.Z10.OrderTypes.SaleReturn))
                feePaid = -absPaid;
            else if (order.Z10Order.OrderType.BitIs(EAP.Logic.Z10.OrderTypes.Sale) || order.Z10Order.OrderType.BitIs(EAP.Logic.Z10.OrderTypes.PurchaseReturn))
                feePaid = absPaid;

            var xOrderStatus = (EAP.Logic.Z10.OrderStatus)orderStatus;

            //throw new Exception(feePaid.ToString());

            if (order != null)
            {
                var result = order.Pay(_tenant.TenantID.Value, _user.UserID.Value, xOrderStatus, feePaid, bank);

                if (result)
                    return Redirect(returnUrl);
                else
                    return Content("系统内部错误，请联系管理员。");
            }
            else
            {
                return Content("不存在的采购单信息！");
            }
        }

        #endregion

        #region 辅助页面
        public ActionResult SelectCustomer(int? PageIndex, int? PageSize, string qTitle, int? qCustomerType, long? orderID, string act)
        {
            int currentPageSize = PageSize ?? 8;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qCustomerType", qCustomerType);

            ViewBag.qCustomerType = qCustomerType;
            Z01Beetle.Entity.Z01Customer cus = new Z01Beetle.Entity.Z01Customer();

            ViewBag.act = act;
            ViewBag.orderID = orderID ?? 0;


            PaginatedList<Z01Beetle.Entity.Z01Customer> result = Z01Beetle.Entity.Helper.Z01CustomerHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, 0);
            result.QueryParameters = hs;
            return View(result);
        }

        public ActionResult SelectProduct(int? PageIndex, int? PageSize, string qTitle)
        {
            int currentPageSize = PageSize ?? 8;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);

            PaginatedList<Z01Beetle.Entity.Z01Product> result = Z01Beetle.Entity.Helper.Z01ProductHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, 0);
            result.QueryParameters = hs;
            return View(result);
        }

        public ActionResult SelectProduct_S1(int? PageIndex, int? PageSize, string qTitle)
        {
            int currentPageSize = PageSize ?? 8;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);

            PaginatedList<Z01Beetle.Entity.Z01Product> result = Z01Beetle.Entity.Helper.Z01ProductHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, 0);
            result.QueryParameters = hs;
            return View(result);
        }
        public ActionResult SelectProduct_S2(long productID, string qTitle)
        {
            string sql = "ProductID=" + productID;
            List<System.Data.Common.DbParameter> paras = new List<System.Data.Common.DbParameter>();
            if (qTitle.IsNotNullOrEmpty())
            {
                sql += " and Title like @Title";
                paras.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
            }

            ViewBag._product = db.FindUnique<Z01Beetle.Entity.Z01Product>(productID);

            var result = db.Take<Z10Cabbage.Entity.Z10DepotProductDetail>(sql + " and StockSum>0", paras.ToArray());
            return View(result);
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


        #region 销售出库

        public ActionResult CreateSellingOrder()
        {
            return View();
        }


        public ActionResult SaveOutOrderItem(int? index, int CountShould, decimal? Price, long ProductID, long itemid)
        {
            if (CountShould <= 0)
            {
                return Content("数量必须大于0。");
            }

            var order = EAP.Logic.Z10.Order.CreateWithSession();
            int xindex = index ?? 0;
            if (xindex <= 0)
            {
                order.Z10Order.OrderType = (int)EAP.Logic.Z10.OrderTypes.Sale;
                Z10Cabbage.Entity.Z10OrderItem item = new Z10OrderItem();
                var productDetail = db.FindUnique<Z10Cabbage.Entity.Z10DepotProductDetail>(itemid);
                var product = db.FindUnique<Z01Beetle.Entity.Z01Product>(ProductID);
                item.Title = product.Title;
                item.ProductID = ProductID;
                item.DepotProductDetailID = itemid;
                item.ExtColor = productDetail.ExtColor;
                item.ExtSize = productDetail.ExtSize;
                item.Price = Price;
                item.CountShould = CountShould;
                item.Total = CountShould * (Price ?? 0);
                item.DepotID = productDetail.DepotID;

                order.Items.Add(item);

            }
            else
            {
                var item = order.Items[xindex - 1];
                item.Price = Price;
                item.CountShould = CountShould;
            }


            return Content("1");
        }
        /// <summary>
        /// 从 Session 的order 中移除某项
        /// </summary>
        /// <param name="index">索引号</param>
        /// <returns></returns>
        public ActionResult RemoveOutOrderItem(int? index)
        {
            var order = EAP.Logic.Z10.Order.CreateWithSession();
            int xindex = index ?? 0;
            if (xindex <= 0)
            {
                return Content("错误的序号。");
            }
            else
            {
                order.Items.RemoveAt(xindex - 1);
            }
            return Content("1");
        }

        /// <summary>
        /// 商品出库
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="CountHappend2">实际发生的出库数量</param>
        /// <returns></returns>
        public ActionResult OutDepotItem(long ItemID, int CountHappend2)
        {
            var item = db.FindUnique<Z10OrderItem>(ItemID);
            if (item == null)
                return Content("错误的商品");

            try
            {
                EAP.Logic.Z10.Order.PutInOut(_tenant.TenantID, _user.UserID, item, -CountHappend2);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return Content("1");
        }

        #endregion

        #region 修改订单信息
        /// <summary>
        /// 改用户
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public ActionResult ChangeOrderCustomer(long oid, long cid)
        {
            EAP.Logic.DictResponse res = new Logic.DictResponse();
            Z10Order data = new Z10Order();
            data.OrderID = oid;
            data.CustomerID = cid;
            db.Update(data);
            res._state = true;

            return Content(res.ToJson());
        }



        public ActionResult ChangeStatus(long oid, int xsts, bool isAdd)
        {
            Logic.DictResponse res = new Logic.DictResponse();
            res._state = false;
            res._message = "有错误";
            string sql = "";
            if (isAdd)
            {
                sql = "update Z10Order set OrderStatus=(OrderStatus|" + xsts + ") where OrderID=" + oid;
            }
            else
            {
                sql = "update Z10Order set OrderStatus=(OrderStatus&~" + xsts + ") where OrderID=" + oid;
            }

            db.ExecuteNonQuery(sql);

            res._state = true;

            return Content(res.ToJson());
        }

        #endregion

        public ActionResult ClearOrder()
        {
            Session.Remove("Z10Order");
            return Return();
        }

        public ActionResult DeleteOrder(long id)
        {
            Logic.DictResponse res = new Logic.DictResponse();
            res._state = false;

            return Content(res.ToJson());
        }
    }
}
