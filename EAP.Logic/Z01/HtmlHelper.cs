using System;
using System.Collections.Generic;
using System.Text;
using Z01Beetle.Entity;

namespace EAP.Logic.Z01
{
    public class HtmlHelper
    {
        /// <summary>
        /// 银行的下拉列表项
        /// </summary>
        /// <returns></returns>
        public static string BankSelectOptions(Guid tenantID)
        {
            StringBuilder sb = new StringBuilder();
            List<Z01Bank> xbank = Helper.GetBanks(tenantID);
            foreach (var bank in xbank)
            {
                sb.AppendLine("<option value='" + bank.BankID + "'>" + bank.Title + "</option>");
            }

            return sb.ToString();
        }
        /// <summary>
        /// 合同条款下拉列表
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static string PaperTemplateSelectOptions(Guid tenantID, Zippy.Data.IDalProvider db)
        {
            StringBuilder sb = new StringBuilder();
            List<Z01PaperTemplate> xobjs =  db.Take<Z01PaperTemplate>("TenantID=@TenantID order by DisplayOrder", db.CreateParameter("TenantID", tenantID));
            foreach (var xobj in xobjs)
            {
                sb.AppendLine("<option value='" + xobj.TemplateID + "'>" + xobj.Title + "</option>");
            }

            return sb.ToString();
        }
    }
}
