<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Logic.Z10.View.V_DepotProduct>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    选择产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $(".xselect").click(function () {
                maxNum = $(this).parent().prev().html();
                title = $(this).parent().prev().prev().html();
                title = ($.trim(title));
                id = $(this).attr("rel");
                parent.SetProductID(id, title, maxNum);
                parent.$("#xaddProduct").attr("show", "true");
            });
        });
    </script>
    <div id="contents">
        <div id="xmain">
            <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                <tr rel="title">
                    <th>
                        标题
                    </th>
                    <th class="w80">
                        库存数量
                    </th>
                    <th class="w30">
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
                        <%=item.ProductTitle%>
                    </td>
                    <td class="tr">
                        <%=(item.StockSum??0).ToString("0.##")%>
                    </td>
                    <td>
                        <a href="javascript:;" rel="<%=item.ProductID %>" class="xselect">选择</a>
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
        <div id="bottom">
            <%=Model.ToPagerHtml() %>
        </div>
    </div>
</asp:Content>
