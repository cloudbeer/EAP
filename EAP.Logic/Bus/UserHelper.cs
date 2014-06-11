using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;
using EAP.Logic.Bus.View;

namespace EAP.Bus.Entity.Helper
{

    public class UserHelper
    {

        #region 本实体的外键实体集合
        /// <summary>
        /// 获取 [用户 的 用户] 的 [用户分组] 集合
        /// </summary>
        public static List<UserGroup> GetUserID_UserGroups(Zippy.Data.IDalProvider db, User entity)
        {
            if (entity.UserID.HasValue)
                return db.Take<UserGroup>("UserID=@UserID", db.CreateParameter("UserID", entity.UserID));
            return new List<UserGroup>();

        }
        /// <summary>
        /// 获取 [用户 的 用户] 的 [用户角色] 集合
        /// </summary>
        public static List<V_UserRole> GetUserID_UserRoles(Zippy.Data.IDalProvider db, User entity)
        {
            if (entity.UserID.HasValue)
                return db.Take<V_UserRole>("UserID=@UserID", db.CreateParameter("UserID", entity.UserID));
            return new List<V_UserRole>();

        }
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [租户] 对应的实体
        /// </summary>
        public static Tenant GetTenantIDEntity(Zippy.Data.IDalProvider db, User entity)
        {
            return db.FindUnique<Tenant>("TenantID=@TenantID", db.CreateParameter("TenantID", entity.TenantID));
        }
        /// <summary>
        /// 表示 [租户] 被选实体集合
        /// </summary>
        public static List<Tenant> GetTenantIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Tenant>();
        }

        /// <summary>
        /// 表示 [分组] 对应的实体
        /// </summary>
        public static Group GetGroupIDEntity(Zippy.Data.IDalProvider db, User entity)
        {
            return db.FindUnique<Group>("GroupID=@GroupID", db.CreateParameter("GroupID", entity.GroupID));
        }
        /// <summary>
        /// 表示 [分组] 被选实体集合
        /// </summary>
        public static List<Group> GetGroupIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Group>();
        }

        #endregion

        public static User Create(Zippy.Data.IDalProvider db, Guid _UserID)
        {
            User rtn = db.FindUnique<User>(_UserID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Guid _UserID)
        {
            return db.Delete<User>(_UserID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, User entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, User entity)
        {
            return db.Update(entity);
        }


        public static List<User> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<User>(true);
        }

        public static List<User> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<User>(count, true);
        }

        public static List<User> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<User>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 用户 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<User> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<User> rtn = new PaginatedList<User>();
            List<User> records = db.Take<User>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<User>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<User> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<User> rtn = new PaginatedList<User>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));

            #region 开始查询
            if (paras != null)
            {
                object qUserName = paras["qUserName"];
                if (qUserName.IsNotNullOrEmpty())
                {
                    where += " and [UserName] like @UserName";
                    dbParams.Add(db.CreateParameter("UserName", "%" + qUserName + "%"));
                }
                object qEmail = paras["qEmail"];
                if (qEmail.IsNotNullOrEmpty())
                {
                    where += " and [Email] like @Email";
                    dbParams.Add(db.CreateParameter("Email", "%" + qEmail + "%"));
                }
                object qName = paras["qName"];
                if (qName.IsNotNullOrEmpty())
                {
                    where += " and [Name] like @Name";
                    dbParams.Add(db.CreateParameter("Name", "%" + qName + "%"));
                }
                object qNickname = paras["qNickname"];
                if (qNickname.IsNotNullOrEmpty())
                {
                    where += " and [Nickname] like @Nickname";
                    dbParams.Add(db.CreateParameter("Nickname", "%" + qNickname + "%"));
                }
                object qMobileID1 = paras["qMobileID1"];
                if (qMobileID1.IsNotNullOrEmpty())
                {
                    where += " and [MobileID1] like @MobileID1";
                    dbParams.Add(db.CreateParameter("MobileID1", "%" + qMobileID1 + "%"));
                }
                object qMobileID2 = paras["qMobileID2"];
                if (qMobileID2.IsNotNullOrEmpty())
                {
                    where += " and [MobileID2] like @MobileID2";
                    dbParams.Add(db.CreateParameter("MobileID2", "%" + qMobileID2 + "%"));
                }
                object qUserType = paras["qUserType"];
                if (qUserType.IsNotNullOrEmpty())
                {
                    Int32 intqUserType = (Int32)qUserType;
                    if (intqUserType > 0)
                    {
                        where += " and ([UserType] & @UserType = @UserType)";
                        dbParams.Add(db.CreateParameter("UserType", qUserType));
                    }
                }
                object qUserStatus = paras["qUserStatus"];
                if (qUserStatus.IsNotNullOrEmpty())
                {
                    Int32 intqUserStatus = (Int32)qUserStatus;
                    if (intqUserStatus > 0)
                    {
                        where += " and [UserStatus] = @UserStatus";
                        dbParams.Add(db.CreateParameter("UserStatus", qUserStatus));
                    }
                }
                object qCreateDateStart = paras["qCreateDateStart"];
                if (qCreateDateStart.IsNotNullOrEmpty())
                {
                    where += " and [CreateDate] >= @CreateDateStart";
                    dbParams.Add(db.CreateParameter("CreateDateStart", qCreateDateStart));
                }
                object qCreateDateEnd = paras["qCreateDateEnd"];
                if (qCreateDateEnd.IsNotNullOrEmpty())
                {
                    where += " and [CreateDate] < @CreateDateEnd";
                    dbParams.Add(db.CreateParameter("CreateDateEnd", ((DateTime)qCreateDateEnd).AddDays(1)));
                }
                object qGroupID=paras["qGroupID"];
                if (qGroupID.IsNotNullOrEmpty())
                {
                    where += " and [UserID] in (select [UserID] from [UserGroup] where [GroupID]=@GroupID)";
                    dbParams.Add(db.CreateParameter("GroupID", qGroupID));
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
                orderBy = "[UserName]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[UserName] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Email]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Email] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[Nickname]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[Nickname] desc";
            }
            int RecordCount = db.Count<User>(where, dbParams.ToArray());
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


            List<User> records = db.Take<User>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        /// <param name="db"></param>
        public static void SetRole(Guid userid, Guid roleid, Guid? tenantid, Guid? creator, Zippy.Data.IDalProvider db)
        {
            if (!db.Exists<UserRole>("UserID=@UserID and RoleID=@RoleID", db.CreateParameter("UserID", userid), db.CreateParameter("RoleID", roleid)))
            {
                UserRole ur = new UserRole();
                ur.RoleID = roleid;
                ur.UserID = userid;
                ur.TenantID = tenantid;
                ur.Creator = creator;
                db.Insert(ur);
            }
        }
        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        /// <param name="tenantid"></param>
        /// <param name="creator"></param>
        /// <param name="db"></param>
        public static void RemoveRole(Guid userid, Guid roleid, Guid? tenantid, Zippy.Data.IDalProvider db)
        {
            db.Delete<UserRole>("UserID=@UserID and RoleID=@RoleID and TenantID=@TenantID",
                db.CreateParameter("UserID", userid),
                db.CreateParameter("RoleID", roleid),
                db.CreateParameter("TenantID", tenantid));

        }
    }

}
