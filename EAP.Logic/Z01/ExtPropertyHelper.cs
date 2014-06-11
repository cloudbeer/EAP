using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class ExtPropertyHelper
    {	
    
		#region 本实体的外键实体集合
        /// <summary>
        /// 获取 [扩展属性模板 的 属性] 的 [产品扩展属性] 集合
        /// </summary>
        public static List<Z01ProductExtValue> GetPropertyID_Z01ProductExtValues(Zippy.Data.IDalProvider db, ExtProperty entity)
        {
                if (entity.PropertyID.HasValue)
                   return db.Take<Z01ProductExtValue>("PropertyID=@PropertyID", db.CreateParameter("PropertyID", entity.PropertyID));            
            return new List<Z01ProductExtValue>();
            
        }
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [模板] 对应的实体
        /// </summary>
        public static PropertyTemplate GetTemplateIDEntity(Zippy.Data.IDalProvider db, ExtProperty entity)
		{
			return db.FindUnique<PropertyTemplate>("TemplateID=@TemplateID", db.CreateParameter("TemplateID", entity.TemplateID));
        }
        /// <summary>
        /// 表示 [模板] 被选实体集合
        /// </summary>
        public static List<PropertyTemplate> GetTemplateIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<PropertyTemplate>();
        }
		
        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetTemplateIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<PropertyTemplate> objs = db.Take<PropertyTemplate>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.TemplateID)
                    sb.AppendLine("<option value='" + obj.TemplateID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.TemplateID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

		#endregion
		
        public static ExtProperty Create(Zippy.Data.IDalProvider db, Int64 _PropertyID)
        {
			ExtProperty rtn =  db.FindUnique<ExtProperty>(_PropertyID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _PropertyID)
        {
            return db.Delete<ExtProperty>(_PropertyID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, ExtProperty entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, ExtProperty entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<ExtProperty> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<ExtProperty>(true);
        }
		
        public static List<ExtProperty> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<ExtProperty>(count, true);
        }
		
        public static List<ExtProperty> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<ExtProperty>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 扩展属性模板 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<ExtProperty> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<ExtProperty> rtn = new PaginatedList<ExtProperty>();
            List<ExtProperty> records = db.Take<ExtProperty>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<ExtProperty>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<ExtProperty> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<ExtProperty> rtn = new PaginatedList<ExtProperty>();
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
                orderBy = "[PropertyID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[PropertyID] desc";
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
                orderBy = "[TemplateID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[TemplateID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[DisplayOrder]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[DisplayOrder] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<ExtProperty>(where, dbParams.ToArray());
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


            List<ExtProperty> records = db.Take<ExtProperty>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
