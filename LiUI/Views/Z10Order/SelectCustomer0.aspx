

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<Z01Beetle.Entity.Z01Customer>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    选择客户
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .xselect { cursor: pointer; }
    </style>
    <script type="text/javascript">
        $(function () {
            $(".xselect").click(function () {
                title = $(this).find("td").html();
                title = ($.trim(title));
                id = $(this).attr("cid");
                parent.SetCustomerID(id, title);
            });

            $("#qTitle").keypress(function (event) {
                if (event.which == 13) {
                    b_query();
                }
            });

            function b_query() {
                location = "/Z10Order/SelectCustomer/?qCustomerType=@ViewBag.qCustomerType&qTitle=" + encodeURIComponent($("#qTitle").val());
            }
            $("#bQuery").click(function () {
                b_query();
            });
        });
    </script>
    <div id="contents">
        <div id="xmain">
            <div style="padding: 5px;">
                关键字：<input type="text" class="text" id="qTitle" />
                <input type="button" value="查询" id="bQuery" style="padding: 5px;" />
            </div>
            <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                <tr rel="title">
                    <th>
                        姓名/名称
                    </th>
                    <% if (ViewBag.qCustomerType.BitIs(EAP.Logic.Z01.CustomerTyps.Customer))
                       { %>
                    <th>
                        电话
                    </th>
                    <th>
                        卡号
                    </th>
                    <%  }  %>
                </tr>
                <%
                    if (Model.Count > 0)
                    {
                        foreach (var item in Model)
                        {
                        
                %>
                <tr rel="item" class="xselect" cid="<%=item.CustomerID %>">
                    <td>
                        <%=item.Title%>
                    </td>
                    <% if (ViewBag.qCustomerType.BitIs(EAP.Logic.Z01.CustomerTyps.Customer))
                       { %>
                    <td>
                        <%=item.Tel1 %>
                    </td>
                    <td>
                        <%=item.Tel2 %>
                    </td>
                    <%  }  %>
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
        <div id="bottom">
            <%=Model.ToPagerHtml("Z10Order","SelectCustomer", 8)%>
        </div>
    </div>
</asp:Content>
