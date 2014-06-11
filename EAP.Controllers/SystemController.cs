using System;
using System.Web.Mvc;
using System.Linq;

namespace EAP.Controllers
{
    public class SystemController : EAP.Logic.__UserController
    {
        #region 数据的备份和还原
        public ActionResult Database()
        {
            string fileDir = Server.MapPath("~/db.bak/");
            var files = System.IO.Directory.GetFiles(fileDir, "*.zip");
            ViewBag.files = files;
            return View();
        }

        public ActionResult DeleteDBBak(string fileName)
        {
            string fileDir = Server.MapPath("~/db.bak/");
            System.IO.File.Delete(System.IO.Path.Combine(fileDir, fileName));
            EAP.Logic.DictResponse res = new Logic.DictResponse();
            res._state = true;
            return Content(res.ToJson());
        }
        public ActionResult BackupDB(string db)
        {
            if (db.IsNullOrEmpty()) db = System.Configuration.ConfigurationManager.AppSettings["DBName"];
            if (db.IsNullOrEmpty()) db = "EAP";
            string fileDir = Server.MapPath("~/db.bak/");
            string fileName = db + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string filePath = System.IO.Path.Combine(fileDir, fileName + ".bak");
            string sqlStr = "backup database " + db + " to disk='" + filePath + "' with format";
            Zippy.Data.StaticDB.DB.ExecuteNonQuery(sqlStr);
            ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
            fz.CreateZip(System.IO.Path.Combine(fileDir, fileName + ".zip"), fileDir, false, fileName + ".bak");
            System.IO.File.Delete(filePath);
            return Redirect("/System/Database/");
        }

        public ActionResult RestoreDB(string db, string zipfilePath)
        {
            string bakPath = "";
            ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
            // fz.ExtractZip(zipfilePath, )
            string sqlStr = "restore database " + db + " from disk='" + bakPath + "' with format";
            Zippy.Data.StaticDB.DB.ExecuteNonQuery(sqlStr);

            return View();
        }
        #endregion

    }
}
