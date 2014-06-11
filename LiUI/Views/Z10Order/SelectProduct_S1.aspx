<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<Z01Beetle.Entity.Z01Product>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    选择产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $(".xselect").click(function () {
                window.parent.hideQuery();
                id = $(this).attr("rel");
                window.location = "/z10order/SelectProduct_S2/?productID=" + id;

                //                tr = $(this).parent().parent();
                //                title = tr.find(".productTitle").html();
                //                title = $.trim(title);
                //                price = tr.find(".stockPrice").val();
                //                parent.SetProductID(id, title, price);
                //                parent.$("#xaddProduct").attr("show", "true");
            });
        });
    </script>
    <div id="contents">
        <div id="xmain">
            <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                <tr rel="title">
                    <th>
                        标题（<a href="javascript:window.parent.showQuery();">查</a>）
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
                            string btitle = "";
                            var brand = Zippy.Data.StaticDB.DB.FindUnique<Z01Beetle.Entity.Z01Brand>(item.BrandID ?? 0);
                            if (brand != null)
                                btitle = brand.Title;
                        
                %>
                <tr rel="item">
                    <td class='productTitle'>
                        <%=btitle%>
                        <%=item.Title%>
                    </td>
                    <td>
                        <a href="javascript:;" rel="<%=item.ProductID %>" class="xselect">选择</a>
                        <input type="hidden" class='stockPrice' value='<%=item.PriceStock %>' />
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
                        没有任何数据，请首先入库商品。
                    </td>
                </tr>
                <!-- 没有数据的时候显示 -->
                <%} %>
            </table>
        </div>
        <div id="bottom">
            <%=Model.ToPagerHtml("Z10Order", "SelectProduct_S1", 8)%>
        </div>
    </div>
</asp:Content>
