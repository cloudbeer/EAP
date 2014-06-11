using System;
using System.Collections;
using System.Web.Mvc;
using Z10Cabbage.Entity;
using Z10Cabbage.Entity.Helper;
using Zippy.Data.Collections;
using System.Collections.Generic;
using Z01Beetle.Entity;
using System.Linq;
using System.Text;

namespace EAP.Controllers.Z10
{
    public class Z10DepotController : EAP.Logic.__UserController
    {

        #region 验证
        protected void ValidateZ10Depot(Z10Depot entity)
        {
            if (!string.IsNullOrEmpty(entity.Title) && entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "长度不适合");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qTitle, int? orderCol)
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
            hs.Add("qTitle", qTitle);
            hs.Add("orderCol", orderCol);
            PaginatedList<Z10Depot> result = Z10DepotHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;


            return View(result);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除一个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return RedirectToAction("NoPermission", "Error");
            if (db.Delete<Z10Depot>(id) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteBatch(string ids)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return RedirectToAction("NoPermission", "Error");
            string sql = "DepotID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z10Depot>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增

        public ActionResult Edit(System.Int64? id, string act)
        {
            Z10Depot entity = null;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    ViewData["VTitle"] = "查看仓库信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改仓库信息";
                }
                entity = Z10DepotHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增仓库";
                entity = new Z10Depot();
            }
            string returnUrl = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;

            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z10Depot entity)
        {
            entity.DepotID = id;
            ValidateZ10Depot(entity);
            if (!ModelState.IsValid)
                return View(entity);
            entity.TenantID = _tenant.TenantID;

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
                    entity.DepotID = null;
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

        public ActionResult Init0(int? PageIndex, int? PageSize, long? qDepot, string qTitle, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 10);

            int currentPageSize = PageSize ?? 10;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qDepot", qDepot);
            hs.Add("orderCol", orderCol);
            PaginatedList<EAP.Logic.Z10.View.V_DepotProduct> result = Z10DepotHelper.ListProduct(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;



            ViewData["DepotOptions"] = EAP.Logic.Z10.HtmlHelper.DepotSelectOptions(_tenant.TenantID.Value, db);
            //ViewData["DepotOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);

            return View(result);
        }

        public ActionResult EditDepotProduct(System.Int64? id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
            Z10DepotProduct entity = null;
            if (id.HasValue && id > 0)
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "修改库存信息";

                entity = Z10DepotProductHelper.Create(db, id.Value);
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增库存商品";
                entity = new Z10DepotProduct();
            }

            return View(entity);
        }


        #region 建库
        public ActionResult Init(string q, long? brand, long? cate)
        {
            long iBrand = brand ?? 0;
            long iCate = cate ?? 0;

            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = "1=1";
            if (q.IsNotNullOrEmpty())
            {
                where += " and [Title] like @Title";
                dbParams.Add(db.CreateParameter("Title", "%" + q + "%"));
            }
            if (iBrand > 0)
            {
                where += " and [BrandID] = @BrandID";
                dbParams.Add(db.CreateParameter("BrandID", iBrand));
            }
            if (iCate > 0)
            {
                where += " and [ProductID] in (select [ProductID] from Z01ProductInCategory where [CategoryID]=@CategoryID)";
                dbParams.Add(db.CreateParameter("CategoryID", iCate));

            }

            List<Z01Product> products = db.Take<Z01Product>(where + " order by ViewCount desc", 20, dbParams.ToArray());

            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag._brands = brands;
            var currencies = db.Take<EAP.Bus.Entity.Currency>("1=1 order by DisplayOrder");
            ViewBag._currencies = currencies;
            var banks = db.Take<Z01Bank>("1=1 order by DisplayOrder");
            ViewBag._banks = banks;
            var depots = db.Take<Z10Depot>();
            ViewBag._depots = depots;
            var units = db.Take<Z01Unit>();
            ViewBag._units = units;
            ViewBag._categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);


            return View(products);

        }


        public ActionResult ShowDetail_S2(long productID, string qTitle)
        {

            var xdetail = Zippy.Data.StaticDB.DB.Take<Z10DepotProductDetail>("ProductID=@ProductID and StockSum>0",
                db.CreateParameter("ProductID", productID));
            var product = Zippy.Data.StaticDB.DB.FindUnique<Z01Product>(productID);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var depots = EAP.Logic.Z10.Helper.GetDepots(_tenant.TenantID.Value, db);
            sb.Append("<span style='font-weight: bold' id='xdetail_title'>" + product.Title + "<br /><strong style='color:green;font-weight:bold'>" + (xdetail.Sum(s => s.StockSum) ?? 0).ToString("0.##") + "</strong></span>");
            sb.Append("<ul>");
            foreach (var xd in xdetail)
            {
                var depotTitle = string.Empty;
                var depot1 = depots.Where(s => s.DepotID == xd.DepotID).FirstOrDefault();
                if (depot1 != null)
                    depotTitle = depot1.Title;
                sb.AppendLine("<li>" + depotTitle + " " + xd.ExtColor + " " + xd.ExtSize + "  (" + (xd.StockSum ?? 0).ToString("0.##") + ") " + "<li>");
            }
            sb.Append("</ul>");
            return Content(sb.ToString());
        }
        public ActionResult SaveProduct_S3(decimal count, decimal? price, long pid, long? did, string color, string size)
        {
            if (count <= 0)
            {
                return Content("数量必须大于0。");
            }
            var Price = price ?? 0;

            EAP.Logic.DictResponse res = new Logic.DictResponse();

            var tenantID = _tenant.TenantID.Value;

            //更新商品库
            if (!db.Exists<Z10DepotProduct>("ProductID=@ProductID and DepotID=@DepotID and TenantID=@TenantID",
                db.CreateParameter("ProductID", pid), db.CreateParameter("DepotID", did ?? 0), db.CreateParameter("TenantID", tenantID)))
            {
                Z10DepotProduct xProduct = new Z10DepotProduct();
                xProduct.TenantID = tenantID;
                xProduct.ProductID = pid;
                xProduct.DepotID = did ?? 0;
                xProduct.StockSum = count;
                xProduct.InSum = count;
                xProduct.Creator = _user.UserID.Value;
                db.Insert(xProduct);
            }
            else
            {
                string sqlUpdateDepot = "update Z10DepotProduct set StockSum=StockSum + @itemHappened, InSum=InSum + @itemHappened" +
                    " where ProductID=@ProductID and DepotID=@DepotID";
                db.ExecuteNonQuery(sqlUpdateDepot, db.CreateParameter("itemHappened", count),
                    db.CreateParameter("ProductID", pid), db.CreateParameter("DepotID", did ?? 0));

            }

            //更新库存详情
            Z10DepotProductDetail dpd = new Z10DepotProductDetail();
            dpd.TenantID = tenantID;
            dpd.ProductID = pid;
            dpd.DepotID = did ?? 0;
            dpd.OrderID = 0;
            dpd.StockSum = count;
            dpd.InSum = count;
            dpd.ExtColor = color;
            dpd.ExtSize = size;
            dpd.PriceStock = Price;
            dpd.Creator = _user.UserID.Value;
            db.Insert(dpd);


            res._state = true;
            return Content(res.ToJson());
        }
        #endregion
    }
}