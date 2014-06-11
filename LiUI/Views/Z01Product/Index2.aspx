<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<Z01Beetle.Entity.Z01Product>>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .tr a { display: block; min-width: 100px; font-weight: bold; }
        .tr a:hover { color: #ff0000; background: #ffeeee; }
    </style>
    <script type="text/javascript">
		var pageSize = <%=ViewData["PageSize"] %>;
		var controller = "/Z01Product";
		var sortUrl = "<%=Model.ToSortUrl() %>";

		$(function () {
			$("#bQuery").click(function () {
				window.location.href = controller + '/?PageSize=' + pageSize 
										+ "&qTitle=" + encodeURIComponent($("#qTitle").val())
										+ "&qModel1=" + encodeURIComponent($("#qModel1").val())
										+ "&qModel2=" + encodeURIComponent($("#qModel2").val())
										+ "&qCreateDateStart=" + encodeURIComponent($("#qCreateDateStart").val())
										+ "&qCreateDateEnd=" + encodeURIComponent($("#qCreateDateEnd").val())
                                        + "&qCateID=" + encodeURIComponent($("#qCateID").val())
                                        + "&qBrandID=" + encodeURIComponent($("#qBrandID").val())
;
										
			});
			
            
            $(".v_detail").click(function(){
                $("#x_product_detail").html("");
                xthis=$(this);
                pid = xthis.attr("rel");
                $.post("/Z10DepotStatistics/ProductDetail/", {id:pid}, function(res){
                    $("#x_product_detail").html(res);
                    xthis.parent().append($("#x_product_detail").show());
                });
            });
            $(".xdetail").click(function(){
                xthis=$(this);
                pid = xthis.attr("rel");
                location="/Z01Product/OrderDetail/"+pid;
            });

		});
    </script>
    <script type="text/javascript" src="/content/scripts/pagemvc.js"></script>
    <ul id="x_product_detail" style='display: none'>
        <li>载入中</li>
    </ul>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <%=ViewData["TopMenu"] %>
                </div>
            </div>
        </div>
        <div id="main">
            <%--<div style="float: left; width: 200px; padding: 4px 0 0 4px" id="xdivcate">
                <%
                    Zippy.Data.Collections.PaginatedList<Z01ProductCategory> xcates = ViewData["xcate"] as Zippy.Data.Collections.PaginatedList<Z01ProductCategory>;
                %>
                <div class="xCategory">
                    <a href='/Z01Product/'>全部显示</a></div>
                <% foreach (Z01ProductCategory xcate in xcates)
                   { %>
                <div class="xCategory">
                    <a href='/Z01Product/Index?qCateID=<%=xcate.CategoryID %>'>
                        <%=xcate.Title%></a></div>
                <%} %>
            </div>--%>
            <div id="xdivcontent">
                <div id="dQuery" style="padding: 10px">
                    关键字：<input id="q" class="text w100" />
                    品牌：<select id="brand">
                        <option value="0">请选择</option>
                        @foreach (var brand in brands) {
                        <option value="@(brand.BrandID ?? 0)">@brand.Title</option>
                        }
                    </select>
                    类别：<select id="category">
                        <option value="0">请选择</option>
                        @foreach (var cate in categories) {
                        <option value="@(cate.CategoryID ?? 0)">@Html.Raw(cate.Title)</option>
                        }
                    </select>
                    <input type="button" value="查询" id="btnQuery" class="button mr20" />
                </div>
                <%
                    var orCol = Request["orderCol"];
                    int iorCol = orCol.ToInt32();
                %>
                <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                    <tr rel="title">
                        <%--<th width="20">
                            <input type="checkbox" rel="check">
                        </th>--%>
                        <th>
                            品牌
                        </th>
                        <th>
                            标题
                            <% if (iorCol == 1)
                               { %>
                            <i title="点击排序" class="icon i_sort i_asc" rel="2"></i>
                            <%}
                               else if (iorCol == 2)
                               { %>
                            <i title="点击排序" class="icon i_sort i_desc" rel="1"></i>
                            <%}
                               else
                               { %>
                            <i title="点击排序" class="icon i_sort i_nosort" rel="1"></i>
                            <%} %>
                        </th>
                        <th style="min-width: 100px">
                            库存(点击看看)
                        </th>
                        <th>
                            分类
                        </th>
                        <th style="width: 100px">
                            标价
                            <% if (iorCol == 5)
                               { %>
                            <i title="点击排序" class="icon i_sort i_asc" rel="6"></i>
                            <%}
                               else if (iorCol == 6)
                               { %>
                            <i title="点击排序" class="icon i_sort i_desc" rel="5"></i>
                            <%}
                               else
                               { %>
                            <i title="点击排序" class="icon i_sort i_nosort" rel="5"></i>
                            <%} %>
                        </th>
                        <th style="width: 0">
                            折扣
                        </th>
                        <th style="width: 120px">
                            操作
                        </th>
                    </tr>
                    <%
                        if (Model.Count > 0)
                        {
                            foreach (var item in Model)
                            {
                                string btitle = "";
                                var brand = Zippy.Data.StaticDB.DB.FindUnique<Z01Beetle.Entity.Z01Brand>(item.BrandID ?? 0);
                                if (brand != null)
                                    btitle = brand.Title;

                                string unitLabel = "/个";
                                Z01Unit unit = Z01ProductHelper.GetUnitIDEntity(Zippy.Data.StaticDB.DB, item);
                                if (unit != null)
                                    unitLabel = "/" + unit.Title;
                           
                    %>
                    <tr rel="item">
                        <%--<td class="tc">
                            <input type="checkbox" value="<%=item.ProductID %>" name='checkID' class="xID" />
                        </td>--%>
                        <%--<td>
                            <%
                                if (item.ImagePath.IsNotNullOrEmpty())
                                {
                                    string ext = System.IO.Path.GetExtension(item.ImagePath);
                                    string fileName = System.IO.Path.GetFileNameWithoutExtension(item.ImagePath);
                            %>
                            <a href="/TenantFiles/<%=ViewData["TenantID"] %>/ProductImage/<%=fileName %><%=ext %>"
                                target="_blank">
                                <img src="/TenantFiles/<%=ViewData["TenantID"] %>/ProductImage/<%=fileName %>_50<%=ext %>"
                                    alt="商品缩略图" />
                            </a>
                            <%
                                }
                            %>
                        </td>--%>
                        <td>
                            <a href='/z01product/Index/?qBrandID=<%=item.BrandID %>'>
                                <%=btitle%></a>
                        </td>
                        <td>
                            <a href='/z01product/orderHistory/<%=item.ProductID %>' class="i_xdetail xdetail bold">
                                <%=item.Title%></a>
                        </td>
                        <td class="tr">
                            <%
                                var xsum = Zippy.Data.StaticDB.DB.Take<Z10Cabbage.Entity.Z10DepotProduct>("ProductID=" + item.ProductID).Sum(s => s.StockSum) ?? 0;
                                if (xsum <= 0)
                                    Response.Write("<strong style='color:red;font-weight:bold'>无货</strong>");
                                else
                                {
                            %>
                            <a href="javascript:;" class="v_detail" rel="<%=item.ProductID %>" style="min-width: 100px;">
                                <%=(xsum).ToString("0.##")%></a>
                            <%
                                }
                            %>
                        </td>
                        <td>
                            <%                                
                                string sql = "CategoryID in (select CategoryID from Z01ProductInCategory where ProductID=@ProductID)";
                                var categories = Zippy.Data.StaticDB.DB.Take<Z01ProductCategory>(sql, Zippy.Data.StaticDB.DB.CreateParameter("ProductID", item.ProductID));
                                foreach (var cate in categories)
                                {
                            %>
                            <a href="/Z01Product/index/?qCateID=<%=cate.CategoryID %>">
                                <%=cate.Title %></a> |
                            <%
                                }
                                
                            %>
                        </td>
                        <td class="tr">
                            <%=item.Currency%><%=(item.PriceList??0).ToString("0.##")%><%=unitLabel %>
                        </td>
                        <td class="tr">
                            <%if (item.PriceList > 0)
                              {  %>
                            <%=((item.PriceStock ?? 0) / (item.PriceList ?? 1)).ToString("0.##")%>
                            <%} %>
                        </td>
                        <td>
                            <a href='/Z01Product/SetCategory/<%=item.ProductID %>?ReturnUrl=<%=System.Web.HttpUtility.UrlEncode(Request.RawUrl) %>'
                                class="i_xcate xcate">分类</a> <a href="javascript:;" rel="<%=item.ProductID %>" class="i_xedit xedit">
                                    修改</a>
                            <%--<a href='javascript:;' rel="<%=item.ProductID %>" class="i_xdetail xdetail">详情</a>--%>
                        </td>
                    </tr>
                    <%
                            }
                        }
                        else
                        { %>
                    <!-- 没有数据的时候显示 -->
                    <tr rel="noitem">
                        <td colspan="9" class="msg-box h200">
                            没有任何数据
                        </td>
                    </tr>
                    <!-- 没有数据的时候显示 -->
                    <%} %>
                </table>
            </div>
        </div>
    </div>
    <div id="bottom">
        <%=Model.ToPagerHtml("Z01Product", "Index", ViewData["PageSize"].ToInt32(20))%>
    </div>
</asp:Content>
