<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
查看调拨单
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/content/styles/print.css" rel="stylesheet" type="text/css" media="print" />
    <% Zippy.Data.IDalProvider db = ViewData["db"] as Zippy.Data.IDalProvider; %>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a href="javascript:;" class="btn img back" rel="/Z10Move/MoveList"><i class="icon i_back">
                    </i>返回<b></b></a>
                    <a href="javascript:;" class="btn img" onclick="window.print()"><i class="icon i_print">
                    </i>打印<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <% using (Html.BeginForm())
               {%>
            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title">查看调拨单</h3>
                </caption>
                <tr>
                    <td class="tt">
                        出库仓库：
                    </td>
                    <td class="tc">
                        <%=Model.Z10Order.GetTop1OrderItem(db).GetDepot(db).Title %>
                    </td>
                    <td class="tt">
                        入库仓库：
                    </td>
                    <td class="tc">
                        <%=Model.Z10Order.GetTop1OrderItem(db).GetDepot2(db).Title%>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td class="tt">备注：</td>
                    <td colspan="3" class="tc"><%=Model.Z10Order.Remark.IsNotNullOrEmpty() ? Model.Z10Order.Remark : ""%></td>
                    <td width="*"></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table width="100%" id='oItem' class="list-table f12">
                            <tr>
                                <th>
                                    产品名称
                                </th>
                                <th class="w100">
                                    库存数量
                                </th>
                                <th style="width: 120px">
                                    调拨数量
                                </th>
                                <th style="width: 120px">
                                    实际出库数量
                                </th>
                                <th style="width: 120px">
                                    实际入库数量
                                </th>
                            </tr>
                            <% foreach (Z10Cabbage.Entity.Z10OrderItem item in Model.Items)
                               { %>
                            <tr>
                                <td><%
                                        var product = item.GetProduct(db);
                                        if (product != null)
                                            Response.Write(product.Title);
                                    %>
                                </td>
                                <td class="tr"><%=(item.GetV_DepotProduct(db).StockSum ?? 0).ToString("0.##")%></td>
                                <td class="tr"><%=(item.CountShould ?? 0).ToString("0.##")%></td>
                                <td class="tr"><%=(Math.Abs(item.CountHappend2 ?? 0)).ToString("0.##")%></td>
                                <td class="tr"><%=(item.CountHappend ?? 0).ToString("0.##")%></td>                                 
                            </tr>
                            <%} %>
                        </table>
                    </td>
                </tr>
            </table>
            <%} %>
        </div>
    </div>
</asp:Content>
