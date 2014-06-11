using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class TenantApplicationHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [应用程序] 对应的实体
        /// </summary>
        public static Application GetApplicationIDEntity(Zippy.Data.IDalProvider db, TenantApplication entity)
		{
			return db.FindUnique<Application>("ApplicationID=@ApplicationID", db.CreateParameter("ApplicationID", entity.ApplicationID));
        }
        /// <summary>
        /// 表示 [应用程序] 被选实体集合
        /// </summary>
        public static List<Application> GetApplicationIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Application>();
        }

		#endregion
		
        public static TenantApplication Create(Zippy.Data.IDalProvider db, Int64 _TenantApplicationID)
        {
			TenantApplication rtn =  db.FindUnique<TenantApplication>(_TenantApplicationID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _TenantApplicationID)
        {
            return db.Delete<TenantApplication>(_TenantApplicationID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, TenantApplication entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, TenantApplication entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<TenantApplication> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<TenantApplication>(true);
        }
		
        public static List<TenantApplication> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<TenantApplication>(count, true);
        }
		
        public static List<TenantApplication> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<TenantApplication>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 租户的应用 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<TenantApplication> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<TenantApplication> rtn = new PaginatedList<TenantApplication>();
            List<TenantApplication> records = db.Take<TenantApplication>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<TenantApplication>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<TenantApplication> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<TenantApplication> rtn = new PaginatedList<TenantApplication>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qTenantApplicationType = paras["qTenantApplicationType"];
				if (qTenantApplicationType != null && qTenantApplicationType.ToString()!="")
				{
					Int32 intqTenantApplicationType = (Int32)qTenantApplicationType;
					if (intqTenantApplicationType > 0)
					{
						where += " and ([TenantApplicationType] & @TenantApplicationType = @TenantApplicationType)";
						dbParams.Add(db.CreateParameter("TenantApplicationType", qTenantApplicationType));
					}
				}
				object qTenantApplicationStatus = paras["qTenantApplicationStatus"];
				if (qTenantApplicationStatus != null && qTenantApplicationStatus.ToString()!="" )
				{
					Int32 intqTenantApplicationStatus = (Int32)qTenantApplicationStatus;
					if (intqTenantApplicationStatus > 0)
					{
						where += " and [TenantApplicationStatus] = @TenantApplicationStatus";
						dbParams.Add(db.CreateParameter("TenantApplicationStatus", qTenantApplicationStatus));
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
                orderBy = "[TenantApplicationID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[TenantApplicationID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[TenantID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[TenantID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[StartDate]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[StartDate] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[EndDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[EndDate] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[ApplicationID]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[ApplicationID] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[TenantApplicationType]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[TenantApplicationType] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[TenantApplicationStatus]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[TenantApplicationStatus] desc";
            }
            else if (orderCol == 15)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 16)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<TenantApplication>(where, dbParams.ToArray());
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


            List<TenantApplication> records = db.Take<TenantApplication>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
