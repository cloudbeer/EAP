using System;
using System.Collections;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z01Beetle.Entity.Helper;
using Zippy.Data.Collections;

namespace Z01Beetle.Controllers
{

    public class Z01CustomerPersonController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateZ01CustomerPerson(Z01CustomerPerson entity)
        {
            if (!string.IsNullOrEmpty(entity.Name) && entity.Name.Length > 200)
                ModelState.AddModelError("Name string length error", "名字：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Nickname) && entity.Nickname.Length > 200)
                ModelState.AddModelError("Nickname string length error", "昵称：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Email) && entity.Email.Length > 500)
                ModelState.AddModelError("Email string length error", "Email：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.QQ) && entity.QQ.Length > 50)
                ModelState.AddModelError("QQ string length error", "QQ：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.MSN) && entity.MSN.Length > 300)
                ModelState.AddModelError("MSN string length error", "MSN：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Skype) && entity.Skype.Length > 300)
                ModelState.AddModelError("Skype string length error", "Skype：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.WangWang) && entity.WangWang.Length > 300)
                ModelState.AddModelError("WangWang string length error", "旺旺：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Fetion) && entity.Fetion.Length > 300)
                ModelState.AddModelError("Fetion string length error", "飞信：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.YahooIM) && entity.YahooIM.Length > 300)
                ModelState.AddModelError("YahooIM string length error", "Yahoo IM：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.OtherIM) && entity.OtherIM.Length > 3000)
                ModelState.AddModelError("OtherIM string length error", "其他IM：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Tel1) && entity.Tel1.Length > 50)
                ModelState.AddModelError("Tel1 string length error", "Tel1：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Tel2) && entity.Tel2.Length > 50)
                ModelState.AddModelError("Tel2 string length error", "Tel2：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Avatar) && entity.Avatar.Length > 200)
                ModelState.AddModelError("Avatar string length error", "头像：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Address) && entity.Address.Length > 500)
                ModelState.AddModelError("Address string length error", "地址：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.PostCode) && entity.PostCode.Length > 30)
                ModelState.AddModelError("PostCode string length error", "邮编：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qName, string qNickname, long? qCustomerID, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?qCustomerID=" + qCustomerID + "&ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?PageSize=" + PageSize + "&qCustomerID=" + qCustomerID) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            if (qCustomerID.HasValue)
            {
                var cus = db.FindUnique<Z01Customer>("CustomerID=@CustomerID", db.CreateParameter("CustomerID", qCustomerID));
                sbMenu.AppendLine("当前客户：" + cus.Title);
                sbMenu.AppendLine(" | <a href='/Z30Customer/'>返回客户列表</a>");
            }
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qName", qName);
            hs.Add("qNickname", qNickname);
            hs.Add("qCustomerID", qCustomerID);

            PaginatedList<Z01CustomerPerson> result = Z01CustomerPersonHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult AjaxDetail(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Z01CustomerPerson entity = Z01CustomerPersonHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Delete<Z01CustomerPerson>(id) > 0)
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
            string sql = "PersonID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z01CustomerPerson>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Int64? id, long? qCustomerID, string act)
        {
            Z01CustomerPerson entity = null;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看客户联系人信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改客户联系人信息";
                }
                entity = Z01CustomerPersonHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增客户联系人";
                entity = new Z01CustomerPerson();
            }

            ViewData["qCustomerID"] = qCustomerID;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z01CustomerPerson entity)
        {
            entity.PersonID = id;


            ValidateZ01CustomerPerson(entity);
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
                    entity.PersonID = null;
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
