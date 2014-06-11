using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class AreaHelper
    {	
    
		#region 本实体的外键实体集合
        /// <summary>
        /// 获取 [地区 的 父地区] 的 [地区] 集合
        /// </summary>
        public static List<Area> GetParentID_Areas(Zippy.Data.IDalProvider db, Area entity)
        {
                if (entity.AreaID.HasValue)
                   return db.Take<Area>("ParentID=@ParentID", db.CreateParameter("ParentID", entity.AreaID));            
            return new List<Area>();
            
        }
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [父地区] 对应的实体
        /// </summary>
        public static Area GetParentIDEntity(Zippy.Data.IDalProvider db, Area entity)
		{
			return db.FindUnique<Area>("AreaID=@AreaID", db.CreateParameter("AreaID", entity.ParentID));
        }
        /// <summary>
        /// 表示 [父地区] 被选实体集合
        /// </summary>
        public static List<Area> GetParentIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Area>();
        }

		#endregion
		
        public static Area Create(Zippy.Data.IDalProvider db, Guid _AreaID)
        {
			Area rtn =  db.FindUnique<Area>(_AreaID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Guid _AreaID)
        {
            return db.Delete<Area>(_AreaID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Area entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Area entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Area> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Area>(true);
        }
		
        public static List<Area> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Area>(count, true);
        }
		
        public static List<Area> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Area>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 地区 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Area> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Area> rtn = new PaginatedList<Area>();
            List<Area> records = db.Take<Area>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Area>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Area> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Area> rtn = new PaginatedList<Area>();
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
				object qParentIDStart = paras["qParentIDStart"];
				if (qParentIDStart != null && qParentIDStart.ToString()!="")
				{
					where += " and [ParentID] >= @ParentIDStart";
					dbParams.Add(db.CreateParameter("ParentIDStart", qParentIDStart));
				}
				object qParentIDEnd = paras["qParentIDEnd"];
				if (qParentIDEnd != null && qParentIDEnd.ToString()!="")
				{
					where += " and [ParentID] <= @ParentID";
					dbParams.Add(db.CreateParameter("ParentIDEnd", qParentIDEnd));
				}
				object qAreaType = paras["qAreaType"];
				if (qAreaType != null && qAreaType.ToString()!="")
				{
					Int32 intqAreaType = (Int32)qAreaType;
					if (intqAreaType > 0)
					{
						where += " and ([AreaType] & @AreaType = @AreaType)";
						dbParams.Add(db.CreateParameter("AreaType", qAreaType));
					}
				}
				object qAreaStatus = paras["qAreaStatus"];
				if (qAreaStatus != null && qAreaStatus.ToString()!="" )
				{
					Int32 intqAreaStatus = (Int32)qAreaStatus;
					if (intqAreaStatus > 0)
					{
						where += " and [AreaStatus] = @AreaStatus";
						dbParams.Add(db.CreateParameter("AreaStatus", qAreaStatus));
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
                orderBy = "[AreaID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[AreaID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Title]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[ParentID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[ParentID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[AreaCode]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[AreaCode] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[AreaType]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[AreaType] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[AreaStatus]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[AreaStatus] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Area>(where, dbParams.ToArray());
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


            List<Area> records = db.Take<Area>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
