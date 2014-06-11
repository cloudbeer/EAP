using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01FinancialCategoryHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z01FinancialCategory Create(Zippy.Data.IDalProvider db, Int64 _CategoryID)
        {
			Z01FinancialCategory rtn =  db.FindUnique<Z01FinancialCategory>(_CategoryID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _CategoryID)
        {
            return db.Delete<Z01FinancialCategory>(_CategoryID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01FinancialCategory entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01FinancialCategory entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01FinancialCategory> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01FinancialCategory>(true);
        }
		
        public static List<Z01FinancialCategory> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01FinancialCategory>(count, true);
        }
		
        public static List<Z01FinancialCategory> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01FinancialCategory>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 财务类目 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01FinancialCategory> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01FinancialCategory> rtn = new PaginatedList<Z01FinancialCategory>();
            List<Z01FinancialCategory> records = db.Take<Z01FinancialCategory>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01FinancialCategory>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01FinancialCategory> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01FinancialCategory> rtn = new PaginatedList<Z01FinancialCategory>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qTitle = paras["qTitle"];
				if (qTitle.IsNotNullOrEmpty())
				{
					where += " and [Title] like @Title";
					dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
				}
				object qParentIDStart = paras["qParentIDStart"];
				if (qParentIDStart != null && qParentIDStart.ToString()!="")
				{
					where += " and [ParentID] >= @ParentIDStart";
					dbParams.Add(db.CreateParameter("ParentIDStart", qParentIDStart));
				}
				object qParentIDEnd = paras["qParentIDEnd"];
				if (qParentIDEnd != null && qParentIDEnd.ToString()!="")
				{
					where += " and [ParentID] <= @ParentID";
					dbParams.Add(db.CreateParameter("ParentIDEnd", qParentIDEnd));
				}
				object qCategoryType = paras["qCategoryType"];
				if (qCategoryType.IsNotNullOrEmpty())
				{
					Int32 intqCategoryType = (Int32)qCategoryType;
					if (intqCategoryType > 0)
					{
						where += " and ([CategoryType] & @CategoryType = @CategoryType)";
						dbParams.Add(db.CreateParameter("CategoryType", qCategoryType));
					}
				}
				object qCategoryStatus = paras["qCategoryStatus"];
				if (qCategoryStatus.IsNotNullOrEmpty())
				{
					Int32 intqCategoryStatus = (Int32)qCategoryStatus;
					if (intqCategoryStatus > 0)
					{
						where += " and [CategoryStatus] = @CategoryStatus";
						dbParams.Add(db.CreateParameter("CategoryStatus", qCategoryStatus));
					}
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
                orderBy = "[CategoryID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[CategoryID] desc";
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
                orderBy = "[ParentID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[ParentID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CategoryType]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CategoryType] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CategoryStatus]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CategoryStatus] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01FinancialCategory>(where, dbParams.ToArray());
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


            List<Z01FinancialCategory> records = db.Take<Z01FinancialCategory>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
