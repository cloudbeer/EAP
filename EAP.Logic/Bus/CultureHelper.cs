using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class CultureHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Culture Create(Zippy.Data.IDalProvider db, Guid _CultureID)
        {
			Culture rtn =  db.FindUnique<Culture>(_CultureID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Guid _CultureID)
        {
            return db.Delete<Culture>(_CultureID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Culture entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Culture entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Culture> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Culture>(true);
        }
		
        public static List<Culture> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Culture>(count, true);
        }
		
        public static List<Culture> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Culture>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 文化 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Culture> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Culture> rtn = new PaginatedList<Culture>();
            List<Culture> records = db.Take<Culture>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Culture>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Culture> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Culture> rtn = new PaginatedList<Culture>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qCultureType = paras["qCultureType"];
				if (qCultureType != null && qCultureType.ToString()!="")
				{
					Int32 intqCultureType = (Int32)qCultureType;
					if (intqCultureType > 0)
					{
						where += " and ([CultureType] & @CultureType = @CultureType)";
						dbParams.Add(db.CreateParameter("CultureType", qCultureType));
					}
				}
				object qCultureStatus = paras["qCultureStatus"];
				if (qCultureStatus != null && qCultureStatus.ToString()!="" )
				{
					Int32 intqCultureStatus = (Int32)qCultureStatus;
					if (intqCultureStatus > 0)
					{
						where += " and [CultureStatus] = @CultureStatus";
						dbParams.Add(db.CreateParameter("CultureStatus", qCultureStatus));
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
                orderBy = "[Location]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[Location] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[CultureType]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[CultureType] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CultureStatus]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CultureStatus] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Culture>(where, dbParams.ToArray());
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


            List<Culture> records = db.Take<Culture>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
