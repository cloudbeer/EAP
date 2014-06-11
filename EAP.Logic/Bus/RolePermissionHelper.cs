using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class RolePermissionHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static RolePermission Create(Zippy.Data.IDalProvider db, Int64 _RolePermissionID)
        {
			RolePermission rtn =  db.FindUnique<RolePermission>(_RolePermissionID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _RolePermissionID)
        {
            return db.Delete<RolePermission>(_RolePermissionID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, RolePermission entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, RolePermission entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<RolePermission> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<RolePermission>(true);
        }
		
        public static List<RolePermission> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<RolePermission>(count, true);
        }
		
        public static List<RolePermission> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<RolePermission>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 角色权限 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<RolePermission> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<RolePermission> rtn = new PaginatedList<RolePermission>();
            List<RolePermission> records = db.Take<RolePermission>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<RolePermission>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<RolePermission> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<RolePermission> rtn = new PaginatedList<RolePermission>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qRoleIDStart = paras["qRoleIDStart"];
				if (qRoleIDStart != null && qRoleIDStart.ToString()!="")
				{
					where += " and [RoleID] >= @RoleIDStart";
					dbParams.Add(db.CreateParameter("RoleIDStart", qRoleIDStart));
				}
				object qRoleIDEnd = paras["qRoleIDEnd"];
				if (qRoleIDEnd != null && qRoleIDEnd.ToString()!="")
				{
					where += " and [RoleID] <= @RoleID";
					dbParams.Add(db.CreateParameter("RoleIDEnd", qRoleIDEnd));
				}
				object qPermissionIDStart = paras["qPermissionIDStart"];
				if (qPermissionIDStart != null && qPermissionIDStart.ToString()!="")
				{
					where += " and [PermissionID] >= @PermissionIDStart";
					dbParams.Add(db.CreateParameter("PermissionIDStart", qPermissionIDStart));
				}
				object qPermissionIDEnd = paras["qPermissionIDEnd"];
				if (qPermissionIDEnd != null && qPermissionIDEnd.ToString()!="")
				{
					where += " and [PermissionID] <= @PermissionID";
					dbParams.Add(db.CreateParameter("PermissionIDEnd", qPermissionIDEnd));
				}
				object qPermissionType = paras["qPermissionType"];
				if (qPermissionType != null && qPermissionType.ToString()!="")
				{
					Int32 intqPermissionType = (Int32)qPermissionType;
					if (intqPermissionType > 0)
					{
						where += " and ([PermissionType] & @PermissionType = @PermissionType)";
						dbParams.Add(db.CreateParameter("PermissionType", qPermissionType));
					}
				}
				object qPermissionStatus = paras["qPermissionStatus"];
				if (qPermissionStatus != null && qPermissionStatus.ToString()!="" )
				{
					Int32 intqPermissionStatus = (Int32)qPermissionStatus;
					if (intqPermissionStatus > 0)
					{
						where += " and [PermissionStatus] = @PermissionStatus";
						dbParams.Add(db.CreateParameter("PermissionStatus", qPermissionStatus));
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
                orderBy = "[RolePermissionID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[RolePermissionID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[RoleID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[RoleID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[PermissionID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[PermissionID] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[PermissionType]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[PermissionType] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[PermissionStatus]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[PermissionStatus] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<RolePermission>(where, dbParams.ToArray());
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


            List<RolePermission> records = db.Take<RolePermission>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
