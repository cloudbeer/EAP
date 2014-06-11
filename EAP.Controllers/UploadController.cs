using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.IO;

namespace EAP.Controllers
{
    public class UploadController : Zippy.SaaS.Mvc.__UserController
    {
        /// <summary>
        /// upload image of products.
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductImage()
        {
            string myName = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");

            string root   = System.Web.HttpContext.Current.Server.MapPath("/TenantFiles");
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            string hostFolder = System.IO.Path.Combine(root, _tenant.TenantID.ToString());
            if (!Directory.Exists(hostFolder))
                Directory.CreateDirectory(hostFolder);

            string productFolder = System.IO.Path.Combine(hostFolder, "ProductImage");
            if (!Directory.Exists(productFolder))
                Directory.CreateDirectory(productFolder);

            if (Request.Files.Count > 0)
            {
                System.Web.HttpPostedFileBase file =   Request.Files[0];

                System.Drawing.Image img  = System.Drawing.Image.FromStream(file.InputStream);
                string fileName           = file.FileName;
                string ext                = Path.GetExtension(fileName);

                img.Scale(Path.Combine(productFolder, myName + "_50" + ext), 50, ScaleModes.Squre);
                img.Scale(Path.Combine(productFolder, myName + "_320" + ext), 320, ScaleModes.Both);
                img.Scale(Path.Combine(productFolder, myName + "_600" + ext), 500, ScaleModes.Both);
                img.Save(Path.Combine(productFolder, myName + ext));
                Session["MyFileName"] = myName + ext;
            }

            return Content("1");
        }


        public ActionResult GetProductImageName()
        {
            if (Session["MyFileName"].IsNotNullOrEmpty())
            {
                string name =Session["MyFileName"].ToString();
                Session.Remove("MyFileName");
                return Content(name);
            }
            return Content("");
        }

        /// <summary>
        /// upload original image
        /// </summary>
        /// <returns></returns>
        public ActionResult OriImage()
        {
            string myName = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");

            string root   = System.Web.HttpContext.Current.Server.MapPath("/TenantFiles");
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            string hostFolder = System.IO.Path.Combine(root, _tenant.TenantID.ToString());
            if (!Directory.Exists(hostFolder))
                Directory.CreateDirectory(hostFolder);

            string productFolder = System.IO.Path.Combine(hostFolder, "Image");
            if (!Directory.Exists(productFolder))
                Directory.CreateDirectory(productFolder);

            if (Request.Files.Count > 0)
            {
                System.Web.HttpPostedFileBase file =   Request.Files[0];

                System.Drawing.Image img  = System.Drawing.Image.FromStream(file.InputStream);
                string fileName           = file.FileName;
                string ext                = Path.GetExtension(fileName);

                img.Save(Path.Combine(productFolder, myName + ext));
                Session["MyOriFileName"] = myName + ext;
            }

            return Content("1");
        }
        public ActionResult GetOriImageName()
        {
            if (Session["MyOriFileName"].IsNotNullOrEmpty())
            {
                string name =Session["MyOriFileName"].ToString();
                Session.Remove("MyOriFileName");
                return Content(name);
            }
            return Content("");
        }
    }
}
