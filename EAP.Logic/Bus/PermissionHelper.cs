using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;
using System.Linq;

namespace EAP.Bus.Entity.Helper
{

    public class PermissionHelper
    {

        #region 本实体的外键实体集合
        /// <summary>
        /// 获取 [权限表 的 父权限] 的 [权限表] 集合
        /// </summary>
        public static List<Permission> GetParentID_Permissions(Zippy.Data.IDalProvider db, Permission entity)
        {
            if (entity.PermissionID.HasValue)
                return db.Take<Permission>("ParentID=@ParentID", db.CreateParameter("ParentID", entity.PermissionID));
            return new List<Permission>();

        }
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [父权限] 对应的实体
        /// </summary>
        public static Permission GetParentIDEntity(Zippy.Data.IDalProvider db, Permission entity)
        {
            return db.FindUnique<Permission>("PermissionID=@PermissionID", db.CreateParameter("PermissionID", entity.ParentID));
        }
        /// <summary>
        /// 表示 [父权限] 被选实体集合
        /// </summary>
        public static List<Permission> GetParentIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Permission>();
        }

        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetParentIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Permission> objs = db.Take<Permission>("ParentID=0");
            foreach (var obj in objs)
            {
                if (selectedValue == obj.PermissionID)
                    sb.AppendLine("<option value='" + obj.PermissionID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.PermissionID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        #endregion

        public static Permission Create(Zippy.Data.IDalProvider db, Int64 _PermissionID)
        {
            Permission rtn = db.FindUnique<Permission>(_PermissionID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _PermissionID)
        {
            return db.Delete<Permission>(_PermissionID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Permission entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Permission entity)
        {
            return db.Update(entity);
        }


        public static List<Permission> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Permission>(true);
        }

        public static List<Permission> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Permission>(count, true);
        }

        public static List<Permission> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Permission>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 权限表 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Permission> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Permission> rtn = new PaginatedList<Permission>();
            List<Permission> records = db.Take<Permission>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Permission>(where, cmdParameters);
            return rtn;
        }

        public static List<Permission> Query(Zippy.Data.IDalProvider db, long parentID)
        {
            return db.Take<Permission>(" [ParentID] = @ParentID order by DisplayOrder", db.CreateParameter("ParentID", parentID));
        }

        public static PaginatedList<Permission> Query(Zippy.Data.IDalProvider db, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Permission> rtn = new PaginatedList<Permission>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " 1=1";

            #region 开始查询
            if (paras != null)
            {
                object qTitle = paras["qTitle"];
                if (qTitle.IsNotNullOrEmpty())
                {
                    where += " and [Title] like @Title";
                    dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
                }
                object qUrl = paras["qUrl"];
                if (qUrl.IsNotNullOrEmpty())
                {
                    where += " and [Url] like @Url";
                    dbParams.Add(db.CreateParameter("Url", "%" + qUrl + "%"));
                }
                object qFlag = paras["qFlag"];
                if (qFlag.IsNotNullOrEmpty())
                {
                    where += " and [Flag] like @Flag";
                    dbParams.Add(db.CreateParameter("Flag", "%" + qFlag + "%"));
                }
                object qParentID = paras["qParentID"];
                if (qParentID.IsNotNullOrEmpty())
                {
                    where += " and [ParentID] = @ParentID";
                    dbParams.Add(db.CreateParameter("ParentID", qParentID));
                }
            }
            #endregion

            string orderBy = " [DisplayOrder] asc";

            int RecordCount = db.Count<Permission>(where, dbParams.ToArray());
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


            List<Permission> records = db.Take<Permission>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            //重组 records
            List<Permission> newRecords = new List<Permission>();
            IEnumerable<Permission> rootRecords = records.Where(s => (s.ParentID ?? 0) == 0).OrderBy(s => s.DisplayOrder);
            foreach (Permission per in rootRecords)
            {
                newRecords.Add(per);
                newRecords.AddRange(records.Where(s => s.ParentID == per.PermissionID).OrderBy(s => s.DisplayOrder));
            }


            rtn.AddRange(newRecords);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }

}
