<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Z10Cabbage.Entity.Z10DepotProduct>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 库存产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("form").submit(function () {
                if ($("#DepotID").val() == "" || $("#DepotID").val() == "0") { $.jvalidate('DepotID', '必须选择仓库。', 59); return false; }
                if ($("#ProductID").val() == "" || $("#ProductID").val() == "0") { $.jvalidate('xselproduct', '必须选择产品。', 59); return false; }
                if (isNaN($("#StockSum").val())) { $.jvalidate('StockSum', '请输入数字。', 59); return false; }
            });

            $(".detail.readonly input").attr("readonly", true);

            $("#xselproduct").click(function () {
                $("#selTitle").html("选择产品");
                $("#ifSel").attr("src", "/z10order/selectproduct");
                $("#dSelPanel").show();
                //$(this).hide();
            });

            <%if (Model.DepotProductID>0){ %>
                $("#DepotID option").each(function(){
                    if (!$(this).attr("selected"))
                    $(this).remove();

                });

                $("#xselproduct").removeClass("a2select");
                $("#xselproduct").unbind("click");
            <%} %>

        });


        function SetProductID(id, tt) {
            $("#ProductID").val(id);
            $("#xselproduct").html(tt);
            $("#dSelPanel").hide();
        }
    </script>
    <div style="left: 261px; top: 10px; display: none" id="dSelPanel" class="pop-box w500">
        <h2 rel="title">
            <a title="关闭" class="close" href="javascript:;">关闭</a><span id="selTitle">选择客户</span></h2>
        <div class="con">
            <div class="top">
                <div rel="msg_box" class="fl">
                    <input class="text w100" id='qTitle' /><input type="button" value='搜索' class="button"
                        id='bQuery' /></div>
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
                        仓库：
                    </td>
                    <td class="tc" colspan="3">
                        <select id="DepotID" name="DepotID">
                            <option value="0">选择仓库</option>
                            <%=ViewData["DepotOptions"] %>
                        </select>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        产品：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.Hidden("ProductID", Model.ProductID)%>
                        <a href='javascript:;' id='xselproduct' class="a2select w100">
                            <%=ViewData["ProductTitle"] %></a>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        库存：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("StockSum", Model.StockSum, new { @class = "text w100 tr" })%>
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
