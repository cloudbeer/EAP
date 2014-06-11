<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<Z01Beetle.Entity.Z01Customer>>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    客户
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
		var pageSize = <%=ViewBag.PageSize%>;
		var controller = "/Z01Customer";
		var sortUrl = "<%=Model.ToSortUrl() %>";

		$(function () {
			$("#bQuery").click(function () {
				window.location.href = controller + '/Index?PageSize=' + pageSize 
										+ "&qTitle=" + encodeURIComponent($("#qTitle").val())
										+ "&qEmail=" + encodeURIComponent($("#qEmail").val())
										+ "&qCateID=" + encodeURIComponent($("#qCateID").val())
;
										
			});


			$("#xdivcontent").width($("#main").width());
            $(".xcomunication").click(function(){
                cid = $(this).attr("rel");
                window.location.href="/Z30Communication/Edit/?customerID=" + cid;
            });
		   
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
            $(".selPrincipal").change(function(){
                var xid = "";
                xid = $(this).find("option:selected").val();
                $.post("/Z01Customer/SetPrincipal/" + xid, {cusid:$(this).attr("rel")}, function(res){
                    if (res=="1"){
                        $.jtip("分配成功","分配结果");
                    }
                });
            });
		});
    </script>
    <script type="text/javascript" src="/content/scripts/pagemvc.js"></script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a class="btn img" href="javascript:;" id="btnSHCate"><i class="icon i_category"></i>
                        分类<b></b></a>
                    <%=ViewData["TopMenu"]%>
                </div>
                <div class="pop-box pop-query w600" rel="search">
                    <div class="con">
                        <a rel="close" class="close mr10" href="javascript:;">关闭</a>
                        <div class="msg-box msg-contents">
                            <table class="query">
                                <tr>
                                    <td class='tt'>
                                        关键字：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text' id='qTitle' />
                                    </td>
                                    <td class='tt'>
                                        Email：
                                    </td>
                                    <td class='tc'>
                                        <input type='text' class='text' id='qEmail' />
                                        <input type="hidden" id="qCateID" value="<%=ViewData["xcateid"] %>" />
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
            <div style="float: left; width: 200px; padding: 4px 0 0 4px; display: none" id="xdivcate">
                <%
                    Zippy.Data.Collections.PaginatedList<Z01CustomerCategory> xcates = ViewData["xcate"] as Zippy.Data.Collections.PaginatedList<Z01CustomerCategory>;
                %>
                <div class="xCategory">
                    <a href='/Z01Customer/'>全部显示</a></div>
                <% foreach (Z01CustomerCategory xcate in xcates)
                   { %>
                <div class="xCategory">
                    <a href='/Z01Customer/Index?qCateID=<%=xcate.CategoryID %>'>
                        <%=xcate.Title%></a></div>
                <%} %>
            </div>
            <div id="xdivcontent">
                <%
                    var orCol = Request["orderCol"];
                    int iorCol = orCol.ToInt32();
                %>
                <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                    <tr rel="title">
                        <%--<th width="20">
						<input type="checkbox" rel="check">
					</th>--%>
                        <th>
                            标题
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
                            电话
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
                            卡号
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
                            客户类型
                        </th>
                        <th style="width: 150px">
                            创建时间
                            <% if (iorCol == 11)
                               { %>
                            <i title="点击排序" class="icon i_sort i_asc" rel="12"></i>
                            <%}
                               else if (iorCol == 12)
                               { %>
                            <i title="点击排序" class="icon i_sort i_desc" rel="11"></i>
                            <%}
                               else
                               { %>
                            <i title="点击排序" class="icon i_sort i_nosort" rel="11"></i>
                            <%} %>
                        </th>
                        <th>
                            备注
                        </th>
                        <th>
                            业务员
                        </th>
                        <th style="width: 220px">
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
                        <%--<td class="tc">
						<input type="checkbox" value="<%=item.CustomerID %>" name='checkID' class="xID" />
					</td>--%>
                        <td>
                            <%=item.Title%>
                        </td>
                        <td>
                            <%=item.Tel1%>
                        </td>
                        <td>
                            <%=item.Tel2%>
                        </td>
                        <td>
                            <%
                                EAP.Logic.Z01.CustomerTyps cts = (EAP.Logic.Z01.CustomerTyps)(item.CustomerType ?? 0);                                
                            %>
                            <%=cts.ToString("F")%>
                        </td>
                        <td class="tc">
                            <%=item.CreateDate%>
                        </td>
                        <td>
                            <%=item.Remark %>
                        </td>
                        <td>
                            <select class='selPrincipal' rel='<%=item.CustomerID %>'>
                                <option value="<%=Guid.Empty %>">无负责人</option>
                                <%
                                var salesTable = ViewBag.Sales as System.Data.DataTable;
                                foreach (System.Data.DataRow row in salesTable.Rows)
                                {
                                %>
                                <option value="<%=row["UserID"] %>" <%=row["UserID"].Equals(item.Principal)?"selected='selected'":"" %>>
                                    <%=row["Name"]%></option>
                                <%
                                }
                                %>
                            </select>
                        </td>
                        <td>
                            <a href='/Z01CustomerPerson/Index?qCustomerID=<%=item.CustomerID %>' class="i_xperson xperson">
                                联系人</a> <a href='/Z01Customer/SetCategory/<%=item.CustomerID %>?ReturnUrl=<%=System.Web.HttpUtility.UrlEncode(Request.RawUrl) %>'
                                    class="i_xcate xcate">分类</a> <a href="javascript:;" rel="<%=item.CustomerID %>" class="i_xedit xedit">
                                        修改</a> <a href='javascript:;' rel="<%=item.CustomerID %>" class="i_xdel xdel">删除</a>
                            <a href='javascript:;' rel="<%=item.CustomerID %>" class="i_xdetail xdetail">详情</a>
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
        <%=Model.ToPagerHtml("Z01Customer", "Index", Model.PageSize)%>
    </div>
</asp:Content>
