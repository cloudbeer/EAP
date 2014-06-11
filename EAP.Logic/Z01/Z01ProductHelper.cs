using System;
using System.Collections;
using System.Collections.Generic;
using Zippy.Data.Collections;
using System.Linq;

namespace Z01Beetle.Entity.Helper
{

    public static class Z01ProductHelper
    {

        #region 本实体的外键实体集合
        /// <summary>
        /// 获取 [产品 的 商品] 的 [产品扩展属性] 集合
        /// </summary>
        public static List<Z01ProductExtValue> GetProductID_Z01ProductExtValues(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            if (entity.ProductID.HasValue)
                return db.Take<Z01ProductExtValue>("ProductID=@ProductID", db.CreateParameter("ProductID", entity.ProductID));
            return new List<Z01ProductExtValue>();

        }
        /// <summary>
        /// 获取 [产品 的 产品] 的 [产品分类关系] 集合
        /// </summary>
        public static List<Z01ProductInCategory> GetProductID_Z01ProductInCategorys(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            if (entity.ProductID.HasValue)
                return db.Take<Z01ProductInCategory>("ProductID=@ProductID", db.CreateParameter("ProductID", entity.ProductID));
            return new List<Z01ProductInCategory>();

        }
        /// <summary>
        /// 获取 [产品 的 产品] 的 [客户分类关系] 集合
        /// </summary>
        public static List<Z01CustomerInCategory> GetProductID_Z01CustomerInCategorys(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            if (entity.ProductID.HasValue)
                return db.Take<Z01CustomerInCategory>("ProductID=@ProductID", db.CreateParameter("ProductID", entity.ProductID));
            return new List<Z01CustomerInCategory>();

        }
        #endregion

        #region 本实体外键对应的实体
        /// <summary>
        /// 表示 [标准计量单位] 对应的实体
        /// </summary>
        public static Z01Unit GetUnitIDEntity(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            return db.FindUnique<Z01Unit>("UnitID=@UnitID", db.CreateParameter("UnitID", entity.UnitID));
        }

        /// <summary>
        /// 表示 [标准计量单位] 被选实体集合
        /// </summary>
        public static List<Z01Unit> GetUnitIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Unit>();
        }

        /// <summary>
        /// 表示 [计量单位] 被选实体集合的 option html
        /// </summary>
        public static string GetUnitIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int32? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01Unit> objs = db.Take<Z01Unit>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            IEnumerable<Z01Unit> newObjs = objs.OrderBy(s => s.DisplayOrder);
            foreach (var obj in newObjs)
            {
                if (selectedValue == obj.UnitID)
                    sb.AppendLine("<option value='" + obj.UnitID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.UnitID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 表示 [品牌] 对应的实体
        /// </summary>
        public static Z01Brand GetBrandIDEntity(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            return db.FindUnique<Z01Brand>("BrandID=@BrandID", db.CreateParameter("BrandID", entity.BrandID));
        }
        /// <summary>
        /// 表示 [品牌] 被选实体集合
        /// </summary>
        public static List<Z01Brand> GetBrandIDEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Brand>();
        }

        /// <summary>
        /// 表示 [品牌] 被选实体集合的 option html
        /// </summary>
        public static string GetBrandIDEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<Z01Brand> objs = db.Take<Z01Brand>("TenantID=@TenantID order by Title", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.BrandID)
                    sb.AppendLine("<option value='" + obj.BrandID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.BrandID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 表示 [属性模板] 对应的实体
        /// </summary>
        public static PropertyTemplate GetPropertyTemplateEntity(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            return db.FindUnique<PropertyTemplate>("TemplateID=@TemplateID", db.CreateParameter("TemplateID", entity.PropertyTemplate));
        }
        /// <summary>
        /// 表示 [属性模板] 被选实体集合
        /// </summary>
        public static List<PropertyTemplate> GetPropertyTemplateEntities(Zippy.Data.IDalProvider db)
        {
            return db.Take<PropertyTemplate>();
        }

        /// <summary>
        /// 表示 [父分类] 被选实体集合的 option html
        /// </summary>
        public static string GetPropertyTemplateEntitiesHtmlOption(Zippy.Data.IDalProvider db, Guid tenantID, System.Int64? selectedValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            List<PropertyTemplate> objs = db.Take<PropertyTemplate>("TenantID=@TenantID", db.CreateParameter("TenantID", tenantID));
            foreach (var obj in objs)
            {
                if (selectedValue == obj.TemplateID)
                    sb.AppendLine("<option value='" + obj.TemplateID + "' selected='selected'>" + obj.Title + "</option>");
                else
                    sb.AppendLine("<option value='" + obj.TemplateID + "'>" + obj.Title + "</option>");
            }
            return sb.ToString();
        }

        #endregion

        public static Z01Product Create(Zippy.Data.IDalProvider db, Int64 _ProductID)
        {
            Z01Product rtn =  db.FindUnique<Z01Product>(_ProductID);
            return rtn;
        }


        public static int Delete(Zippy.Data.IDalProvider db, Int64 _ProductID)
        {
            return db.Delete<Z01Product>(_ProductID);
        }

        public static int Insert(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            int rtn = db.Insert(entity);
            return rtn;
        }

        public static int Update(Zippy.Data.IDalProvider db, Z01Product entity)
        {
            return db.Update(entity);
        }


        public static List<Z01Product> Take(Zippy.Data.IDalProvider db)
        {
            return db.Take<Z01Product>(true);
        }

        public static List<Z01Product> Take(Zippy.Data.IDalProvider db, int count)
        {
            return db.Take<Z01Product>(count, true);
        }

        public static List<Z01Product> Take(Zippy.Data.IDalProvider db, string sqlEntry, params System.Data.Common.DbParameter[] cmdParameters)
        {
            return db.Take<Z01Product>(sqlEntry, cmdParameters);
        }

        /// <summary>
        /// 产品 查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="PageSize">每页数量</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="cmdParameters">查询条件赋值</param>
        /// <returns></returns>
        public static PaginatedList<Z01Product> Take(Zippy.Data.IDalProvider db, string @where, string orderby, int PageSize, int PageIndex, params System.Data.Common.DbParameter[] cmdParameters)
        {
            PaginatedList<Z01Product> rtn = new PaginatedList<Z01Product>();
            List<Z01Product> records = db.Take<Z01Product>(where + " order by " + orderby, PageSize, PageIndex, cmdParameters);
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = db.Count<Z01Product>(where, cmdParameters);
            return rtn;
        }

        public static PaginatedList<Z01Product> Query(Zippy.Data.IDalProvider db, Guid tenantID, int PageSize, int PageIndex, Hashtable paras, int? orderCol)
        {
            PaginatedList<Z01Product> rtn = new PaginatedList<Z01Product>();
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
                object qBrief = paras["qBrief"];
                if (qBrief.IsNotNullOrEmpty())
                {
                    where += " and [Brief] like @Brief";
                    dbParams.Add(db.CreateParameter("Brief", "%" + qBrief + "%"));
                }
                object qCreateDateStart = paras["qCreateDateStart"];
                if (qCreateDateStart.IsNotNullOrEmpty())
                {
                    where += " and [CreateDate] >= @CreateDateStart";
                    dbParams.Add(db.CreateParameter("CreateDateStart", qCreateDateStart));
                }
                object qCreateDateEnd = paras["qCreateDateEnd"];
                if (qCreateDateEnd.IsNotNullOrEmpty())
                {
                    where += " and [CreateDate] < @CreateDateEnd";
                    dbParams.Add(db.CreateParameter("CreateDateEnd", ((DateTime)qCreateDateEnd).AddDays(1)));
                }
                object qModel1 = paras["qModel1"];
                if (qModel1.IsNotNullOrEmpty())
                {
                    where += " and [Model1] like @Model1";
                    dbParams.Add(db.CreateParameter("Model1", "%" + qModel1 + "%"));
                }
                object qModel2 = paras["qModel2"];
                if (qModel2.IsNotNullOrEmpty())
                {
                    where += " and [Model2] = @Model2";
                    dbParams.Add(db.CreateParameter("Model2", qModel2));
                }
                var qBrandID = paras["qBrandID"].ToInt64(0);
                if (qBrandID>0)
                {
                    where += " and [BrandID] = @BrandID";
                    dbParams.Add(db.CreateParameter("BrandID", qBrandID));
                }
                var qStatus = paras["qStatus"].ToInt32(0);
                if (qStatus > 0)
                {
                    where += " and [ProductStatus] = @ProductStatus";
                    dbParams.Add(db.CreateParameter("ProductStatus", qStatus));
                }
                var qCateID=paras["qCateID"].ToInt64(0);
                if (qCateID>0)
                {
                    where += " and [ProductID] in (select [ProductID] from Z01ProductInCategory where [CategoryID]=@CategoryID)";
                    dbParams.Add(db.CreateParameter("CategoryID", qCateID));
                }
            }
            #endregion

            string orderBy = "[ViewCount] desc, ProductID";
            if (orderCol == 0)
            {
                orderBy = "[ViewCount] desc, ProductID";
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
                orderBy = "[UnitID]";
            }
            else if (orderCol == 4)
            {
                orderBy = "[UnitID] desc";
            }
            else if (orderCol == 5)
            {
                orderBy = "[PriceList]";
            }
            else if (orderCol == 6)
            {
                orderBy = "[PriceList] desc";
            }
            else if (orderCol == 7)
            {
                orderBy = "[ProductStock]";
            }
            else if (orderCol == 8)
            {
                orderBy = "[ProductStock] desc";
            }
            else if (orderCol == 9)
            {
                orderBy = "[PriceSelling]";
            }
            else if (orderCol == 10)
            {
                orderBy = "[PriceSelling] desc";
            }
            else if (orderCol == 11)
            {
                orderBy = "[ImagePath]";
            }
            else if (orderCol == 12)
            {
                orderBy = "[ImagePath] desc";
            }
            else if (orderCol == 13)
            {
                orderBy = "[CreateDate]";
            }
            else if (orderCol == 14)
            {
                orderBy = "[CreateDate] desc";
            }
            int RecordCount = db.Count<Z01Product>(where, dbParams.ToArray());
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


            List<Z01Product> records = db.Take<Z01Product>(where + " order by " + orderBy, PageSize, PageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = PageIndex;
            rtn.PageSize = PageSize;
            rtn.TotalCount = RecordCount;

            return rtn;

        }
    }

}
