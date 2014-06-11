using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.IO;

namespace EAP.Controllers
{
    public class HelperController : System.Web.Mvc.Controller
    {

        public ActionResult VImage(string id, int? w, int? h)
        {
            string seedString = "0123456789";
            int xwidth = w ?? 60;
            int xheight = h ?? 23;
            string vcode = seedString.ToRandom(4);

            int iwidth = (int)(vcode.Length * 24);

            System.Drawing.Image image = vcode.ToHipImage(new Size(xwidth, xheight));
            Response.ContentType = "image/png";
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
            }

            image.Dispose();

            string sessionID = "vcode";
            if (!string.IsNullOrEmpty(id))
            {
                sessionID += id;
            }

            Session.Add(sessionID, vcode);
            return null;
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

        public ActionResult PubCustomer()
        {
            StringBuilder sb = new StringBuilder();
            using (System.IO.StreamReader sr = new StreamReader(Server.MapPath("~/cus.csv"), System.Text.Encoding.UTF8))
            {
                while (sr.Peek() > 0)
                {
                    string x = sr.ReadLine();
                    string[] xxs = x.Split(',');

                    string xname = xxs[0].Trim();
                    string xgender = xxs[1].Trim();
                    string xcode = xxs[2].Trim();
                    string xtel = xxs[3].Trim();

                    if (xname.IsNotNullOrEmpty() && (xcode.IsNotNullOrEmpty()||xtel.IsNotNullOrEmpty()) && xname!="姓名")
                    {
                        sb.AppendFormat("insert into Z01Customer (Title,Tel1,Tel2,CustomerType) values('{0}','{1}','{2}',1)", xname, xtel, xcode); //  xname + "-" + xgender + " -" + xcode + "-" + xtel);
                        sb.AppendLine("<br />");
                    }
                }
                sr.Close();
            }

            return Content(sb.ToString());
        }

        [AcceptVerbs( HttpVerbs.Post)]
        public ActionResult Test(int? id)
        {
            return Content(DateTime.Now.ToString() + " " + (1000+(id??0)));

        }

    }
}
