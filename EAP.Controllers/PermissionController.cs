using System;
using System.Collections;
using System.Web.Mvc;
using EAP.Bus.Entity;
using EAP.Bus.Entity.Helper;
using Zippy.Data.Collections;

namespace EAP.Bus.Controllers
{

    public class PermissionController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidatePermission(Permission entity)
        {
            if (!string.IsNullOrEmpty(entity.Title) && entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "标题：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Url) && entity.Url.Length > 300)
                ModelState.AddModelError("Url string length error", "链接地址：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Flag) && entity.Flag.Length > 100)
                ModelState.AddModelError("Flag string length error", "标识：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qTitle, string qUrl, string qFlag, int? orderCol)
        {
            ///todo: 控制权限

            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?PageSize=" + PageSize) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", 2000);
            int currentPageSize =  2000;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qUrl", qUrl);
            hs.Add("qFlag", qFlag);

            PaginatedList<Permission> result = PermissionHelper.Query(db, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Permission entity = PermissionHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            System.Web.HttpContext.Current.Cache.Remove("Permissions_" + _tenant.TenantID);
            if (db.Delete<Permission>(id) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        /// <summary>
        /// 批量删除，仅适用id为整形的处理。
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteBatch(string ids)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            string sql = "PermissionID in (" + ids + "0)";
            //return Content("1");
            System.Web.HttpContext.Current.Cache.Remove("Permissions_" + _tenant.TenantID);

            if (db.Delete<Permission>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Int64? id, string act)
        {
            Permission entity = null;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看权限表信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改权限表信息";
                }
                entity = PermissionHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增权限表";
                entity = new Permission();
            }

            ViewData["ParentIDOptions"] = PermissionHelper.GetParentIDEntitiesHtmlOption(db, entity.ParentID);
            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Permission entity)
        {
            System.Web.HttpContext.Current.Cache.Remove("Permissions_" + _tenant.TenantID);
            entity.PermissionID = id;
            string formPermissionType = Request.Form["PermissionType"];
            entity.PermissionType = formPermissionType.ToEnumInt32();
            ValidatePermission(entity);

            if (!ModelState.IsValid)
                return View(entity);
            try
            {
                if (id.HasValue && id > 0)
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    db.Update(entity);
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                    entity.PermissionID = null;
                    db.Insert(entity);
                }

                return Return();
            }
            catch (Exception ex)
            {
                return View(entity);
            }
        }
        #endregion

        #region 排序
        public ActionResult Sort(long id)
        {
            Hashtable hs = new Hashtable();
            hs.Add("qParentID", id);
            System.Collections.Generic. List<Permission> result = PermissionHelper.Query(db, id);
            return View(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Sort(FormCollection collection)
        {
            string oriOrders = collection["result"];
            oriOrders = oriOrders.Replace("d_","");
            string[] results = oriOrders.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < results.Length; i++)
            {
                sb.AppendFormat("update Permission set DisplayOrder={0} where PermissionID={1}; ", i, results[i].ToInt64());
            }
            db.ExecuteNonQuery(sb.ToString());
            System.Web.HttpContext.Current.Cache.Remove("Permissions_" + _tenant.TenantID);
            return Content("1");

        }

        #endregion
    }
}
