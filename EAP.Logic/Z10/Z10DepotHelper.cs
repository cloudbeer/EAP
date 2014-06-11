using System;
using System.Collections;
using System.Collections.Generic;
using Z10Cabbage.Entity;
using Zippy.Data.Collections;

namespace Z10Cabbage.Entity.Helper
{

    public class Z10DepotHelper
    {

        #region 本实体的外键实体集合
        #endregion

        #region 本实体外键对应的实体
        #endregion

        public static Z10Depot Create(Zippy.Data.IDalProvider db, Int64 _DepotID)
        {
            Z10Depot rtn = db.FindUnique<Z10Depot>(_DepotID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _DepotID)
        {
            return db.Delete<Z10Depot>(_DepotID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Z10Depot entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Z10Depot entity)
        {
            return db.Update(entity);
        }


        public static List<Z10Depot> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z10Depot>(true);
        }

        public static List<Z10Depot> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Z10Depot>(count, true);
        }

        public static List<Z10Depot> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z10Depot>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 仓库 执行查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z10Depot> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z10Depot> rtn = new PaginatedList<Z10Depot>();
            List<Z10Depot> records = db.Take<Z10Depot>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z10Depot>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<Z10Depot> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z10Depot> rtn = new PaginatedList<Z10Depot>();
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
            }
            #endregion

            string orderBy = "[CreateDate] desc";
            if (orderCol == 0)
            {
                orderBy = "[CreateDate] desc";
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
                orderBy = "[Code]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Code] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CreateDate] desc";
            }

            int RecordCount = db.Count<Z10Depot>(where, dbParams.ToArray());
            int PageCount = 0;
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


            List<Z10Depot> records = db.Take<Z10Depot>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }

        public static PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> ListProduct(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> rtn = new PaginatedList<EAP.Logic.Z10.View.V_DepotProduct>();

            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));


            #region 开始查询
            if (paras != null)
            {
                object qTitle = paras["qTitle"];
                if (qTitle.IsNotNullOrEmpty())
                {
                    where += " and ([ProductTitle] like @Title or [DepotTitle] like @Title) ";
                    dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
                }
                object qDepot = paras["qDepot"];
                if (qDepot.IsNotNullOrEmpty() && qDepot.ToInt64() > 0)
                {
                    where += " and [DepotID] = @DepotID ";
                    dbParams.Add(db.CreateParameter("DepotID", qDepot));
                }
            }
            #endregion

            string orderBy = "[DepotProductID] desc";
            if (orderCol == 0)
            {
                orderBy = "[DepotProductID] desc";
            }
            else if (orderCol == 1)
            {
                orderBy = "[ProductTitle]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[ProductTitle] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[DepotTitle]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[DepotTitle] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[StockSum]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[StockSum] desc";
            }

            int RecordCount = db.Count<EAP.Logic.Z10.View.V_DepotProduct>(where, dbParams.ToArray());
            int PageCount = 0;
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


            List<EAP.Logic.Z10.View.V_DepotProduct> records = db.Take<EAP.Logic.Z10.View.V_DepotProduct>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;
        }
    }

}
