using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01BankHelper
    {	
    
		#region 本实体的外键实体集合
        /// <summary>
        /// 获取 [银行 的 银行] 的 [财务流水] 集合
        /// </summary>
        public static List<Z01FinancialFlow> GetBankID_Z01FinancialFlows(Zippy.Data.IDalProvider db, Z01Bank entity)
        {
                if (entity.BankID.HasValue)
                   return db.Take<Z01FinancialFlow>("BankID=@BankID", db.CreateParameter("BankID", entity.BankID));            
            return new List<Z01FinancialFlow>();
            
        }
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z01Bank Create(Zippy.Data.IDalProvider db, Int64 _BankID)
        {
			Z01Bank rtn =  db.FindUnique<Z01Bank>(_BankID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _BankID)
        {
            return db.Delete<Z01Bank>(_BankID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01Bank entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01Bank entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01Bank> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Bank>(true);
        }
		
        public static List<Z01Bank> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01Bank>(count, true);
        }
		
        public static List<Z01Bank> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01Bank>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 银行 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01Bank> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01Bank> rtn = new PaginatedList<Z01Bank>();
            List<Z01Bank> records = db.Take<Z01Bank>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01Bank>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01Bank> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01Bank> rtn = new PaginatedList<Z01Bank>();
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
				object qAccount = paras["qAccount"];
				if (qAccount.IsNotNullOrEmpty())
				{
					where += " and [Account] like @Account";
					dbParams.Add(db.CreateParameter("Account", "%" + qAccount + "%"));
				}
			}
            #endregion
			
			string orderBy = "[DisplayOrder]";
            if (orderCol == 0)
            {
                orderBy = "[DisplayOrder] desc";
            }
            else if (orderCol == 1)
            {
                orderBy = "[Title]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Brief]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Brief] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[Account]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[Account] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[Contact]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[Contact] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[Tel]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[Tel] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[Fax]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[Fax] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01Bank>(where, dbParams.ToArray());
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


            List<Z01Bank> records = db.Take<Z01Bank>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
