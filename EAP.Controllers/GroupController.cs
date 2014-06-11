using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using EAP.Bus.Entity;
using EAP.Bus.Entity.Helper;
using Zippy.Data.Collections;

namespace EAP.Bus.Controllers
{

    public class GroupController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateGroup(Group entity)
        {
            if (string.IsNullOrEmpty(entity.Title))
                ModelState.AddModelError("Title required", "必须填写");
            else if (entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Code) && entity.Code.Length > 30)
                ModelState.AddModelError("Code string length error", "填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Content) && entity.Content.Length > 2000)
                ModelState.AddModelError("Content string length error", "填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qTitle, Guid? qParentID, Int32? qCategoryType, Int32? qCategoryStatus, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");


            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?PageSize=" + PageSize) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            //if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
            //    sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();


            ViewData["ParentIDOptions"] = GroupHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, null, null);
            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qParentID", qParentID);
            hs.Add("qCategoryType", qCategoryType);
            hs.Add("qCategoryStatus", qCategoryStatus);

            PaginatedList<Group> result = GroupHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Guid id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Group entity = GroupHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Guid id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return RedirectToAction("NoPermission", "Error");
            if (db.Delete<Group>(id) > 0)
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
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return RedirectToAction("NoPermission", "Error");
            string sql = "GroupID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Group>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Guid? id, Guid? xParentID, string act)
        {
            Group entity = null;
            if (id.HasValue && id != Guid.Empty)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看组信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改组信息";
                }
                entity = GroupHelper.Create(db, id.Value);
                ViewData["ParentIDOptions"] = GroupHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, entity.ParentID, id);

            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增组";
                entity = new Group();
                ViewData["ParentIDOptions"] = GroupHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, xParentID, null);
            }
            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;

            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Guid? id, Group entity)
        {
            string pid = Request.Form["ParentID"];
            if (pid.IsNotNullOrEmpty())
                entity.ParentID = new Guid(pid);

            entity.GroupID = id;
            string formCategoryType = Request.Form["CategoryType"];
            entity.CategoryType = formCategoryType.ToEnumInt32();
            ValidateGroup(entity);

            if (!ModelState.IsValid)
                return View(entity);


            try
            {
                if (id.HasValue && id != Guid.Empty)
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    //此处必须加入判断：不能是自己，也不能是自己的子孙。
                    if (id == entity.ParentID)
                    {
                        ViewData["ParentIDOptions"] = GroupHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, null, id);
                        ModelState.AddModelError("ParentID Error", "父分类不可以是自己。");
                    }
                    if (!ModelState.IsValid)
                        return View(entity);
                    db.Update(entity);
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                    entity.GroupID = Guid.NewGuid();
                    entity.TenantID = _tenant.TenantID;
                    db.Insert(entity);
                }

                return Return();
            }
            catch
            {
                return View(entity);
            }
        }
        #endregion
    }
}
