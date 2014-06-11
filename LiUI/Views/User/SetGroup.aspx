<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    设置分类
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .xCategory { padding: 0 20px; }
        .xCategory:hover { background: #ffeeee; }
    </style>
    <script type="text/javascript">
		$(function () {
			$(":checkbox").unbind("click");
			var myUserID = '<%= ViewData["UserID"]%>';
			$(":checkbox").click(function () {
                $(".xCategory").hide();
				$.post("/User/SetGroup/"+myUserID,{groupID:$(this).val(),isAdd:$(this).attr("checked")},function(res){
                    $(".xCategory").show();
                    parent.$.jshowtip("设置成功","success");
				});
			});
		});
    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a href="<%=ViewData["ReturnUrl"]  %>" class="btn img"><i class="icon i_back"></i>返回<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <table cellspacing="0" cellpadding="0" border="0" class="detail">
                <caption>
                    <h3 id="title">
                        单击进行选择或者取消</h3>
                </caption>
                <tr>
                    <td>
                    </td>
                    <td>
                        <%
                            if (Model.Count > 0)
                            {
                                var myCategories=    ViewData["MyGroups"] as List<EAP.Bus.Entity.UserGroup>;
                                foreach (var item in Model)
                                {
                                    var isChecked = myCategories.Exists(s => s.GroupID == item.GroupID) ? "checked='checked'" : "";
                        %>
                        <div class="xCategory">
                            <input type="checkbox" value="<%=item.GroupID %>" <%=isChecked %> />
                            <%=item.Title%></div>
                        <%
                            }
                            } %>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
