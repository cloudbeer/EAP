using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace Z01Beetle.Entity.Helper
{

	public class Z01CustomerHelper
	{	
	
		#region 本实体的外键实体集合
		/// <summary>
		/// 获取 [客户 的 生产商] 的 [品牌] 集合
		/// </summary>
		public static List<Z01Brand> GetProducer_Z01Brands(Zippy.Data.IDalProvider db, Z01Customer entity)
		{
				if (entity.CustomerID.HasValue)
				   return db.Take<Z01Brand>("Producer=@Producer", db.CreateParameter("Producer", entity.CustomerID));            
			return new List<Z01Brand>();
			
		}
		/// <summary>
		/// 获取 [客户 的 客户] 的 [客户联系人] 集合
		/// </summary>
		public static List<Z01CustomerPerson> GetCustomerID_Z01CustomerPersons(Zippy.Data.IDalProvider db, Z01Customer entity)
		{
				if (entity.CustomerID.HasValue)
				   return db.Take<Z01CustomerPerson>("CustomerID=@CustomerID", db.CreateParameter("CustomerID", entity.CustomerID));            
			return new List<Z01CustomerPerson>();
			
		}
		#endregion
		
		#region 本实体外键对应的实体
		/// <summary>
		/// 表示 [客户分类] 对应的实体
		/// </summary>
		public static Z01CustomerCategory GetCategoryIDEntity(Zippy.Data.IDalProvider db, Z01Customer entity)
		{
			return db.FindUnique<Z01CustomerCategory>("CategoryID=@CategoryID", db.CreateParameter("CategoryID", entity.CategoryID));
		}
		/// <summary>
		/// 表示 [客户分类] 被选实体集合
		/// </summary>
		public static List<Z01CustomerCategory> GetCategoryIDEntities(Zippy.Data.IDalProvider db)
		{
			return db.Take<Z01CustomerCategory>();
		}
		
		/// <summary>
		/// 表示 [父分类] 被选实体集合的 option html
		/// </summary>
		public static string GetCategoryIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			List<Z01CustomerCategory> objs = db.Take<Z01CustomerCategory>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
			foreach (var obj in objs)
			{
				if (selectedValue == obj.CategoryID)
					sb.AppendLine("<option value='" + obj.CategoryID + "' selected='selected'>" + obj.Title + "</option>");
				else
					sb.AppendLine("<option value='" + obj.CategoryID + "'>" + obj.Title + "</option>");
			}
			return sb.ToString();
		}

		#endregion
		
		public static Z01Customer Create(Zippy.Data.IDalProvider db, Int64 _CustomerID)
		{
			Z01Customer rtn =  db.FindUnique<Z01Customer>(_CustomerID);
			return rtn;
		}
		

		public static int Delete(Zippy.Data.IDalProvider db, Int64 _CustomerID)
		{
			return db.Delete<Z01Customer>(_CustomerID);
		}
		
		public static int Insert(Zippy.Data.IDalProvider db, Z01Customer entity)
		{
			int rtn = db.Insert(entity);
			return rtn;
		}
		
		public static int Update(Zippy.Data.IDalProvider db, Z01Customer entity)
		{
			return db.Update(entity);
		}
		
		
		public static List<Z01Customer> Take(Zippy.Data.IDalProvider db)
		{
			return db.Take<Z01Customer>(true);
		}
		
		public static List<Z01Customer> Take(Zippy.Data.IDalProvider db,int count)
		{
			return db.Take<Z01Customer>(count, true);
		}
		
		public static List<Z01Customer> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
		{
			return db.Take<Z01Customer>(sqlEntry, cmdParameters);
		}
		
		/// <summary>
		/// 客户 查询
		/// </summary>
		/// <param name="where">查询条件</param>
		/// <param name="orderby">排序方式</param>
		/// <param name="PageSize">每页数量</param>
		/// <param name="PageIndex">页码</param>
		/// <param name="cmdParameters">查询条件赋值</param>
		/// <returns></returns>
		public static PaginatedList<Z01Customer> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
		{
			PaginatedList<Z01Customer> rtn = new PaginatedList<Z01Customer>();
			List<Z01Customer> records = db.Take<Z01Customer>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = db.Count<Z01Customer>(where, cmdParameters);
			return rtn;
		}
		
		public static PaginatedList<Z01Customer> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
		{
			PaginatedList<Z01Customer> rtn = new PaginatedList<Z01Customer>();
			List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();
			
			string where = " [TenantID]=@TenantID";
			dbParams.Add(db.CreateParameter("TenantID", tenantID));
			
			#region 开始查询
			if (paras != null)
			{
				object qTitle = paras["qTitle"];
				if (qTitle.IsNotNullOrEmpty())
				{
                    where += " and ( [Title] like @Title or [Tel1] like @Title or  [Tel2] like @Title)";
					dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
				}
				object qEmail = paras["qEmail"];
				if (qEmail.IsNotNullOrEmpty())
				{
					where += " and [Email] like @Email";
					dbParams.Add(db.CreateParameter("Email", "%" + qEmail + "%"));
				}
                object qCustomerType = paras["qCustomerType"];
                if (qCustomerType.IsNotNullOrEmpty())
                {
                    where += " and ([CustomerType] & @CustomerType) = @CustomerType";
                    dbParams.Add(db.CreateParameter("CustomerType", qCustomerType));
                }
                object qCateID=paras["qCateID"];
                if (qCateID.IsNotNullOrEmpty())
                {
                    where += " and [CustomerID] in (select [CustomerID] from Z01CustomerInCategory where [CategoryID]=@CategoryID)";
                    dbParams.Add(db.CreateParameter("CategoryID", qCateID));
                }
                object qPrincipal = paras["qPrincipal"];
                if (qPrincipal.IsNotNullOrEmpty())
                {
                    where += " and [Principal]=@Principal";
                    dbParams.Add(db.CreateParameter("Principal", qPrincipal));
                }
                object qSiteStatus = paras["qSiteStatus"];
                if (qSiteStatus.IsNotNullOrEmpty())
                {
                    where += " and [CustomerStatus]=@CustomerStatus";
                    dbParams.Add(db.CreateParameter("CustomerStatus", qSiteStatus));
                }
                object qSuccessRatio = paras["qSuccessRatio"];
                if (qSuccessRatio.IsNotNullOrEmpty())
                {
                    where += " and [CustomerID] in (select CustomerID from Z30Communication where SuccessRatio>=@SuccessRatio)";
                    dbParams.Add(db.CreateParameter("SuccessRatio", qSuccessRatio));
                }
			}
			#endregion

            string orderBy = "[ManageHot] desc, CustomerID desc";
			if (orderCol == 0)
			{
                orderBy = "[ManageHot] desc, CustomerID desc";
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
				orderBy = "[Tel1]";
			}
			else if (orderCol == 4)
			{
				orderBy = "[Tel1] desc";
			}
			else if (orderCol == 5)
			{
				orderBy = "[Tel2]";
			}
			else if (orderCol == 6)
			{
				orderBy = "[Tel2] desc";
			}
			else if (orderCol == 7)
			{
				orderBy = "[Email]";
			}
			else if (orderCol == 8)
			{
				orderBy = "[Email] desc";
			}
			else if (orderCol == 11)
			{
				orderBy = "[CreateDate]";
			}
			else if (orderCol == 12)
			{
				orderBy = "[CreateDate] desc";
			}
			int RecordCount = db.Count<Z01Customer>(where, dbParams.ToArray());
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


			List<Z01Customer> records = db.Take<Z01Customer>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = RecordCount;

			return rtn;

		}
	}
	
}
