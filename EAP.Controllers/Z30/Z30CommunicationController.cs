using System;
using System.Collections;
using System.Web.Mvc;
using Z30CRM.Entity;
using Z30CRM.Entity.Helper;
using Zippy.Data.Collections;

namespace Z30CRM.Controllers
{
    public class Z30CommunicationController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateZ30Communication(Z30Communication entity)
        {
            if (!string.IsNullOrEmpty(entity.Content) && entity.Content.Length > 2000)
                ModelState.AddModelError("Content string length error", "沟通内容：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(Int64? id, int? PageIndex, int? PageSize, string qContent, Int32? qWish, Int32? qSuccessRatio, DateTime? qCreateDateStart, DateTime? qCreateDateEnd, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) == Zippy.SaaS.Entity.CRUD.Read)
                sbMenu.AppendLine("<a href='javascript:;' class='btn list img' id='search'><i class='icon i_search'></i>查询<b></b></a>");
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) == Zippy.SaaS.Entity.CRUD.Delete)
                sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bDelete'><i class='icon i_delete'></i>删除<b></b></a>");
            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewBag.TopMenu = sbMenu.ToString();

            ViewBag.db = db;
            ViewBag.PageSize = PageSize;
            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qCreator", _user.UserID); //我的客户
            hs.Add("qCustomerID", id);
            hs.Add("qContent", qContent);
            hs.Add("qWish", qWish);
            hs.Add("qSuccessRatio", qSuccessRatio);
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);

            PaginatedList<Z30Communication> result = Z30CommunicationHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Z30Communication entity = Z30CommunicationHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Delete<Z30Communication>(id) > 0)
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
            string sql = "CommunicationID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z30Communication>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Int64? id, string act, long? customerID)
        {
            Z30Communication entity = null;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewBag.VTitle = "查看沟通记录信息";
                    ViewBag.IsDetail = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewBag.VTitle = "修改沟通记录信息";
                }
                entity = Z30CommunicationHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewBag.VTitle = "新增沟通记录";
                entity = new Z30Communication();
                entity.CustomerID = customerID;
            }
            if (customerID.HasValue)
            {
                var persons = db.Take<Z01Beetle.Entity.Z01CustomerPerson>("CustomerID=@CustomerID", db.CreateParameter("CustomerID", customerID));
                ViewBag.Persons = persons;
            }

            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z30Communication entity, long customerID)
        {
            entity.CommunicationID = id;
            entity.CustomerID = customerID;
            ValidateZ30Communication(entity);
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
                    entity.CommunicationID = null;
                    entity.TenantID = _tenant.TenantID;
                    entity.Creator = _user.UserID;
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

        #region 快速插入
        public ActionResult QuickInsert()
        {
            return View(new EAP.Logic.Z30.QuickCommunication());
        }

        protected void ValidateQCommunication(EAP.Logic.Z30.QuickCommunication entity)
        {
            if (entity.CorpName.IsNullOrEmpty())
                ModelState.AddModelError("CorpName required", "必须填写公司名称");
            if (entity.LinkName.IsNullOrEmpty())
                ModelState.AddModelError("LinkName required", "必须填写联系人，如果不清楚联系人，请填写“先生”或“女士”");
            if (!entity.VisitWay.HasValue)
                ModelState.AddModelError("VisitWay required", "拜访方式必须填写");
            if (!entity.SuccessRatio.HasValue)
                ModelState.AddModelError("SuccessRatio required", "成功概率必须填写");
            if (!string.IsNullOrEmpty(entity.Content) && entity.Content.Length > 2000)
                ModelState.AddModelError("Content string length error", "沟通内容：填写的内容太多");

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult QuickInsert(EAP.Logic.Z30.QuickCommunication qCom)
        {
            //string buyerTypes = Request["BuyerPersonas"];
            //int intBT = buyerTypes.ToEnumInt32();
            //qCom.BuyerPersonas = intBT;

            ValidateQCommunication(qCom);
            if (!ModelState.IsValid)
                return View(qCom);
            qCom.Save(_tenant.TenantID.Value, _user.UserID.Value);
            return Redirect("/Z30Customer/");
        }
        #endregion

    }
}
