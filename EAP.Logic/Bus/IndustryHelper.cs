using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class IndustryHelper
    {	
    
		#region 本实体的外键实体集合
        /// <summary>
        /// 获取 [行业 的 父行业] 的 [行业] 集合
        /// </summary>
        public static List<Industry> GetParentID_Industrys(Zippy.Data.IDalProvider db, Industry entity)
        {
                if (entity.IndustryID.HasValue)
                   return db.Take<Industry>("ParentID=@ParentID", db.CreateParameter("ParentID", entity.IndustryID));            
            return new List<Industry>();
            
        }
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [父行业] 对应的实体
        /// </summary>
        public static Industry GetParentIDEntity(Zippy.Data.IDalProvider db, Industry entity)
		{
			return db.FindUnique<Industry>("IndustryID=@IndustryID", db.CreateParameter("IndustryID", entity.ParentID));
        }
        /// <summary>
        /// 表示 [父行业] 被选实体集合
        /// </summary>
        public static List<Industry> GetParentIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Industry>();
        }

		#endregion
		
        public static Industry Create(Zippy.Data.IDalProvider db, Guid _IndustryID)
        {
			Industry rtn =  db.FindUnique<Industry>(_IndustryID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Guid _IndustryID)
        {
            return db.Delete<Industry>(_IndustryID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Industry entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Industry entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Industry> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Industry>(true);
        }
		
        public static List<Industry> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Industry>(count, true);
        }
		
        public static List<Industry> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Industry>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 行业 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Industry> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Industry> rtn = new PaginatedList<Industry>();
            List<Industry> records = db.Take<Industry>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Industry>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Industry> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Industry> rtn = new PaginatedList<Industry>();
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
				object qIndustryType = paras["qIndustryType"];
				if (qIndustryType != null && qIndustryType.ToString()!="")
				{
					Int32 intqIndustryType = (Int32)qIndustryType;
					if (intqIndustryType > 0)
					{
						where += " and ([IndustryType] & @IndustryType = @IndustryType)";
						dbParams.Add(db.CreateParameter("IndustryType", qIndustryType));
					}
				}
				object qIndustryStatus = paras["qIndustryStatus"];
				if (qIndustryStatus != null && qIndustryStatus.ToString()!="" )
				{
					Int32 intqIndustryStatus = (Int32)qIndustryStatus;
					if (intqIndustryStatus > 0)
					{
						where += " and [IndustryStatus] = @IndustryStatus";
						dbParams.Add(db.CreateParameter("IndustryStatus", qIndustryStatus));
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
                orderBy = "[Title]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[ParentID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[ParentID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[IndustryType]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[IndustryType] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[IndustryStatus]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[IndustryStatus] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Industry>(where, dbParams.ToArray());
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


            List<Industry> records = db.Take<Industry>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
