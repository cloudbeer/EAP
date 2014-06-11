<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Bus.Entity.User>" %>

<%@ Import Namespace="EAP.Bus.Entity" %>
<%@ Import Namespace="EAP.Bus.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 用户
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("form").submit(function () {
                if ($("#UserName").val() == "") { $.jvalidate('UserName', '必须输入用户名。', 59); return false }
                if ($("#UserName").val().length > 100) { $.jvalidate('UserName', '您输入的文字太多。', 59); return false }
                if ($("#Email").val().length > 500) { $.jvalidate('Email', '您输入的文字太多。', 59); return false }
                if ($("#Name").val().length > 200) { $.jvalidate('Name', '您输入的文字太多。', 59); return false }
                if ($("#Nickname").val().length > 200) { $.jvalidate('Nickname', '您输入的文字太多。', 59); return false }
                if ($("#Address").val().length > 500) { $.jvalidate('Address', '您输入的文字太多。', 59); return false }
                if ($("#PostCode").val().length > 30) { $.jvalidate('PostCode', '您输入的文字太多。', 59); return false }
                if ($("#MobileID1").val().length > 30) { $.jvalidate('MobileID1', '您输入的文字太多。', 59); return false }
                if ($("#Tel1").val().length > 50) { $.jvalidate('Tel1', '您输入的文字太多。', 59); return false }
                
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
                    <h3 id="title">
                        <%=ViewData["VTitle"]%></h3>
                </caption>
                <tr>
                    <td class="tt">
                        用户名：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("UserName", Model.UserName, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        密码：
                    </td>
                    <td class="tc" colspan="3">
                        默认为：touch，请登录后及时修改密码。
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        Email：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Email", Model.Email, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        名字：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Name", Model.Name, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        昵称：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Nickname", Model.Nickname, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        地址：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Address", Model.Address, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        邮编：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("PostCode", Model.PostCode, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        手机号码：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("MobileID1", Model.MobileID1, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        联系电话：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Tel1", Model.Tel1, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        用户类型：
                    </td>
                    <td class="tc" colspan="3">
                        <%=(typeof(Zippy.SaaS.UserTypes)).ToHtmlControlList("UserType", "checkbox", typeof(Resources.X), Model.UserType)%>
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
