using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01TitleHelper
    {

        #region 本实体的外键实体集合
        /// <summary>
        /// 获取 [头衔/职务 的 头衔/职务] 的 [用户信息] 集合
        /// </summary>
        public static List<Z01UserInfo> GetTitleID_Z01UserInfos(Zippy.Data.IDalProvider db, Z01Title entity)
        {
            if (entity.TitleID.HasValue)
                return db.Take<Z01UserInfo>("TitleID=@TitleID", db.CreateParameter("TitleID", entity.TitleID));
            return new List<Z01UserInfo>();

        }
        /// <summary>
        /// 获取 [头衔/职务 的 头衔/职务] 的 [客户联系人] 集合
        /// </summary>
        public static List<Z01CustomerPerson> GetTitleID_Z01CustomerPersons(Zippy.Data.IDalProvider db, Z01Title entity)
        {
            if (entity.TitleID.HasValue)
                return db.Take<Z01CustomerPerson>("TitleID=@TitleID", db.CreateParameter("TitleID", entity.TitleID));
            return new List<Z01CustomerPerson>();

        }
        #endregion

        #region 本实体外键对应的实体
        #endregion

        public static Z01Title Create(Zippy.Data.IDalProvider db, Int64 _TitleID)
        {
            Z01Title rtn =  db.FindUnique<Z01Title>(_TitleID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _TitleID)
        {
            return db.Delete<Z01Title>(_TitleID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Z01Title entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Z01Title entity)
        {
            return db.Update(entity);
        }


        public static List<Z01Title> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Title>(true);
        }

        public static List<Z01Title> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Z01Title>(count, true);
        }

        public static List<Z01Title> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01Title>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 头衔/职务 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01Title> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01Title> rtn = new PaginatedList<Z01Title>();
            List<Z01Title> records = db.Take<Z01Title>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01Title>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<Z01Title> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01Title> rtn = new PaginatedList<Z01Title>();
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
                object qContent = paras["qContent"];
                if (qContent.IsNotNullOrEmpty())
                {
                    where += " and [Content] like @Content";
                    dbParams.Add(db.CreateParameter("Content", "%" + qContent + "%"));
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

            string orderBy = "[DisplayOrder]";
            if (orderCol == 0)
            {
                orderBy = "[DisplayOrder] desc";
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
            int RecordCount = db.Count<Z01Title>(where, dbParams.ToArray());
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


            List<Z01Title> records = db.Take<Z01Title>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }

}
