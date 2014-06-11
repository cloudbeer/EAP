using System;
using System.Collections;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z01Beetle.Entity.Helper;
using Zippy.Data.Collections;

namespace Z01Beetle.Controllers
{

    public class Z01BankController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateZ01Bank(Z01Bank entity)
        {
            if (string.IsNullOrEmpty(entity.Title))
                ModelState.AddModelError("Title required", "标题：必须填写");
            else if (entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "标题：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Brief) && entity.Brief.Length > 20)
                ModelState.AddModelError("Brief string length error", "简称：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Account) && entity.Account.Length > 50)
                ModelState.AddModelError("Account string length error", "帐号：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Address) && entity.Address.Length > 300)
                ModelState.AddModelError("Address string length error", "地址：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Contact) && entity.Contact.Length > 50)
                ModelState.AddModelError("Contact string length error", "联系人：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Tel) && entity.Tel.Length > 20)
                ModelState.AddModelError("Tel string length error", "电话：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Fax) && entity.Fax.Length > 20)
                ModelState.AddModelError("Fax string length error", "传真：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Url) && entity.Url.Length > 300)
                ModelState.AddModelError("Url string length error", "银行网址：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qTitle, string qAccount, int? orderCol)
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
            hs.Add("qTitle", qTitle);
            hs.Add("qAccount", qAccount);

            PaginatedList<Z01Bank> result = Z01BankHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Z01Bank entity = Z01BankHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Delete<Z01Bank>(id) > 0)
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
            string sql = "BankID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z01Bank>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Int64? id, string act)
        {
            Z01Bank entity = null;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看银行信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改银行信息";
                }
                entity = Z01BankHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增银行";
                entity = new Z01Bank();
            }

            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z01Bank entity)
        {
            entity.BankID = id;


            ValidateZ01Bank(entity);
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
                    entity.BankID = null;
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
            System.Collections.Generic. List<Z01Bank> result = db.Take<Z01Bank>(" [TenantID] = @TenantID order by DisplayOrder", db.CreateParameter("TenantID", _tenant.TenantID));
            return View(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Sort(FormCollection collection)
        {
            string oriOrders = collection["result"];
            oriOrders = oriOrders.Replace("d_", "");
            string[] results = oriOrders.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < results.Length; i++)
            {
                sb.AppendFormat("update Z01Bank set DisplayOrder={0} where BankID={1}; ", i, results[i].ToInt64());
            }
            db.ExecuteNonQuery(sb.ToString());
            return Content("1");

        }
        #endregion
    }
}
