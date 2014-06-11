using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z10Cabbage.Entity.Helper
{

    public class Z10DepotProductHelper
    {

        #region 本实体的外键实体集合
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [仓库] 对应的实体
        /// </summary>
        public static Z10Depot GetDepotIDEntity(Zippy.Data.IDalProvider db, Z10DepotProduct entity)
        {
            return db.FindUnique<Z10Depot>("DepotID=@DepotID", db.CreateParameter("DepotID", entity.DepotID));
        }
        /// <summary>
        /// 表示 [仓库] 被选实体集合
        /// </summary>
        public static List<Z10Depot> GetDepotIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z10Depot>();
        }

        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetDepotIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z10Depot> objs = db.Take<Z10Depot>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.DepotID)
                    sb.AppendLine("<option value='" + obj.DepotID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.DepotID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        #endregion

        public static Z10DepotProduct Create(Zippy.Data.IDalProvider db, Int64 _DepotProductID)
        {
            Z10DepotProduct rtn =  db.FindUnique<Z10DepotProduct>(_DepotProductID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _DepotProductID)
        {
            return db.Delete<Z10DepotProduct>(_DepotProductID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Z10DepotProduct entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Z10DepotProduct entity)
        {
            return db.Update(entity);
        }


        public static List<Z10DepotProduct> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z10DepotProduct>(true);
        }

        public static List<Z10DepotProduct> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Z10DepotProduct>(count, true);
        }

        public static List<Z10DepotProduct> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z10DepotProduct>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 库存产品 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z10DepotProduct> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z10DepotProduct> rtn = new PaginatedList<Z10DepotProduct>();
            List<Z10DepotProduct> records = db.Take<Z10DepotProduct>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z10DepotProduct>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> rtn = new PaginatedList<EAP.Logic.Z10.View.V_DepotProduct>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));

            #region 开始查询
            if (paras != null)
            {
                object qProductID = paras["qProductID"];
                if (qProductID.IsNotNullOrEmpty())
                {
                    where += " and [ProductID] = @ProductID";
                    dbParams.Add(db.CreateParameter("ProductID", qProductID));
                }
                object qDepotID = paras["qDepotID"];
                if (qDepotID.IsNotNullOrEmpty())
                {
                    where += " and [DepotID] = @DepotID";
                    dbParams.Add(db.CreateParameter("DepotID", qDepotID));
                }
                object qProductTitle = paras["qProductTitle"];
                if (qDepotID.IsNotNullOrEmpty())
                {
                    where += " and [ProductTitle] like @ProductTitle";
                    dbParams.Add(db.CreateParameter("ProductTitle", "%" + qProductTitle + "%"));
                }
            }
            #endregion

            string orderBy = "[DepotID]";
            if (orderCol == 0)
            {
                orderBy = "[DepotID] desc";
            }
            else if (orderCol == 1)
            {
                orderBy = "[ProductID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[ProductID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[DepotID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[DepotID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CountAlarm]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CountAlarm] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[StockSum]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[StockSum] desc";
            }
            int RecordCount = db.Count<EAP.Logic.Z10.View.V_DepotProduct>(where, dbParams.ToArray());
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


            List<EAP.Logic.Z10.View.V_DepotProduct> records = db.Take<EAP.Logic.Z10.View.V_DepotProduct>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }

}
