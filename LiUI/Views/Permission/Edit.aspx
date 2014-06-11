<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Bus.Entity.Permission>" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="EAP.Bus.Entity" %>
<%@ Import Namespace="EAP.Bus.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 权限表
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("form").submit(function () {
                if ($("#Title").val() == "") { $.jvalidate($(this), '必须输入标题', 59); return false }
                if ($("#Title").val().length > 300) { $.jvalidate($(this), '您输入的内容太多。', 59); return false }
                if ($("#Url").val().length > 300) { $.jvalidate($(this), '您输入的内容太多。', 59); return false }
                if ($("#Flag").val().length > 100) { $.jvalidate($(this), '您输入的内容太多。', 59); return false }

                alert($(":check[name=PermissionType][checked=checked]").val());
                //if (isNaN($(":check[name=PermissionType]").val())) { $.jvalidate($(this).parent(), '请输入数字。', 59); return false }
            });

            $(".detail.readonly input").attr("readonly", true);
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
            <% using (Html.BeginForm())
               {%>
            <table cellspacing="0" cellpadding="0" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title"><%=ViewData["VTitle"]%></h3>
                </caption>
                <tr>
                    <td class="tt">
                        标题：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Title", Model.Title, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        链接地址：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Url", Model.Url, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        标识：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Flag", Model.Flag, new { @class = "text w100" })%>
                        填写 MVC 的 Controller
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        图标：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Icon", Model.Icon, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        父路径：
                    </td>
                    <td class="tc" colspan="3">
                        <select id="ParentID" name="ParentID">
                            <option value="">根</option>
                            <%=ViewData["ParentIDOptions"]%>
                        </select>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        最大权限：
                    </td>
                    <td class="tc" colspan="3">
                        <%=(typeof(Zippy.SaaS.Entity.CRUD)).ToHtmlControlList("PermissionType", "checkbox", typeof(Resources.X), Model.PermissionType)%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        是否显示成菜单：
                    </td>
                    <td class="tc" colspan="3">
                    <%
                        string cbx1 = "";
                        string cbx0 = "";
                        if (Model.PermissionStatus == 0)
                            cbx0 = " checked='checked'";
                        else
                            cbx1 = " checked='checked'";
                    %>
                        <input type="radio" value="1" name="PermissionStatus" id="PermissionStatus1"<%=cbx1 %> /><label for="PermissionStatus1">是</label> 
                        <input type="radio" value="0" name="PermissionStatus" id="PermissionStatus0"<%=cbx0 %> /><label for="PermissionStatus0">否</label> 
                    </td>
                    <td>
                    </td>
                </tr>
                <% if (ViewData["IsDetail"] == null)
                   { %>
                <tr class="action">
                    <td>
                    </td>
                    <td class="tc">
                        <input type="submit" value="保存" class="gbutton mr20" />
                        <%=Html.ValidationSummary() %>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <%} %>
            </table>
            <%} %>
        </div>
    </div>
</asp:Content>
