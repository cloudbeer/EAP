using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01FinancialFlowHelper
    {

        #region 本实体的外键实体集合
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [银行] 对应的实体
        /// </summary>
        public static Z01Bank GetBankIDEntity(Zippy.Data.IDalProvider db, Z01FinancialFlow entity)
        {
            return db.FindUnique<Z01Bank>("BankID=@BankID", db.CreateParameter("BankID", entity.BankID));
        }
        /// <summary>
        /// 表示 [银行] 被选实体集合
        /// </summary>
        public static List<Z01Bank> GetBankIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Bank>();
        }

        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetBankIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01Bank> objs = db.Take<Z01Bank>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.BankID)
                    sb.AppendLine("<option value='" + obj.BankID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.BankID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        #endregion

        public static Z01FinancialFlow Create(Zippy.Data.IDalProvider db, Int64 _FlowID)
        {
            Z01FinancialFlow rtn =  db.FindUnique<Z01FinancialFlow>(_FlowID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _FlowID)
        {
            return db.Delete<Z01FinancialFlow>(_FlowID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Z01FinancialFlow entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Z01FinancialFlow entity)
        {
            return db.Update(entity);
        }


        public static List<Z01FinancialFlow> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01FinancialFlow>(true);
        }

        public static List<Z01FinancialFlow> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Z01FinancialFlow>(count, true);
        }

        public static List<Z01FinancialFlow> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01FinancialFlow>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 财务流水 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01FinancialFlow> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01FinancialFlow> rtn = new PaginatedList<Z01FinancialFlow>();
            List<Z01FinancialFlow> records = db.Take<Z01FinancialFlow>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01FinancialFlow>(where, cmdParameters);
            return rtn;
        }

        #region 查询 Query
        public static PaginatedList<EAP.Logic.Z01.View.V_FinancialFlow> Query(Zippy.Data.IDalProvider db,
            Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol, out decimal sumAll)
        {
            PaginatedList<EAP.Logic.Z01.View.V_FinancialFlow> rtn = new PaginatedList<EAP.Logic.Z01.View.V_FinancialFlow>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));

            object qCurrency = null;
            #region 开始查询
            if (paras != null)
            {
                object qBankID = paras["qBankID"];
                if (qBankID.IsNotNullOrEmpty())
                {
                    where += " and [BankID] = @BankID";
                    dbParams.Add(db.CreateParameter("BankID", qBankID));
                }
                object qCategoryID = paras["qCategoryID"];
                if (qCategoryID.IsNotNullOrEmpty())
                {
                    where += " and [CategoryID] = @CategoryID";
                    dbParams.Add(db.CreateParameter("CategoryID", qCategoryID));
                }
                qCurrency = paras["qCurrency"];
                if (qCurrency.IsNotNullOrEmpty())
                {
                    where += " and [Currency] = @Currency";
                    dbParams.Add(db.CreateParameter("Currency", qCurrency));
                }
                object qFlowType = paras["qFlowType"];
                if (qFlowType.IsNotNullOrEmpty())
                {
                    Int32 intqFlowType = (Int32)qFlowType;
                    if (intqFlowType > 0)
                    {
                        where += " and ([FlowType] & @FlowType = @FlowType)";
                        dbParams.Add(db.CreateParameter("FlowType", qFlowType));
                    }
                }
                object qFlowStatus = paras["qFlowStatus"];
                if (qFlowStatus.IsNotNullOrEmpty())
                {
                    Int32 intqFlowStatus = (Int32)qFlowStatus;
                    if (intqFlowStatus > 0)
                    {
                        where += " and [FlowStatus] = @FlowStatus";
                        dbParams.Add(db.CreateParameter("FlowStatus", qFlowStatus));
                    }
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
                    where += " and [CreateDate] < @CreateDateEnd";
                    dbParams.Add(db.CreateParameter("CreateDateEnd", ((DateTime)qCreateDateEnd).AddDays(1)));
                }
                object qInOut = paras["qInOut"];
                if (qInOut.IsNotNullOrEmpty())
                {
                    if (qInOut.Equals("In"))
                    {
                        where += " and [Amount]>=0";
                    }
                    else if (qInOut.Equals("Out"))
                    {
                        where += " and [Amount]<0";

                    }
                }

            }
            #endregion

            string orderBy = "[CreateDate] desc";
            if (orderCol == 0)
            {
                orderBy = "[CreateDate] desc";
            }
            else if (orderCol == 1)
            {
                orderBy = "[FlowID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[FlowID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[BankID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[BankID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CategoryID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CategoryID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[OrderID]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[OrderID] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[FlowType]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[FlowType] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[FlowStatus]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[FlowStatus] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<EAP.Logic.Z01.View.V_FinancialFlow>(where, dbParams.ToArray());
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


            List<EAP.Logic.Z01.View.V_FinancialFlow> records = db.Take<EAP.Logic.Z01.View.V_FinancialFlow>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;
            if (qCurrency.IsNotNullOrEmpty())
            {
                string sqlSum = "select sum(Amount) as sAmount from V_FinancialFlow where " + where;
                object oSum = db.ExecuteScalar(sqlSum, dbParams.ToArray());
                sumAll = oSum.ToDecimal();
            }
            else
            {
                sumAll = 0;
            }

            return rtn;

        }
        #endregion

        public static List<Z01FinancialFlow> QueryForStat(Zippy.Data.IDalProvider db,
            Guid tenantID, Hashtable paras)
        {
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));

            object qCurrency = null;
            #region 开始查询
            if (paras != null)
            {
                object qDateFrom = paras["qDateFrom"];
                if (qDateFrom.IsNotNullOrEmpty())
                {
                    where += " and [CreateDate] >= @CreateDateStart";
                    dbParams.Add(db.CreateParameter("CreateDateStart", qDateFrom));
                }
                object qDateTo = paras["qDateTo"];
                if (qDateTo.IsNotNullOrEmpty())
                {
                    where += " and [CreateDate] < @CreateDateEnd";
                    dbParams.Add(db.CreateParameter("CreateDateEnd", ((DateTime)qDateTo).AddDays(1)));
                }
                qCurrency = paras["qCurrency"];
                if (qCurrency.IsNotNullOrEmpty())
                {
                    where += " and [Currency] = @Currency";
                    dbParams.Add(db.CreateParameter("Currency", qCurrency));
                }
                object qIn=paras["qIn"];
                if (qIn.IsNotNullOrEmpty())
                {
                    if (qIn.Equals(1))
                        where += " and Amount>0";
                    else
                        where += " and Amount<0";
                }
            }
            #endregion

            return db.Take<Z01FinancialFlow>(where, dbParams.ToArray());

        }
    }

}
