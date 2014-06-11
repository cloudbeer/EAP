<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<Z10Cabbage.Entity.Z10DepotProduct>>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    库存产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var pageSize = <%=ViewData["PageSize"] %>;
        var controller = "/Z10DepotProduct";
        var sortUrl = "<%=Model.ToSortUrl() %>";

        $(function () {
            $("#bQuery").click(function () {
                window.location.href = controller + '/?PageSize=' + pageSize 
										+ "&qProductIDStart=" + encodeURIComponent($("#qProductIDStart").val())

										+ "&qProductIDEnd=" + encodeURIComponent($("#qProductIDEnd").val())
										+ "&qDepotIDStart=" + encodeURIComponent($("#qDepotIDStart").val())
										+ "&qDepotIDEnd=" + encodeURIComponent($("#qDepotIDEnd").val())
;
										
            });
        });
    </script>
    <script type="text/javascript" src="/content/scripts/pagemvc.js"></script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <%=ViewData["TopMenu"] %>
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
                                        <input type='text' class='text' id='qProductIDStart' />
                                        -
                                        <input type='text' class='text' id='qProductIDEnd' />
                                    </td>
                                    <td class='tt'>
                                        仓库：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text' id='qDepotIDStart' />
                                        -
                                        <input type='text' class='text' id='qDepotIDEnd' />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
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
                    <th width="20">
                        <input type="checkbox" rel="check">
                    </th>
                    <th>
                        编号
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
                    <th>
                        产品
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
                        仓库
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
                    <th>
                        总进货
                        <% if (iorCol == 7)
                           { %>
                        <i title="点击排序" class="icon i_sort i_asc" rel="8"></i>
                        <%}
                           else if (iorCol == 8)
                           { %>
                        <i title="点击排序" class="icon i_sort i_desc" rel="7"></i>
                        <%}
                           else
                           { %>
                        <i title="点击排序" class="icon i_sort i_nosort" rel="7"></i>
                        <%} %>
                    </th>
                    <th>
                        总出货
                        <% if (iorCol == 9)
                           { %>
                        <i title="点击排序" class="icon i_sort i_asc" rel="10"></i>
                        <%}
                           else if (iorCol == 10)
                           { %>
                        <i title="点击排序" class="icon i_sort i_desc" rel="9"></i>
                        <%}
                           else
                           { %>
                        <i title="点击排序" class="icon i_sort i_nosort" rel="9"></i>
                        <%} %>
                    </th>
                    <th>
                        库存
                        <% if (iorCol == 11)
                           { %>
                        <i title="点击排序" class="icon i_sort i_asc" rel="12"></i>
                        <%}
                           else if (iorCol == 12)
                           { %>
                        <i title="点击排序" class="icon i_sort i_desc" rel="11"></i>
                        <%}
                           else
                           { %>
                        <i title="点击排序" class="icon i_sort i_nosort" rel="11"></i>
                        <%} %>
                    </th>
                    <th>
                        操作
                    </th>
                </tr>
                <%
                    if (Model.Count > 0)
                    {
                        foreach (var item in Model)
                        {
                        
                %>
                <tr rel="item">
                    <td class="tc">
                        <input type="checkbox" value="<%=item.DepotProductID %>" name='checkID' class="xID" />
                    </td>
                    <td>
                        <%=item.DepotProductID%>
                    </td>
                    <td>
                        <%=item.ProductID%>
                    </td>
                    <td>
                        <%=item.DepotID%>
                    </td>
                    <td>
                        <%=item.InSum%>
                    </td>
                    <td>
                        <%=item.OutSum%>
                    </td>
                    <td>
                        <%=item.StockSum%>
                    </td>
                    <td>
                        <a href="javascript:;" rel="<%=item.DepotProductID %>" class="xedit">修改</a> <a href='javascript:;'
                            rel="<%=item.DepotProductID %>" class="xdel">删除</a> <a href='javascript:;' rel="<%=item.DepotProductID %>"
                                class="xdetail">详情</a>
                    </td>
                </tr>
                <%
                        }
                    }
                    else
                    { %>
                <!-- 没有数据的时候显示 -->
                <tr rel="item">
                    <td colspan="8" class="msg-box h200">
                        没有任何数据
                    </td>
                </tr>
                <!-- 没有数据的时候显示 -->
                <%} %>
            </table>
        </div>
    </div>
    <div id="bottom">
        <%=Model.ToPagerHtml() %>
    </div>
</asp:Content>
