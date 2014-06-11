<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Z01Beetle.Entity.Z01CustomerPerson>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 客户联系人
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("form").submit(function () {
                if ($("#Name").val().length > 200) { $(document).jvalidate('Name', '您输入的内容太多。', 59); return false }
                if ($("#Nickname").val().length > 200) { $(document).jvalidate('Nickname', '您输入的内容太多。', 59); return false }
                if ($("#Email").val().length > 500) { $(document).jvalidate('Email', '您输入的内容太多。', 59); return false }
                if ($("#QQ").val().length > 50) { $(document).jvalidate('QQ', '您输入的内容太多。', 59); return false }
                if ($("#MSN").val().length > 300) { $(document).jvalidate('MSN', '您输入的内容太多。', 59); return false }
                if ($("#Skype").val().length > 300) { $(document).jvalidate('Skype', '您输入的内容太多。', 59); return false }
                if ($("#WangWang").val().length > 300) { $(document).jvalidate('WangWang', '您输入的内容太多。', 59); return false }
                if ($("#Fetion").val().length > 300) { $(document).jvalidate('Fetion', '您输入的内容太多。', 59); return false }
                if ($("#YahooIM").val().length > 300) { $(document).jvalidate('YahooIM', '您输入的内容太多。', 59); return false }
                if ($("#OtherIM").val().length > 3000) { $(document).jvalidate('OtherIM', '您输入的内容太多。', 59); return false }
                if ($("#Tel1").val().length > 50) { $(document).jvalidate('Tel1', '您输入的内容太多。', 59); return false }
                if ($("#Tel2").val().length > 50) { $(document).jvalidate('Tel2', '您输入的内容太多。', 59); return false }
                if ($("#Avatar").val().length > 200) { $(document).jvalidate('Avatar', '您输入的内容太多。', 59); return false }
                if ($("#Address").val().length > 500) { $(document).jvalidate('Address', '您输入的内容太多。', 59); return false }
                if ($("#PostCode").val().length > 30) { $(document).jvalidate('PostCode', '您输入的内容太多。', 59); return false }
            });

            $(".detail.readonly input").attr("readonly", true);
        });
    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a href="javascript:;" class="btn img" onclick="history.back()"><i class="icon i_back">
                    </i>返回<b></b></a>
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
                        名字：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Name", Model.Name, new { @class = "text w300" })%>
                        <%=Html.Hidden("CustomerID", ViewData["qCustomerID"]) %>
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
                        QQ：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("QQ", Model.QQ, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        MSN：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("MSN", Model.MSN, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        Skype：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Skype", Model.Skype, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        旺旺：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("WangWang", Model.WangWang, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        飞信：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Fetion", Model.Fetion, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        Yahoo IM：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("YahooIM", Model.YahooIM, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        其他IM：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("OtherIM", Model.OtherIM, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        Tel1：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Tel1", Model.Tel1, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        Tel2：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Tel2", Model.Tel2, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        头像：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Avatar", Model.Avatar, new { @class = "text w300" })%>
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
                        头衔/职务：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("TitleID", Model.TitleID, new { @class = "text w300" })%>
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
