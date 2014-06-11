<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
出库单出库
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
	    $(function () {
	        $(".xok").click(function () {
	            var $this = $(this);
	            var countHappendTr = $this.parent().prev();
	            var countHappend = countHappendTr.find("input[type=text]");
	            var itemID = $this.prev().val();

	            if (countHappend.val() == "") {
	                $.jvalidate(countHappend, '请输入实际出库数量！', 58, 10);
	                return;
	            }
	            else if (isNaN(countHappend.val())) {
	                $.jvalidate(countHappend, '您还没有数据没保存！', 58, 10);
	                return;
	            }

	            $.post("/Z10Order/ModiItemCountHappend2Session",
                    { 'ItemID': itemID,
                      'CountHappend2': countHappend.val()
                    }, function (data) {
                        if (data == 1) {
                            parent.$.jshowtip("已保存！", "success");

                            countHappendTr.addClass("readonly");
                            countHappend.attr("readonly", true);
                            $this.next().show();
                            $this.hide();
                        } else {
                            parent.$.jshowtip(data, "error", 2);
                            return;
                        }
                    }
                );
	        });

	        $(".xedit").click(function () {
	            var countHappendTr = $(this).parent().prev();
	            var countHappend = countHappendTr.find("input[type=text]");

	            countHappendTr.removeClass("readonly");
	            countHappend.removeAttr("readonly");
	            $(this).prev().show();
	            $(this).hide();
	        });

	        $("#xsubmit").click(function () {
	            var readonly = false;
	            var noreadonlytxt = null;
	            $("#oItem input.text").each(function () {
	                readonly = $(this).attr("readonly");

	                if (!readonly) {
	                    noreadonlytxt = $(this);
	                    return false;
	                }
	            });

	            if (!readonly) {
	                $.jvalidate(noreadonlytxt, '您还有未保存的数据！', 58, 10);
	                return;
	            }

	            var orderStatus = $("input[type=radio]:checked").val();
	            if (orderStatus == undefined) {
	                $.jvalidate($("tr.focus td.tc"), '您还没有选择单据完成进度！', 58);
	                return;
	            }

	            $.post("/Z10Returned/SavePutOut", { 'orderStatus': orderStatus },
                    function (data) {
                        if (data == 1) {
                            parent.$.jshowtip("出库单处理完成！", "success");
                            parent.go2("/Z10Returned/ReturnedList");
                        }
                        else {
                            parent.$.jshowtip(data, "error", 1);
                        }
                    }
                );
	        });
	    });
    </script>
	<% Zippy.Data.IDalProvider db = ViewData["db"] as Zippy.Data.IDalProvider; %>
	<div id="contents">
		<div id="top">
			<div class="con clearfix">
				<div class="fl">
					<a href="javascript:;" class="btn img back" rel="/Z10Returned/ReturnedList"><i class="icon i_back">
					</i>返回<b></b></a>
				</div>
			</div>
		</div>
		<div id="main">
			<% using (Html.BeginForm())
			   {%>
			<table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
				<caption>
					<h3 id="title">出库操作</h3>
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
						<table width="100%" id='oItem' class="list-table f12">
							<tr>
								<th>产品名称</th>
								<th class="w100 tc">应出库数量</th>
                                <th class="w100 tc">已出库数量</th>
								<th class="w100 tc">当前出库数量</th>
								<th class="w80">操作</th>
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
								<td class="tr"><%=(item.CountShould ?? 0).ToString("0.##") %></td>
                                <td class="tr"><%=(System.Math.Abs(item.CountHappend2 ?? 0)).ToString("0.##")%></td>
								<td><input type="text" class="text w90 tr" /></td>
								<td>
                                <input type="hidden" name="itemID" value="<%=item.ItemID %>" />
								<a href='javascript:;' class='xok i_xok'>保存</a>
								<a href='javascript:;' class='xedit i_xedit none'>编辑</a>
								</td>
							</tr>
							<%} %>
						</table>
					</td>
				</tr>
				<tr class="focus">
					<td class="tt">单据完成进度：</td>
					<td class="tc">
						<label class="lradio"><input type="radio" name="action" value="<%=(int)EAP.Logic.Z10.OrderStatus.OuttedSome %>" /> 这个单据还没有结束，下次我还需要补充。</label>
						<label class="lradio"><input type="radio" name="action" value="<%=(int)EAP.Logic.Z10.OrderStatus.Outted %>" /> 出库已经完成但没有付款。</label>
						<label class="lradio"><input type="radio" name="action" value="<%=(int)(EAP.Logic.Z10.OrderStatus.Finished | EAP.Logic.Z10.OrderStatus.Outted)%>" /> 交易完成，结束单据。</label>
					</td>
					<td></td>
					<td></td>
					<td></td>
				</tr>
				<tr class="action">
					<td></td>
					<td class="tc"><input type="button" value="确认出库" id="xsubmit" class="gbutton mr20" /></td>
					<td></td>
					<td></td>
					<td></td>
				</tr>
			</table>
			<%} %>
		</div>
	</div>
</asp:Content>