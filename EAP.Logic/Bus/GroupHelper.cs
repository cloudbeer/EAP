using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;
using System.Linq;

namespace EAP.Bus.Entity.Helper
{

    public static class GroupHelper
    {

        #region 本实体的外键实体集合
        /// <summary>
        /// 获取 [组 的 父分类] 的 [组] 集合
        /// </summary>
        public static List<Group> GetParentID_Groups(Zippy.Data.IDalProvider db, Group entity)
        {
            if (entity.GroupID.HasValue)
                return db.Take<Group>("ParentID=@ParentID", db.CreateParameter("ParentID", entity.GroupID));
            return new List<Group>();

        }
        /// <summary>
        /// 获取 [组 的 分组] 的 [用户] 集合
        /// </summary>
        public static List<User> GetGroupID_Users(Zippy.Data.IDalProvider db, Group entity)
        {
            if (entity.GroupID.HasValue)
                return db.Take<User>("GroupID=@GroupID", db.CreateParameter("GroupID", entity.GroupID));
            return new List<User>();

        }
        /// <summary>
        /// 获取 [组 的 组] 的 [用户分组] 集合
        /// </summary>
        public static List<UserGroup> GetGroupID_UserGroups(Zippy.Data.IDalProvider db, Group entity)
        {
            if (entity.GroupID.HasValue)
                return db.Take<UserGroup>("GroupID=@GroupID", db.CreateParameter("GroupID", entity.GroupID));
            return new List<UserGroup>();

        }
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [父分类] 对应的实体
        /// </summary>
        public static Group GetParentIDEntity(Zippy.Data.IDalProvider db, Group entity)
        {
            return db.FindUnique<Group>("GroupID=@GroupID", db.CreateParameter("GroupID", entity.ParentID));
        }
        public static Group GetParentIDEntity(this Group entity)
        {
            Zippy.Data.IDalProvider db = Zippy.Data.DalFactory.CreateProvider();
            return db.FindUnique<Group>("GroupID=@GroupID", db.CreateParameter("GroupID", entity.ParentID));
        }
        /// <summary>
        /// 表示 [父分类] 被选实体集合
        /// </summary>
        public static List<Group> GetParentIDEntities(Zippy.Data.IDalProvider db, Guid tenantID)
        {
            return db.Take<Group>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
        }

        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tenantID"></param>
        /// <param name="selectedValue"></param>
        /// <param name="removalID">必须移除的ID</param>
        /// <returns></returns>
        public static string GetParentIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, Guid? selectedValue, Guid? removalID)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Group> objs = db.Take<Group>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            List<Group> newObjs = new List<Group>();
            ReGroup(objs, newObjs, Guid.Empty, "├", removalID);
            foreach (var obj in newObjs)
            {
                if (selectedValue == obj.GroupID)
                    sb.AppendLine("<option value='" + obj.GroupID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.GroupID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        #endregion

        public static Group Create(Zippy.Data.IDalProvider db, Guid _GroupID)
        {
            Group rtn = db.FindUnique<Group>(_GroupID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Guid _GroupID)
        {
            return db.Delete<Group>(_GroupID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Group entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Group entity)
        {
            return db.Update(entity);
        }


        public static List<Group> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Group>(true);
        }

        public static List<Group> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Group>(count, true);
        }

        public static List<Group> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Group>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 组 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Group> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Group> rtn = new PaginatedList<Group>();
            List<Group> records = db.Take<Group>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Group>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<Group> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PageSize = 5000;
            PaginatedList<Group> rtn = new PaginatedList<Group>();
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
                object qParentID = paras["qParentID"];
                if (qParentID.IsNotNullOrEmpty())
                {
                    where += " and [ParentID] = @ParentID";
                    dbParams.Add(db.CreateParameter("ParentID", qParentID));
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
                orderBy = "[Code]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Code] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[ParentID]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[ParentID] desc";
            }
            int RecordCount = db.Count<Group>(where, dbParams.ToArray());
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


            List<Group> records = db.Take<Group>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            //递归重组
            List<Group> newRecords = new List<Group>();
            ReGroup(records, newRecords, Guid.Empty, "├", null);


            rtn.AddRange(newRecords);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }

        private static void ReGroup(List<Group> records, List<Group> newRecords, Guid? parentID, string prefix, Guid? removeID)
        {
            prefix += "─";

            IEnumerable<Group> thisGroups = records.Where(s => s.ParentID == parentID && s.GroupID != removeID);

            foreach (Group tGroup in thisGroups)
            {
                if (tGroup.ParentID != Guid.Empty && prefix != "├")
                    tGroup.Title = prefix + " " + tGroup.Title;
                newRecords.Add(tGroup);
                ReGroup(records, newRecords, tGroup.GroupID, prefix, removeID);
            }

        }
    }

}
