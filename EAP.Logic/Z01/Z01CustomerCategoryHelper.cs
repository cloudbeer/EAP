using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;
using System.Linq;

namespace Z01Beetle.Entity.Helper
{

    public static class Z01CustomerCategoryHelper
    {

        #region 本实体的外键实体集合
        /// <summary>
        /// 获取 [客户分类 的 客户分类] 的 [客户] 集合
        /// </summary>
        public static List<Z01Customer> GetCategoryID_Z01Customers(Zippy.Data.IDalProvider db, Z01CustomerCategory entity)
        {
            if (entity.CategoryID.HasValue)
                return db.Take<Z01Customer>("CategoryID=@CategoryID", db.CreateParameter("CategoryID", entity.CategoryID));
            return new List<Z01Customer>();

        }
        /// <summary>
        /// 获取 [客户分类 的 分类] 的 [客户分类关系] 集合
        /// </summary>
        public static List<Z01CustomerInCategory> GetCategoryID_Z01CustomerInCategorys(Zippy.Data.IDalProvider db, Z01CustomerCategory entity)
        {
            if (entity.CategoryID.HasValue)
                return db.Take<Z01CustomerInCategory>("CategoryID=@CategoryID", db.CreateParameter("CategoryID", entity.CategoryID));
            return new List<Z01CustomerInCategory>();

        }
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetParentIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, long? selectedValue, long removeID)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01CustomerCategory> objs = db.Take<Z01CustomerCategory>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            List<Z01CustomerCategory> newObjs = new List<Z01CustomerCategory>();
            ReGroup(objs, newObjs, 0, "├", removeID);
            foreach (var obj in newObjs)
            {
                if (selectedValue == obj.CategoryID)
                    sb.AppendLine("<option value='" + obj.CategoryID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.CategoryID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        #endregion

        public static Z01CustomerCategory GetParentIDEntity(this Z01CustomerCategory entity)
        {
            Zippy.Data.IDalProvider db = Zippy.Data.DalFactory.CreateProvider();
            return db.FindUnique<Z01CustomerCategory>("CategoryID=@CategoryID", db.CreateParameter("CategoryID", entity.ParentID));
        }

        public static Z01CustomerCategory Create(Zippy.Data.IDalProvider db, Int64 _CategoryID)
        {
            Z01CustomerCategory rtn = db.FindUnique<Z01CustomerCategory>(_CategoryID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _CategoryID)
        {
            return db.Delete<Z01CustomerCategory>(_CategoryID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Z01CustomerCategory entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Z01CustomerCategory entity)
        {
            return db.Update(entity);
        }


        public static List<Z01CustomerCategory> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01CustomerCategory>(true);
        }

        public static List<Z01CustomerCategory> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Z01CustomerCategory>(count, true);
        }

        public static List<Z01CustomerCategory> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01CustomerCategory>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 客户分类 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01CustomerCategory> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01CustomerCategory> rtn = new PaginatedList<Z01CustomerCategory>();
            List<Z01CustomerCategory> records = db.Take<Z01CustomerCategory>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01CustomerCategory>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<Z01CustomerCategory> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01CustomerCategory> rtn = new PaginatedList<Z01CustomerCategory>();
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
            int RecordCount = db.Count<Z01CustomerCategory>(where, dbParams.ToArray());
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


            List<Z01CustomerCategory> records = db.Take<Z01CustomerCategory>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            //递归重组
            List<Z01CustomerCategory> newRecords = new List<Z01CustomerCategory>();
            ReGroup(records, newRecords, 0, "├", 0);
            rtn.AddRange(newRecords);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }

        private static void ReGroup(List<Z01CustomerCategory> records, List<Z01CustomerCategory> newRecords, long? parentID, string prefix, long removeID)
        {
            prefix += "─";
            IEnumerable<Z01CustomerCategory> thisGroups = records.Where(s => s.ParentID == parentID && s.CategoryID != removeID);
            foreach (Z01CustomerCategory tGroup in thisGroups)
            {
                if (tGroup.ParentID != 0 && prefix != "├")
                    tGroup.Title = prefix + " " + tGroup.Title;
                newRecords.Add(tGroup);
                ReGroup(records, newRecords, tGroup.CategoryID, prefix,removeID);
            }

        }
    }

}
