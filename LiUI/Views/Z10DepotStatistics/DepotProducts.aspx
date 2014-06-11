<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Logic.Z10.View.V_DepotProduct>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    库存产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .tr a { display: block; width: 300px; font-weight: bold; }
        .tr a:hover { color: #ff0000; background: #ffeeee; }
    </style>
    <script type="text/javascript">
		var pageSize = <%=ViewData["PageSize"] %>;
		var controller = "/Z10DepotStatistics";
		var sortUrl = "<%=Model.ToSortUrl() %>";

		$(function () {
			$("#bQuery").click(function () {
				window.location.href = controller + '/DepotProducts?PageSize=' + pageSize 
										+ "&qProductID=" + encodeURIComponent($("#qProductID").val())
										+ "&qDepotID=" + encodeURIComponent($("#qDepotID").val())
;
										
			});
            $("#xselproduct").click(function () {
                $("#selTitle").html("选择产品");
                $("#ifSel").attr("src", "/z10order/selectproduct");
                $("#dSelPanel").show();
            });
            $("#bQuery2").click(function () {
                $("#ifSel").attr("src", "/z10order/selectproduct?qTitle=" + encodeURIComponent($("#qTitle").val()));
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
            
            $(".savesprice").live("click",function(){
                xthis = $(this);
                xid = xthis.attr("rel");
                xprice=xthis.prev().val();
                $.post("/Z10DepotStatistics/UpdateStockPrice/",{xid:xid,xprice:xprice},function(res){

                });
            });


		});

        function SetProductID(id, tt) {
            $("#qProductID").val(id);
            $("#xselproduct").html(tt);
            $("#dSelPanel").hide();
        }




    </script>
    <script type="text/javascript" src="/content/scripts/pagemvc.js"></script>
    <div style="left: 261px; top: 10px; display: none; z-index: 1999" id="dSelPanel"
        class="pop-box w500">
        <h2 rel="title">
            <a title="关闭" class="close" href="javascript:;">关闭</a><span id="selTitle">选择产品</span></h2>
        <div class="con">
            <div class="top">
                <div rel="msg_box" class="fl">
                    <input class="text w100" id='qTitle' /><input type="button" value='搜索' class="button"
                        id='bQuery2' /></div>
            </div>
            <div class="main" style="height: 300px;">
                <iframe id="ifSel" width="100%" height="100%" frameborder="0"></iframe>
            </div>
            <div class="bottom">
                <a rel="jclose" rel="close" class="button" href="javascript:;" style="">关闭<b></b></a>
            </div>
        </div>
    </div>
    <ul id="x_product_detail" style='display: none'>
        <li>载入中</li>
    </ul>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <%=ViewData["TopMenu"] %>
                </div>
                <div style="font-size: 16px; font-weight: bold" class="fr">
                    <%
                        string sqlAll = "select sum(PriceStock*InSum) from Z10DepotProductDetail";
                        string sqlStock = "select sum(PriceStock*StockSum) from Z10DepotProductDetail";
                        var xall = (decimal)Zippy.Data.StaticDB.DB.ExecuteScalar(sqlAll);
                        var xstock = (decimal)Zippy.Data.StaticDB.DB.ExecuteScalar(sqlStock);
                    %>
                    总进货金额：<%=xall.ToString("0.00") %>
                    存货金额：<%=xstock.ToString("0.00")%>
                </div>
                <div class="pop-box pop-query w600" rel="search">
                    <div class="con">
                        <a rel="close" class="close mr10" href="javascript:;">关闭</a>
                        <div class="msg-box msg-contents">
                            <table class="query">
                                <tr>
                                    <td class='tt'>
                                        产品：
                                    </td>
                                    <td class='tc'>
                                        <%= Html.Hidden("qProductID")%>
                                        <a href='javascript:;' id='xselproduct' class="a2select w100">选择产品</a>
                                    </td>
                                    <td class='tt'>
                                        仓库：
                                    </td>
                                    <td class='tc'>
                                        <select id="qDepotID" name="qDepotID">
                                            <option value="">请选择仓库</option>
                                            <%=ViewData["DepotOptions"] %>
                                        </select>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="bottom">
                            <a class="jenter button" href="javascript:;" id='bShowAll'>显示全部<b></b></a> <a class="jenter button active"
                                href="javascript:;" id='bQuery'>查询<b></b></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="main">
            <%
                var orCol = Request["orderCol"];
                int iorCol = orCol.ToInt32();
            %>
            <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                <tr rel="title">
                    <th>
                        产品
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
                    <th style="width: 300px">
                        库存数量
                        <% if (iorCol == 7)
                           { %>
                        <i title="点击排序" class="icon i_sort i_asc" rel="7"></i>
                        <%}
                           else if (iorCol == 8)
                           { %>
                        <i title="点击排序" class="icon i_sort i_desc" rel="8"></i>
                        <%}
                           else
                           { %>
                        <i title="点击排序" class="icon i_sort i_nosort" rel="7"></i>
                        <%} %>
                    </th>
                    <th style="width: 80px">
                        总进货
                    </th>
                    <th style="width: 80px">
                        总出货
                    </th>
                    <th>
                        仓库
                        <% if (iorCol == 3)
                           { %>
                        <i title="点击排序" class="icon i_sort i_asc" rel="4"></i>
                        <%}
                           else if (iorCol == 4)
                           { %>
                        <i title="点击排序" class="icon i_sort i_desc" rel="3"></i>
                        <%}
                           else
                           { %>
                        <i title="点击排序" class="icon i_sort i_nosort" rel="3"></i>
                        <%} %>
                    </th>
                    <th>
                        报警数量
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
						
                %>
                <tr rel="item">
                    <td>
                    <a href='/z01product/orderHistory/<%=item.ProductID %>'>
                        <%=btitle%>
                        <%=item.ProductTitle%></a>
                    </td>
                    <td class="tr">
                        <a href="javascript:;" class="v_detail" rel="<%=item.ProductID %>" style="width: 300px;">
                            <%=(item.StockSum??0).ToString("0.##")%></a>
                    </td>
                    <td class="tr">
                        <%=(item.InSum??0).ToString("0.##")%>
                    </td>
                    <td class="tr">
                        <%=Math.Abs( (item.OutSum??0)).ToString("0.##")%>
                    </td>
                    <td>
                        <%=item.DepotTitle%>
                    </td>
                    <td class="w100 tr">
                        <%=(item.CountAlarm??0).ToString("0.##")%>
                    </td>
                </tr>
                <%
                        }
                    }
                    else
                    { %>
                <!-- 没有数据的时候显示 -->
                <tr rel="item">
                    <td colspan="100" class="msg-box h200">
                        没有任何数据
                    </td>
                </tr>
                <!-- 没有数据的时候显示 -->
                <%} %>
            </table>
        </div>
    </div>
    <div id="bottom">
        <%=Model.ToPagerHtml("Z10DepotStatistics", "DepotProducts", ViewData["PageSize"].ToInt32(20))%>
    </div>
</asp:Content>
