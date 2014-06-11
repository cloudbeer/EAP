using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z30CRM.Entity.Helper
{

    public class Z30CommunicationHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z30Communication Create(Zippy.Data.IDalProvider db, Int64 _CommunicationID)
        {
			Z30Communication rtn =  db.FindUnique<Z30Communication>(_CommunicationID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _CommunicationID)
        {
            return db.Delete<Z30Communication>(_CommunicationID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z30Communication entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z30Communication entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z30Communication> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z30Communication>(true);
        }
		
        public static List<Z30Communication> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z30Communication>(count, true);
        }
		
        public static List<Z30Communication> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z30Communication>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 沟通记录 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z30Communication> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z30Communication> rtn = new PaginatedList<Z30Communication>();
            List<Z30Communication> records = db.Take<Z30Communication>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z30Communication>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z30Communication> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z30Communication> rtn = new PaginatedList<Z30Communication>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {

                object qCreator = paras["qCreator"];
                if (qCreator.IsNotNullOrEmpty())
                {
                    where += " and [Creator] = @Creator";
                    dbParams.Add(db.CreateParameter("Creator", qCreator));
                }
				object qCustomerID = paras["qCustomerID"];
                if (qCustomerID.IsNotNullOrEmpty())
				{
					where += " and [CustomerID] = @CustomerID";
                    dbParams.Add(db.CreateParameter("CustomerID", qCustomerID));
				}
				object qContent = paras["qContent"];
				if (qContent.IsNotNullOrEmpty())
				{
					where += " and [Content] like @Content";
					dbParams.Add(db.CreateParameter("Content", "%" + qContent + "%"));
				}
                object qWish = paras["qWish"];
                if (qWish.IsNotNullOrEmpty())
				{
                    where += " and [Wish] >= @Wish";
                    dbParams.Add(db.CreateParameter("Wish", qWish));
				}
                object qSuccessRatio = paras["qSuccessRatio"];
                if (qSuccessRatio.IsNotNullOrEmpty())
				{
                    where += " and [SuccessRatio] >= @SuccessRatio";
                    dbParams.Add(db.CreateParameter("SuccessRatio", qSuccessRatio));
				}
				object qCreateDateStart = paras["qCreateDateStart"];
				if (qCreateDateStart.IsNotNullOrEmpty())
				{
					where += " and [CreateDate] >= @CreateDateStart";
					dbParams.Add(db.CreateParameter("CreateDateStart", qCreateDateStart));
				}
				object qCreateDateEnd = paras["qCreateDateEnd"];
				if (qCreateDateEnd.IsNotNullOrEmpty())
				{
					where += " and [CreateDate] < @CreateDate";
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
                orderBy = "[CommunicationID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[CommunicationID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[CustomerID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[CustomerID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[VisitWay]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[VisitWay] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[Wish]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[Wish] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[NextVisitDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[NextVisitDate] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[SuccessRatio]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[SuccessRatio] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[VisitDate]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[VisitDate] desc";
            }
            else if (orderCol == 15)
            {
                orderBy = "[VisitDuration]";
            }
            else if (orderCol == 16)
            {
                orderBy = "[VisitDuration] desc";
            }
            else if (orderCol == 17)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 18)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z30Communication>(where, dbParams.ToArray());
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


            List<Z30Communication> records = db.Take<Z30Communication>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
