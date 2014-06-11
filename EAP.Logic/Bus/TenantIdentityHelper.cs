using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class TenantIdentityHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [租户编号] 对应的实体
        /// </summary>
        public static Tenant GetTenantIDEntity(Zippy.Data.IDalProvider db, TenantIdentity entity)
		{
			return db.FindUnique<Tenant>("TenantID=@TenantID", db.CreateParameter("TenantID", entity.TenantID));
        }
        /// <summary>
        /// 表示 [租户编号] 被选实体集合
        /// </summary>
        public static List<Tenant> GetTenantIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Tenant>();
        }

		#endregion
		
        public static TenantIdentity Create(Zippy.Data.IDalProvider db, Int64 _IdentityID)
        {
			TenantIdentity rtn =  db.FindUnique<TenantIdentity>(_IdentityID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _IdentityID)
        {
            return db.Delete<TenantIdentity>(_IdentityID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, TenantIdentity entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, TenantIdentity entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<TenantIdentity> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<TenantIdentity>(true);
        }
		
        public static List<TenantIdentity> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<TenantIdentity>(count, true);
        }
		
        public static List<TenantIdentity> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<TenantIdentity>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 租户标识 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<TenantIdentity> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<TenantIdentity> rtn = new PaginatedList<TenantIdentity>();
            List<TenantIdentity> records = db.Take<TenantIdentity>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<TenantIdentity>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<TenantIdentity> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<TenantIdentity> rtn = new PaginatedList<TenantIdentity>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qTenantIdentityType = paras["qTenantIdentityType"];
				if (qTenantIdentityType != null && qTenantIdentityType.ToString()!="")
				{
					Int32 intqTenantIdentityType = (Int32)qTenantIdentityType;
					if (intqTenantIdentityType > 0)
					{
						where += " and ([TenantIdentityType] & @TenantIdentityType = @TenantIdentityType)";
						dbParams.Add(db.CreateParameter("TenantIdentityType", qTenantIdentityType));
					}
				}
				object qTenantIdentityStatus = paras["qTenantIdentityStatus"];
				if (qTenantIdentityStatus != null && qTenantIdentityStatus.ToString()!="" )
				{
					Int32 intqTenantIdentityStatus = (Int32)qTenantIdentityStatus;
					if (intqTenantIdentityStatus > 0)
					{
						where += " and [TenantIdentityStatus] = @TenantIdentityStatus";
						dbParams.Add(db.CreateParameter("TenantIdentityStatus", qTenantIdentityStatus));
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
                orderBy = "[TenantID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[TenantID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[TenantIdentityType]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[TenantIdentityType] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[TenantIdentityStatus]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[TenantIdentityStatus] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<TenantIdentity>(where, dbParams.ToArray());
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


            List<TenantIdentity> records = db.Take<TenantIdentity>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
