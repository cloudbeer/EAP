using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using System.Web;

namespace EAP.Controllers
{
    public class CabbageController : EAP.Logic.__UserController
    {
        public ActionResult Index()
        {
            //当用户的浏览器为IE6 强制其升级浏览器
            HttpBrowserCapabilitiesBase bc = Request.Browser;

            if (bc.Browser + bc.Version == "IE6.0")
            {
                return RedirectToAction("UpdateIE", "Account");
            }

            var menus = EAP.Logic.Bus.Helper.ToJsonMenu(_user, _tenant.TenantID, db);
            //menus.Append("{");
            //menus.Append("'01' : [['01','进销存系统','cabbage'],{");
            //menus.Append("  	        '0101' : ['仓库管理','/Z10Depot','Z10Depot'],");
            //menus.Append("              '0102' : ['错误提示页','/Error/NoPermission','Z10Depot'],");
            //menus.Append("              '0104' : ['进货入库','/Z10Order/PurchaseList','Z10Depot'],");
            //menus.Append("              '0199' : ['初始建库','/Z10Depot/Init','Z10Depot']");
            //menus.Append("            }],");
            //menus.Append("'02' : [['02','基础数据','baobiao'],{");
            //menus.Append("  	        '0201' : ['产品管理','/Z01Product','product'],");
            //menus.Append("  	        '0202' : ['客户管理','/Z01Customer','customer'],");
            //menus.Append("  	        '0203' : ['产品分类','/Z01ProductCategory','category'],");
            //menus.Append("  	        '0204' : ['品牌','/Z01Brand','brand'],");
            //menus.Append("  	        '0205' : ['计量单位','/Z01Unit','unit'],");
            //menus.Append(" 	            '0206' : ['银行','/Z01Bank','bank'],");
            //menus.Append("  	        '0208' : ['头衔/职务','/Z01Title','title'],");
            //menus.Append("  	        '0208' : ['币种','/Currency','money'],");
            //menus.Append("  	        '0207' : ['属性模板','/Z01Bank','property'],");
            //menus.Append("  	        '0210' : ['客户分类','/Z01CustomerCategory','category']");
            //menus.Append("            }],");
            //menus.Append("'03' : [['03','系统管理','system'],{");
            //menus.Append("  	        '0301' : ['角色管理','/role','role'],");
            //menus.Append("	            '0302' : ['组管理','/group','group'],");
            //menus.Append("  	        '0303' : ['用户管理','/user','user'],");
            //menus.Append("  	        '0304' : ['权限管理','/permission','permission']");
            //menus.Append("            }]");
            //menus.Append("}");

            var u = _user;

            var indexHtml = new StringBuilder();

            indexHtml.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            indexHtml.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            indexHtml.AppendLine("<head>");
            indexHtml.AppendLine("    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
            indexHtml.AppendLine("    <title>踏骐销售管理系统</title>");
            indexHtml.AppendLine("    <link rel='stylesheet' type='text/css' href='/content/styles/base.css' />");
            indexHtml.AppendLine("    <link rel='stylesheet' type='text/css' href='/content/styles/icon.css' />");
            indexHtml.AppendLine("    <link rel='stylesheet' type='text/css' href='/content/styles/default.css' />");
            indexHtml.AppendLine("    <link rel='stylesheet' type='text/css' href='/content/styles/common.css' />");
            //indexHtml.AppendLine("    <script type='text/javascript' src='http://firebug/firebug-lite.js'></script>");
            indexHtml.AppendLine("    <script type='text/javascript' src='/content/scripts/jquery.js'></script>");
            //indexHtml.AppendLine("    <script type='text/javascript' src='http://weather.news.qq.com/inc/minisite_296.js' charset='utf-8'></script>");
            indexHtml.AppendLine("    <script type='text/javascript' src='/content/scripts/jquery.tip.js'></script>");
            indexHtml.AppendLine("    <script type='text/javascript' src='/content/scripts/default.js'></script>");
            indexHtml.AppendLine("    <script type='text/javascript'>");
            indexHtml.AppendLine("        var pageSize = 10;");
            indexHtml.AppendFormat("        var mymenu = {0};", menus);
            indexHtml.AppendLine("    </script>");
            indexHtml.AppendLine("</head>");
            indexHtml.AppendLine("<body>");
            indexHtml.AppendLine("    <div id='page'>");
            indexHtml.AppendLine("        <div id='wrap'>");
            indexHtml.AppendLine("            <div id='header'>");
            indexHtml.AppendLine("                <span class='usertip fr pt50 pr10'>");
            indexHtml.AppendFormat("{0}，您好！ <span class='ml10'>[<a href='javascript:;' class='pl5 pr5' id='js_changpwd'>修改密码</a>]</span>", u.Name);
            indexHtml.AppendLine("<span class='ml10'>[<a href='javascript:;' class='pl5 pr5' id='js_exit'>退出系统</a>]</span><span class='ml10'>[<a href='/Z10Public/QueryDepot/' target='_blank'>库存查询</a>]</span></span>");
            indexHtml.AppendLine("                <h1>");
            indexHtml.AppendLine("                    <a href='/cabbage/desk' target='main_frame'>");
            indexHtml.AppendLine("                        <img src='/content/images/logo2.gif' alt='踏骐销售管理系统' /></a></h1>");
            indexHtml.AppendLine("            </div>");
            indexHtml.AppendLine("            <div id='continer'>");
            indexHtml.AppendLine("                <div id='siderbar'>");
            indexHtml.AppendLine("                    <div id='js_sidbar_menu' class='subnav'></div>");
            indexHtml.AppendLine("                    <div id='js_sidbar_zmenu'></div>");
            indexHtml.AppendLine("                </div>");
            indexHtml.AppendLine("                <div id='content'>");
            indexHtml.AppendLine("                    <div id='path'>");
            //indexHtml.AppendLine("                        <span id='weather' class='fr pr10'>天气预报加载中...</span>");
            indexHtml.AppendLine("                        <span id='js_location'>今天是：" + DateTime.Now.ToString("yyyy年MM月dd日") + "</span>");
            indexHtml.AppendLine("                    </div>");
            indexHtml.AppendLine("                </div>");
            indexHtml.AppendLine("            </div>");
            indexHtml.AppendLine("        </div>");
            indexHtml.AppendLine("    </div>");
            indexHtml.AppendLine("</body>");
            indexHtml.AppendLine("</html>");



            return Content(indexHtml.ToString(), "text/html; charset=utf-8");
        }

        public ActionResult Desk()
        {
            var deskHtml = new StringBuilder();
            deskHtml.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            deskHtml.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            deskHtml.AppendLine("<head>");
            deskHtml.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
            deskHtml.AppendLine("<title>踏骐销售管理系统</title>");
            deskHtml.AppendLine("<link rel='stylesheet' type='text/css' href='/content/styles/base.css'/>");
            deskHtml.AppendLine("<link rel='stylesheet' type='text/css' href='/content/styles/frame.css'/>");
            deskHtml.AppendLine("<script type='text/javascript' src='/content/scripts/jquery.js'></script>");
            deskHtml.AppendLine("<script type='text/javascript' src='/content/scripts/frame.js'></script>");
            deskHtml.AppendLine("</head>");
            deskHtml.AppendLine("<body>");
            deskHtml.AppendLine("<div id='contents' class='deskbg'>");

            deskHtml.AppendLine("<ul class='shortcut'>");
            deskHtml.AppendLine("<li><a class='gbutton mr20' href='/Z10SalesOrder/Create/'>销售</a></li>");
            deskHtml.AppendLine("<li><a class='gbutton mr20' href='/Z10StockOrder/Create/'>进货</a></li>");
            deskHtml.AppendLine("<li><a class='gbutton mr20' href='/Z01Product/Index/'>商品</a></li>");
            deskHtml.AppendLine("</ul>");


            deskHtml.AppendLine("</div>");
            deskHtml.AppendLine("<div id='bottom'>");
            deskHtml.AppendLine("    <div class='pl10 fl'><a target='_blank' href='#'>更新日志</a> - <a target='_blank' href='#'>使用帮助</a> - <a target='_blank' href='#'>意见反馈</a></div>");
            //string conn = "";
            //#if debug

            //#endif

            deskHtml.AppendLine("    <div class='pr10 fr'>&copy; 2009-2012 TouchBike.  All Rights Reserved.</div>");
            deskHtml.AppendLine("</div>");
            deskHtml.AppendLine("</body>");
            deskHtml.AppendLine("</html>");

            return Content(deskHtml.ToString(), "text/html; charset=utf-8");
        }
    }
}
