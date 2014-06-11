using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class UserRoleHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [用户] 对应的实体
        /// </summary>
        public static User GetUserIDEntity(Zippy.Data.IDalProvider db, UserRole entity)
		{
			return db.FindUnique<User>("UserID=@UserID", db.CreateParameter("UserID", entity.UserID));
        }
        /// <summary>
        /// 表示 [用户] 被选实体集合
        /// </summary>
        public static List<User> GetUserIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<User>();
        }

        /// <summary>
        /// 表示 [角色] 对应的实体
        /// </summary>
        public static Role GetRoleIDEntity(Zippy.Data.IDalProvider db, UserRole entity)
		{
			return db.FindUnique<Role>("RoleID=@RoleID", db.CreateParameter("RoleID", entity.RoleID));
        }
        /// <summary>
        /// 表示 [角色] 被选实体集合
        /// </summary>
        public static List<Role> GetRoleIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Role>();
        }

		#endregion
		
        public static UserRole Create(Zippy.Data.IDalProvider db, Int64 _UserRoleID)
        {
			UserRole rtn =  db.FindUnique<UserRole>(_UserRoleID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _UserRoleID)
        {
            return db.Delete<UserRole>(_UserRoleID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, UserRole entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, UserRole entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<UserRole> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<UserRole>(true);
        }
		
        public static List<UserRole> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<UserRole>(count, true);
        }
		
        public static List<UserRole> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<UserRole>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 用户角色 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<UserRole> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<UserRole> rtn = new PaginatedList<UserRole>();
            List<UserRole> records = db.Take<UserRole>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<UserRole>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<UserRole> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<UserRole> rtn = new PaginatedList<UserRole>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
            if (paras != null)
            {
				object qUserIDStart = paras["qUserIDStart"];
				if (qUserIDStart != null && qUserIDStart.ToString()!="")
				{
					where += " and [UserID] >= @UserIDStart";
					dbParams.Add(db.CreateParameter("UserIDStart", qUserIDStart));
				}
				object qUserIDEnd = paras["qUserIDEnd"];
				if (qUserIDEnd != null && qUserIDEnd.ToString()!="")
				{
					where += " and [UserID] <= @UserID";
					dbParams.Add(db.CreateParameter("UserIDEnd", qUserIDEnd));
				}
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
				object qUserRoleType = paras["qUserRoleType"];
				if (qUserRoleType != null && qUserRoleType.ToString()!="")
				{
					Int32 intqUserRoleType = (Int32)qUserRoleType;
					if (intqUserRoleType > 0)
					{
						where += " and ([UserRoleType] & @UserRoleType = @UserRoleType)";
						dbParams.Add(db.CreateParameter("UserRoleType", qUserRoleType));
					}
				}
				object qUserRoleStatus = paras["qUserRoleStatus"];
				if (qUserRoleStatus != null && qUserRoleStatus.ToString()!="" )
				{
					Int32 intqUserRoleStatus = (Int32)qUserRoleStatus;
					if (intqUserRoleStatus > 0)
					{
						where += " and [UserRoleStatus] = @UserRoleStatus";
						dbParams.Add(db.CreateParameter("UserRoleStatus", qUserRoleStatus));
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
                orderBy = "[UserRoleID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[UserRoleID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[UserRoleType]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[UserRoleType] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[UserRoleStatus]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[UserRoleStatus] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<UserRole>(where, dbParams.ToArray());
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


            List<UserRole> records = db.Take<UserRole>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
