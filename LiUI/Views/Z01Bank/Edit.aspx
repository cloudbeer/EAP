<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Z01Beetle.Entity.Z01Bank>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 银行
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("form").submit(function () {
                if ($("#Title").val() == "") { $.jvalidate('Title', '必须输入标题', 59); return false }
                if ($("#Title").val().length > 300) { $.jvalidate('Title', '您输入的内容太多。', 59); return false }
                if ($("#Brief").val().length > 20) { $.jvalidate('Brief', '您输入的内容太多。', 59); return false }
                if ($("#Account").val().length > 50) { $.jvalidate('Account', '您输入的内容太多。', 59); return false }
                if ($("#Address").val().length > 300) { $.jvalidate('Address', '您输入的内容太多。', 59); return false }
                if ($("#Contact").val().length > 50) { $.jvalidate('Contact', '您输入的内容太多。', 59); return false }
                if ($("#Tel").val().length > 20) { $.jvalidate('Tel', '您输入的内容太多。', 59); return false }
                if ($("#Fax").val().length > 20) { $.jvalidate('Fax', '您输入的内容太多。', 59); return false }
                if ($("#Url").val().length > 300) { $.jvalidate('Url', '您输入的内容太多。', 59); return false }
                if (isNaN($("#DisplayOrder").val())) { $.jvalidate('DisplayOrder', '请输入数字。', 59); return false }
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
                        简称：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Brief", Model.Brief, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        帐号：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Account", Model.Account, new { @class = "text w300" })%>
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
                        联系人：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Contact", Model.Contact, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        电话：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Tel", Model.Tel, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        传真：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Fax", Model.Fax, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        银行网址：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Url", Model.Url, new { @class = "text w300" })%>
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
