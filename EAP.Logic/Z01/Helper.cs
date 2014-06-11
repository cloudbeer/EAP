using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Z01Beetle.Entity;

namespace EAP.Logic.Z01
{
    /// <summary>
    /// 此处获取缓存不支持多租户
    /// </summary>
    public class Helper
    {
        #region 银行缓存
        /// <summary>
        /// 获得银行信息
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<Z01Bank> GetBanks(Guid tenantID)
        {
            if (System.Web.HttpContext.Current.Cache["BankList"].IsNotNullOrEmpty())
                return System.Web.HttpContext.Current.Cache["BankList"] as List<Z01Bank>;

            List<Z01Bank> bankList = Zippy.Data.StaticDB.DB.Take<Z01Bank>("TenantID=@TenantID order by DisplayOrder", Zippy.Data.StaticDB.DB.CreateParameter("TenantID", tenantID));
            System.Web.HttpContext.Current.Cache["BankList"] = bankList;

            return bankList;
        }
        public static void ClearBanks()
        {
            System.Web.HttpContext.Current.Cache.Remove("BankList");
        }
        #endregion

        #region 产品分类缓存和清除
        public static List<Z01ProductCategory> GetProductCategories(Guid tenantID)
        {
            if (System.Web.HttpContext.Current.Cache["Z01ProductCategoryList"].IsNotNullOrEmpty())
                return System.Web.HttpContext.Current.Cache["Z01ProductCategoryList"] as List<Z01ProductCategory>;

            List<Z01ProductCategory> xlist = Zippy.Data.StaticDB.DB.Take<Z01ProductCategory>("TenantID=@TenantID", Zippy.Data.StaticDB.DB.CreateParameter("TenantID", tenantID));

            List<Z01ProductCategory> newRecords = new List<Z01ProductCategory>();
            ReGroup(xlist, newRecords, 0, "├", 0);

            System.Web.HttpContext.Current.Cache["Z01ProductCategoryList"] = newRecords;
            return newRecords;
        }
        public static void ClearProductCategories()
        {
            System.Web.HttpContext.Current.Cache.Remove("Z01ProductCategoryList");
        }
        public static void ReGroup(List<Z01ProductCategory> records, List<Z01ProductCategory> newRecords, long? parentID, string prefix, long removeID)
        {

            prefix = "&nbsp; " + prefix + "─";
            IEnumerable<Z01ProductCategory> thisGroups = records.Where(s => s.ParentID == parentID && s.CategoryID != removeID).OrderBy(s => s.DisplayOrder);
            foreach (Z01ProductCategory tGroup in thisGroups)
            {
                Z01ProductCategory nGroup = new Z01ProductCategory();
                nGroup.CategoryID = tGroup.CategoryID;
                nGroup.Code = tGroup.Code;
                if (tGroup.ParentID != 0 && prefix != "├")
                    nGroup.Title = prefix + " " + tGroup.Title;
                else
                    nGroup.Title = tGroup.Title;
                newRecords.Add(nGroup);
                //if (tGroup.ParentID != 0 && prefix != "├")
                //    nGroup.Title = prefix + " " + tGroup.Title;
                //newRecords.Add(tGroup);
                ReGroup(records, newRecords, tGroup.CategoryID, prefix, removeID);
            }
        }
        #endregion

        /// <summary>
        /// 获得银行账号信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="bankID"></param>
        /// <returns></returns>
        public static Z01Bank GetBankInfo(long? bankID)
        {
            return Zippy.Data.StaticDB.DB.FindUnique<Z01Bank>(bankID);
        }

        //public static List<Z01PaperTemplate> GetPaperTemplates(Guid tenantID, Zippy.Data.IDalProvider db)
        //{
        //    List<Z01Bank> bankList = db.Take<Z01Bank>("TenantID=@TenantID order by DisplayOrder", db.CreateParameter("TenantID", tenantID));

        //    return bankList;
        //}
    }
}
