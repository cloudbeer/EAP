using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z10Cabbage.Entity.Helper
{

    public class Z10ConfigHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z10Config Create(Zippy.Data.IDalProvider db, Int64 _ConfigID)
        {
			Z10Config rtn =  db.FindUnique<Z10Config>(_ConfigID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _ConfigID)
        {
            return db.Delete<Z10Config>(_ConfigID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z10Config entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z10Config entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z10Config> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z10Config>(true);
        }
		
        public static List<Z10Config> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z10Config>(count, true);
        }
		
        public static List<Z10Config> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z10Config>(sqlEntry, cmdParameters);
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
        public static PaginatedList<Z10Config> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z10Config> rtn = new PaginatedList<Z10Config>();
            List<Z10Config> records = db.Take<Z10Config>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z10Config>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z10Config> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z10Config> rtn = new PaginatedList<Z10Config>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qKey = paras["qKey"];
				if (qKey.IsNotNullOrEmpty())
				{
					where += " and [Key] like @Key";
					dbParams.Add(db.CreateParameter("Key", "%" + qKey + "%"));
				}
				object qValue = paras["qValue"];
				if (qValue.IsNotNullOrEmpty())
				{
					where += " and [Value] like @Value";
					dbParams.Add(db.CreateParameter("Value", "%" + qValue + "%"));
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
                orderBy = "[Key]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[Key] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Value]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Value] desc";
            }
            int RecordCount = db.Count<Z10Config>(where, dbParams.ToArray());
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


            List<Z10Config> records = db.Take<Z10Config>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
