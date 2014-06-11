using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class UserGroupHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [用户] 对应的实体
        /// </summary>
        public static User GetUserIDEntity(Zippy.Data.IDalProvider db, UserGroup entity)
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
        /// 表示 [组] 对应的实体
        /// </summary>
        public static Group GetGroupIDEntity(Zippy.Data.IDalProvider db, UserGroup entity)
		{
			return db.FindUnique<Group>("GroupID=@GroupID", db.CreateParameter("GroupID", entity.GroupID));
        }
        /// <summary>
        /// 表示 [组] 被选实体集合
        /// </summary>
        public static List<Group> GetGroupIDEntities(Zippy.Data.IDalProvider db)
        {
			return db.Take<Group>();
        }

		#endregion
		
        public static UserGroup Create(Zippy.Data.IDalProvider db, Int64 _UGID)
        {
			UserGroup rtn =  db.FindUnique<UserGroup>(_UGID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _UGID)
        {
            return db.Delete<UserGroup>(_UGID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, UserGroup entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, UserGroup entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<UserGroup> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<UserGroup>(true);
        }
		
        public static List<UserGroup> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<UserGroup>(count, true);
        }
		
        public static List<UserGroup> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<UserGroup>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 用户分组 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<UserGroup> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<UserGroup> rtn = new PaginatedList<UserGroup>();
            List<UserGroup> records = db.Take<UserGroup>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<UserGroup>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<UserGroup> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<UserGroup> rtn = new PaginatedList<UserGroup>();
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
				object qGroupIDStart = paras["qGroupIDStart"];
				if (qGroupIDStart != null && qGroupIDStart.ToString()!="")
				{
					where += " and [GroupID] >= @GroupIDStart";
					dbParams.Add(db.CreateParameter("GroupIDStart", qGroupIDStart));
				}
				object qGroupIDEnd = paras["qGroupIDEnd"];
				if (qGroupIDEnd != null && qGroupIDEnd.ToString()!="")
				{
					where += " and [GroupID] <= @GroupID";
					dbParams.Add(db.CreateParameter("GroupIDEnd", qGroupIDEnd));
				}
				object qUserGroupType = paras["qUserGroupType"];
				if (qUserGroupType != null && qUserGroupType.ToString()!="")
				{
					Int32 intqUserGroupType = (Int32)qUserGroupType;
					if (intqUserGroupType > 0)
					{
						where += " and ([UserGroupType] & @UserGroupType = @UserGroupType)";
						dbParams.Add(db.CreateParameter("UserGroupType", qUserGroupType));
					}
				}
				object qUserGroupStatus = paras["qUserGroupStatus"];
				if (qUserGroupStatus != null && qUserGroupStatus.ToString()!="" )
				{
					Int32 intqUserGroupStatus = (Int32)qUserGroupStatus;
					if (intqUserGroupStatus > 0)
					{
						where += " and [UserGroupStatus] = @UserGroupStatus";
						dbParams.Add(db.CreateParameter("UserGroupStatus", qUserGroupStatus));
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
                orderBy = "[UGID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[UGID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[UserGroupType]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[UserGroupType] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[UserGroupStatus]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[UserGroupStatus] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<UserGroup>(where, dbParams.ToArray());
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


            List<UserGroup> records = db.Take<UserGroup>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
