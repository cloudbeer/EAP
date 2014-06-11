<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Logic.Z10.View.V_DepotFlow>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	库存流水
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" src="/content/scripts/pagemvc.js"></script>
	<script type="text/javascript">
		var pageSize = <%=ViewData["PageSize"] %>;
		var controller = "/Z10DepotStatistics";
		var sortUrl = "<%=Model.ToSortUrl() %>";

		$(function () {
			$("#bQuery").click(function () {
				window.location.href = controller + '/FlowList?PageSize=' + pageSize 
										+ "&qDepotID=" + encodeURIComponent($("#qDepotID").val())
										+ "&qProductID=" + encodeURIComponent($("#qProductID").val())
										+ "&qCreateDateStart=" + encodeURIComponent($("#qCreateDateStart").val())
										+ "&qCreateDateEnd=" + encodeURIComponent($("#qCreateDateEnd").val())
;
										
			});
            $("#xselproduct").click(function () {
                $("#selTitle").html("选择产品");
                $("#ifSel").attr("src", "/z10order/selectproduct");
                $("#dSelPanel").show();
            });
            $("#bQuery2").click(function () {
                $("#ifSel").attr("src", "/z10order/selectproduct?qTitle=" + encodeURIComponent($("#qTitle").val()));
            });
		});
        function SetProductID(id, tt) {
            $("#qProductID").val(id);
            $("#xselproduct").html(tt);
            $("#dSelPanel").hide();
        }
	</script>
    <div style="left: 261px; top: 10px; display: none; z-index: 1999" id="dSelPanel" class="pop-box w500">
        <h2 rel="title">
            <a title="关闭" class="close" href="javascript:;">关闭</a><span id="selTitle">选择产品</span></h2>
        <div class="con">
            <div class="top">
                <div rel="msg_box" class="fl">
                    <input class="text w100" id='qTitle' /><input type="button" value='搜索' class="button"
                        id='bQuery2' /></div>
            </div>
            <div class="main" style="height: 300px;">
                <iframe id="ifSel" width="100%" height="100%" frameborder="0"></iframe>
            </div>
            <div class="bottom">
                <a rel="jclose" rel="close" class="button" href="javascript:;" style="">关闭<b></b></a>
            </div>
        </div>
    </div>
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
										仓库：
									</td>
									<td class='tc'>
										<select id="qDepotID" name="qDepotID">
											<option value="">请选择仓库</option>
											<%=ViewData["DepotOptions"] %>
										</select>
									</td>
									<td class='tt'>
										产品：
									</td>
									<td class='tc'>
                                        <%= Html.Hidden("qProductID")%>
                                        <a href='javascript:;' id='xselproduct' class="a2select w100">选择产品</a>
									</td>
									<td>
									</td>
								</tr>
								<tr>
									<td class='tt'>
										发生日期：
									</td>
									<td class='tc'>
										<input type='text' class='text' id='qCreateDateStart' />
										-
										<input type='text' class='text' id='qCreateDateEnd' />
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
						订单
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
						仓库
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
					<th>
						产品
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
						数量
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
						发生时间
						<% if (iorCol == 9)
						   { %>
						<i title="点击排序" class="icon i_sort i_asc" rel="10"></i>
						<%}
						   else if (iorCol == 10)
						   { %>
						<i title="点击排序" class="icon i_sort i_desc" rel="9"></i>
						<%}
						   else
						   { %>
						<i title="点击排序" class="icon i_sort i_nosort" rel="9"></i>
						<%} %>
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
						<a href="/Z10Order/Detail/<%=item.OrderID%>?ReturnUrl=<%=Server.UrlEncode(Request.RawUrl) %>"><%=item.OrderID%> </a>
					</td>
					<td>
						<%=item.DepotTitle%>
					</td>
					<td>
						<%=item.ProductTitle%>
					</td>
					<td class="tr">
						<%=(item.Count??0).ToString("0.##")%>
					</td>
					<td>
						<%=item.CreateDate.Value.ToPassedAway()%>
					</td>
				</tr>
				<%
					}
					}
					else
					{ %>
				<!-- 没有数据的时候显示 -->
				<tr rel="item">
					<td colspan="5" class="msg-box h200">
						没有任何数据
					</td>
				</tr>
				<!-- 没有数据的时候显示 -->
				<%} %>
			</table>
		</div>
	</div>
	<div id="bottom">
		<%=Model.ToPagerHtml("Z10DepotStatistics", "FlowList", ViewData["PageSize"].ToInt32(20))%>
	</div>

</asp:Content>
