using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01PaperTemplateHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
        public static Z01PaperTemplate Create(Zippy.Data.IDalProvider db, Int64 _TemplateID)
        {
			Z01PaperTemplate rtn =  db.FindUnique<Z01PaperTemplate>(_TemplateID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Int64 _TemplateID)
        {
            return db.Delete<Z01PaperTemplate>(_TemplateID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01PaperTemplate entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01PaperTemplate entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01PaperTemplate> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01PaperTemplate>(true);
        }
		
        public static List<Z01PaperTemplate> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01PaperTemplate>(count, true);
        }
		
        public static List<Z01PaperTemplate> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01PaperTemplate>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 单据条款 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01PaperTemplate> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01PaperTemplate> rtn = new PaginatedList<Z01PaperTemplate>();
            List<Z01PaperTemplate> records = db.Take<Z01PaperTemplate>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01PaperTemplate>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01PaperTemplate> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01PaperTemplate> rtn = new PaginatedList<Z01PaperTemplate>();
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
			
			string orderBy = "[DisplayOrder]";
            if (orderCol == 0)
            {
                orderBy = "[DisplayOrder] desc";
            }
            else if (orderCol == 1)
            {
                orderBy = "[TemplateID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[TemplateID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Title]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Title] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01PaperTemplate>(where, dbParams.ToArray());
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


            List<Z01PaperTemplate> records = db.Take<Z01PaperTemplate>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
