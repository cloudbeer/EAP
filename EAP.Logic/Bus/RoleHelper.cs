using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

    public class RoleHelper
    {

        #region 本实体的外键实体集合
        /// <summary>
        /// 获取 [角色 的 角色] 的 [用户角色] 集合
        /// </summary>
        public static List<UserRole> GetRoleID_UserRoles(Zippy.Data.IDalProvider db, Role entity)
        {
            if (entity.RoleID.HasValue)
                return db.Take<UserRole>("RoleID=@RoleID", db.CreateParameter("RoleID", entity.RoleID));
            return new List<UserRole>();

        }
        #endregion

        #region 本实体外键对应的实体
        #endregion

        public static Role Create(Zippy.Data.IDalProvider db, Guid _RoleID)
        {
            Role rtn = db.FindUnique<Role>(_RoleID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Guid _RoleID)
        {
            return db.Delete<Role>(_RoleID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Role entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Role entity)
        {
            return db.Update(entity);
        }


        public static List<Role> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Role>(true);
        }

        public static List<Role> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Role>(count, true);
        }

        public static List<Role> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Role>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 角色 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Role> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Role> rtn = new PaginatedList<Role>();
            List<Role> records = db.Take<Role>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Role>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<Role> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Role> rtn = new PaginatedList<Role>();
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
                object qCreateDateStart = paras["qCreateDateStart"];
                if (qCreateDateStart != null && qCreateDateStart.ToString() != "")
                {
                    where += " and [CreateDate] >= @CreateDateStart";
                    dbParams.Add(db.CreateParameter("CreateDateStart", qCreateDateStart));
                }
                object qCreateDateEnd = paras["qCreateDateEnd"];
                if (qCreateDateEnd != null && qCreateDateEnd.ToString() != "")
                {
                    where += " and [CreateDate] < @CreateDateEnd";
                    dbParams.Add(db.CreateParameter("CreateDateEnd", ((DateTime)qCreateDateEnd).AddDays(1)));
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
            int RecordCount = db.Count<Role>(where, dbParams.ToArray());
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


            List<Role> records = db.Take<Role>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
        /// <summary>
        /// 增加一种权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="permissionID"></param>
        /// <param name="permissionType"></param>
        /// <param name="tenantID"></param>
        /// <param name="optUser"></param>
        /// <param name="db"></param>
        public static void SetPermission(Guid roleID, long permissionID, int permissionType, Guid? tenantID, Guid? optUser, Zippy.Data.IDalProvider db)
        {
            RolePermission rp = db.FindUnique<RolePermission>("RoleID=@RoleID and PermissionID=@PermissionID and TenantID=@TenantID",
                db.CreateParameter("RoleID", roleID), db.CreateParameter("PermissionID", permissionID), db.CreateParameter("TenantID", tenantID));
            if (rp == null)
            {
                rp = new RolePermission();
                rp.RoleID = roleID;
                rp.PermissionID = permissionID;
                rp.PermissionType = permissionType;
                rp.TenantID = tenantID;
                rp.Creator = optUser;
                db.Insert(rp);
            }
            else
            {
                rp.PermissionType = rp.PermissionType | permissionType;
                rp.Updater = optUser;
                rp.UpdateDate = DateTime.Now;
                db.Update(rp);
            }

        }
        /// <summary>
        /// 删除一种权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="permissionID"></param>
        /// <param name="permissionType"></param>
        /// <param name="tenantID"></param>
        /// <param name="optUser"></param>
        /// <param name="db"></param>
        public static void RemovePermission(Guid roleID, long permissionID, int permissionType, Guid? tenantID, Guid? optUser, Zippy.Data.IDalProvider db)
        {
            RolePermission rp = db.FindUnique<RolePermission>("RoleID=@RoleID and PermissionID=@PermissionID and TenantID=@TenantID",
                db.CreateParameter("RoleID", roleID), db.CreateParameter("PermissionID", permissionID), db.CreateParameter("TenantID", tenantID));
            if (rp != null)
            {
                rp.PermissionType = rp.PermissionType & (~permissionType);
                rp.Updater = optUser;
                rp.UpdateDate = DateTime.Now;
                db.Update(rp);
            }

        }
        /// <summary>
        /// 获取某个角色的所有权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="tenantID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<RolePermission> GetPermissions(Guid roleID, Guid? tenantID, Zippy.Data.IDalProvider db)
        {
            List<RolePermission> rn = db.Take<RolePermission>("RoleID=@RoleID and TenantID=@TenantID order by DisplayOrder",
                db.CreateParameter("RoleID", roleID), db.CreateParameter("TenantID", tenantID));
            return rn;
        }

    }

}
