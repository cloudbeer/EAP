using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z10Cabbage.Entity.Helper
{

	public static class Z10OrderHelper
	{	
	
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		public static Z01Beetle.Entity.Z01Customer GetCustomer(this Z10Order order, Zippy.Data.IDalProvider db)
		{
			Z01Beetle.Entity.Z01Customer rtn = db.FindUnique<Z01Beetle.Entity.Z01Customer>("CustomerID=@CustomerID", db.CreateParameter("CustomerID", order.CustomerID));
            if (rtn == null)
                rtn = new Z01Beetle.Entity.Z01Customer();
            return rtn;
        }

		public static EAP.Bus.Entity.Currency GetCurrency(this Z10Order order, Zippy.Data.IDalProvider db)
		{
			return db.FindUnique<EAP.Bus.Entity.Currency>("ID=@ID", db.CreateParameter("ID",order.Currency));
		}

        public static Z10Cabbage.Entity.Z10OrderItem GetTop1OrderItem(this Z10Order order, Zippy.Data.IDalProvider db)
        {
            return db.FindUnique<Z10Cabbage.Entity.Z10OrderItem>("OrderID=@OrderID", db.CreateParameter("OrderID", order.OrderID));
        }
		#endregion
		
		public static Z10Order Create(Zippy.Data.IDalProvider db, Int64 _OrderID)
		{
			Z10Order rtn =  db.FindUnique<Z10Order>(_OrderID);
			return rtn;
		}


        public static int Delete(Zippy.Data.IDalProvider db, Z10Order xOrder)
		{
			//db.Delete<Z10OrderItem>("OrderID=@OrderID", db.CreateParameter("OrderID", _OrderID));
			//db.Delete<Z10Order>(_OrderID);

            xOrder.DeleteFlag = (int)EAP.Logic.DeleteFlags.Deleted;
            var result = db.Update<Z10Order>(xOrder);

            return result;
		}
		
		public static int Insert(Zippy.Data.IDalProvider db, Z10Order entity)
		{
			int rtn = db.Insert(entity);
			return rtn;
		}
		
		public static int Update(Zippy.Data.IDalProvider db, Z10Order entity)
		{
			return db.Update(entity);
		}
		
		
		public static List<Z10Order> Take(Zippy.Data.IDalProvider db)
		{
			return db.Take<Z10Order>(true);
		}
		
		public static List<Z10Order> Take(Zippy.Data.IDalProvider db,int count)
		{
			return db.Take<Z10Order>(count, true);
		}
		
		public static List<Z10Order> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
		{
			return db.Take<Z10Order>(sqlEntry, cmdParameters);
		}
		
		/// <summary>
		/// 进销存订单 查询
		/// </summary>
		/// <param name="where">查询条件</param>
		/// <param name="orderby">排序方式</param>
		/// <param name="PageSize">每页数量</param>
		/// <param name="PageIndex">页码</param>
		/// <param name="cmdParameters">查询条件赋值</param>
		/// <returns></returns>
		public static PaginatedList<Z10Order> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
		{
			PaginatedList<Z10Order> rtn = new PaginatedList<Z10Order>();
			List<Z10Order> records = db.Take<Z10Order>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = db.Count<Z10Order>(where, cmdParameters);
			return rtn;
		}
		
		public static PaginatedList<Z10Order> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
		{
			PaginatedList<Z10Order> rtn = new PaginatedList<Z10Order>();
			List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
			string where = " [TenantID]=@TenantID";
			dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
			if (paras != null)
			{
                object qCustomerID = paras["qCustomerID"];
                if (qCustomerID.IsNotNullOrEmpty())
				{
                    where += " and [CustomerID] = @CustomerID";
                    dbParams.Add(db.CreateParameter("CustomerID", qCustomerID));
				}
				object qCreateDateStart = paras["qCreateDateStart"];
                if (qCreateDateStart.IsNotNullOrEmpty())
				{
					where += " and [CreateDate] >= @CreateDateStart";
					dbParams.Add(db.CreateParameter("CreateDateStart", qCreateDateStart));
				}
				object qCreateDateEnd = paras["qCreateDateEnd"];
				if (qCreateDateEnd.IsNotNullOrEmpty())
				{
                    where += " and [CreateDate] < @CreateDateEnd";
					dbParams.Add(db.CreateParameter("CreateDateEnd", ((DateTime)qCreateDateEnd).AddDays(1)));
                }
                object qOrderType = paras["qOrderType"];
                if (qOrderType.IsNotNullOrEmpty())
                {
                    where += " and ([OrderType]&@OrderType)=@OrderType";
                    dbParams.Add(db.CreateParameter("OrderType", qOrderType));
                }
				object qIsSnap = paras["qIsSnap"];
				if (qIsSnap.IsNotNullOrEmpty())
				{
					where += " and [IsSnap] = @IsSnap";
					dbParams.Add(db.CreateParameter("IsSnap",qIsSnap));
				}
                object qDeleteFlag = paras["qDeleteFlag"];
                if (qDeleteFlag.IsNotNullOrEmpty())
                {
                    where += " and [DeleteFlag] = @DeleteFlag";
                    dbParams.Add(db.CreateParameter("DeleteFlag", qDeleteFlag));
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
				orderBy = "[OrderID]";
			}
			else if (orderCol == 2)
			{
				orderBy = "[OrderID] desc";
			}
			else if (orderCol == 3)
			{
				orderBy = "[CustomerID]";
			}
			else if (orderCol == 4)
			{
				orderBy = "[CustomerID] desc";
			}
			else if (orderCol == 5)
			{
				orderBy = "[DateOrder]";
			}
			else if (orderCol == 6)
			{
				orderBy = "[DateOrder] desc";
			}
			else if (orderCol == 7)
			{
				orderBy = "[DateShip]";
			}
			else if (orderCol == 8)
			{
				orderBy = "[DateShip] desc";
			}
			else if (orderCol == 9)
			{
                orderBy = "[CreateDate]";
			}
			else if (orderCol == 10)
			{
				orderBy = "[CreateDate] desc";
			}
			int RecordCount = db.Count<Z10Order>(where, dbParams.ToArray());
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


			List<Z10Order> records = db.Take<Z10Order>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = RecordCount;

			return rtn;

		}
	}
	
}
