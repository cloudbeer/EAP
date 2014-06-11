using System;
using System.Collections;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z01Beetle.Entity.Helper;
using Zippy.Data.Collections;
using System.Collections.Generic;

namespace Z01Beetle.Controllers
{

    public class Z01CustomerController : EAP.Logic.__UserController
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
        public ActionResult Index(int? PageIndex, int? PageSize, string qTitle, string qEmail, long? qCateID, int? qCustomerType, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?PageSize=" + PageSize) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            qCustomerType = qCustomerType ??0;
            ViewData.Add("db", db);
            ViewBag.PageSize = PageSize ?? 10;
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qEmail", qEmail);
            hs.Add("qCateID", qCateID);
            hs.Add("qCustomerType", qCustomerType);

            PaginatedList<Z01Customer> result = Z01CustomerHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;

            PaginatedList<Z01CustomerCategory> cateRes = Z01CustomerCategoryHelper.Query(db, _tenant.TenantID.Value, 2000, 1, null, null);
            ViewData.Add("xcate", cateRes);
            ViewData.Add("xcateid", qCateID);

            //角色为销售的用户
            var salesTable = db.ExecuteDataSet("select * from [V_UserRole] where [Title]=@Title", db.CreateParameter("Title", "销售")).Tables[0];
            ViewBag.Sales = salesTable;

            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Z01Customer entity = Z01CustomerHelper.Create(db, id);
            ViewBag.act = "detail";
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");

            if (db.Delete<Z01Customer>(id) > 0)
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
            string sql = "CustomerID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z01Customer>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情


        public ActionResult SaveAjax(string c_title, string c_tel1, string c_tel2)
        {
            EAP.Logic.DictResponse res = new EAP.Logic.DictResponse();
            Z01Customer data = new Z01Customer();
            data.Title = c_title;
            data.Tel1 = c_tel1;
            data.Tel2 = c_tel2;
            data.CustomerType = (int)EAP.Logic.Z01.CustomerTyps.Customer;
            data.TenantID = _tenant.TenantID;
            data.Creator = _user.UserID;
            var bid = db.Insert(data);
            res._state = true;
            res._data.Add("id", bid);
            return Content(res.ToJson());

        }

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
        #endregion

        #region 分类
        public ActionResult SetCategory(long id, string qTitle)
        {

            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            string returnUrl = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);

            PaginatedList<Z01CustomerCategory> result = Z01CustomerCategoryHelper.Query(db, _tenant.TenantID.Value, 2000, 1, hs, null);

            List<Z01CustomerInCategory> myCategories = db.Take<Z01CustomerInCategory>("CustomerID=@CustomerID and TenantID=@TenantID",
                    db.CreateParameter("CustomerID", id), db.CreateParameter("TenantID", _tenant.TenantID));

            ViewData["CustomerID"] = id;
            ViewData["MyCategories"] = myCategories;
            return View(result);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SetCategory(long id, int categoryID, bool isAdd)
        {
            if (isAdd)
            {
                Z01CustomerInCategory pic = new Z01CustomerInCategory();
                pic.CustomerID = id;
                pic.CategoryID = categoryID;
                pic.TenantID = _tenant.TenantID;
                pic.Creator = _user.UserID;
                db.Insert(pic);
            }
            else
            {
                db.Delete<Z01ProductInCategory>("CustomerID=@CustomerID and CategoryID=@CategoryID and TenantID=@TenantID",
                    db.CreateParameter("CustomerID", id), db.CreateParameter("CategoryID", categoryID), db.CreateParameter("TenantID", _tenant.TenantID));
            }
            return Content("1");
        }
        #endregion

        /// <summary>
        /// 给客户分配管理员
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="cusid">客户ID</param>
        /// <returns></returns>
        public ActionResult SetPrincipal(Guid id, long cusid)
        {
            Z01Customer cus = new Z01Customer();
            cus.CustomerID = cusid;
            cus.Principal = id;

            db.Update(cus);

            return Content("1");
        }
    }
}
