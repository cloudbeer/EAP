using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP.Logic.Bus
{
    public class HtmlHelper
    {
        /// <summary>
        /// 币种的下拉列表项
        /// </summary>
        /// <returns></returns>
        public static string CurrencySelectOptions(Zippy.Data.IDalProvider db)
        {
            StringBuilder sb = new StringBuilder();
            List<EAP.Bus.Entity.Currency> xcurrencies = Helper.GetCurrencies(db);
            foreach (EAP.Bus.Entity.Currency xcurrency in xcurrencies)
            {
                sb.AppendLine("<option value='" + xcurrency.ID + "'>" + xcurrency.Title + "(" + xcurrency.ID + ")</option>");
            }

            return sb.ToString();
        }
        public static string CurrencySelectOptions(string selectedID, Zippy.Data.IDalProvider db)
        {
            StringBuilder sb = new StringBuilder();
            List<EAP.Bus.Entity.Currency> xcurrencies = Helper.GetCurrencies(db);
            foreach (EAP.Bus.Entity.Currency xcurrency in xcurrencies)
            {
                if (selectedID == xcurrency.ID)
                    sb.AppendLine("<option value='" + xcurrency.ID + "' selected='selected'>" + xcurrency.Title + "(" + xcurrency.ID + ")</option>");
                else
                    sb.AppendLine("<option value='" + xcurrency.ID + "'>" + xcurrency.Title + "(" + xcurrency.ID + ")</option>");
            }

            return sb.ToString();
        }
    }
}
