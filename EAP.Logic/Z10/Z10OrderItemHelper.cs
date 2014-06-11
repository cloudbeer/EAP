using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z10Cabbage.Entity.Helper
{

    public static class Z10OrderItemHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion

        #region 本实体外键对应的实体
        public static Z10Cabbage.Entity.Z10Depot GetDepot(this Z10OrderItem item, Zippy.Data.IDalProvider db)
        {
            return db.FindUnique<Z10Cabbage.Entity.Z10Depot>("DepotID=@DepotID", db.CreateParameter("DepotID", item.DepotID));
        }
        public static Z10Cabbage.Entity.Z10Depot GetDepot2(this Z10OrderItem item, Zippy.Data.IDalProvider db)
        {
            return db.FindUnique<Z10Cabbage.Entity.Z10Depot>("DepotID=@DepotID", db.CreateParameter("DepotID", item.DepotID2));
        }
        public static Z01Beetle.Entity.Z01Product GetProduct(this Z10OrderItem item, Zippy.Data.IDalProvider db)
        {
            return db.FindUnique<Z01Beetle.Entity.Z01Product>("ProductID=@ProductID", db.CreateParameter("ProductID", item.ProductID));
        }

        public static EAP.Logic.Z10.View.V_DepotProduct GetV_DepotProduct(this Z10OrderItem item, Zippy.Data.IDalProvider db)
        {
            return db.FindUnique<EAP.Logic.Z10.View.V_DepotProduct>("DepotID=@DepotID and ProductID=@ProductID",db.CreateParameter("DepotID",item.DepotID),db.CreateParameter("ProductID",item.ProductID));
        }
		#endregion
		
        public static Z10OrderItem Create(Zippy.Data.IDalProvider db, Int64 _ItemID)
        {
			Z10OrderItem rtn =  db.FindUnique<Z10OrderItem>(_ItemID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _ItemID)
        {
            return db.Delete<Z10OrderItem>(_ItemID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z10OrderItem entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z10OrderItem entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z10OrderItem> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z10OrderItem>(true);
        }
		
        public static List<Z10OrderItem> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z10OrderItem>(count, true);
        }
		
        public static List<Z10OrderItem> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z10OrderItem>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 订单产品 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z10OrderItem> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z10OrderItem> rtn = new PaginatedList<Z10OrderItem>();
            List<Z10OrderItem> records = db.Take<Z10OrderItem>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z10OrderItem>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z10OrderItem> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z10OrderItem> rtn = new PaginatedList<Z10OrderItem>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qProductIDStart = paras["qProductIDStart"];
				if (qProductIDStart != null && qProductIDStart.ToString()!="")
				{
					where += " and [ProductID] >= @ProductIDStart";
					dbParams.Add(db.CreateParameter("ProductIDStart", qProductIDStart));
				}
				object qProductIDEnd = paras["qProductIDEnd"];
				if (qProductIDEnd != null && qProductIDEnd.ToString()!="")
				{
					where += " and [ProductID] <= @ProductID";
					dbParams.Add(db.CreateParameter("ProductIDEnd", qProductIDEnd));
				}
				object qTitle = paras["qTitle"];
				if (qTitle.IsNotNullOrEmpty())
				{
					where += " and [Title] like @Title";
					dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
				}
				object qDepotIDStart = paras["qDepotIDStart"];
				if (qDepotIDStart != null && qDepotIDStart.ToString()!="")
				{
					where += " and [DepotID] >= @DepotIDStart";
					dbParams.Add(db.CreateParameter("DepotIDStart", qDepotIDStart));
				}
				object qDepotIDEnd = paras["qDepotIDEnd"];
				if (qDepotIDEnd != null && qDepotIDEnd.ToString()!="")
				{
					where += " and [DepotID] <= @DepotID";
					dbParams.Add(db.CreateParameter("DepotIDEnd", qDepotIDEnd));
				}
				object qCreateDateStart = paras["qCreateDateStart"];
				if (qCreateDateStart != null && qCreateDateStart.ToString()!="")
				{
					where += " and [CreateDate] >= @CreateDateStart";
					dbParams.Add(db.CreateParameter("CreateDateStart", qCreateDateStart));
				}
				object qCreateDateEnd = paras["qCreateDateEnd"];
				if (qCreateDateEnd != null && qCreateDateEnd.ToString()!="")
				{
					where += " and [CreateDate] < @CreateDateEnd";
					dbParams.Add(db.CreateParameter("CreateDateEnd", ((DateTime)qCreateDateEnd).AddDays(1)));
				}
			}
            #endregion
			
			string orderBy = "[CreateDate] desc";
            if (orderCol == 0)
            {
                orderBy =  "[CreateDate] desc";
            }
            else if (orderCol == 1)
            {
                orderBy = "[ProductID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[ProductID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Title]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[DepotID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[DepotID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z10OrderItem>(where, dbParams.ToArray());
            int PageCount =0;
            if (RecordCount % PageSize == 0)
            {
                PageCount = RecordCount / PageSize;
            }
            else
            {
                PageCount = RecordCount / PageSize + 1;
            }
            if (PageIndex > PageCount)
                PageIndex = PageCount;
            if (PageIndex < 1)
                PageIndex = 1;


            List<Z10OrderItem> records = db.Take<Z10OrderItem>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
