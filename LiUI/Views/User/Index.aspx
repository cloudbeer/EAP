<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Bus.Entity.User>>" %>

<%@ Import Namespace="EAP.Bus.Entity" %>
<%@ Import Namespace="EAP.Bus.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	用户
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		var pageSize = <%=ViewData["PageSize"] %>;
		var controller = "/User";
		var sortUrl = "<%=Model.ToSortUrl() %>";

		$(function () {
			$("#bQuery").click(function () {
				window.location.href = controller + '/?PageSize=' + pageSize 
										+ "&qUserName=" + encodeURIComponent($("#qUserName").val())
										+ "&qEmail=" + encodeURIComponent($("#qEmail").val())
										+ "&qName=" + encodeURIComponent($("#qName").val())
										+ "&qNickname=" + encodeURIComponent($("#qNickname").val())
										+ "&qMobileID1=" + encodeURIComponent($("#qMobileID1").val())
										+ "&qMobileID2=" + encodeURIComponent($("#qMobileID2").val())
										+ "&qGroupID=" + encodeURIComponent($("#qGroupID").val());
										
			});

			$(".a2select").hover(function(){

				var next = $(this).next();

				if(next[0] != undefined){
					$(this).css("border-bottom-width","0");
					next.show().hover(function(){
						$(this).prev().css("border-bottom-width","0");
					},function(){
						$(this).prev().css("border-bottom-width","1px");
						$(this).hide();
					});
				}
			},function(){
				var next = $(this).next();

				if(next[0] != undefined){
					$(this).css("border-bottom-width","1px");
					$(this).next().hide();
				}
			});
			
			$("#xdivcontent").width($("#main").width()-228);
		   
			$("#btnSHCate").toggle(
				function(){
					$("#xdivcate").hide();
					$("#xdivcontent").width($("#main").width());
				},
				function(){
					$("#xdivcate").show();
					$("#xdivcate").width(200);
					$("#xdivcontent").width($("#main").width()-228);
				}
			);
		});
	</script>
	<script type="text/javascript" src="/content/scripts/pagemvc.js"></script>
	<div id="contents">
		<div id="top">
			<div class="con clearfix">
				<div class="fl">
					<a class="btn img" href="javascript:;" id="btnSHCate"><i class="icon i_category"></i>
						分类<b></b></a>
					<%=ViewData["TopMenu"] %>
				</div>
				<div class="pop-box pop-query w600" rel="search">
					<div class="con">
						<a rel="close" class="close mr10" href="javascript:;">关闭</a>
						<div class="msg-box msg-contents">
							<table class="query">
								<tr>
									<td class='tt'>
										用户名：
									</td>
									<td class='tc'>
										<input type='text' class='text' id='qUserName' />
										<input type="hidden" id="qGroupID" value="<%=ViewData["xgroupid"] %>" />
									</td>
									<td class='tt'>
										Email：
									</td>
									<td class='tc'>
										<input type='text' class='text' id='qEmail' />
									</td>
									<td>
									</td>
								</tr>
								<tr>
									<td class='tt'>
										名字：
									</td>
									<td class='tc'>
										<input type='text' class='text' id='qName' />
									</td>
									<td class='tt'>
										昵称：
									</td>
									<td class='tc'>
										<input type='text' class='text' id='qNickname' />
									</td>
									<td>
									</td>
								</tr>
								<tr>
									<td class='tt'>
										手机号码：
									</td>
									<td class='tc'>
										<input type='text' class='text' id='qMobileID1' />
									</td>
									<td class='tt'>
										手机号码2：
									</td>
									<td class='tc'>
										<input type='text' class='text' id='qMobileID2' />
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
		<div style="left: 261px; top: 10px; display: none" id="dSelPanel" class="pop-box w500">
			<h2 rel="title">
				<a title="关闭" class="close" href="javascript:;">关闭</a><span id="selTitle">选择客户</span></h2>
			<div class="con">
				<div class="top">
					<div rel="msg_box" class="fl" id='curUser'>
					</div>
				</div>
				<div class="main" style="height: 300px;">
					<iframe id="ifSel" width="100%" height="100%" frameborder="0"></iframe>
				</div>
			</div>
		</div>
		<div id="main">
			<div style="float: left; width: 200px; padding: 4px 0 0 4px" id="xdivcate">
				<%
					Zippy.Data.Collections.PaginatedList<EAP.Bus.Entity.Group> xgroups =   ViewData["xgroup"] as Zippy.Data.Collections.PaginatedList<EAP.Bus.Entity.Group>;
				%>
				<div class="xCategory">
					<a href='/User'>全部显示</a></div>
				<% foreach (EAP.Bus.Entity.Group xgroup in xgroups)
				   { %>
				<div class="xCategory">
					<a href='/User/Index?qGroupID=<%=xgroup.GroupID %>'>
						<%=xgroup.Title%></a></div>
				<%} %>
			</div>
			<div id="xdivcontent">
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
							用户名
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
							Email
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
							名字
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
							昵称
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
							手机号码
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
						<%--<th style="width: 125px">
							分组
						</th>--%>
						<th style="width: 125px">
							角色
						</th>
						<th style="width: 140px">
							操作
						</th>
					</tr>
					<script type="text/javascript">
						$(function () {
							$(".xsetrole").click(function () {
								id = $(this).attr("rel");
								userhtml = $(this).parent().parent();
								$("#curUser").html((userhtml.children(".ynick")).html() + " [" + (userhtml.children(".yname")).html() + "]");
								$("#selTitle").html("请勾选角色");
								$("#ifSel").attr("src", "/user/listrole/" + id);
								$("#dSelPanel").slideDown();
							});
						});
					</script>
					<%
						Zippy.Data.IDalProvider db = ViewData["db"] as Zippy.Data.IDalProvider;
						if (Model.Count > 0)
						{
							foreach (var item in Model)
							{
						
					%>
					<tr rel="item">
						<td class="tc">
							<input type="checkbox" value="<%=item.UserID %>" name='checkID' class="xID" />
						</td>
						<td class='yname'>
							<%=item.UserName%>
						</td>
						<td>
							<a href="mailto:<%=item.Email%>"><%=item.Email%></a>
						</td>
						<td>
							<%=item.Name%>
						</td>
						<td class='ynick'>
							<%=item.Nickname%>
						</td>
						<td>
							<%=item.MobileID1%>
						</td>
						<%--<td>
							<a href='javascript:;' class="xselgroup a2select w100" rel="<%=item.UserID %>">设定组</a>
						</td>--%>
						<td>
							<% List<EAP.Logic.Bus.View.V_UserRole> roles = UserHelper.GetUserID_UserRoles(db, item);
							   string allRoles = "";
							   string firstRole = "设定角色";
							   if (roles.Count() > 0)
								   firstRole = roles[0].Title;
							   roles.ForEach(s =>
							   {
								   allRoles += ("<li>" + s.Title + "</li>\r\n");
							   });
							%>
							<div style="position: relative">
								<a href='javascript:;' class="xsetrole a2select w100" rel="<%=item.UserID %>">
									<%=firstRole.ToStringChopped(10)%>...</a>
								<ul class="hover-menu-list none">
									<%=allRoles %>
								</ul>
							</div>
						</td>
						<td>
							<a href='/User/SetGroup/<%=item.UserID %>?ReturnUrl=<%=System.Web.HttpUtility.UrlEncode(Request.RawUrl) %>'
								class="i_xcate xcate">分组</a> <a href="javascript:" rel="<%=item.UserID %>" class="i_xdetail xdetail"
									rel="<%=item.UserID %>">详情</a> <a href="javascript:;" rel="<%=item.UserID %>" class="i_xedit xedit">
										修改</a> <a href="javascript:;" rel="<%=item.UserID %>" class="i_xdel xdel">删除</a>
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
	</div>
	<div id="bottom">
		<%=Model.ToPagerHtml() %>
	</div>
</asp:Content>
