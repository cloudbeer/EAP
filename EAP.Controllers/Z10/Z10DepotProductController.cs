using System;
using System.Collections;
using System.Web.Mvc;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using Zippy.Data.Collections;

namespace Z10Cabbage.Controllers
{

    public class Z10DepotProductController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateZ10DepotProduct(Z10DepotProduct entity)
        {

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, Int64? qProductIDStart, Int64? qProductIDEnd, Int64? qDepotIDStart, Int64? qDepotIDEnd, int? orderCol)
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
            hs.Add("qProductIDStart", qProductIDStart);
            hs.Add("qProductIDEnd", qProductIDEnd);
            hs.Add("qDepotIDStart", qDepotIDStart);
            hs.Add("qDepotIDEnd", qDepotIDEnd);

            PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> result = Z10DepotProductHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;
            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Z10DepotProduct entity = Z10DepotProductHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Delete<Z10DepotProduct>(id) > 0)
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
            string sql = "DepotProductID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z10DepotProduct>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Int64? id, string act)
        {
            Z10DepotProduct entity = null;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看库存产品信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改库存产品信息";
                }
                entity = Z10DepotProductHelper.Create(db, id.Value);
                ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, entity.DepotID, db);
                ViewData["ProductTitle"] = db.FindUnique<Z01Beetle.Entity.Z01Product>("ProductID=@ProductID and TenantID=@TenantID",
                    "Title", db.CreateParameter("ProductID", entity.ProductID), db.CreateParameter("TenantID", _tenant.TenantID.Value));
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增库存产品";
                entity = new Z10DepotProduct();
                ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            }

            string returnUrl                         = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z10DepotProduct entity)
        {
            entity.DepotProductID = id;


            ValidateZ10DepotProduct(entity);
            if (!ModelState.IsValid)
                return View(entity);
            try
            {
                if (id.HasValue && id > 0)
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    //仓库和商品不允许修改
                    entity.DepotID = null;
                    entity.ProductID = null;
                    db.Update(entity);
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                    if (db.Exists<Z10DepotProduct>("DepotID=@DepotID and ProductID=@ProductID and TenantID=@TenantID",
                        db.CreateParameter("DepotID", entity.DepotID), db.CreateParameter("ProductID", entity.ProductID), db.CreateParameter("TenantID", _tenant.TenantID.Value)))
                    {
                        ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, entity.DepotID, db);
                        ViewData["ProductTitle"] = db.FindUnique<Z01Beetle.Entity.Z01Product>("ProductID=@ProductID and TenantID=@TenantID",
                            "Title", db.CreateParameter("ProductID", entity.ProductID), db.CreateParameter("TenantID", _tenant.TenantID.Value));
                        ModelState.AddModelError("这个仓库已经有此种商品。", "这个仓库已经有此种商品。");
                        return View(entity);
                    }
                    
                    entity.DepotProductID = null;
                    
                    
                    
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
