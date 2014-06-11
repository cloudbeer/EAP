using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;

namespace EAP.Bus.Entity.Helper
{

	public class ApplicationHelper
	{	
	
		#region 本实体的外键实体集合
		/// <summary>
		/// 获取 [应用 的 应用程序] 的 [租户的应用] 集合
		/// </summary>
		public static List<TenantApplication> GetApplicationID_TenantApplications(Zippy.Data.IDalProvider db, Application entity)
		{
				if (entity.ApplicationID.HasValue)
				   return db.Take<TenantApplication>("ApplicationID=@ApplicationID", db.CreateParameter("ApplicationID", entity.ApplicationID));            
			return new List<TenantApplication>();
			
		}
		#endregion
		
		#region 本实体外键对应的实体
		#endregion
		
		public static Application Create(Zippy.Data.IDalProvider db, Guid _ApplicationID)
		{
			Application rtn =  db.FindUnique<Application>(_ApplicationID);
			return rtn;
		}
		

		public static int Delete(Zippy.Data.IDalProvider db, Guid _ApplicationID)
		{
			return db.Delete<Application>(_ApplicationID);
		}
		
		public static int Insert(Zippy.Data.IDalProvider db, Application entity)
		{
			int rtn = db.Insert(entity);
			return rtn;
		}
		
		public static int Update(Zippy.Data.IDalProvider db, Application entity)
		{
			return db.Update(entity);
		}
		
		
		public static List<Application> Take(Zippy.Data.IDalProvider db)
		{
			return db.Take<Application>(true);
		}
		
		public static List<Application> Take(Zippy.Data.IDalProvider db,int count)
		{
			return db.Take<Application>(count, true);
		}
		
		public static List<Application> Take(Zippy.Data.IDalProvider db,string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
		{
			return db.Take<Application>(sqlEntry, cmdParameters);
		}
		
		/// <summary>
		/// 应用 查询
		/// </summary>
		/// <param name="where">查询条件</param>
		/// <param name="orderby">排序方式</param>
		/// <param name="PageSize">每页数量</param>
		/// <param name="PageIndex">页码</param>
		/// <param name="cmdParameters">查询条件赋值</param>
		/// <returns></returns>
		public static PaginatedList<Application> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
		{
			PaginatedList<Application> rtn = new PaginatedList<Application>();
			List<Application> records = db.Take<Application>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = db.Count<Application>(where, cmdParameters);
			return rtn;
		}
		
		public static PaginatedList<Application> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
		{
			PaginatedList<Application> rtn = new PaginatedList<Application>();
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
				object qApplicationType = paras["qApplicationType"];
				if (qApplicationType != null && qApplicationType.ToString()!="")
				{
					Int32 intqApplicationType = (Int32)qApplicationType;
					if (intqApplicationType > 0)
					{
						where += " and ([ApplicationType] & @ApplicationType = @ApplicationType)";
						dbParams.Add(db.CreateParameter("ApplicationType", qApplicationType));
					}
				}
				object qApplicationStatus = paras["qApplicationStatus"];
				if (qApplicationStatus != null && qApplicationStatus.ToString()!="" )
				{
					Int32 intqApplicationStatus = (Int32)qApplicationStatus;
					if (intqApplicationStatus > 0)
					{
						where += " and [ApplicationStatus] = @ApplicationStatus";
						dbParams.Add(db.CreateParameter("ApplicationStatus", qApplicationStatus));
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
				orderBy = "[ApplicationID]";
			}
			else if (orderCol == 2)
			{
				orderBy = "[ApplicationID] desc";
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
				orderBy = "[ClientApiStartServeice]";
			}
			else if (orderCol == 6)
			{
				orderBy = "[ClientApiStartServeice] desc";
			}
			else if (orderCol == 7)
			{
				orderBy = "[ClientApiStopService]";
			}
			else if (orderCol == 8)
			{
				orderBy = "[ClientApiStopService] desc";
			}
			else if (orderCol == 9)
			{
				orderBy = "[ClientApiDeleteService]";
			}
			else if (orderCol == 10)
			{
				orderBy = "[ClientApiDeleteService] desc";
			}
			else if (orderCol == 11)
			{
				orderBy = "[ClientApiPauseService]";
			}
			else if (orderCol == 12)
			{
				orderBy = "[ClientApiPauseService] desc";
			}
			else if (orderCol == 13)
			{
				orderBy = "[ClientApiGetMenu]";
			}
			else if (orderCol == 14)
			{
				orderBy = "[ClientApiGetMenu] desc";
			}
			else if (orderCol == 15)
			{
				orderBy = "[ApplicationType]";
			}
			else if (orderCol == 16)
			{
				orderBy = "[ApplicationType] desc";
			}
			else if (orderCol == 17)
			{
				orderBy = "[ApplicationStatus]";
			}
			else if (orderCol == 18)
			{
				orderBy = "[ApplicationStatus] desc";
			}
			else if (orderCol == 19)
			{
				orderBy = "[CreateDate]";
			}
			else if (orderCol == 20)
			{
				orderBy = "[CreateDate] desc";
			}
			int RecordCount = db.Count<Application>(where, dbParams.ToArray());
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


			List<Application> records = db.Take<Application>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
			rtn.AddRange(records);
			rtn.PageIndex = PageIndex;
			rtn.PageSize = PageSize;
			rtn.TotalCount = RecordCount;

			return rtn;

		}
	}
	
}
