using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP.Logic.Z10
{
    public class HtmlHelper
    {

        /// <summary>
        /// 仓库的下拉列表项
        /// </summary>
        /// <returns></returns>
        public static string DepotSelectOptions(Guid tenantID, Zippy.Data.IDalProvider db)
        {
            StringBuilder sb = new StringBuilder();
            List<Z10Cabbage.Entity.Z10Depot> xdepots = Helper.GetDepots(tenantID, db);
            foreach (Z10Cabbage.Entity.Z10Depot depot in xdepots)
            {
                sb.Append("<option value='" + depot.DepotID + "'>" + depot.Title + "</option>");
            }

            return sb.ToString();
        }

        public static string DepotSelectOptions(Guid tenantID, long? selectedID, Zippy.Data.IDalProvider db)
        {
            StringBuilder sb = new StringBuilder();
            List<Z10Cabbage.Entity.Z10Depot> xdepots = Helper.GetDepots(tenantID, db);
            foreach (Z10Cabbage.Entity.Z10Depot depot in xdepots)
            {
                if (selectedID == depot.DepotID)
                    sb.Append("<option value='" + depot.DepotID + "' selected='selected'>" + depot.Title + "</option>");
                else
                    sb.Append("<option value='" + depot.DepotID + "'>" + depot.Title + "</option>");
            }

            return sb.ToString();
        }
    }
}
