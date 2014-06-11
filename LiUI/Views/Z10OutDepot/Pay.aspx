<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
出库收款
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
	$(function () {
	    $("#xsubmit").click(function () {
	        var payAccount = $("#PayAccount");
	        var feePaid = $("#FeePaid");

	        if (payAccount.val() == "0") {
	            $.jvalidate(payAccount, "请选择收入帐户！", 52);
	            return;
	        }

	        if (feePaid.val() == "") {
	            $.jvalidate(feePaid, "请填写当前收款金额！", 56);
	            return;
	        }

	        var orderStatus = $("input[type=radio]:checked").val();
	        if (orderStatus == undefined) {
	            $.jvalidate($("tr.focus td.tc"), '您还没有选择单据完成进度！', 58);
	            return;
	        }


	        $.post("/Z10OutDepot/PayAction", {
	            'bank': payAccount.val(),
	            'feePaid': feePaid.val(),
	            'orderStatus': orderStatus
	        }, function (data) {
	            if (data == 1) {
	                parent.$.jshowtip("收款操作成功！", "success");
	                parent.go2("/Z10OutDepot/OutDepotList");
	            }
	            else {
	                parent.$.jshowtip(data, "error");
	            }
	        });
	    });
	});
    </script>
	<% Zippy.Data.IDalProvider db = ViewData["db"] as Zippy.Data.IDalProvider; %>
	<div id="contents">
		<div id="top">
			<div class="con clearfix">
				<div class="fl">
					<a href="javascript:;" class="btn img back" rel="/Z10OutDepot/OutDepotList"><i class="icon i_back">
					</i>返回<b></b></a>
				</div>
			</div>
		</div>
		<div id="main">
			<% using (Html.BeginForm())
			   {%>
			<table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
				<caption>
					<h3 id="title">出库收款</h3>
				</caption>
				<tr>
					<td class="tt">
						客户：
					</td>
					<td class="tc">
						<%=Model.Z10Order.GetCustomer(db).Title %>
					</td>
					<td class="tt">
						下单日期：
					</td>
					<td class="tc">
						<%=Model.Z10Order.DateOrder.HasValue ? Model.Z10Order.DateOrder.Value.ToString("yyyy年MM月dd日") : "还没有交货"%>
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
					<td class="tt">
						应收款：
					</td>
					<td class="tc">
						<%=(Model.Z10Order.FeeShould ?? 0).ToString("0.##") %>
					</td>
					<td class="tt">
						已收款：
					</td>
					<td class="tc">
						<%=(Model.Z10Order.FeePaid ?? 0).ToString("0.##") %>
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
					<td class="tt">收入帐户：</td>
                    <td class="tc" colspan="3">
                    <select id="PayAccount" name="PayAccount">
                        <option value="0">- 请选择银行帐户 -</option>
                        <%=ViewData["BankList"] %>
                    </select>
                    </td>
                    <td width="*"></td>
				</tr>
				<tr>
					<td class="tt">当前收款：</td>
                    <td colspan="3" class="tc"><input type="text" id="FeePaid" name="FeePaid" class="text" value="<%=((Model.Z10Order.FeeShould ?? 0)-(Model.Z10Order.FeePaid ?? 0)).ToString("0.##")  %>" /></td>
                    <td width="*"></td>
				</tr>
				<tr class="focus">
					<td class="tt">单据完成进度：</td>
					<td class="tc">
						<label class="lradio"><input type="radio" name="action" value="<%=(int)EAP.Logic.Z10.OrderStatus.PaidSome %>" /> 只收到了一部分款项，我还需要继续操作。</label>
						<label class="lradio"><input type="radio" name="action" value="<%=(int)EAP.Logic.Z10.OrderStatus.Paid %>" /> 已经收款了但还产品还没有出库。</label>
						<label class="lradio"><input type="radio" name="action" value="<%=(int)EAP.Logic.Z10.OrderStatus.Finished %>" /> 交易完成，结束单据。</label>
					</td>
					<td></td>
					<td></td>
					<td></td>
				</tr>
				<tr class="action">
					<td></td>
					<td class="tc"><input type="button" value="确认收款" id="xsubmit" class="gbutton mr20" /></td>
					<td></td>
					<td></td>
					<td></td>
				</tr>
			</table>
			<%} %>
		</div>
	</div>
</asp:Content>