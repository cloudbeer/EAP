<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Bus.Entity.Currency>" %>

<%@ Import Namespace="EAP.Bus.Entity" %>
<%@ Import Namespace="EAP.Bus.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 币种
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var xcurrencis = [
            ['人民币', 'RMB', '￥', '{0}元'],
            ['美元', 'USD', '$', '{0}美元'],
            ['日元', 'JPY', '￥', '{0}日元'],
            ['欧元 ', 'EUR', 'EUR', '{0}欧元'],
            ['港元 ', 'HKD', 'HK$', '{0}港元'],
            ['加元 ', 'CAD', 'CAD', '{0}加元'],
            ['澳元 ', 'AUD', 'AUD', '{0}澳元']
        ];

        $(function () {
            $("form").submit(function () {
                if ($("#ID").val() == "") { $.jvalidate('ID', '必须输入英文简称', 59); return false }
                if ($("#ID").val().length > 3) { $.jvalidate('ID', '您输入的内容太多。', 59); return false }
                if ($("#Title").val() == "") { $.jvalidate('Title', '必须输入标题', 59); return false }
                if ($("#Title").val().length > 300) { $.jvalidate('Title', '您输入的内容太多。', 59); return false }
                if ($("#Symbol").val().length > 10) { $.jvalidate('Symbol', '您输入的内容太多。', 59); return false }
                if ($("#FormatPattern").val().length > 50) { $.jvalidate('FormatPattern', '您输入的内容太多。', 59); return false }
                if (isNaN($("#DisplayOrder").val())) { $.jvalidate('DisplayOrder', '请输入数字。', 59); return false }
            });

            $(".detail.readonly input").attr("readonly", true);

            $("#selBtn").click(function () {
                var xmainCurrOpts = "";
                for (i = 0; i < xcurrencis.length; i++) {
                    xmainCurrOpts += "<input type='radio' name='xselCurr' class='xselCurr' value='" + i + "' />" + xcurrencis[i][0] + " <br />";
                }
                $("#dSel").html(xmainCurrOpts);
            });

            $(".xselCurr").live('click', function () {
                xindex = $(this).val();
                $("#ID").val(xcurrencis[xindex][1]);
                $("#Title").val(xcurrencis[xindex][0]);
                $("#Symbol").val(xcurrencis[xindex][2]);
                $("#FormatPattern").val(xcurrencis[xindex][3]);
                $("#dSel").html("");
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
            <% using (Html.BeginForm())
               {%>
            <table cellspacing="0" cellpadding="0" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title">
                        <%=ViewData["VTitle"]%></h3>
                </caption>
                <tr>
                    <td class="tt">
                        英文简称：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("ID", Model.ID, new { @class = "text w100" })%>
                        <a href="javascript:;" id="selBtn">选择</a>
                        <div id="dSel">
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
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
                        符号：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Symbol", Model.Symbol, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        格式化字串：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("FormatPattern", Model.FormatPattern, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        排列顺序：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("DisplayOrder", Model.DisplayOrder, new { @class = "text w30" })%>
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
