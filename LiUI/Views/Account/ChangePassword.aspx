<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    修改密码
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () {

                if ($("#OPassword").val() == "") { $.jvalidate('OPassword', '必须输入旧密码', 59); return false }
                if ($("#NPassword").val() == "") { $.jvalidate('NPassword', '必须输入新密码。', 59); return false }
                if ($("#NPassword").val() != $("#NPassword2").val()) { $.jvalidate('NPassword2', '两次密码输入不符。', 59); return false }

                $.post("/account/changepassword", { "OPassword": $("#OPassword").val(), "NPassword": $("#NPassword").val() }, function (res) {
                    if (res == "1") {
                        parent.$.jshowtip("密码修改成功。", "success", 2, null, function () { parent.go2("/cabbage/desk"); });
                        return;
                    }
                    parent.$.jhidetip();
                    parent.$.jalert(res, "错误", "error", true);
                });
            });

        });
    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                </div>
            </div>
        </div>
        <div id="main">
            <table cellspacing="0" cellpadding="0" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title">
                        修改密码</h3>
                </caption>
                <tr>
                    <td class="tt">
                        原密码：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.Password("OPassword", "", new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        新密码：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.Password("NPassword", "", new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        再次输入密码：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.Password("NPassword2", "", new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr class="action">
                    <td>
                    </td>
                    <td class="tc">
                        <input type="button" value="修改密码" class="gbutton mr20" id="btnSubmit" />
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
    </div>
</asp:Content>
