using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01CustomerPersonHelper
    {

        #region 本实体的外键实体集合
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [客户] 对应的实体
        /// </summary>
        public static Z01Customer GetCustomerIDEntity(Zippy.Data.IDalProvider db, Z01CustomerPerson entity)
        {
            return db.FindUnique<Z01Customer>("CustomerID=@CustomerID", db.CreateParameter("CustomerID", entity.CustomerID));
        }
        /// <summary>
        /// 表示 [客户] 被选实体集合
        /// </summary>
        public static List<Z01Customer> GetCustomerIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Customer>();
        }

        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetCustomerIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01Customer> objs = db.Take<Z01Customer>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.CustomerID)
                    sb.AppendLine("<option value='" + obj.CustomerID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.CustomerID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 表示 [头衔/职务] 对应的实体
        /// </summary>
        public static Z01Title GetTitleIDEntity(Zippy.Data.IDalProvider db, Z01CustomerPerson entity)
        {
            return db.FindUnique<Z01Title>("TitleID=@TitleID", db.CreateParameter("TitleID", entity.TitleID));
        }
        /// <summary>
        /// 表示 [头衔/职务] 被选实体集合
        /// </summary>
        public static List<Z01Title> GetTitleIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Title>();
        }

        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetTitleIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01Title> objs = db.Take<Z01Title>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.TitleID)
                    sb.AppendLine("<option value='" + obj.TitleID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.TitleID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        #endregion

        public static Z01CustomerPerson Create(Zippy.Data.IDalProvider db, Int64 _PersonID)
        {
            Z01CustomerPerson rtn =  db.FindUnique<Z01CustomerPerson>(_PersonID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _PersonID)
        {
            return db.Delete<Z01CustomerPerson>(_PersonID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Z01CustomerPerson entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Z01CustomerPerson entity)
        {
            return db.Update(entity);
        }


        public static List<Z01CustomerPerson> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01CustomerPerson>(true);
        }

        public static List<Z01CustomerPerson> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Z01CustomerPerson>(count, true);
        }

        public static List<Z01CustomerPerson> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01CustomerPerson>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 客户联系人 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01CustomerPerson> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01CustomerPerson> rtn = new PaginatedList<Z01CustomerPerson>();
            List<Z01CustomerPerson> records = db.Take<Z01CustomerPerson>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01CustomerPerson>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<Z01CustomerPerson> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01CustomerPerson> rtn = new PaginatedList<Z01CustomerPerson>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", tenantID));

            #region 开始查询
            if (paras != null)
            {
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
                object qCustomerID = paras["qCustomerID"];
                if (qCustomerID.IsNotNullOrEmpty())
                {
                    where += " and [CustomerID] = @CustomerID";
                    dbParams.Add(db.CreateParameter("CustomerID", qCustomerID));
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
                orderBy = "[Name]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[Name] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Nickname]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Nickname] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[Email]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[Email] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[Tel1]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[Tel1] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[Tel2]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[Tel2] desc";
            }
            int RecordCount = db.Count<Z01CustomerPerson>(where, dbParams.ToArray());
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


            List<Z01CustomerPerson> records = db.Take<Z01CustomerPerson>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }

}
