using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01ConfigHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z01Config Create(Zippy.Data.IDalProvider db, Int64 _ConfigID)
        {
			Z01Config rtn =  db.FindUnique<Z01Config>(_ConfigID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _ConfigID)
        {
            return db.Delete<Z01Config>(_ConfigID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01Config entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01Config entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01Config> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Config>(true);
        }
		
        public static List<Z01Config> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01Config>(count, true);
        }
		
        public static List<Z01Config> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01Config>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 配置 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01Config> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01Config> rtn = new PaginatedList<Z01Config>();
            List<Z01Config> records = db.Take<Z01Config>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01Config>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01Config> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01Config> rtn = new PaginatedList<Z01Config>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qConfigType = paras["qConfigType"];
				if (qConfigType.IsNotNullOrEmpty())
				{
					Int32 intqConfigType = (Int32)qConfigType;
					if (intqConfigType > 0)
					{
						where += " and ([ConfigType] & @ConfigType = @ConfigType)";
						dbParams.Add(db.CreateParameter("ConfigType", qConfigType));
					}
				}
				object qConfigStatus = paras["qConfigStatus"];
				if (qConfigStatus.IsNotNullOrEmpty())
				{
					Int32 intqConfigStatus = (Int32)qConfigStatus;
					if (intqConfigStatus > 0)
					{
						where += " and [ConfigStatus] = @ConfigStatus";
						dbParams.Add(db.CreateParameter("ConfigStatus", qConfigStatus));
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
                orderBy = "[ConfigID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[ConfigID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[ConfigType]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[ConfigType] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[ConfigStatus]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[ConfigStatus] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01Config>(where, dbParams.ToArray());
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


            List<Z01Config> records = db.Take<Z01Config>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
