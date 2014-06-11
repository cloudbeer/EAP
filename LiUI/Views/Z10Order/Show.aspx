<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
采购单
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="/content/styles/print.css" rel="stylesheet" type="text/css" media="print" />
    <% Zippy.Data.IDalProvider db = ViewData["db"] as Zippy.Data.IDalProvider; %>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a href="javascript:;" class="btn img back" rel="<%=ViewData["ReturnUrl"] %>"><i class="icon i_back"></i>返回<b></b></a>
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
                    <h3 id="title">
                        <%=ViewData["VTitle"]%></h3>
                </caption>
                <tr>
                    <td class="tt">
                        供应商：
                    </td>
                    <td class="tc">
                        <%=Model.Z10Order.GetCustomer(db).Title %>
                    </td>
                    <td class="tt">
                        下单日期：
                    </td>
                    <td class="tc">
                        <%=Model.Z10Order.DateOrder.HasValue ? Model.Z10Order.DateOrder.Value.ToString("yyyy年MM月dd日") : "无时间限制"%>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        交易币种：
                    </td>
                    <td class="tc">
                        <%=Model.Z10Order.GetCurrency(db).Title %>
                    </td>
                    <td class="tt">
                        交货日期：
                    </td>
                    <td class="tc">
                        <%=Model.Z10Order.DateShip.HasValue ? Model.Z10Order.DateShip.Value.ToString("yyyy年MM月dd日") : "无时间限制"%>
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
                        <%
                            decimal sCount = 0;
                            decimal tCount = 0;
                            decimal cCount = 0;
                        %>
                        <table width="100%" id='oItem' class="list-table f12">
                            <tr>
                                <th>产品名称</th>
                                <th class="w100 tc">单价</th>
                                <th class="w100 tc">数量</th>
                                <th class="w100 tc">小计</th>
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
                                <td class="tr"><%=(item.Price ?? 0).ToString("0.##")%></td>
                                <td class="tr"><%=(item.CountShould ?? 0).ToString("0.##")%></td>
                                <% sCount += item.CountShould ?? 0;%>
                                <td class="tr" rel="priceCount"><% tCount = (item.Price * item.CountShould) ?? 0; Response.Write(tCount.ToString("0.##")); %></td>
                                <% cCount += tCount; %>
                            </tr>
                            <%} %>
                            <tr class="focus readonly" rel="count">
                                <td class="count">合计</td>
                                <td class="tr" rel="pCount"></td>
                                <td class="tr" rel="sCount"><%=sCount.ToString("0.##") %></td>
                                <td class="tr" rel="cCount"><%=cCount.ToString("0.##")%></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%} %>
        </div>
    </div>
</asp:Content>
