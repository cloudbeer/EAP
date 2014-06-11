<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<Z01Beetle.Entity.Z01Brand>>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    品牌
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var pageSize = <%=ViewData["PageSize"] %>;
        var controller = "/Z01Brand";
        var sortUrl = "<%=Model.ToSortUrl() %>";

        $(function () {
            $("#bQuery").click(function () {
                window.location.href = controller + '/?PageSize=' + pageSize 
										+ "&qTitle=" + encodeURIComponent($("#qTitle").val())
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
                                        名称：
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
                    <th width="20">
                        <input type="checkbox" rel="check">
                    </th>
                    <th>
                        Logo
                    </th>
                    <th>
                        名称
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
                    <th style="width:150px">
                        创建日期
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
                    <td class="tc">
                        <input type="checkbox" value="<%=item.BrandID %>" name='checkID' class="xID" />
                    </td>
                    <td>
                        <%
                            if (item.ImagePath.IsNotNullOrEmpty())
                            {
                        %>
                            <img src="/TenantFiles/<%=ViewData["TenantID"] %>/Image/<%=item.ImagePath %>"
                                alt="品牌logo" />
                        <%
                            }
                        %>
                    </td>
                    <td>
                        <%=item.Title%>
                    </td>
                    <td class="tc">
                        <%=item.CreateDate.Value.ToString("yyyy-MM-dd")%>
                    </td>
                    <td>
                        <a href="javascript:;" rel="<%=item.BrandID %>" class="i_xedit xedit">修改</a> <a href='javascript:;'
                            rel="<%=item.BrandID %>" class="i_xdel xdel">删除</a> <a href='javascript:;' rel=""
                                class="i_xdetail xdetail">详情</a> <a href="/Z01Product/Index?qBrandID=<%=item.BrandID %>">查看产品</a>
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
    <div id="bottom">
        <%=Model.ToPagerHtml("Z01Brand", "Index", ViewData["PageSize"].ToInt32())%>
    </div>
</asp:Content>
