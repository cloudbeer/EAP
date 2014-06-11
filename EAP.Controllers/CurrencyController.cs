using System;
using System.Collections;
using System.Web.Mvc;
using EAP.Bus.Entity;
using EAP.Bus.Entity.Helper;
using Zippy.Data.Collections;

namespace EAP.Bus.Controllers
{

    public class CurrencyController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateCurrency(Currency entity)
        {
            if (string.IsNullOrEmpty(entity.ID))
                ModelState.AddModelError("ID required", "英文简称：必须填写");
            else if (entity.ID.Length > 3)
                ModelState.AddModelError("ID string length error", "英文简称：填写的内容太多");
            if (string.IsNullOrEmpty(entity.Title))
                ModelState.AddModelError("Title required", "标题：必须填写");
            else if (entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "标题：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qID, string qTitle, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?PageSize=" + PageSize) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Update) == Zippy.SaaS.Entity.CRUD.Update)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Sort/0' class='btn img' id='bDelete'><i class='icon i_sort'></i>排序<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qID", qID);
            hs.Add("qTitle", qTitle);
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);

            PaginatedList<Currency> result = CurrencyHelper.Query(db, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.String id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Currency entity = CurrencyHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.String id)
        {
            System.Web.HttpContext.Current.Cache.Remove("Currencies");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Delete<Currency>(id) > 0)
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
            System.Web.HttpContext.Current.Cache.Remove("Currencies");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            string sql = "ID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Currency>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.String id, string act)
        {
            Currency entity = null;
            if (id.IsNotNullOrEmpty())
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看币种信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改币种信息";
                }
                entity = CurrencyHelper.Create(db, id);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增币种";
                entity = new Currency();
            }

            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(Currency entity)
        {
            ValidateCurrency(entity);
            if (!ModelState.IsValid)
                return View(entity);

            System.Web.HttpContext.Current.Cache.Remove("Currencies");
            try
            {
                if (db.Exists<Currency>("ID=@ID", db.CreateParameter("ID", entity.ID)))
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    
                    db.Update(entity);
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                    
                    db.Insert(entity);
                }

                return Return();
            }
            catch(Exception ex)
            {
                return View(entity);
            }
        }
        #endregion

        #region 排序
        public ActionResult Sort(long id)
        {
            Hashtable hs = new Hashtable();
            System.Collections.Generic. List<Currency> result = db.Take<Currency>(" 1 = 1 order by DisplayOrder", db.CreateParameter("ParentID", id));
            return View(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Sort(FormCollection collection)
        {
            string oriOrders = collection["result"];
            string[] results = oriOrders.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < results.Length; i++)
            {
                sb.AppendFormat("update [Currency] set DisplayOrder={0} where ID='{1}'; ", i, results[i]);
            }
            db.ExecuteNonQuery(sb.ToString());
            System.Web.HttpContext.Current.Cache.Remove("Currencies");
            return Content("1");

        }
        #endregion
    }
}
