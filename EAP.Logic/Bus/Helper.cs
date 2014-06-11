using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EAP.Bus.Entity;

namespace EAP.Logic.Bus
{
    public class Helper
    {
        /// <summary>
        /// 获取货币
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<Currency> GetCurrencies(Zippy.Data.IDalProvider db)
        {
            //if (System.Web.HttpContext.Current.Cache["Currencies"].IsNotNullOrEmpty())
            //    return System.Web.HttpContext.Current.Cache["Currencies"] as List<Currency>;

            List<EAP.Bus.Entity.Currency> xcurrencies = db.Take<EAP.Bus.Entity.Currency>("1=1 order by DisplayOrder");
            System.Web.HttpContext.Current.Cache["Currencies"] = xcurrencies;
            return xcurrencies;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<Role> GetRoles(Zippy.Data.IDalProvider db, Guid tenantID)
        {
            //if (System.Web.HttpContext.Current.Cache["Roles_" + tenantID].IsNotNullOrEmpty())
            //    return System.Web.HttpContext.Current.Cache["Roles_" + tenantID] as List<Role>;

            List<Role> xroles = db.Take<Role>("TenantID=@TenantID order by DisplayOrder", db.CreateParameter("TenantID", tenantID));
            System.Web.HttpContext.Current.Cache["Roles_" + tenantID] = xroles;
            return xroles;
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<Permission> GetPermissions(Zippy.Data.IDalProvider db, Guid? tenantID)
        {
            //if (System.Web.HttpContext.Current.Cache["Permissions_" + tenantID].IsNotNullOrEmpty())
            //    return System.Web.HttpContext.Current.Cache["Permissions_" + tenantID] as List<Permission>;

            List<Permission> xobjs = db.Take<Permission>();
            //按照父子关系顺序重建
            List<Permission> newRecords = new List<Permission>();
            IEnumerable<Permission> rootRecords = xobjs.Where(s => (s.ParentID ?? 0) == 0).OrderBy(s => s.DisplayOrder);
            foreach (Permission per in rootRecords)
            {
                newRecords.Add(per);
                newRecords.AddRange(xobjs.Where(s => s.ParentID == per.PermissionID).OrderBy(s => s.DisplayOrder));
            }

            System.Web.HttpContext.Current.Cache["Permissions_" + tenantID] = newRecords;
            return newRecords;
        }
        /// <summary>
        /// j将当前用户的url都显示出来
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tenantID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string ToJsonMenu(Zippy.SaaS.Entity.User user, Guid? tenantID, Zippy.Data.IDalProvider db)
        {
            System.Text.StringBuilder menus = new StringBuilder();
            menus.AppendLine("{");
            List<Permission> xobjs = GetPermissions(db, tenantID);
            List<EAP.Logic.Bus.View.V_UserPermission> mePers = null;
            if (!user.UserType.BitIs(Zippy.SaaS.UserTypes.SystemAdministrator))
            {
                mePers = db.Take<EAP.Logic.Bus.View.V_UserPermission>("UserID=@UserID", db.CreateParameter("UserID", user.UserID));
            }

            var rootRecords = xobjs.Where(s => (s.ParentID ?? 0) == 0 && s.PermissionStatus == 1).OrderBy(s => s.DisplayOrder);
            foreach (Permission per in rootRecords)
            {
                if (mePers == null || (mePers != null && mePers.Exists(s => s.PermissionID == per.PermissionID)))
                {
                    if (per.PermissionStatus == 1)
                        menus.AppendLine("'" + per.PermissionID + "' : [['" + per.PermissionID + "','" + per.Title + "','" + per.Icon + "'],{");

                    var subRecords = xobjs.Where(s => s.ParentID == per.PermissionID).OrderBy(s => s.DisplayOrder);
                    foreach (Permission subper in subRecords)
                    {
                        if (mePers == null || (mePers != null && mePers.Exists(s => s.PermissionID == subper.PermissionID)))
                        {
                            if (subper.PermissionStatus == 1)
                                menus.AppendLine("'" + subper.PermissionID + "' : ['" + subper.Title + "','" + subper.Url + "','" + subper.Icon + "'],");
                        }

                    }
                    //if (menus.ToString().EndsWith(",")) menus.Remove(menus.Length - 1, 1);
                    menus.AppendLine("}],");

                }
            }
            //if (menus.ToString().EndsWith(",\n")) menus.Remove(menus.Length - 1, 1);

            menus.AppendLine("}");


            return menus.ToString().Replace(",\r\n}", "\r\n}");
        }
    }
}
