using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class TextResourceHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static TextResource Create(Zippy.Data.IDalProvider db, Int64 _ResourceID)
        {
			TextResource rtn =  db.FindUnique<TextResource>(_ResourceID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _ResourceID)
        {
            return db.Delete<TextResource>(_ResourceID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, TextResource entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, TextResource entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<TextResource> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<TextResource>(true);
        }
		
        public static List<TextResource> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<TextResource>(count, true);
        }
		
        public static List<TextResource> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<TextResource>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 资源配置 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<TextResource> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<TextResource> rtn = new PaginatedList<TextResource>();
            List<TextResource> records = db.Take<TextResource>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<TextResource>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<TextResource> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<TextResource> rtn = new PaginatedList<TextResource>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qTenantIDStart = paras["qTenantIDStart"];
				if (qTenantIDStart != null && qTenantIDStart.ToString()!="")
				{
					where += " and [TenantID] >= @TenantIDStart";
					dbParams.Add(db.CreateParameter("TenantIDStart", qTenantIDStart));
				}
				object qTenantIDEnd = paras["qTenantIDEnd"];
				if (qTenantIDEnd != null && qTenantIDEnd.ToString()!="")
				{
					where += " and [TenantID] <= @TenantID";
					dbParams.Add(db.CreateParameter("TenantIDEnd", qTenantIDEnd));
				}
				object qResourceType = paras["qResourceType"];
				if (qResourceType != null && qResourceType.ToString()!="")
				{
					Int32 intqResourceType = (Int32)qResourceType;
					if (intqResourceType > 0)
					{
						where += " and ([ResourceType] & @ResourceType = @ResourceType)";
						dbParams.Add(db.CreateParameter("ResourceType", qResourceType));
					}
				}
				object qResourceStatus = paras["qResourceStatus"];
				if (qResourceStatus != null && qResourceStatus.ToString()!="" )
				{
					Int32 intqResourceStatus = (Int32)qResourceStatus;
					if (intqResourceStatus > 0)
					{
						where += " and [ResourceStatus] = @ResourceStatus";
						dbParams.Add(db.CreateParameter("ResourceStatus", qResourceStatus));
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
                orderBy = "[ResourceID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[ResourceID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Key]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Key] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CultureID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CultureID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[Language]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[Language] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[Location]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[Location] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[TenantID]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[TenantID] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[ResourceType]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[ResourceType] desc";
            }
            else if (orderCol == 15)
            {
                orderBy = "[ResourceStatus]";
            }
            else if (orderCol == 16)
            {
                orderBy = "[ResourceStatus] desc";
            }
            else if (orderCol == 17)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 18)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<TextResource>(where, dbParams.ToArray());
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


            List<TextResource> records = db.Take<TextResource>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
