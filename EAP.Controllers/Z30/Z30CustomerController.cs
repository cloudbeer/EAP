using System;
using System.Collections;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z01Beetle.Entity.Helper;
using Zippy.Data.Collections;

namespace EAP.Controllers.Z30
{
    /// <summary>
    /// 这个和 Z01Customer相同，但体现的是个人，专门为 CRM 使用的界面
    /// </summary>
    public class Z30CustomerController : EAP.Logic.__UserController
    {
        #region 验证
        protected void ValidateZ01Customer(Z01Customer entity)
        {
            if (string.IsNullOrEmpty(entity.Title))
                ModelState.AddModelError("Title required", "标题：必须填写");
            else if (entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "标题：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Tel1) && entity.Tel1.Length > 50)
                ModelState.AddModelError("Tel1 string length error", "电话：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Fax) && entity.Fax.Length > 50)
                ModelState.AddModelError("Fax string length error", "传真：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Email) && entity.Email.Length > 500)
                ModelState.AddModelError("Email string length error", "Email：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.PostCode) && entity.PostCode.Length > 30)
                ModelState.AddModelError("PostCode string length error", "邮编：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Address) && entity.Address.Length > 500)
                ModelState.AddModelError("Address string length error", "地址：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Avatar) && entity.Avatar.Length > 200)
                ModelState.AddModelError("Avatar string length error", "头像：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.IM) && entity.IM.Length > 3000)
                ModelState.AddModelError("IM string length error", "即时通信：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Url) && entity.Url.Length > 500)
                ModelState.AddModelError("Url string length error", "网址：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? pageindex, int? pagesize, string qTitle, string qEmail, long? qCateID, int? qSiteStatus, int? qSuccessRatio, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/Z01Customer/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?pagesize=" + pagesize) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewBag.TopMenu = sbMenu.ToString();
            ViewBag.db = db;
            ViewBag.PageSize = pagesize ?? 10;
            int currentPageSize = pagesize ?? 10;
            int currentPageIndex = pageindex ?? 1;


            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qEmail", qEmail);
            hs.Add("qCateID", qCateID);
            hs.Add("qSiteStatus", qSiteStatus);
            hs.Add("qSuccessRatio", qSuccessRatio);
            hs.Add("qPrincipal", _user.UserID);

            PaginatedList<Z01Customer> result = Z01CustomerHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;

            PaginatedList<Z01CustomerCategory> cateRes = Z01CustomerCategoryHelper.Query(db, _tenant.TenantID.Value, 2000, 1, null, null);
            ViewBag.xcate = cateRes;
            ViewBag.xcateid = qCateID;

            return View(result);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(System.Int64? id, string act)
        {
            Z01Customer entity = null;
            ViewBag.act = act;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看客户信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改客户信息";
                }
                entity = Z01CustomerHelper.Create(db, id.Value);
                if (entity.Principal != _user.UserID) return RedirectToAction("NoPermission", "Error");
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增客户";
                entity = new Z01Customer();
            }

            string returnUrl = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z01Customer entity)
        {
            entity.CustomerID = id;
            string formCustomerType = Request.Form["CustomerType"];
            entity.CustomerType = formCustomerType.ToEnumInt32();


            ValidateZ01Customer(entity);
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
                    entity.CustomerID = null;
                    if (!entity.Principal.HasValue)
                        entity.Principal = _user.UserID;
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

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return RedirectToAction("NoPermission", "Error");

            if (db.Delete<Z01Customer>("CustomerID=@CustomerID and Principal=@Principal", db.CreateParameter("CustomerID", id), db.CreateParameter("Principal", _user.UserID)) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }
        #endregion

        public ActionResult Detail(long id)
        {
            var Customer = db.FindUnique<Z01Beetle.Entity.Z01Customer>(id);
            if (Customer == null)
            {
                return Content("无效的用户");
            }
            //if (Customer.Principal != _user.UserID)
            //    return Content("您对此用户没有权限");




            return View(Customer);
        }
    }
}
