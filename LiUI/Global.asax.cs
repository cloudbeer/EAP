using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LiUI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "Check", // 路由名称
            //    "check/{action}", // 带有参数的 URL
            //    new { controller = "Account"} // 参数默认值                
            //);
            //routes.MapRoute(
            //    "IndexPager", // 分页查询
            //    "{controller}/{action}/{PageSize}/{PageIndex}", // URL with parameters
            //    new { controller = "Home", action = "Index", PageSize = 20, PageIndex = 1 } // Parameter defaults
            //);

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Cabbage", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

            //ViewEngines.Engines.Add(
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SESSIONID";

                if (HttpContext.Current.Request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                Response.Write("Error Initializing Session");

            }

        }

        void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookie_name);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }



        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            EAP.Logic.Helper.SetSkippedRefererUrls();

            ModelMetadataProviders.Current = new EAP.Logic.MyDataAnnotationsModelMetadataProvider();
        }

        protected void Session_Start()
        {
            Zippy.Data.IDalProvider db = Zippy.Data.DalFactory.CreateProvider();
            Zippy.SaaS.Helper.UserHelper.LogonFromCookie(db);
        }
    }
}