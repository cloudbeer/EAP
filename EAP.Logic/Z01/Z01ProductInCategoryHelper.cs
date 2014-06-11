using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01ProductInCategoryHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [产品] 对应的实体
        /// </summary>
        public static Z01Product GetProductIDEntity(Zippy.Data.IDalProvider db, Z01ProductInCategory entity)
		{
			return db.FindUnique<Z01Product>("ProductID=@ProductID", db.CreateParameter("ProductID", entity.ProductID));
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
        public static Z01ProductCategory GetCategoryIDEntity(Zippy.Data.IDalProvider db, Z01ProductInCategory entity)
		{
			return db.FindUnique<Z01ProductCategory>("CategoryID=@CategoryID", db.CreateParameter("CategoryID", entity.CategoryID));
        }
        /// <summary>
        /// 表示 [分类] 被选实体集合
        /// </summary>
        public static List<Z01ProductCategory> GetCategoryIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Z01ProductCategory>();
        }
		
        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetCategoryIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01ProductCategory> objs = db.Take<Z01ProductCategory>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
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
		
        public static Z01ProductInCategory Create(Zippy.Data.IDalProvider db, Int64 _PICID)
        {
			Z01ProductInCategory rtn =  db.FindUnique<Z01ProductInCategory>(_PICID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _PICID)
        {
            return db.Delete<Z01ProductInCategory>(_PICID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01ProductInCategory entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01ProductInCategory entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01ProductInCategory> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01ProductInCategory>(true);
        }
		
        public static List<Z01ProductInCategory> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01ProductInCategory>(count, true);
        }
		
        public static List<Z01ProductInCategory> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01ProductInCategory>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 产品分类关系 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01ProductInCategory> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01ProductInCategory> rtn = new PaginatedList<Z01ProductInCategory>();
            List<Z01ProductInCategory> records = db.Take<Z01ProductInCategory>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01ProductInCategory>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01ProductInCategory> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01ProductInCategory> rtn = new PaginatedList<Z01ProductInCategory>();
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
                orderBy = "[PICID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[PICID] desc";
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
            int RecordCount = db.Count<Z01ProductInCategory>(where, dbParams.ToArray());
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


            List<Z01ProductInCategory> records = db.Take<Z01ProductInCategory>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
