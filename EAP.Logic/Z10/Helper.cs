using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP.Logic.Z10
{
    public class Helper
    {
        public static List<Z10Cabbage.Entity.Z10Depot> GetDepots(Guid tenantID, Zippy.Data.IDalProvider db)
        {
            if (System.Web.HttpContext.Current.Cache["Depots"].IsNotNullOrEmpty())
                return System.Web.HttpContext.Current.Cache["Depots"] as List<Z10Cabbage.Entity.Z10Depot>;
            
            List<Z10Cabbage.Entity.Z10Depot> xdepots = db.Take<Z10Cabbage.Entity.Z10Depot>("TenantID=@TenantID order by DisplayOrder", db.CreateParameter("TenantID", tenantID));
            System.Web.HttpContext.Current.Cache["Depots"] = xdepots;
            return xdepots;
        }
    }
}
