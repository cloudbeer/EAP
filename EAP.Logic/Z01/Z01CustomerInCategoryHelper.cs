using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01CustomerInCategoryHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [产品] 对应的实体
        /// </summary>
        public static Z01Product GetProductIDEntity(Zippy.Data.IDalProvider db, Z01CustomerInCategory entity)
		{
            return db.FindUnique<Z01Product>("CustomerID=@CustomerID", db.CreateParameter("CustomerID", entity.CustomerID));
        }
        /// <summary>
        /// 表示 [产品] 被选实体集合
        /// </summary>
        public static List<Z01Product> GetProductIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Z01Product>();
        }
		
        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetProductIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01Product> objs = db.Take<Z01Product>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.ProductID)
                    sb.AppendLine("<option value='" + obj.ProductID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.ProductID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 表示 [分类] 对应的实体
        /// </summary>
        public static Z01CustomerCategory GetCategoryIDEntity(Zippy.Data.IDalProvider db, Z01CustomerInCategory entity)
		{
			return db.FindUnique<Z01CustomerCategory>("CategoryID=@CategoryID", db.CreateParameter("CategoryID", entity.CategoryID));
        }
        /// <summary>
        /// 表示 [分类] 被选实体集合
        /// </summary>
        public static List<Z01CustomerCategory> GetCategoryIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Z01CustomerCategory>();
        }
		
        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetCategoryIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01CustomerCategory> objs = db.Take<Z01CustomerCategory>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.CategoryID)
                    sb.AppendLine("<option value='" + obj.CategoryID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.CategoryID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

		#endregion
		
        public static Z01CustomerInCategory Create(Zippy.Data.IDalProvider db, Int64 _CICID)
        {
			Z01CustomerInCategory rtn =  db.FindUnique<Z01CustomerInCategory>(_CICID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _CICID)
        {
            return db.Delete<Z01CustomerInCategory>(_CICID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01CustomerInCategory entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01CustomerInCategory entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01CustomerInCategory> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01CustomerInCategory>(true);
        }
		
        public static List<Z01CustomerInCategory> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01CustomerInCategory>(count, true);
        }
		
        public static List<Z01CustomerInCategory> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01CustomerInCategory>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 客户分类关系 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01CustomerInCategory> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01CustomerInCategory> rtn = new PaginatedList<Z01CustomerInCategory>();
            List<Z01CustomerInCategory> records = db.Take<Z01CustomerInCategory>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01CustomerInCategory>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01CustomerInCategory> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01CustomerInCategory> rtn = new PaginatedList<Z01CustomerInCategory>();
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
				object qCategoryIDStart = paras["qCategoryIDStart"];
				if (qCategoryIDStart != null && qCategoryIDStart.ToString()!="")
				{
					where += " and [CategoryID] >= @CategoryIDStart";
					dbParams.Add(db.CreateParameter("CategoryIDStart", qCategoryIDStart));
				}
				object qCategoryIDEnd = paras["qCategoryIDEnd"];
				if (qCategoryIDEnd != null && qCategoryIDEnd.ToString()!="")
				{
					where += " and [CategoryID] <= @CategoryID";
					dbParams.Add(db.CreateParameter("CategoryIDEnd", qCategoryIDEnd));
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
                orderBy = "[CICID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[CICID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[ProductID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[ProductID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CategoryID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CategoryID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01CustomerInCategory>(where, dbParams.ToArray());
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


            List<Z01CustomerInCategory> records = db.Take<Z01CustomerInCategory>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
