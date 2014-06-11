<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Logic.Z01.View.V_FinancialFlow>>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    财务流水
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
		var pageSize = <%=ViewData["PageSize"] %>;
		var controller = "/Z01FinancialStatistics";
		var sortUrl = "<%=Model.ToSortUrl() %>";

		$(function () {
			$("#bQuery").click(function () {
				window.location.href = controller + '/CashFlowList?PageSize=' + pageSize 
										+ "&qBankID=" + encodeURIComponent($("#qBankID").val())
										+ "&qCategoryID=" + encodeURIComponent($("#qCategoryID").val())
										+ "&qFlowType=" + encodeURIComponent($("#qFlowType").val())
										+ "&qCreateDateStart=" + encodeURIComponent($("#qCreateDateStart").val())
										+ "&qCreateDateEnd=" + encodeURIComponent($("#qCreateDateEnd").val())
                                        + "&qCurrency=" + encodeURIComponent($("#qCurrency").val())
                                        + "&qInOut=" + encodeURIComponent($("#qInOut").val());

			});
            $(".c_bank").change(function(){
                flowid = $(this).attr("rel"); bankid = $(this).find(":selected").val();
                $.post("/Z01FinancialStatistics/ChangeBank/", {fid:flowid,bid:bankid}, function(res){
                    if (!res.state){
                            alert(res.message);
                    }
                    alert("更新成功。");
                }, 'json');
            });


            $(".btnChangeDate").click(function(){
                prevText = $(this).prev();
                if (prevText.is(":hidden")){
                    prevText.show();
                }
                else{
                    fid = $(this).attr("rel")
                    $.post("/Z01FinancialStatistics/ChangeOrderDate/", {fid:fid, dt:prevText.val()}, function(res){
                        if (res.state){
                            alert("更新成功。");
                            prevText.hide();
                        }
                    },'json');
                }
            });
		});
    </script>
    <style type="text/css">
    select.readonly{border:0}        
    </style>
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
                                        银行：
                                    </td>
                                    <td class='tc'>
                                        <select id="qBankID" name="qBankID">
                                            <option value="">选择银行</option>
                                            <%=ViewData["BankOptions"] %>
                                        </select>
                                    </td>
                                    <td class='tt'>
                                        货币：
                                    </td>
                                    <td class='tc'>
                                        <select id="qCurrency" name="qCurrency">
                                            <option value="">选择货币</option>
                                            <%=ViewData["CurrencyOptions"] %>
                                        </select>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class='tt'>
                                        发生时间：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text w60' id='qCreateDateStart' />
                                        -
                                        <input type='text' class='text w60' id='qCreateDateEnd' />
                                    </td>
                                    <td class='tt'>
                                        查：
                                    </td>
                                    <td class='tc'>
                                        <select id="qInOut" name="qCurrency">
                                            <option value="">收入和支出</option>
                                            <option value="In">收入</option>
                                            <option value="Out">支出</option>
                                        </select>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class='tt'>
                                        类目：
                                    </td>
                                    <td class='tc'>
                                        <input type="hidden" id="qCategoryID" name="qCategoryID" />
                                    </td>
                                    <td class='tt'>
                                        类型：
                                    </td>
                                    <td class='tc'>
                                        <input type="hidden" id="qFlowType" name="qFlowType" />
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
                var banks = ViewBag.banks as List<Z01Bank>;
            %>
            <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                <tr rel="title">
                    <th>
                        银行
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
                        类目
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
                        订单
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
                        创建时间
                        <% if (iorCol == 13)
                           { %>
                        <i title="点击排序" class="icon i_sort i_asc" rel="14"></i>
                        <%}
                           else if (iorCol == 14)
                           { %>
                        <i title="点击排序" class="icon i_sort i_desc" rel="13"></i>
                        <%}
                           else
                           { %>
                        <i title="点击排序" class="icon i_sort i_nosort" rel="13"></i>
                        <%} %>
                    </th>
                    <th>
                        金额
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
                </tr>
                <%
                    if (Model.Count > 0)
                    {
                        foreach (var item in Model)
                        {
                            string colorStyle = item.Amount > 0 ? "red" : "green";
						
                %>
                <tr rel="item" class="<%=colorStyle %>">
                    <td>
                    <select class="c_bank readonly" rel="<%=item.FlowID %>">
                    <%
                            foreach (var sbank in banks)
                            {
                                var seled = "";
                                if (sbank.BankID == item.BankID) seled = "selected='selected'";
                         %>
                         <option value="<%=sbank.BankID %>" <%=seled %>><%=sbank.Title %></option>
                         <%
                            } %>
                    </select>
                    </td>
                    <td>
                        <%=item.CategoryTitle%>
                    </td>
                    <td>
                        <a href="/Z10Order/Detail/<%=item.OrderID%> ">
                            <%=item.OrderID%></a>
                    </td>
                    <td>
                        <%=item.CreateDate.Value.ToPassedAway()%>
                        <input type="text" value="<%=item.CreateDate.Value.ToString("yyyy-MM-dd") %>" style="display: none" />
                        <input type="button" class="btnChangeDate" value="改" rel="<%=item.FlowID %>" />
                    </td>
                    <td class="tr">
                        <%=item.Currency %>
                        <%=(item.Amount??0).ToString("0.##")%>
                    </td>
                </tr>
                <%
                        }
                %>
                <tr class="focus">
                    <td class="tr count">
                        本页合计：
                    </td>
                    <td colspan="6" class="tr">
                        <%=ViewData["ThisPageSum"]%>
                    </td>
                </tr>
                <tr class="focus">
                    <td class="tr count">
                        总计：
                    </td>
                    <td colspan="6" class="tr">
                        <%=ViewData["AllSum"] %>
                    </td>
                </tr>
                <%
                    }
                    else
                    { %>
                <!-- 没有数据的时候显示 -->
                <tr rel="item">
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
        <%=Model.ToPagerHtml("Z01FinancialStatistics", "CashFlowList", ViewData["PageSize"].ToInt32(20))%>
    </div>
</asp:Content>
