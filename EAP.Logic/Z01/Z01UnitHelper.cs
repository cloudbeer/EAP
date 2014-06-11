using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01UnitHelper
    {	
    
		#region 本实体的外键实体集合
        /// <summary>
        /// 获取 [计量单位 的 标准计量单位] 的 [产品] 集合
        /// </summary>
        public static List<Z01Product> GetUnitID_Z01Products(Zippy.Data.IDalProvider db, Z01Unit entity)
        {
                if (entity.UnitID.HasValue)
                   return db.Take<Z01Product>("UnitID=@UnitID", db.CreateParameter("UnitID", entity.UnitID));            
            return new List<Z01Product>();
            
        }
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z01Unit Create(Zippy.Data.IDalProvider db, Int32 _UnitID)
        {
			Z01Unit rtn =  db.FindUnique<Z01Unit>(_UnitID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int32 _UnitID)
        {
            return db.Delete<Z01Unit>(_UnitID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01Unit entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01Unit entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01Unit> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Unit>(true);
        }
		
        public static List<Z01Unit> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01Unit>(count, true);
        }
		
        public static List<Z01Unit> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01Unit>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 计量单位 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01Unit> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01Unit> rtn = new PaginatedList<Z01Unit>();
            List<Z01Unit> records = db.Take<Z01Unit>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01Unit>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01Unit> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01Unit> rtn = new PaginatedList<Z01Unit>();
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
                orderBy = "[Title]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01Unit>(where, dbParams.ToArray());
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


            List<Z01Unit> records = db.Take<Z01Unit>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
