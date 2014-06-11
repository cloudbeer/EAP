<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Z01Beetle.Entity.Z01ProductCategory>>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    产品分类
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
		var controller = "/Z01ProductCategory";
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
                                        标题：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text' id='qTitle' />
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
                        编号
                    </th>
                    <th class="w200">
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
                        <%=item.Title%>
                    </td>
                    <td>
                        <%=item.Code%>
                    </td>
                    <td>
                        <a href="/Z01ProductCategory/Edit?xParentID=<%=item.CategoryID %>&ReturnUrl=<%=System.Web.HttpUtility.UrlEncode(Request.RawUrl) %>"
                            class="i_xadd xadd">添加</a> <a href="javascript:;" rel="<%=item.CategoryID %>" class="i_xedit xedit">
                                修改</a> <a href='javascript:;' rel="<%=item.CategoryID %>" class="i_xdel xdel">删除</a>
                        <a href='javascript:;' rel="<%=item.CategoryID %>" class="i_xdetail xdetail">详情</a>
                        <% if (Model.Count(s => s.ParentID == item.CategoryID) > 1)
                           { %>
                        <a href='/Z01ProductCategory/Sort/<%=item.CategoryID %>' class='i_xsort' id='xsort'>
                            排序</a>
                        <%} %>
                    </td>
                </tr>
                <%
                        }
                    }
                    else
                    { %>
                <!-- 没有数据的时候显示 -->
                <tr rel="noitem">
                    <td colspan="3" class="msg-box h200">
                        没有任何数据
                    </td>
                </tr>
                <!-- 没有数据的时候显示 -->
                <%} %>
            </table>
        </div>
    </div>
</asp:Content>
