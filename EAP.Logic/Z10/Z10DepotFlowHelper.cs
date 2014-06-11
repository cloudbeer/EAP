using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z10Cabbage.Entity.Helper
{

    public class Z10DepotFlowHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z10DepotFlow Create(Zippy.Data.IDalProvider db, Int64 _FlowID)
        {
			Z10DepotFlow rtn =  db.FindUnique<Z10DepotFlow>(_FlowID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _FlowID)
        {
            return db.Delete<Z10DepotFlow>(_FlowID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z10DepotFlow entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z10DepotFlow entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z10DepotFlow> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z10DepotFlow>(true);
        }
		
        public static List<Z10DepotFlow> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z10DepotFlow>(count, true);
        }
		
        public static List<Z10DepotFlow> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z10DepotFlow>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 库存流水 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z10DepotFlow> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z10DepotFlow> rtn = new PaginatedList<Z10DepotFlow>();
            List<Z10DepotFlow> records = db.Take<Z10DepotFlow>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z10DepotFlow>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<EAP.Logic.Z10.View.V_DepotFlow> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<EAP.Logic.Z10.View.V_DepotFlow> rtn = new PaginatedList<EAP.Logic.Z10.View.V_DepotFlow>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
                object qDepotID = paras["qDepotID"];
                if (qDepotID.IsNotNullOrEmpty())
				{
                    where += " and [DepotID] = @DepotID";
                    dbParams.Add(db.CreateParameter("DepotID", qDepotID));
                }
                object qProductID = paras["qProductID"];
                if (qProductID.IsNotNullOrEmpty())
                {
                    where += " and [ProductID] = @ProductID";
                    dbParams.Add(db.CreateParameter("ProductID", qProductID));
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
                orderBy = "[DepotID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[DepotID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[OrderID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[OrderID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[Count]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[Count] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<EAP.Logic.Z10.View.V_DepotFlow>(where, dbParams.ToArray());
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


            List<EAP.Logic.Z10.View.V_DepotFlow> records = db.Take<EAP.Logic.Z10.View.V_DepotFlow>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
