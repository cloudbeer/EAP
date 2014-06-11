using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01ProductExtValueHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [商品] 对应的实体
        /// </summary>
        public static Z01Product GetProductIDEntity(Zippy.Data.IDalProvider db, Z01ProductExtValue entity)
		{
			return db.FindUnique<Z01Product>("ProductID=@ProductID", db.CreateParameter("ProductID", entity.ProductID));
        }
        /// <summary>
        /// 表示 [商品] 被选实体集合
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
        /// 表示 [属性] 对应的实体
        /// </summary>
        public static ExtProperty GetPropertyIDEntity(Zippy.Data.IDalProvider db, Z01ProductExtValue entity)
		{
			return db.FindUnique<ExtProperty>("PropertyID=@PropertyID", db.CreateParameter("PropertyID", entity.PropertyID));
        }
        /// <summary>
        /// 表示 [属性] 被选实体集合
        /// </summary>
        public static List<ExtProperty> GetPropertyIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<ExtProperty>();
        }
		
        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetPropertyIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<ExtProperty> objs = db.Take<ExtProperty>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.PropertyID)
                    sb.AppendLine("<option value='" + obj.PropertyID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.PropertyID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

		#endregion
		
        public static Z01ProductExtValue Create(Zippy.Data.IDalProvider db, Int64 _ValueID)
        {
			Z01ProductExtValue rtn =  db.FindUnique<Z01ProductExtValue>(_ValueID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _ValueID)
        {
            return db.Delete<Z01ProductExtValue>(_ValueID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01ProductExtValue entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01ProductExtValue entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01ProductExtValue> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01ProductExtValue>(true);
        }
		
        public static List<Z01ProductExtValue> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01ProductExtValue>(count, true);
        }
		
        public static List<Z01ProductExtValue> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01ProductExtValue>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 产品扩展属性 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01ProductExtValue> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01ProductExtValue> rtn = new PaginatedList<Z01ProductExtValue>();
            List<Z01ProductExtValue> records = db.Take<Z01ProductExtValue>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01ProductExtValue>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01ProductExtValue> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01ProductExtValue> rtn = new PaginatedList<Z01ProductExtValue>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
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
                orderBy = "[ValueID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[ValueID] desc";
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
                orderBy = "[PropertyID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[PropertyID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[Value]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[Value] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01ProductExtValue>(where, dbParams.ToArray());
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


            List<Z01ProductExtValue> records = db.Take<Z01ProductExtValue>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
