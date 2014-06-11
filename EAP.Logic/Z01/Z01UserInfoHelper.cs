using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

    public class Z01UserInfoHelper
    {	
    
		#region 本实体的外键实体集合
		#endregion
		
		#region 本实体外键对应的实体
        /// <summary>
        /// 表示 [头衔/职务] 对应的实体
        /// </summary>
        public static Z01Title GetTitleIDEntity(Zippy.Data.IDalProvider db, Z01UserInfo entity)
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
		
        public static Z01UserInfo Create(Zippy.Data.IDalProvider db, Guid _UserID)
        {
			Z01UserInfo rtn =  db.FindUnique<Z01UserInfo>(_UserID);
            return rtn;
        }
        

        public static int Delete(Zippy.Data.IDalProvider db, Guid _UserID)
        {
            return db.Delete<Z01UserInfo>(_UserID);
        }
		
        public static int Insert(Zippy.Data.IDalProvider db, Z01UserInfo entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }
		
        public static int Update(Zippy.Data.IDalProvider db, Z01UserInfo entity)
        {
            return db.Update(entity);
        }
		
		
        public static List<Z01UserInfo> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01UserInfo>(true);
        }
		
        public static List<Z01UserInfo> Take(Zippy.Data.IDalProvider db,int count)
        {
            return db.Take<Z01UserInfo>(count, true);
        }
		
        public static List<Z01UserInfo> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01UserInfo>(sqlEntry, cmdParameters);
        }
		
        /// <summary>
        /// 用户信息 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01UserInfo> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01UserInfo> rtn = new PaginatedList<Z01UserInfo>();
            List<Z01UserInfo> records = db.Take<Z01UserInfo>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01UserInfo>(where, cmdParameters);
            return rtn;
        }
		
        public static PaginatedList<Z01UserInfo> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01UserInfo> rtn = new PaginatedList<Z01UserInfo>();
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
				object qEmail = paras["qEmail"];
				if (qEmail.IsNotNullOrEmpty())
				{
					where += " and [Email] like @Email";
					dbParams.Add(db.CreateParameter("Email", "%" + qEmail + "%"));
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
                orderBy = "[UserID]";
            }
            else if (orderCol == 2)
            {
                orderBy = "[UserID] desc";
            }
            else if (orderCol == 3)
            {
                orderBy = "[Name]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[Name] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[Nickname]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[Nickname] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[Email]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[Email] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[Tel1]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[Tel1] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[Tel2]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[Tel2] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[Avatar]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[Avatar] desc";
            }
            else if (orderCol == 15)
            {
                orderBy = "[TitleID]";
            }
            else if (orderCol == 16)
            {
                orderBy = "[TitleID] desc";
            }
            else if (orderCol == 17)
            {
                orderBy = "[TenantID]";
            }
            else if (orderCol == 18)
            {
                orderBy = "[TenantID] desc";
            }
            else if (orderCol == 19)
            {
                orderBy = "[UserStatus]";
            }
            else if (orderCol == 20)
            {
                orderBy = "[UserStatus] desc";
            }
            else if (orderCol == 21)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 22)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01UserInfo>(where, dbParams.ToArray());
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


            List<Z01UserInfo> records = db.Take<Z01UserInfo>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }
	
}
