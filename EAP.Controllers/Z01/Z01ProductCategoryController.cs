using System;
using System.Collections;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z01Beetle.Entity.Helper;
using Zippy.Data.Collections;
using System.Collections.Generic;

namespace Z01Beetle.Controllers
{

    public class Z01ProductCategoryController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateZ01ProductCategory(Z01ProductCategory entity)
        {
            if (string.IsNullOrEmpty(entity.Title))
                ModelState.AddModelError("Title required", "标题：必须填写");
            else if (entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "标题：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Code) && entity.Code.Length > 30)
                ModelState.AddModelError("Code string length error", "编号：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Content) && entity.Content.Length > 2000)
                ModelState.AddModelError("Content string length error", "描述：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index()
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/")  + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
       
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) == Zippy.SaaS.Entity.CRUD.Update)
                sbMenu.AppendLine("<a href='/Z01ProductCategory/Sort/0' class='btn img' id='bDelete'><i class='icon i_sort'></i>排序<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            var categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);
            return View(categories);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Z01ProductCategory entity = Z01ProductCategoryHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");

            if (db.Exists<Z01ProductCategory>("ParentID=" + id))  //已经有分类引用
            {
                return this.Content("0");
            }

            if (db.Exists<Z01ProductInCategory>("CategoryID=" + id))  //已经有商品绑定
            {
                return this.Content("0");
            }

            if (db.Delete<Z01ProductCategory>(id) > 0)
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
            string sql = "CategoryID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z01ProductCategory>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Int64? id, long? xParentID, string act)
        {
            Z01ProductCategory entity = null;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看产品分类信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改产品分类信息";
                }
                entity = Z01ProductCategoryHelper.Create(db, id.Value);
                ViewData["ParentIDOptions"] = Z01ProductCategoryHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, entity.ParentID, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增产品分类";
                entity = new Z01ProductCategory();
                ViewData["ParentIDOptions"] = Z01ProductCategoryHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, xParentID, 0);
            }

            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;

            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z01ProductCategory entity)
        {
            EAP.Logic.Z01.Helper.ClearProductCategories();
            entity.CategoryID = id;


            ValidateZ01ProductCategory(entity);
            if (!ModelState.IsValid)
                return View(entity);
            try
            {
                if (id.HasValue && id > 0)
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    if (id == entity.ParentID)
                    {
                        ViewData["ParentIDOptions"] = Z01ProductCategoryHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, null, id.Value);
                        ModelState.AddModelError("ParentID Error", "父分类不可以是自己。");
                    }
                    if (!ModelState.IsValid)
                        return View(entity);
                    db.Update(entity);
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                    entity.CategoryID = null;
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


        #region 排序
        public ActionResult Sort(long id)
        {
            Hashtable hs = new Hashtable();
            hs.Add("qParentID", id);
            System.Collections.Generic. List<Z01ProductCategory> result = db.Take<Z01ProductCategory>(" [ParentID] = @ParentID order by DisplayOrder", db.CreateParameter("ParentID", id));
            return View(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Sort(FormCollection collection)
        {
            EAP.Logic.Z01.Helper.ClearProductCategories();
            string oriOrders = collection["result"];
            oriOrders = oriOrders.Replace("d_", "");
            string[] results = oriOrders.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < results.Length; i++)
            {
                sb.AppendFormat("update Z01ProductCategory set DisplayOrder={0} where CategoryID={1}; ", i, results[i].ToInt64());
            }
            db.ExecuteNonQuery(sb.ToString());
            System.Web.HttpContext.Current.Cache.Remove("ProductCategorys_" + _tenant.TenantID);
            return Content("1");

        }

        #endregion
    }
}
