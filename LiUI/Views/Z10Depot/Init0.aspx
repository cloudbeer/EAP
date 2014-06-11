<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Logic.Z10.View.V_DepotProduct>>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    初始建库
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $(".xdel").click(function () {
                var xthis = $(this);
                var id = xthis.attr("rel");
                alert(id);
                $.jconfirmDel(function () {
                    $.post("/Z10DepotProduct/Delete/" + id, {ajax : 1}, function (res) {
                        xthis.parent().parent().fadeOut('fast', function () {
                            $(this).remove();
                            parent.$.jshowtip('数据删除成功！', 'success');
                        });
                    });
                });
            });
            $(".xedit").click(function () {
                var id = $(this).attr("rel");
                window.location.href = "/Z10DepotProduct/Edit/" + id + "?ReturnUrl=" + encodeURIComponent(window.location.href);

            });
            $("#search").click(function () {
                window.location.href = "/z10depot/init?qTitle=" + encodeURIComponent($("#qTitle").val())
                + "&qDepot=" + encodeURIComponent($("#qDepot").val());
            });
        });
    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    选择仓库：
                    <select id="qDepot" name="qDepot">
                        <option value="0">请选择仓库</option>
                        <%=ViewData["DepotOptions"] %>
                    </select>
                    <input type="text" name="qTitle" id="qTitle" class="text w100" />
                    <a id="search" class="btn img" href="javascript:;"><i class="icon i_search"></i>查询<b></b></a>
                    | <a href="/Z10DepotProduct/Edit?ReturnUrl=<%=System.Web.HttpUtility.UrlEncode("/Z10Depot/Init?PageSize=" + ViewData["PageSize"])%>"
                        class="btn img"><i class="icon i_create"></i>添加<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                <tr rel="title">
                    <th>
                        仓库
                    </th>
                    <th>
                        产品
                    </th>
                    <th>
                        数量
                    </th>
                    <th class="w100">
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
                    <td>
                        <%=item.DepotTitle %>
                    </td>
                    <td>
                        <%=item.ProductTitle %>
                    </td>
                    <td class="tr">
                        <%=item.StockSum.Value.ToString("0.####") %>
                    </td>
                    <td>
                        <a href="javascript:;" rel="<%=item.DepotProductID %>" class="i_xedit xedit">修改</a> <a href='javascript:;'
                            rel="<%=item.DepotProductID %>" class="i_xdel xdel">删除</a>
                    </td>
                </tr>
                <%
                    }
                    }
                    else
                    { %>
                <!-- 没有数据的时候显示 -->
                <tr rel="noitem">
                    <td colspan="100" class="msg-box h200">
                        没有任何数据
                    </td>
                </tr>
                <!-- 没有数据的时候显示 -->
                <%} %>
            </table>
        </div>
    </div>
</asp:Content>
