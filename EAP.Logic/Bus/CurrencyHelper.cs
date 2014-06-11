using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

	public class CurrencyHelper
	{	
	
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
		public static Currency Create(Zippy.Data.IDalProvider db, String _ID)
		{
			Currency rtn =  db.FindUnique<Currency>("ID=@ID", db.CreateParameter("ID", _ID));
			return rtn;
		}
		

		public static int Delete(Zippy.Data.IDalProvider db, String _ID)
		{
			return db.Delete<Currency>(_ID);
		}
		
		public static int Insert(Zippy.Data.IDalProvider db, Currency entity)
		{
			int rtn = db.Insert(entity);
			return rtn;
		}
		
		public static int Update(Zippy.Data.IDalProvider db, Currency entity)
		{
			return db.Update(entity);
		}
		
		
		public static List<Currency> Take(Zippy.Data.IDalProvider db)
		{
			return db.Take<Currency>(true);
		}
		
		public static List<Currency> Take(Zippy.Data.IDalProvider db,int count)
		{
			return db.Take<Currency>(count, true);
		}
		
		public static List<Currency> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
		{
			return db.Take<Currency>(sqlEntry, cmdParameters);
		}
		
		/// <summary>
		/// 币种 查询
		/// </summary>
		/// <param name="where">查询条件</param>
		/// <param name="orderby">排序方式</param>
		/// <param name="PageSize">每页数量</param>
		/// <param name="PageIndex">页码</param>
		/// <param name="cmdParameters">查询条件赋值</param>
		/// <returns></returns>
		public static PaginatedList<Currency> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
		{
			PaginatedList<Currency> rtn = new PaginatedList<Currency>();
			List<Currency> records = db.Take<Currency>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = db.Count<Currency>(where, cmdParameters);
			return rtn;
		}
		
		public static PaginatedList<Currency> Query(Zippy.Data.IDalProvider db, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
		{
			PaginatedList<Currency> rtn = new PaginatedList<Currency>();
			List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
			string where = " 1=1";
			
			#region 开始查询
			if (paras != null)
			{
				object qID = paras["qID"];
				if (qID.IsNotNullOrEmpty())
				{
					where += " and [ID] like @ID";
					dbParams.Add(db.CreateParameter("ID", "%" + qID + "%"));
				}
				object qTitle = paras["qTitle"];
				if (qTitle.IsNotNullOrEmpty())
				{
					where += " and [Title] like @Title";
					dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
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
			}
			#endregion
			
			string orderBy = "[DisplayOrder]";
			if (orderCol == 0)
			{
				orderBy = "[DisplayOrder]";
			}
			else if (orderCol == 1)
			{
				orderBy = "[ID]";
			}
			else if (orderCol == 2)
			{
				orderBy = "[ID] desc";
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
				orderBy = "[CreateDate]";
			}
			else if (orderCol == 6)
			{
				orderBy = "[CreateDate] desc";
			}
			int RecordCount = db.Count<Currency>(where, dbParams.ToArray());
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


			List<Currency> records = db.Take<Currency>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = RecordCount;

			return rtn;

		}
	}
	
}
