using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace EAP.Logic
{
    public class Helper
    {
        public static void SetSkippedRefererUrls()
        {
            List<string> skippedRefererUrls =
                System.Web.HttpContext.Current.Application["SkippedRefererUrls"] as List<string>;
            if (skippedRefererUrls == null)
            {
                skippedRefererUrls = new List<string>();
                System.Web.HttpContext.Current.Application["SkippedRefererUrls"] = skippedRefererUrls;
            }
            skippedRefererUrls.Add("/Z01FinancialStatistics/GetMonthlyChartXml");

        }

        /// <summary>
        /// 系统管理员或者主机管理员是管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            var _user = System.Web.HttpContext.Current.Session["user"] as Zippy.SaaS.Entity.User;

            if (_user != null)
            {
                return _user.UserType.BitIs(Zippy.SaaS.UserTypes.HostAdministrator) || _user.UserType.BitIs(Zippy.SaaS.UserTypes.SystemAdministrator);
            }
            return false;
        }
        /// <summary>
        /// 是否有财务权限
        /// </summary>
        /// <returns></returns>
        public static bool IsCFO()
        {
            var _user = System.Web.HttpContext.Current.Session["user"] as Zippy.SaaS.Entity.User;

            if (_user != null)
            {
                return _user.UserType.BitIs(Zippy.SaaS.UserTypes.HostAdministrator) ||
                    _user.UserType.BitIs(Zippy.SaaS.UserTypes.SystemAdministrator) ||
                    _user.UserType.BitIs(Zippy.SaaS.UserTypes.CFO);
            }
            return false;
        }


        public static Hashtable ColorsDict()
        {
            Hashtable hs = new Hashtable();
            hs.Add("灰", "#d9d6c3");
            hs.Add("红", "#ed1941");
            hs.Add("黑", "#000");
            hs.Add("蓝", "#2a5caa");
            hs.Add("绿", "#00ff00");
            hs.Add("白", "#fff");
            hs.Add("黄", "#ffd400");
            hs.Add("金", "#e0861a");
            hs.Add("棕", "#cd9a5b");
            hs.Add("银", "#a1a3a6");
            hs.Add("橙", "#f58220");
            hs.Add("桔", "#faa755");
            hs.Add("粉", "#f58f98");
            return hs;
        }

        public static string ColorHtml(string label)
        {
            StringBuilder sb = new StringBuilder();
            var mycolors = new List<string>();

            var label_chars = label.ToCharArray();

            var colors = ColorsDict();
            var keys = colors.Keys;
            foreach (var cc in label_chars)
            {
                if (colors.Contains(cc.ToString()))
                {
                    mycolors.Add(colors[cc.ToString()].ToString());
                }
            }

            switch (mycolors.Count)
            {
                case 1:
                    return "<div class='fcolor'><div class='full' style='background:" + mycolors[0] + "'></div></div>";
                case 2:
                    return "<div class='fcolor'><div class='half' style='background:" + mycolors[0] + "'></div><div class='half' style='background:" + mycolors[1] + "'></div></div>";
                case 3:
                    return "<div class='fcolor'><div class='half' style='background:" + mycolors[1] + "'></div><div class='half' style='background:" + mycolors[2] + "'></div></div>";
            }
            return "";
        }
    }
}
