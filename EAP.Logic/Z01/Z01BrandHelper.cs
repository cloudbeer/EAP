using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01BrandHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [生产商] 对应的实体
        /// </summary>
        public static Z01Customer GetProducerEntity(Zippy.Data.IDalProvider db, Z01Brand entity)
		{
			return db.FindUnique<Z01Customer>("CustomerID=@CustomerID", db.CreateParameter("CustomerID", entity.Producer));
        }
        /// <summary>
        /// 表示 [生产商] 被选实体集合
        /// </summary>
        public static List<Z01Customer> GetProducerEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Z01Customer>();
        }
		
        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetProducerEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int32? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01Customer> objs = db.Take<Z01Customer>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.CustomerID)
                    sb.AppendLine("<option value='" + obj.CustomerID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.CustomerID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

		#endregion
		
        public static Z01Brand Create(Zippy.Data.IDalProvider db, Int64 _BrandID)
        {
			Z01Brand rtn =  db.FindUnique<Z01Brand>(_BrandID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _BrandID)
        {
            return db.Delete<Z01Brand>(_BrandID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01Brand entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01Brand entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01Brand> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Brand>(true);
        }
		
        public static List<Z01Brand> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01Brand>(count, true);
        }
		
        public static List<Z01Brand> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01Brand>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 品牌 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01Brand> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01Brand> rtn = new PaginatedList<Z01Brand>();
            List<Z01Brand> records = db.Take<Z01Brand>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01Brand>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01Brand> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01Brand> rtn = new PaginatedList<Z01Brand>();
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
			}
            #endregion
			
			string orderBy = "[CreateDate] desc";
            if (orderCol == 0)
            {
                orderBy =  "[CreateDate] desc";
            }
            else if (orderCol == 1)
            {
                orderBy = "[Title]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[ImagePath]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[ImagePath] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01Brand>(where, dbParams.ToArray());
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


            List<Z01Brand> records = db.Take<Z01Brand>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
