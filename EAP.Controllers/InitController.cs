using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using System.Web;
using System.Linq;

namespace EAP.Controllers
{
    public class InitController : Zippy.SaaS.Mvc.Controller
    {
        public ActionResult Index()
        {
            
            //return Content("租户和管理员已经创建过。<br />租户 Flag： Liwill；管理员帐号：admin@liwill.com/admin");
            Guid tenantID = Guid.NewGuid();
            Bus.Entity.Tenant t = new Bus.Entity.Tenant();
            t.TenantID = tenantID;
            t.Title = "励惠";
            t.TenantCode = "TouchBike";
            t.Flag = "TouchBike";
            db.Insert(t);


            Bus.Entity.User u = new Bus.Entity.User();
            u.UserID = Guid.NewGuid();
            u.UserName = "admin";
            string pwd = "admin";
            u.Password = pwd.Md6();
            u.UserType = (int)Zippy.SaaS.UserTypes.SystemAdministrator;
            u.Nickname = "管理员";
            u.Name = "踏骐";
            u.Email = "cloudbeer@gmail.com";
            u.TenantID = tenantID;
            db.Insert(u);

            return Content("租户和管理员创建成功。<br />租户 Flag： TouchBike；管理员帐号：admin/admin");
        }

        public ActionResult ToPermissionSql()
        {
            Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();
            var db = Zippy.Data.DalFactory.CreateProvider();
            List<EAP.Bus.Entity.Permission> pers = db.Take<EAP.Bus.Entity.Permission>();

            IEnumerable<EAP.Bus.Entity.Permission> roots = from x in pers
                                                           where x.ParentID == 0
                                                           select x;
            sb.AppendLine("declare @myid int;");
            foreach (var root in roots)
            {
                sb.AppendLine("insert into Permission(Title,Url,Flag,ParentID,Icon,PermissionType,PermissionStatus,DisplayOrder) ");
                sb.AppendLine("values ('" + root.Title + "','" + root.Url + "','" + root.Flag + "',0,'" + root.Icon + "'," + root.PermissionType + "," + root.PermissionStatus + "," + root.DisplayOrder + ");");
                sb.AppendLine("SELECT @myid= SCOPE_IDENTITY();");

                IEnumerable<EAP.Bus.Entity.Permission> subs = from x in pers
                                                              where x.ParentID == root.PermissionID
                                                              select x;
                foreach (var sub in subs)
                {
                    sb.Append("insert into Permission(Title,Url,Flag,ParentID,Icon,PermissionType,PermissionStatus,DisplayOrder) ");
                    sb.AppendLine("values ('" + sub.Title + "','" + sub.Url + "','" + sub.Flag + "',@myid,'" + sub.Icon + "'," + sub.PermissionType + "," + sub.PermissionStatus + "," + sub.DisplayOrder + ");");

                }
                sb.AppendLine();
                sb.AppendLine();
            }


            //insert into X80Menu (Title,Url,ParentID,RelativeMenu,SoftwareID) values (@title, @url, @parent,@relative,@softwareid);
            //SELECT @myid= SCOPE_IDENTITY();

            return Content(sb.ToString());
        }
    }
}
