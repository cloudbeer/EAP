using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class PropertyTemplateHelper
    {	
    
		#region 本实体的外键实体集合
        /// <summary>
        /// 获取 [属性模板 的 模板] 的 [扩展属性模板] 集合
        /// </summary>
        public static List<ExtProperty> GetTemplateID_ExtPropertys(Zippy.Data.IDalProvider db, PropertyTemplate entity)
        {
                if (entity.TemplateID.HasValue)
                   return db.Take<ExtProperty>("TemplateID=@TemplateID", db.CreateParameter("TemplateID", entity.TemplateID));            
            return new List<ExtProperty>();
            
        }
        /// <summary>
        /// 获取 [属性模板 的 属性模板] 的 [产品] 集合
        /// </summary>
        public static List<Z01Product> GetPropertyTemplate_Z01Products(Zippy.Data.IDalProvider db, PropertyTemplate entity)
        {
                if (entity.TemplateID.HasValue)
                   return db.Take<Z01Product>("PropertyTemplate=@PropertyTemplate", db.CreateParameter("PropertyTemplate", entity.TemplateID));            
            return new List<Z01Product>();
            
        }
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static PropertyTemplate Create(Zippy.Data.IDalProvider db, Int64 _TemplateID)
        {
			PropertyTemplate rtn =  db.FindUnique<PropertyTemplate>(_TemplateID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _TemplateID)
        {
            return db.Delete<PropertyTemplate>(_TemplateID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, PropertyTemplate entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, PropertyTemplate entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<PropertyTemplate> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<PropertyTemplate>(true);
        }
		
        public static List<PropertyTemplate> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<PropertyTemplate>(count, true);
        }
		
        public static List<PropertyTemplate> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<PropertyTemplate>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 属性模板 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<PropertyTemplate> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<PropertyTemplate> rtn = new PaginatedList<PropertyTemplate>();
            List<PropertyTemplate> records = db.Take<PropertyTemplate>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<PropertyTemplate>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<PropertyTemplate> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<PropertyTemplate> rtn = new PaginatedList<PropertyTemplate>();
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
				object qTemplateType = paras["qTemplateType"];
				if (qTemplateType.IsNotNullOrEmpty())
				{
					Int32 intqTemplateType = (Int32)qTemplateType;
					if (intqTemplateType > 0)
					{
						where += " and ([TemplateType] & @TemplateType = @TemplateType)";
						dbParams.Add(db.CreateParameter("TemplateType", qTemplateType));
					}
				}
				object qTemplateStatus = paras["qTemplateStatus"];
				if (qTemplateStatus.IsNotNullOrEmpty())
				{
					Int32 intqTemplateStatus = (Int32)qTemplateStatus;
					if (intqTemplateStatus > 0)
					{
						where += " and [TemplateStatus] = @TemplateStatus";
						dbParams.Add(db.CreateParameter("TemplateStatus", qTemplateStatus));
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
                orderBy = "[TemplateID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[TemplateID] desc";
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
                orderBy = "[TemplateType]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[TemplateType] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[TemplateStatus]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[TemplateStatus] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<PropertyTemplate>(where, dbParams.ToArray());
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


            List<PropertyTemplate> records = db.Take<PropertyTemplate>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
