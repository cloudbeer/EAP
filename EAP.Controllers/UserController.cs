using System;
using System.Collections;
using System.Web.Mvc;
using EAP.Bus.Entity;
using EAP.Bus.Entity.Helper;
using Zippy.Data.Collections;
using System.Linq;
using System.Collections.Generic;

namespace EAP.Bus.Controllers
{

    public class UserController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateUser(User entity)
        {
            if (string.IsNullOrEmpty(entity.UserName))
                ModelState.AddModelError("UserName required", "用户名：必须填写");
            else if (entity.UserName.Length > 100)
                ModelState.AddModelError("UserName string length error", "用户名：填写的内容太多");
            if (string.IsNullOrEmpty(entity.Email))
                ModelState.AddModelError("Email required", "Email：必须填写");
            else if (entity.Email.Length > 500)
                ModelState.AddModelError("Email string length error", "Email：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Name) && entity.Name.Length > 200)
                ModelState.AddModelError("Name string length error", "名字：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Nickname) && entity.Nickname.Length > 200)
                ModelState.AddModelError("Nickname string length error", "昵称：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Address) && entity.Address.Length > 500)
                ModelState.AddModelError("Address string length error", "地址：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.PostCode) && entity.PostCode.Length > 30)
                ModelState.AddModelError("PostCode string length error", "邮编：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.FullName) && entity.FullName.Length > 200)
                ModelState.AddModelError("FullName string length error", "全名：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Avatar) && entity.Avatar.Length > 200)
                ModelState.AddModelError("Avatar string length error", "头像：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.MobileID1) && entity.MobileID1.Length > 30)
                ModelState.AddModelError("MobileID1 string length error", "手机号码：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.MobileID2) && entity.MobileID2.Length > 30)
                ModelState.AddModelError("MobileID2 string length error", "手机号码2：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Tel1) && entity.Tel1.Length > 50)
                ModelState.AddModelError("Tel1 string length error", "Tel1：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Tel2) && entity.Tel2.Length > 50)
                ModelState.AddModelError("Tel2 string length error", "Tel2：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qUserName, string qEmail, string qName, 
            string qNickname, string qMobileID1, string qMobileID2, Guid? qGroupID, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            ViewData["ParentIDOptions"] = GroupHelper.GetParentIDEntitiesHtmlOption(db, _tenant.TenantID.Value, null, null);

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
            hs.Add("qUserName", qUserName);
            hs.Add("qEmail", qEmail);
            hs.Add("qName", qName);
            hs.Add("qNickname", qNickname);
            hs.Add("qMobileID1", qMobileID1);
            hs.Add("qMobileID2", qMobileID2);
            hs.Add("qGroupID", qGroupID);

            PaginatedList<Group> groupRes = GroupHelper.Query(db, _tenant.TenantID.Value, 2000, 1, null, null);
            ViewData.Add("xgroup", groupRes);
            ViewData.Add("xgroupid", qGroupID);


            PaginatedList<User> result = UserHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Guid id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            User entity = UserHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Guid id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Delete<User>(id) > 0)
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
            string sql = "UserID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<User>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Guid? id, string act)
        {
            User entity = null;
            if (id.HasValue && id != Guid.Empty)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看用户信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改用户信息";
                }
                entity = UserHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增用户";
                entity = new User();
            }
            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;

            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Guid? id, User entity)
        {
            entity.UserID = id;
            string formUserType = Request.Form["UserType"];
            entity.UserType = formUserType.ToEnumInt32();


            ValidateUser(entity);
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
                    ///创建新用户，密码默认为 liwill
                    entity.Password = "touch".Md6();
                    entity.UserID = Guid.NewGuid();
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

        /// <summary>
        /// 列出用户的角色
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        public ActionResult ListRole(Guid id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return Content("401");
            List<Role> allRoles = EAP.Logic.Bus.Helper.GetRoles(db, _tenant.TenantID.Value);
            List<UserRole> myRoles = db.Take<UserRole>("UserID=@UserID", db.CreateParameter("UserID", id));
            ViewData["myRoles"] = myRoles;
            ViewData["UserID"] = id;
            return View(allRoles);
        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="id">userid</param>
        /// <param name="role">roleid</param>
        /// <returns></returns>
        public ActionResult SetRole(Guid id, Guid roleid)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Check) != Zippy.SaaS.Entity.CRUD.Check) return Content("401");
            UserHelper.SetRole(id, roleid, _tenant.TenantID, _user.UserID, db);
            return Content("1");

        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public ActionResult RemoveRole(Guid id, Guid roleid)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Check) != Zippy.SaaS.Entity.CRUD.Check) return Content("401");
            UserHelper.RemoveRole(id, roleid, _tenant.TenantID, db);

            return Content("1");
        }

        #region 分类
        public ActionResult SetGroup(Guid id, string qTitle)
        {

            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);

            PaginatedList<Group> result = GroupHelper.Query(db, _tenant.TenantID.Value, 2000, 1, hs, null);

            List<UserGroup> myGroups = db.Take<UserGroup>("UserID=@UserID and TenantID=@TenantID",
                    db.CreateParameter("UserID", id), db.CreateParameter("TenantID", _tenant.TenantID));

            ViewData["UserID"] = id;
            ViewData["MyGroups"] = myGroups;
            return View(result);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SetGroup(Guid id, Guid groupID, bool isAdd)
        {
            if (isAdd)
            {
                UserGroup pic = new UserGroup();
                pic.UserID = id;
                pic.GroupID = groupID;
                pic.TenantID = _tenant.TenantID;
                pic.Creator = _user.UserID;
                db.Insert(pic);
            }
            else
            {
                db.Delete<UserGroup>("UserID=@UserID and CategoryID=@CategoryID and TenantID=@TenantID",
                    db.CreateParameter("UserID", id), db.CreateParameter("GroupID", groupID), db.CreateParameter("TenantID", _tenant.TenantID));
            }
            return Content("1");
        }
        #endregion

    }
}
