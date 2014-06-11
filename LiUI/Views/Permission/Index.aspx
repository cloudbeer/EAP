<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Bus.Entity.Permission>>" %>

<%@ Import Namespace="EAP.Bus.Entity" %>
<%@ Import Namespace="EAP.Bus.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    权限表
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
		var pageSize = <%=ViewData["PageSize"] %>;
		var controller = "/Permission";
		var sortUrl = "<%=Model.ToSortUrl() %>";

		$(function () {
			$("#bQuery").click(function () {
				window.location.href = controller + '/?PageSize=' + pageSize 
										+ "&qTitle=" + encodeURIComponent($("#qTitle").val())
										+ "&qUrl=" + encodeURIComponent($("#qUrl").val())
										+ "&qFlag=" + encodeURIComponent($("#qFlag").val())
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
                    <a href="/Permission/Sort/0" class="i_xsort">排序</a>
                </div>
                <div class="pop-box pop-query w600" rel="search">
                    <div class="con">
                        <a rel="close" class="close mr10" href="javascript:;">关闭</a>
                        <div class="msg-box msg-contents">
                            <table class="query">
                                <tr>
                                    <td class='tt'>
                                        标题：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text' id='qTitle' />
                                    </td>
                                    <td class='tt'>
                                        链接地址：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text' id='qUrl' />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class='tt'>
                                        标识：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text' id='qFlag' />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
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
                        标题
                    </th>
                    <th>
                        链接地址
                    </th>
                    <th>
                        标识
                    </th>
                    <th>
                        可分配权限
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
                        <%=(item.ParentID > 0 ? "&nbsp; &nbsp; &nbsp; &nbsp; ├─ " : "")%>
                        <i class="icon i_<%=item.Icon %>"></i>
                        <%=item.Title%>
                    </td>
                    <td>
                        <%=item.Url%>
                    </td>
                    <td>
                        <%=item.Flag%>
                    </td>
                    <td>
                        <%= typeof(Zippy.SaaS.Entity.CRUD).ToString(item.PermissionType??0, typeof(Resources.X)) %>
                    </td>
                    <td>
                        <a href="javascript:;" rel="<%=item.PermissionID %>" class="i_xedit xedit">修改</a>
                        <a href='javascript:;' rel="<%=item.PermissionID %>" class="i_xdel xdel">删除</a>
                        <a href='javascript:;' rel="<%=item.PermissionID %>" class="i_xdetail xdetail">详情</a>
                        <%if (item.ParentID == 0)
                          {
                        %>
                        <a href="/Permission/Sort/<%=item.PermissionID %>" class="i_xsort">排序</a>
                        <%
                            } %>
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
