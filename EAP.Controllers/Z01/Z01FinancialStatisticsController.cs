using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zippy.Data.Collections;
using Z01Beetle.Entity;
using System.Text;

namespace EAP.Controllers.Z01
{
    public class Z01FinancialStatisticsController : EAP.Logic.__UserController
    {

        #region cashFlow
        public ActionResult CashFlowList(int? PageIndex, int? PageSize, long? qBankID, long? qCategoryID, long? qOrderID,
            int? qFlowType, int? qFlowStatus, string qInOut, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, string qCurrency, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);

            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qBankID", qBankID);
            hs.Add("qCategoryID", qCategoryID);
            hs.Add("qOrderID", qOrderID);
            hs.Add("qFlowType", qFlowType);
            hs.Add("qFlowStatus", qFlowStatus);
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);
            hs.Add("qCurrency", qCurrency);
            hs.Add("qInOut", qInOut);
            hs.Add("orderCol", orderCol);

            decimal allSum = 0;
            PaginatedList<EAP.Logic.Z01.View.V_FinancialFlow> result =
                Z01Beetle.Entity.Helper.Z01FinancialFlowHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol, out allSum);
            result.QueryParameters = hs;

            ViewData["BankOptions"] = EAP.Logic.Z01.HtmlHelper.BankSelectOptions(_tenant.TenantID.Value);
            ViewData["CurrencyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);
            if (qCurrency.IsNotNullOrEmpty())
                ViewData["AllSum"] = qCurrency.ToString() + ": " + allSum.ToString("0.00");
            else
                ViewData["AllSum"] = "选择货币查询后方可统计";

            string thisPage = "";
            IEnumerable<IGrouping<string, EAP.Logic.Z01.View.V_FinancialFlow>> grouped = result.GroupBy(s => s.Currency);
            foreach (IGrouping<string, EAP.Logic.Z01.View.V_FinancialFlow> kvs in grouped)
            {
                thisPage += kvs.Key + ": " + (kvs.Sum(x => x.Amount) ?? 0).ToString("0.00") + "<br />";

            }

            var banks = EAP.Logic.Z01.Helper.GetBanks(_tenant.TenantID.Value);
            ViewBag.banks = banks;

            ViewData["ThisPageSum"] = thisPage;
            return View(result);

        }
        public ActionResult ChangeOrderDate(long fid, DateTime dt)
        {
            EAP.Logic.DictResponse res = new Logic.DictResponse();
            Z01FinancialFlow ff = new Z01FinancialFlow();
            ff.FlowID = fid;
            ff.CreateDate = dt;
            db.Update(ff);
            res._state = true;
            return Content(res.ToJson());
        }
        public ActionResult ChangeBank(long fid, long bid)
        {
            EAP.Logic.DictResponse res = new Logic.DictResponse();
            Z01FinancialFlow ff = new Z01FinancialFlow();
            ff.FlowID = fid;
            ff.BankID = bid;
            db.Update(ff);
            res._state = true;
            return Content(res.ToJson());
        }
        #endregion


        #region 图表
        public ActionResult MonthlyChart()
        {
            ViewData["CurrencyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);
            return View();
        }
        public ActionResult GetMonthlyChartXml(DateTime qDateFrom, DateTime qDateTo, string qCurrency, int qIn, int? qDuration)
        {
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.ContentType = "text/xml";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            Hashtable hs = new Hashtable();
            hs.Add("qDateFrom", qDateFrom);
            hs.Add("qDateTo", qDateTo);
            hs.Add("qCurrency", qCurrency);
            hs.Add("qIn", qIn);


            string strInOut = qIn == 1 ? "Income" : "Expense";
            string title = qIn == 1 ? "收入现金流 " + qCurrency : "支出现金流 " + qCurrency;
            string xcolor = qIn == 1 ? "ff0000" : "009900";
            List<Z01Beetle.Entity.Z01FinancialFlow> flows =
                Z01Beetle.Entity.Helper.Z01FinancialFlowHelper.QueryForStat(db, _tenant.TenantID.Value, hs);

            sb.AppendLine("<?xml version='1.0' encoding='gb2312'?>");
            if (qDuration == 0) //月
            {
                DateTime xDate = new DateTime(qDateFrom.Year, qDateFrom.Month, 1);
                DateTime endDate = (new DateTime(qDateTo.Year, qDateTo.Month, 1)).AddMonths(1);
                decimal allSum = flows.Sum(s => s.Amount) ?? 0;
                if (allSum == 0)
                    sb.AppendLine("<graph animation='1' yAxisMaxValue='1' caption='" + title + "' xAxisName='月' yAxisName='" + qCurrency + "' decimalPrecision='2' formatNumberScale='0' baseFontColor='" + xcolor + "' baseFont='microsoft yahei' >");
                else
                    sb.AppendLine("<graph animation='1' caption='" + title + "' xAxisName='月' yAxisName='" + qCurrency + "' decimalPrecision='2' formatNumberScale='0' baseFontColor='" + xcolor + "' baseFont='microsoft yahei' >");

                while (true)
                {
                    decimal xsum = flows.Where(s => s.CreateDate >= xDate && s.CreateDate < xDate.AddMonths(1)).Sum(s => s.Amount) ?? 0;
                    sb.AppendLine("<set name='" + xDate.ToString("yyyy-MM") + "' value='" + Math.Abs(xsum) + "' color='" + xcolor + "' />");
                    xDate = xDate.AddMonths(1);

                    if (xDate >= endDate)
                        break;
                }
            }
            else //日
            {
                DateTime xDate = qDateFrom;
                decimal allSum = flows.Sum(s => s.Amount) ?? 0;
                if (allSum == 0)
                    sb.AppendLine("<graph animation='1' yAxisMaxValue='1' caption='" + title + "' xAxisName='日' yAxisName='" + qCurrency + "' decimalPrecision='2' formatNumberScale='0' baseFontColor='" + xcolor + "' baseFont='microsoft yahei' >");
                else
                    sb.AppendLine("<graph animation='1' caption='" + title + "' xAxisName='日' yAxisName='" + qCurrency + "' decimalPrecision='2' formatNumberScale='0' baseFontColor='" + xcolor + "' baseFont='microsoft yahei' >");

                while (true)
                {
                    decimal xsum = flows.Where(s => s.CreateDate >= xDate && s.CreateDate < xDate.AddDays(1)).Sum(s => s.Amount) ?? 0;
                    sb.AppendLine("<set name='" + xDate.ToString("yyyy-MM-dd") + "' value='" + Math.Abs(xsum) + "' color='" + xcolor + "' />");
                    xDate = xDate.AddDays(1);

                    if (xDate >= qDateTo.AddDays(1))
                        break;
                }
            }
            //sb.AppendLine("<styles>");
            //sb.AppendLine("<definition>");
            //sb.AppendLine("</definition>");
            //sb.AppendLine("</styles>");
            sb.AppendLine("</graph>");

            return Content(sb.ToString());
        }

        #endregion
        #region 总结
        public ActionResult DateSummary(DateTime? toDate, DateTime? fromDate)
        {
            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day);
            var maxDate = toDate ?? today;
            maxDate = maxDate.AddDays(1);
            var minDate = fromDate ?? today;
            var diffDate = maxDate - minDate;

            StringBuilder sbOrder = new StringBuilder();
            sbOrder.Append("开始日期：" + minDate.ToString("yyyy-MM-dd"));
            sbOrder.Append(" 结束日期：" + maxDate.AddDays(-1).ToString("yyyy-MM-dd"));

            if (diffDate.TotalDays > 366 || diffDate.TotalDays < 0)
            {
                return Content("不允许超过1年，且结束日期不能大于开始日期。");
            }
            List<System.Data.Common.DbParameter> dbFlowParams = new List<System.Data.Common.DbParameter>();
            string sqlFlow = "TenantID=@TenantID and CreateDate>=@fromDate and CreateDate<@toDate and OrderID>0 and Amount>0";
            dbFlowParams.Add(db.CreateParameter("TenantID", _tenant.TenantID));
            dbFlowParams.Add(db.CreateParameter("fromDate", minDate));
            dbFlowParams.Add(db.CreateParameter("toDate", maxDate));
            var fFlow = db.Take<Z01FinancialFlow>(sqlFlow, dbFlowParams.ToArray());
            ViewBag.cashflows = fFlow;


            ViewBag.minDate = minDate;
            ViewBag.maxDate = maxDate;
            ViewBag.otitle = sbOrder.ToString();

            return View();

        }
        #endregion
    }
}
