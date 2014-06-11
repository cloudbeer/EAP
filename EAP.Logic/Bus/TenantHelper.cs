using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class TenantHelper
    {	
    
		#region 本实体的外键实体集合
        /// <summary>
        /// 获取 [租户 的 租户编号] 的 [租户标识] 集合
        /// </summary>
        public static List<TenantIdentity> GetTenantID_TenantIdentitys(Zippy.Data.IDalProvider db, Tenant entity)
        {
                if (entity.TenantID.HasValue)
                   return db.Take<TenantIdentity>("TenantID=@TenantID", db.CreateParameter("TenantID", entity.TenantID));            
            return new List<TenantIdentity>();
            
        }
        /// <summary>
        /// 获取 [租户 的 租户] 的 [用户] 集合
        /// </summary>
        public static List<User> GetTenantID_Users(Zippy.Data.IDalProvider db, Tenant entity)
        {
                if (entity.TenantID.HasValue)
                   return db.Take<User>("TenantID=@TenantID", db.CreateParameter("TenantID", entity.TenantID));            
            return new List<User>();
            
        }
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Tenant Create(Zippy.Data.IDalProvider db, Guid _TenantID)
        {
			Tenant rtn =  db.FindUnique<Tenant>(_TenantID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Guid _TenantID)
        {
            return db.Delete<Tenant>(_TenantID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Tenant entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Tenant entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Tenant> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Tenant>(true);
        }
		
        public static List<Tenant> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Tenant>(count, true);
        }
		
        public static List<Tenant> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Tenant>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 租户 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Tenant> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Tenant> rtn = new PaginatedList<Tenant>();
            List<Tenant> records = db.Take<Tenant>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Tenant>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Tenant> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Tenant> rtn = new PaginatedList<Tenant>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qTitle = paras["qTitle"];
				if (qTitle != null)
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
                orderBy = "[TenantID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[TenantID] desc";
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
                orderBy = "[AreaID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[AreaID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[TenantStatus]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[TenantStatus] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Tenant>(where, dbParams.ToArray());
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


            List<Tenant> records = db.Take<Tenant>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
