using System;
using System.Collections;
using System.Web.Mvc;
using EAP.Bus.Entity;
using EAP.Bus.Entity.Helper;
using Zippy.Data.Collections;
using System.Collections.Generic;

namespace EAP.Bus.Controllers
{

    public class RoleController : EAP.Logic.__UserController
    {
        #region 验证
        protected void ValidateRole(Role entity)
        {
            if (!string.IsNullOrEmpty(entity.Title) && entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Content) && entity.Content.Length > 2000)
                ModelState.AddModelError("Content string length error", "填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qTitle, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
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
            ViewData.Add("PageSize", PageSize ?? 10);
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);

            PaginatedList<Role> result = RoleHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Guid id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Role entity = RoleHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Guid id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Exists<UserRole>("RoleID=@RoleID", db.CreateParameter("RoleID", id))) return Content("701");

            System.Web.HttpContext.Current.Cache.Remove("Roles_" + _tenant.TenantID);
            if (db.Delete<Role>(id) > 0)
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
            string sql = "RoleID in (" + ids + "0)";

            if (db.Exists<UserRole>(sql)) return Content("701");


            System.Web.HttpContext.Current.Cache.Remove("Roles_" + _tenant.TenantID);
            if (db.Delete<Role>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Guid? id, string act)
        {
            Role entity = null;
            if (id.HasValue && id != Guid.Empty)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看角色信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改角色信息";
                }
                entity = RoleHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增角色";
                entity = new Role();
            }

            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Guid? id, Role entity)
        {
            entity.RoleID = id;

            ValidateRole(entity);
            if (!ModelState.IsValid)
                return View(entity);
            try
            {
                if (id.HasValue && id != Guid.Empty)
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    db.Update(entity);
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                    entity.RoleID = Guid.NewGuid();
                    entity.TenantID = _tenant.TenantID;
                    db.Insert(entity);
                }

                System.Web.HttpContext.Current.Cache.Remove("Roles_" + _tenant.TenantID);

                return Return();
            }
            catch
            {
                return View(entity);
            }
        }
        #endregion

        //id 角色id
        public ActionResult GrantList(Guid id, int? PageIndex, int? PageSize, string qTitle, string qUrl, string qFlag, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");


            ViewData.Add("RoleID", id);
            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 2000);


            List<RolePermission> myPers = RoleHelper.GetPermissions(id, _tenant.TenantID, db);
            ViewData.Add("MyPermissions", myPers);

            int currentPageSize = PageSize ?? 2000;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qUrl", qUrl);
            hs.Add("qFlag", qFlag);

            PaginatedList<Permission> result = PermissionHelper.Query(db, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">roleid</param>
        /// <param name="pid">permissionid</param>
        /// <param name="pt">permissiontype</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetPermission(Guid id, long pid, int pt)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Check) != Zippy.SaaS.Entity.CRUD.Check) return RedirectToAction("NoPermission", "Error");
            RoleHelper.SetPermission(id, pid, pt, _tenant.TenantID, _user.UserID, db);
            return Content("1");

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemovePermission(Guid id, long pid, int pt)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Check) != Zippy.SaaS.Entity.CRUD.Check) return RedirectToAction("NoPermission", "Error");
            RoleHelper.RemovePermission(id, pid, pt, _tenant.TenantID, _user.UserID, db);
            return Content("1");

        }

    }
}
