using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class TrustIPHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static TrustIP Create(Zippy.Data.IDalProvider db, Int64 _TrustIPID)
        {
			TrustIP rtn =  db.FindUnique<TrustIP>(_TrustIPID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _TrustIPID)
        {
            return db.Delete<TrustIP>(_TrustIPID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, TrustIP entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, TrustIP entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<TrustIP> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<TrustIP>(true);
        }
		
        public static List<TrustIP> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<TrustIP>(count, true);
        }
		
        public static List<TrustIP> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<TrustIP>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 信任IP列表 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<TrustIP> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<TrustIP> rtn = new PaginatedList<TrustIP>();
            List<TrustIP> records = db.Take<TrustIP>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<TrustIP>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<TrustIP> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<TrustIP> rtn = new PaginatedList<TrustIP>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qIP = paras["qIP"];
				if (qIP != null)
				{
					where += " and [IP] like @IP";
					dbParams.Add(db.CreateParameter("IP", "%" + qIP + "%"));
				}
				object qTrustIPType = paras["qTrustIPType"];
				if (qTrustIPType != null && qTrustIPType.ToString()!="")
				{
					Int32 intqTrustIPType = (Int32)qTrustIPType;
					if (intqTrustIPType > 0)
					{
						where += " and ([TrustIPType] & @TrustIPType = @TrustIPType)";
						dbParams.Add(db.CreateParameter("TrustIPType", qTrustIPType));
					}
				}
				object qTrustIPStatus = paras["qTrustIPStatus"];
				if (qTrustIPStatus != null && qTrustIPStatus.ToString()!="" )
				{
					Int32 intqTrustIPStatus = (Int32)qTrustIPStatus;
					if (intqTrustIPStatus > 0)
					{
						where += " and [TrustIPStatus] = @TrustIPStatus";
						dbParams.Add(db.CreateParameter("TrustIPStatus", qTrustIPStatus));
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
                orderBy = "[TrustIPID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[TrustIPID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[IP]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[IP] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[TrustIPType]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[TrustIPType] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[TrustIPStatus]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[TrustIPStatus] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<TrustIP>(where, dbParams.ToArray());
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


            List<TrustIP> records = db.Take<TrustIP>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
