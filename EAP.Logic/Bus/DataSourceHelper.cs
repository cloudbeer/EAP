using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class DataSourceHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static DataSource Create(Zippy.Data.IDalProvider db, Guid _DataSourceID)
        {
			DataSource rtn =  db.FindUnique<DataSource>(_DataSourceID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Guid _DataSourceID)
        {
            return db.Delete<DataSource>(_DataSourceID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, DataSource entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, DataSource entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<DataSource> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<DataSource>(true);
        }
		
        public static List<DataSource> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<DataSource>(count, true);
        }
		
        public static List<DataSource> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<DataSource>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 数据源 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<DataSource> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<DataSource> rtn = new PaginatedList<DataSource>();
            List<DataSource> records = db.Take<DataSource>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<DataSource>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<DataSource> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<DataSource> rtn = new PaginatedList<DataSource>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qDataSourceType = paras["qDataSourceType"];
				if (qDataSourceType != null && qDataSourceType.ToString()!="")
				{
					Int32 intqDataSourceType = (Int32)qDataSourceType;
					if (intqDataSourceType > 0)
					{
						where += " and ([DataSourceType] & @DataSourceType = @DataSourceType)";
						dbParams.Add(db.CreateParameter("DataSourceType", qDataSourceType));
					}
				}
				object qTitle = paras["qTitle"];
				if (qTitle != null)
				{
					where += " and [Title] like @Title";
					dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
				}
				object qDataSourceStatus = paras["qDataSourceStatus"];
				if (qDataSourceStatus != null && qDataSourceStatus.ToString()!="" )
				{
					Int32 intqDataSourceStatus = (Int32)qDataSourceStatus;
					if (intqDataSourceStatus > 0)
					{
						where += " and [DataSourceStatus] = @DataSourceStatus";
						dbParams.Add(db.CreateParameter("DataSourceStatus", qDataSourceStatus));
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
                orderBy = "[DataSourceID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[DataSourceID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[DataSourceType]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[DataSourceType] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[Title]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[DataSourceStatus]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[DataSourceStatus] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<DataSource>(where, dbParams.ToArray());
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


            List<DataSource> records = db.Take<DataSource>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
