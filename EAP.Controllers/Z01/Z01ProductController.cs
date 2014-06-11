using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Z01Beetle.Entity;
using Z01Beetle.Entity.Helper;
using Zippy.Data.Collections;

namespace Z01Beetle.Controllers
{

    public class Z01ProductController : EAP.Logic.__UserController
    {


        #region 验证
        protected void ValidateZ01Product(Z01Product entity)
        {
            if (string.IsNullOrEmpty(entity.Title))
                ModelState.AddModelError("Title required", "标题：必须填写");
            else if (entity.Title.Length > 300)
                ModelState.AddModelError("Title string length error", "标题：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Brief) && entity.Brief.Length > 2000)
                ModelState.AddModelError("Brief string length error", "商品简介：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.ImagePath) && entity.ImagePath.Length > 50)
                ModelState.AddModelError("ImagePath string length error", "商品图片：填写的内容太多");
            if (!string.IsNullOrEmpty(entity.Code) && entity.Code.Length > 50)
                ModelState.AddModelError("Code string length error", "编码：填写的内容太多");

        }
        #endregion

        #region 查询
        public ActionResult Index(int? PageIndex, int? PageSize, string qTitle,
            string qBrief, string qModel1, string qModel2, long? qBrandID,
            DateTime? qCreateDateStart, DateTime? qCreateDateEnd, long? qCateID, int? qStatus, int? orderCol)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");

            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?PageSize=" + PageSize) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");

            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("db", db);
            ViewData.Add("PageSize", PageSize ?? 20);
            int currentPageSize = PageSize ?? 20;
            int currentPageIndex = PageIndex ?? 1;

            //throw new Exception(qTitle);

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qBrief", qBrief);
            hs.Add("qCreateDateStart", qCreateDateStart);
            hs.Add("qCreateDateEnd", qCreateDateEnd);
            hs.Add("qCateID", qCateID);
            hs.Add("qModel1", qModel1);
            hs.Add("qModel2", qModel2);
            hs.Add("qBrandID", qBrandID);
            hs.Add("qStatus", qStatus);

            PaginatedList<Z01Product> result = Z01ProductHelper.Query(db, _tenant.TenantID.Value, currentPageSize, currentPageIndex, hs, orderCol);
            result.QueryParameters = hs;

            ViewData.Add("xcateid", qCateID);
            ViewData.Add("xbrandid", qBrandID);


            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag.brands = brands;
            ViewBag.categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);


            return View(result);
        }
        #endregion

        #region 详情

        public ActionResult Details(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
            Z01Product entity = Z01ProductHelper.Create(db, id);
            return View(entity);
        }
        #endregion

        #region 删除

        public ActionResult Delete(System.Int64 id)
        {
            if ((_crud & Zippy.SaaS.Entity.CRUD.Delete) != Zippy.SaaS.Entity.CRUD.Delete) return Content("401");
            if (db.Delete<Z01Product>(id) > 0)
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
            string sql = "ProductID in (" + ids + "0)";
            //return Content("1");

            if (db.Delete<Z01Product>(sql) > 0)
                return this.Content("1");
            else
                return this.Content("0");
        }

        #endregion

        #region 修改和新增和查看详情

        public ActionResult Edit(System.Int64? id, string act)
        {
            Z01Product entity = null;
            long defaultBrandID = 0;
            if (id.HasValue && id > 0)
            {
                if (act == "detail")
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Read) != Zippy.SaaS.Entity.CRUD.Read) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "查看产品信息";
                    ViewData["IsDetail"] = true;
                }
                else
                {
                    if ((_crud & Zippy.SaaS.Entity.CRUD.Update) != Zippy.SaaS.Entity.CRUD.Update) return RedirectToAction("NoPermission", "Error");
                    ViewData["VTitle"] = "修改产品信息";
                }
                entity = Z01ProductHelper.Create(db, id.Value);
                defaultBrandID = entity.BrandID ?? 0;
            }
            else
            {
                if ((_crud & Zippy.SaaS.Entity.CRUD.Create) != Zippy.SaaS.Entity.CRUD.Create) return RedirectToAction("NoPermission", "Error");
                ViewData["VTitle"] = "新增产品";
                entity = new Z01Product();
                try
                {
                    defaultBrandID = Request.Cookies["defaultBrandID"].Value.ToInt64();
                }
                catch { }
            }
            ViewData["UnitIDOptions"] = Z01ProductHelper.GetUnitIDEntitiesHtmlOption(db, _tenant.TenantID.Value, entity.UnitID);
            ViewData["BrandIDOptions"] = Z01ProductHelper.GetBrandIDEntitiesHtmlOption(db, _tenant.TenantID.Value, defaultBrandID);
            ViewData["MoneyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);

            string returnUrl = Request["ReturnUrl"];
            if (returnUrl.IsNullOrEmpty()) returnUrl = "/" + _ContollerName;
            ViewData["ReturnUrl"] = returnUrl;
            return View(entity);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(System.Int64? id, Z01Product entity)
        {
            entity.ProductID = id;
            string formProductType = Request.Form["ProductType"];
            entity.ProductType = formProductType.ToEnumInt32();

            var ck_brandid = new System.Web.HttpCookie("defaultBrandID", (entity.BrandID ?? 0).ToString());
            ck_brandid.Expires = DateTime.Now.AddMinutes(20);
            Response.SetCookie(ck_brandid);
            ValidateZ01Product(entity);
            if (!ModelState.IsValid)
            {
                ViewData["UnitIDOptions"] = Z01ProductHelper.GetUnitIDEntitiesHtmlOption(db, _tenant.TenantID.Value, entity.UnitID);
                ViewData["BrandIDOptions"] = Z01ProductHelper.GetBrandIDEntitiesHtmlOption(db, _tenant.TenantID.Value, entity.BrandID);
                ViewData["MoneyOptions"] = EAP.Logic.Bus.HtmlHelper.CurrencySelectOptions(db);

                return View(entity);
            }
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
                    entity.ProductID = null;
                    entity.TenantID = _tenant.TenantID;
                    int pkid = db.Insert(entity);

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

            PaginatedList<Z01ProductCategory> result = Z01ProductCategoryHelper.Query(db, _tenant.TenantID.Value, 2000, 1, hs, null);

            List<Z01ProductInCategory> myCategories = db.Take<Z01ProductInCategory>("ProductID=@ProductID and TenantID=@TenantID",
                    db.CreateParameter("ProductID", id), db.CreateParameter("TenantID", _tenant.TenantID));

            ViewData["ProductID"] = id;
            ViewData["MyCategories"] = myCategories;
            return View(result);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SetCategory(long id, int categoryID, bool isAdd)
        {
            if (isAdd)
            {
                Z01ProductInCategory pic = new Z01ProductInCategory();
                pic.ProductID = id;
                pic.CategoryID = categoryID;
                pic.TenantID = _tenant.TenantID;
                pic.Creator = _user.UserID;
                db.Insert(pic);
            }
            else
            {
                db.Delete<Z01ProductInCategory>("ProductID=@ProductID and CategoryID=@CategoryID and TenantID=@TenantID",
                    db.CreateParameter("ProductID", id), db.CreateParameter("CategoryID", categoryID), db.CreateParameter("TenantID", _tenant.TenantID));
            }
            return Content("1");
        }
        #endregion

        public ActionResult OrderHistory(long id)
        {
            ViewBag._user = _user;
            Z01Product product = db.FindUnique<Z01Product>(id);
            var items = db.Take<EAP.Logic.Z10.View.V_OrderItemDetail>("ProductID=@ProductID and DeleteFlag=@not_delete and (OrderType&@Sale)=@Sale  order by ItemID desc",
                db.CreateParameter("ProductID", id), db.CreateParameter("not_delete", (int)EAP.Logic.DeleteFlags.Normal), db.CreateParameter("Sale", (int)EAP.Logic.Z10.OrderTypes.Sale));
            ViewBag._product = product;
            ViewBag._brand = db.FindUnique<Z01Brand>(product.BrandID);
            return View(items);
        }

        public ActionResult SetStatus(int id, int sts)
        {
            Z01Product data = new Z01Product();
            data.ProductID = id;
            data.ProductStatus = sts;
            db.Update(data);
            
            EAP.Logic.DictResponse res = new EAP.Logic.DictResponse();
            res._state = true;
            return Content(res.ToJson());
        }

        public ActionResult SaveAjax(string c_title, long? c_brand, string c_code, string c_currency, decimal? c_stock_price, decimal? c_sale_price, int? c_unit)
        {
            EAP.Logic.DictResponse res = new EAP.Logic.DictResponse();
            Z01Product data = new Z01Product();
            data.Title = c_title;
            data.BrandID = c_brand;
            data.Currency = c_currency;
            data.PriceStock = c_stock_price;
            data.PriceList = c_sale_price;
            data.PriceSelling = c_sale_price;
            data.UnitID = c_unit;
            data.TenantID = _tenant.TenantID;
            data.Creator = _user.UserID;
            data.Code = c_code.Trim();
            if (c_code.IsNotNullOrEmpty())
            {
                if (db.Exists<Z01Product>("Code=@Code", db.CreateParameter("Code", c_code.Trim())))
                {
                    res._state = false;
                    res._message = "编号重复，请核对";
                    return Content(res.ToJson());
                }
            }
            var bid = db.Insert(data);
            res._state = true;
            res._data.Add("title", c_title);
            res._data.Add("id", bid);
            return Content(res.ToJson());
        }

        public ActionResult SaveAjaxAdmin(long c_id, string c_title, string c_code, long? c_brand, string c_currency, decimal? c_stock_price, decimal? c_sale_price, int? c_unit)
        {
            EAP.Logic.DictResponse res = new EAP.Logic.DictResponse();
            Z01Product data = new Z01Product();
            data.ProductID = c_id;
            data.Title = c_title;
            data.BrandID = c_brand;
            data.Currency = c_currency;
            data.PriceStock = c_stock_price;
            data.PriceList = c_sale_price;
            data.PriceSelling = c_sale_price;
            data.UnitID = c_unit;
            data.Updater = _user.UserID;
            data.Code = c_code;
            var bid = db.Update(data);

            //更新库存的进货价格
            if (db.Exists<Z10Cabbage.Entity.Z10OrderItem>("ProductID=@proid", db.CreateParameter("proid", c_id)))
            {
                res._state = false;
                res._message = "该商品已经进入订单系统，库存价格未修改，请使用其他方法修订。";
            }
            else
            {
                string sql = "update Z10DepotProductDetail set PriceStock=@xprice where ProductID=@proid";
                db.ExecuteNonQuery(sql, db.CreateParameter("xprice", c_stock_price ?? 0), db.CreateParameter("proid", c_id));
                res._state = true;
            }
            return Content(res.ToJson());

        }


        public ActionResult List4Price(int? PageIndex, int? PageSize, string qTitle, long? qBrandID, long? qCateID)
        {
            System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();
            if ((_crud & Zippy.SaaS.Entity.CRUD.Create) == Zippy.SaaS.Entity.CRUD.Create)
                sbMenu.AppendLine("<a href='/" + _ContollerName + "/Edit?ReturnUrl=" + System.Web.HttpUtility.UrlEncode("/" + _ContollerName + "/?PageSize=" + PageSize) + "' class='btn img'><i class='icon i_create'></i>添加<b></b></a>");

            sbMenu.AppendLine("<a href='javascript:;' class='btn img' id='bReload'><i class='icon i_refresh'></i>刷新<b></b></a>");
            ViewData["TopMenu"] = sbMenu.ToString();

            ViewData.Add("PageSize", PageSize ?? 20);
            int currentPageSize = PageSize ?? 20;
            int currentPageIndex = PageIndex ?? 1;

            Hashtable hs = new Hashtable();
            hs.Add("qTitle", qTitle);
            hs.Add("qCateID", qCateID);
            hs.Add("qBrandID", qBrandID);


            PaginatedList<Z01Product> rtn = new PaginatedList<Z01Product>();
            List<System.Data.Common.DbParameter> dbParams = new List<System.Data.Common.DbParameter>();

            string where = " [TenantID]=@TenantID";
            dbParams.Add(db.CreateParameter("TenantID", _tenant.TenantID.Value));


            if (qTitle.IsNotNullOrEmpty())
            {
                where += " and ([Title] like @Title or Code like @Title)";
                dbParams.Add(db.CreateParameter("Title", "%" + qTitle + "%"));
            }
            if (qBrandID > 0)
            {
                where += " and [BrandID] = @BrandID";
                dbParams.Add(db.CreateParameter("BrandID", qBrandID));
            }
            if (qCateID > 0)
            {
                where += " and [ProductID] in (select [ProductID] from Z01ProductInCategory where [CategoryID]=@CategoryID)";
                dbParams.Add(db.CreateParameter("CategoryID", qCateID));
            }


            int RecordCount = db.Count<Z01Product>(where, dbParams.ToArray());
            int PageCount = 0;
            if (RecordCount % PageSize == 0)
            {
                PageCount = RecordCount / currentPageSize;
            }
            else
            {
                PageCount = RecordCount / currentPageSize + 1;
            }
            if (PageIndex > PageCount)
                PageIndex = PageCount;
            if (PageIndex < 1)
                PageIndex = 1;


            List<Z01Product> records = db.Take<Z01Product>(where + " order by PriceStock", currentPageSize, currentPageIndex, dbParams.ToArray());
            rtn.AddRange(records);
            rtn.PageIndex = currentPageIndex;
            rtn.PageSize = currentPageSize;
            rtn.TotalCount = RecordCount;
            rtn.QueryParameters = hs;


            var brands = db.Take<Z01Brand>("1=1 order by Title");
            ViewBag.brands = brands;
            ViewBag.categories = EAP.Logic.Z01.Helper.GetProductCategories(_tenant.TenantID.Value);
            var currencies = db.Take<EAP.Bus.Entity.Currency>("1=1 order by DisplayOrder");
            ViewBag._currencies = currencies;
            var units = db.Take<Z01Unit>();
            ViewBag._units = units;

            return View(rtn);
        }

        public ActionResult EditAjax(long productID)
        {
            EAP.Logic.DictResponse res = new EAP.Logic.DictResponse();
            var product = db.FindUnique<Z01Product>(productID);
            if (product != null)
            {
                res._state = true;
                res._data.Add("brand", product.BrandID ?? 0);
                res._data.Add("title", product.Title);
                res._data.Add("currency", product.Currency);
                res._data.Add("pricestock", (product.PriceStock ?? 0).ToString("0.##"));
                res._data.Add("pricesale", (product.PriceSelling ?? 0).ToString("0.##"));
                res._data.Add("unit", product.UnitID ?? 0);
                res._data.Add("code", product.Code);

            }
            else
            {
                res._message = "错误的商品";
                res._state = false;
            }

            return Content(res.ToJson());
        }

    }
}
