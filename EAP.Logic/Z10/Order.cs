using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z10Cabbage.Entity;
using System.Collections;

namespace EAP.Logic.Z10
{
    public class Order
    {
        public Order()
        {
            Items = new List<Z10OrderItem>();
        }
        public Z10Order Z10Order { get; set; }
        public List<Z10OrderItem> Items { get; set; }

        /// <summary>
        /// 在 Session 中创建或获取一个订单。
        /// 如果把订单保存在session中，仍然会存在多个订单窗口间迷惑的问题
        /// </summary>
        /// <returns></returns>
        public static Order CreateWithSession()
        {
            return CreateWithSession("Z10Order");
        }

        public static Order CreateWithSession(string orderKey)
        {
            Order order = null;
            if (System.Web.HttpContext.Current.Session[orderKey] != null)
            {
                order = System.Web.HttpContext.Current.Session[orderKey] as Order;
            }
            else
            {
                order = new Order();
                order.Z10Order = new Z10Order();
                System.Web.HttpContext.Current.Session[orderKey] = order;
            }

            return order;
        }
        /// <summary>
        /// 从数据库中载入订单
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="tenantID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Order LoadFromDB(long orderID, Guid tenantID, Zippy.Data.IDalProvider db)
        {
            Order order = new Order();
            order.Z10Order = db.FindUnique<Z10Order>("OrderID=@OrderID and TenantID=@TenantID",
                db.CreateParameter("OrderID", orderID),
                db.CreateParameter("TenantID", tenantID));
            order.Items = db.Take<Z10OrderItem>("OrderID=@OrderID and TenantID=@TenantID",
                db.CreateParameter("OrderID", orderID),
                db.CreateParameter("TenantID", tenantID));
            return order;
        }

        /// <summary>
        /// 制作一个订单的快照
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="tenantID"></param>
        /// <param name="userID"></param>
        /// <param name="db"></param>
        public static void Snap(long orderID, Guid tenantID, Guid userID, Zippy.Data.IDalProvider db)
        {
            Order order = new Order();
            order.Z10Order = db.FindUnique<Z10Order>("OrderID=@OrderID and TenantID=@TenantID",
                db.CreateParameter("OrderID", orderID),
                db.CreateParameter("TenantID", tenantID));
            order.Items = db.Take<Z10OrderItem>("OrderID=@OrderID and TenantID=@TenantID",
                db.CreateParameter("OrderID", orderID),
                db.CreateParameter("TenantID", tenantID));

            order.Z10Order.IsSnap = 1;
            order.Z10Order.UpdateDate = DateTime.Now;
            order.Z10Order.Updater = userID;
            foreach (Z10OrderItem item in order.Items)
            {
                item.UpdateDate = DateTime.Now;
                item.Updater = userID;
            }

            order.Save(tenantID, db, null);
        }

        /// <summary>
        /// 将订单保存到数据库
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="db"></param>
        /// <param name="oid"></param>
        public void Save(Guid tenantID, Zippy.Data.IDalProvider db, long? oid)
        {
            long orderID = 0;
            Z10Order.Total = Items.Sum(s => s.Total);
            Z10Order.TenantID = tenantID;
            try
            {
                // 此处应该加入事务
                db.BeginTransaction();
                if (Z10Order.OrderID.HasValue && Z10Order.OrderID > 0)
                {
                    orderID = Z10Order.OrderID.Value;
                    db.Update(Z10Order);
                }
                else
                {
                    orderID = db.Insert(Z10Order);

                    Z10Order zorder = new Z10Order();

                    if (!oid.HasValue)
                    {
                        zorder.OrderID = orderID;
                        zorder.OriID = orderID;
                    }
                    else
                    {
                        zorder.OriID = oid;
                    }

                    //zorder.FeeShould = this.CalFeeShould();

                    db.Update(zorder);
                }

                foreach (Z10OrderItem item in Items)
                {
                    item.ItemStatus = 1;
                    item.TenantID = tenantID;
                    item.Total = (item.Price ?? 0) * (item.CountShould ?? 0);
                    if (item.ItemID.HasValue && item.ItemID > 0 && item.OrderID == orderID)
                    {
                        db.Update(item);
                    }
                    else
                    {
                        item.OrderID = orderID;
                       var itemID = db.Insert(item);
                       item.ItemID = itemID;
                    }
                }

                Z10Order.OrderID = orderID;
                db.Commit();
            }
            catch (Exception ex)
            {
                db.RollbackTransaction();
                throw ex;
            }

        }

        /// <summary>
        /// 计算此订单的应付费用。
        /// </summary>
        /// <returns></returns>
        public decimal CalFeeShould()
        {
            decimal feeSum = 0;
            Items.ForEach(s =>
            {
                feeSum += (s.Price ?? 0) * (s.CountShould ?? 0);
            });

            return feeSum;
        }

        public bool Pay(Guid? tenantID, Guid? userID, decimal paidAmount, long? bankID)
        {
            int orderSts = Z10Order.OrderStatus ?? 0;
            if ((orderSts & (int)EAP.Logic.Z10.OrderStatus.Paid) == (int)EAP.Logic.Z10.OrderStatus.Paid ||
                (orderSts & (int)EAP.Logic.Z10.OrderStatus.Finished) == (int)EAP.Logic.Z10.OrderStatus.Finished ||
                (orderSts & (int)EAP.Logic.Z10.OrderStatus.Archived) == (int)EAP.Logic.Z10.OrderStatus.Archived)
            {
                throw new Exception("已经支付的订单，已经结束的订单和已经存档的订单不能支付。");
            }
            Zippy.Data.IDalProvider db = Zippy.Data.StaticDB.DB;

            Z10Order xorder = new Z10Order();
            xorder.OrderID = Z10Order.OrderID;
            xorder.FeePaid = (Z10Order.FeePaid??0) + paidAmount;
            db.Update(xorder);

            Z01Beetle.Entity.Z01FinancialFlow dflow = new Z01Beetle.Entity.Z01FinancialFlow();

            dflow.FlowID = null;
            dflow.OrderID = Z10Order.OrderID;
            dflow.Amount = paidAmount;
            dflow.Creator = userID;
            dflow.TenantID = tenantID;
            dflow.BankID = bankID;
            dflow.Currency = Z10Order.Currency;
            db.Insert(dflow);


            string sqlUpdateStatus = "update Z10Order set OrderStatus=OrderStatus|@paid where OrderID=@orderid ";
            Zippy.Data.StaticDB.DB.ExecuteNonQuery(sqlUpdateStatus,
                Zippy.Data.StaticDB.DB.CreateParameter("paid", (int)EAP.Logic.Z10.OrderStatus.Paid),
                Zippy.Data.StaticDB.DB.CreateParameter("orderid", Z10Order.OrderID ?? 0));


            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="userID"></param>
        /// <param name="oStatus">用户指定的状态</param>
        /// <param name="paidAmount">支付金额</param>
        /// <param name="bankID">银行</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Pay(Guid tenantID, Guid userID, EAP.Logic.Z10.OrderStatus oStatus, decimal paidAmount, long? bankID)
        {

            int orderSts = Z10Order.OrderStatus ?? 0;
            if ((orderSts & (int)EAP.Logic.Z10.OrderStatus.Paid) == (int)EAP.Logic.Z10.OrderStatus.Paid ||
                (orderSts & (int)EAP.Logic.Z10.OrderStatus.Finished) == (int)EAP.Logic.Z10.OrderStatus.Finished ||
                (orderSts & (int)EAP.Logic.Z10.OrderStatus.Archived) == (int)EAP.Logic.Z10.OrderStatus.Archived)
            {
                throw new Exception("已经支付的订单，已经结束的订单和已经存档的订单不能支付。");
            }

            Zippy.Data.IDalProvider db = Zippy.Data.StaticDB.DB;

            Z10Order xorder = new Z10Order();
            xorder.OrderID = Z10Order.OrderID;
            xorder.FeePaid = Z10Order.FeePaid + paidAmount;

            if (oStatus == OrderStatus.Paid)
            {
                xorder.OrderStatus = orderSts & ~(int)OrderStatus.PaidSome;
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) | (int)oStatus;
            }
            else
            {
                //throw new Exception(orderSts.ToString() + "  -- " + oStatus + " -- " + ((Z10Order.OrderStatus ?? 0) | (int)oStatus));
                xorder.OrderStatus = orderSts | (int)oStatus;
            }

            db.Update(xorder);

            Z01Beetle.Entity.Z01FinancialFlow dflow = new Z01Beetle.Entity.Z01FinancialFlow();

            dflow.FlowID = null;
            dflow.OrderID = Z10Order.OrderID;
            dflow.Amount = paidAmount;
            dflow.Creator = userID;
            dflow.TenantID = tenantID;
            dflow.BankID = bankID;
            dflow.Currency = Z10Order.Currency;
            db.Insert(dflow);


            //string sqlUpdateStatus = "update Z10Order set OrderStatus=OrderStatus|@paid where OrderID=@orderid ";
            //Zippy.Data.StaticDB.DB.ExecuteNonQuery(sqlUpdateStatus,
            //    Zippy.Data.StaticDB.DB.CreateParameter("paid", (int)EAP.Logic.Z10.OrderStatus.Paid),
            //    Zippy.Data.StaticDB.DB.CreateParameter("orderid", Z10Order.OrderID ?? 0));


            return true;
        }

        /// <summary>
        /// 将整个订单入库(完成整个订单)
        /// </summary>
        /// <param name="tenantID">租户</param>
        /// <param name="userID">操作者</param>
        /// <param name="order">订单</param>
        /// <returns></returns>
        public bool PutIn(Guid tenantID, Guid userID, EAP.Logic.Z10.Order order)
        {
            return false;
        }

        /// <summary>
        /// 单项入库和出库（按照明细一个一个的入库）
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="userID"></param>
        /// <param name="item"></param>
        /// <param name="itemHappened">当前发生的入库量</param>
        /// <returns></returns>
        public static void PutInOut(Guid? tenantID, Guid? userID, Z10Cabbage.Entity.Z10OrderItem item, decimal itemHappened)
        {
            var db = Zippy.Data.StaticDB.DB;

            if (itemHappened > 0)  //入库 ，数量为正数 则入库，否则出库
            {
                #region 入库
                //更新商品库
                if (!db.Exists<Z10DepotProduct>("ProductID=@ProductID and DepotID=@DepotID and TenantID=@TenantID",
                    db.CreateParameter("ProductID", item.ProductID), db.CreateParameter("DepotID", item.DepotID), db.CreateParameter("TenantID", tenantID)))
                {
                    Z10DepotProduct xProduct = new Z10DepotProduct();
                    xProduct.TenantID = tenantID;
                    xProduct.ProductID = item.ProductID;
                    xProduct.DepotID = item.DepotID;
                    xProduct.StockSum = itemHappened;
                    xProduct.InSum = itemHappened;
                    xProduct.Creator = userID;
                    db.Insert(xProduct);
                }
                else
                {
                    string sqlUpdateDepot = "update Z10DepotProduct set StockSum=StockSum + @itemHappened, InSum=InSum + @itemHappened" +
                        " where ProductID=@ProductID and DepotID=@DepotID";
                    db.ExecuteNonQuery(sqlUpdateDepot, db.CreateParameter("itemHappened", itemHappened),
                        db.CreateParameter("ProductID", item.ProductID), db.CreateParameter("DepotID", item.DepotID));

                }

                //更新库存详情
                Z10DepotProductDetail dpd = new Z10DepotProductDetail();
                dpd.TenantID = tenantID;
                dpd.ProductID = item.ProductID;
                dpd.DepotID = item.DepotID;
                dpd.OrderID = item.OrderID;
                dpd.StockSum = itemHappened;
                dpd.InSum = itemHappened;
                dpd.ExtColor = item.ExtColor;
                dpd.ExtSize = item.ExtSize;
                dpd.ExtSpecification = item.ExtSpecification;
                dpd.ExtModel1 = item.ExtModel1;
                dpd.ExtModel2 = item.ExtModel2;
                dpd.ExtModel3 = item.ExtModel3;
                dpd.ExtModel4 = item.ExtModel4;
                dpd.ExtModel5 = item.ExtModel5;
                dpd.PriceStock = item.Price;
                dpd.Creator = userID;
                db.Insert(dpd);


                //更新订单项
                db.ColAdd<Z10OrderItem>("CountHappend", itemHappened, item.ItemID);
                #endregion
            }
            else
            {
                #region 出库
                var productDetail = db.FindUnique<Z10Cabbage.Entity.Z10DepotProductDetail>(item.DepotProductDetailID);
                if (productDetail == null || productDetail.StockSum <= 0)
                {
                    throw new Exception("错误的库存商品，或者商品数量为0。");
                }
                if (productDetail.StockSum < Math.Abs(itemHappened))
                {
                    throw new Exception("库存数量太少，无法完成此次出货。");
                }

                //更新总库存
                string sqlUpdateDepot = "update Z10DepotProduct set StockSum=StockSum + @itemHappened, OutSum=OutSum + @itemHappened" +
                    " where ProductID=@ProductID and DepotID=@DepotID";
                db.ExecuteNonQuery(sqlUpdateDepot, db.CreateParameter("itemHappened", itemHappened),
                    db.CreateParameter("ProductID", item.ProductID), db.CreateParameter("DepotID", item.DepotID));

                //更新单项（型号）库存
                string sqlUpdateDepotDetail = "update Z10DepotProductDetail set StockSum=StockSum + @itemHappened, OutSum=OutSum + @itemHappened" +
                    " where DepotProductID=@DepotProductID";
                db.ExecuteNonQuery(sqlUpdateDepotDetail, db.CreateParameter("itemHappened", itemHappened),
                    db.CreateParameter("DepotProductID", item.DepotProductDetailID));

                //更新订单项
                db.ColAdd<Z10OrderItem>("CountHappend2", itemHappened, item.ItemID);
                #endregion
            }

            //增加库存流水
            Z10DepotFlow xflow = new Z10DepotFlow();
            xflow.Creator = userID;
            xflow.Count = itemHappened;
            xflow.DepotID = item.DepotID;
            xflow.OrderID = item.OrderID;
            xflow.ProductID = item.ProductID;
            xflow.TenantID = tenantID;
            
            db.Insert(xflow);


        }

        /// <summary>
        /// 销售订单出库操作
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="userID"></param>
        public void SaleOutDepot(Guid? tenantID, Guid? userID)
        {
            if ((Z10Order.OrderID ?? 0) <= 0) //必须是保存过的
            {
                throw new Exception("请首先保存定单。");
            }
            var db = Zippy.Data.StaticDB.DB;
            //db.BeginTransaction();
            try
            {
                foreach (var item in Items)
                {
                    PutInOut(tenantID, userID, item, -(item.CountShould ?? 0));
                }

                string sqlUpdateStatus = "update Z10Order set OrderStatus=OrderStatus|@outdepot where OrderID=@orderid ";
                db.ExecuteNonQuery(sqlUpdateStatus, db.CreateParameter("outdepot", (int)EAP.Logic.Z10.OrderStatus.Outted), db.CreateParameter("orderid", Z10Order.OrderID ?? 0));
               
            }
            catch
            {
                //db.RollbackTransaction();
            }
        }
        public void StockInDepot(Guid? tenantID, Guid? userID)
        {
            if ((Z10Order.OrderID ?? 0) <= 0) //必须是保存过的
            {
                throw new Exception("请首先保存定单。");
            }
            var db = Zippy.Data.StaticDB.DB;
            try
            {
                foreach (var item in Items)
                {
                    PutInOut(tenantID, userID, item, item.CountShould ?? 0);
                }

                string sqlUpdateStatus = "update Z10Order set OrderStatus=OrderStatus|@outdepot where OrderID=@orderid ";
                db.ExecuteNonQuery(sqlUpdateStatus, db.CreateParameter("outdepot", (int)EAP.Logic.Z10.OrderStatus.Inned), db.CreateParameter("orderid", Z10Order.OrderID ?? 0));

            }
            catch
            {
            }
        }


        /// <summary>
        /// 采购出入库
        /// </summary>
        /// <param name="tenantID">租户</param>
        /// <param name="userID">操作者</param>
        /// <param name="oStatus">当前状态</param>
        /// <param name="hsItemCount">各个商品的具体入库数量</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool InOutDepot(Guid tenantID, Guid userID, EAP.Logic.Z10.OrderStatus oStatus, Hashtable hsItemCount, Zippy.Data.IDalProvider db)
        {
            throw new Exception("这个入库方式需要商榷。");
            int orderSts = Z10Order.OrderStatus ?? 0;
            if ((orderSts & (int)EAP.Logic.Z10.OrderStatus.Outted) == (int)EAP.Logic.Z10.OrderStatus.Inned ||
                (orderSts & (int)EAP.Logic.Z10.OrderStatus.Finished) == (int)EAP.Logic.Z10.OrderStatus.Finished ||
                (orderSts & (int)EAP.Logic.Z10.OrderStatus.Archived) == (int)EAP.Logic.Z10.OrderStatus.Archived)
            {
                throw new Exception("已经出入库的订单，已经结束的订单和已经存档的订单不能出入库。");
            }


            Z10Order xorder = new Z10Order();
            xorder.OrderID = Z10Order.OrderID;

            if (oStatus == OrderStatus.Inned && ((Z10Order.OrderStatus ?? 0) & (int)OrderStatus.InnedSome) == (int)OrderStatus.InnedSome)
            {
                oStatus = oStatus & (~OrderStatus.InnedSome);
                xorder.OrderStatus = (Z10Order.OrderStatus ?? 0) & (~(int)OrderStatus.InnedSome);
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) | (int)oStatus;
            }
            else if (oStatus == OrderStatus.Outted && ((Z10Order.OrderStatus ?? 0) & (int)OrderStatus.OuttedSome) == (int)OrderStatus.OuttedSome)
            {
                oStatus = oStatus & (~OrderStatus.OuttedSome);
                xorder.OrderStatus = (Z10Order.OrderStatus ?? 0) & (~(int)OrderStatus.OuttedSome);
                xorder.OrderStatus = (xorder.OrderStatus ?? 0) | (int)oStatus;
            }
            else
            {
                xorder.OrderStatus = (Z10Order.OrderStatus ?? 0) | (int)oStatus;
            }

            db.Update(xorder);

            Z10DepotFlow dflow = new Z10DepotFlow();
            foreach (Z10OrderItem item in Items)
            {
                decimal itemHappened = hsItemCount[item.ItemID].ToDecimal();

                if ((oStatus & OrderStatus.Outted) == OrderStatus.Outted || (oStatus & OrderStatus.OuttedSome) == OrderStatus.OuttedSome)
                    db.ColAdd<Z10OrderItem>("CountHappend2", itemHappened, item.ItemID);
                else
                    db.ColAdd<Z10OrderItem>("CountHappend", itemHappened, item.ItemID);

                if (item.CountHappend != 0 || item.CountHappend2 != 0)
                {
                    dflow.FlowID = null;
                    dflow.OrderID = Z10Order.OrderID;
                    dflow.Count = itemHappened;
                    dflow.DepotID = item.DepotID;
                    dflow.Creator = userID;
                    dflow.ProductID = item.ProductID;
                    dflow.TenantID = tenantID;
                    db.Insert(dflow);

                    if (!db.Exists<Z10DepotProduct>("ProductID=@ProductID and DepotID=@DepotID and TenantID=@TenantID",
                        db.CreateParameter("ProductID", item.ProductID), db.CreateParameter("DepotID", item.DepotID), db.CreateParameter("TenantID", tenantID)))
                    {
                        Z10DepotProduct xProduct = new Z10DepotProduct();
                        xProduct.TenantID = tenantID;
                        xProduct.ProductID = item.ProductID;
                        xProduct.DepotID = item.DepotID;
                        xProduct.StockSum = itemHappened;
                        db.Insert(xProduct);
                    }
                    else
                    {
                        string sqlUpdateDepot = "update Z10DepotProduct set StockSum=StockSum + @itemHappened, InSum=InSum + @itemHappened" +
                            " where ProductID=@ProductID and DepotID=@DepotID";
                        db.ExecuteNonQuery(sqlUpdateDepot, db.CreateParameter("itemHappened", itemHappened),
                            db.CreateParameter("ProductID", item.ProductID), db.CreateParameter("DepotID", item.DepotID));

                    }
                }
            }
            return true;
        }
    }
}
