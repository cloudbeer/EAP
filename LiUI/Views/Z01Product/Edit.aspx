<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Z01Beetle.Entity.Z01Product>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Content/Scripts/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/fileprogress.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/swfupload.queue.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/jquery.XSWFUpload.js"></script>
    <link rel="stylesheet" type="text/css" href="/Content/Scripts/swfupload/default.css" />
    <script type="text/javascript" src="/Content/Scripts/kindeditor/kindeditor-min.js"></script>
    <script type="text/javascript">
        KE.show({
            id: 'Content'
        });
        KE.show({
            id: 'Specification',
            allowPreviewEmoticons: false,
            allowUpload: false,
            items: [
				'fontname', 'fontsize', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
				'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
				'insertunorderedlist', '|', 'emoticons', 'image', 'link']
        });
    </script>
    <script type="text/javascript">

        $(function () {
            $(document).XSWFUpload({
                button_placeholder_id: 'uploadButton',
                upload_url: '/upload/productimage',
                file_types: "*.jpg;*.gif;*.png",
                file_size_limit: "1 MB",
                post_params: { 'ASPSESSID': '<%=Session.SessionID %>' },
                custom_settings: {
                    'progressTarget': 'progressBar',
                    'cancelButtonId': 'canelButton'
                },
                button_text: "<span class='redText'>浏览</span>",
                button_text_style: ".redText { color: #FF0000;}",

                upload_success_handler: function () {
                    $.post("/upload/GetProductImageName?f=" + (new Date) * 1, null, function (data) { $("#ImagePath").val(data); $("#progressBar").html(""); });

                }
            });

        });
    </script>
    <script type="text/javascript">
        $(function () {

            $("form").submit(function () {
                if ($("#Title").val() == "") { $.jvalidate('Title', '必须输入标题', 59); return false }
                if ($("#Title").val().length > 300) { $.jvalidate('Title', '您输入的内容太多。', 59); return false }
                if (isNaN($("#UnitID").val())) { $.jvalidate('UnitID', '请输入数字。', 59); return false }
                if (isNaN($("#PriceList").val())) { $.jvalidate('PriceList', '请输入数字。', 59); return false }
                if (isNaN($("#ProductStock").val())) { $.jvalidate('ProductStock', '请输入数字。', 59); return false }
                if (isNaN($("#PriceSelling").val())) { $.jvalidate('PriceSelling', '请输入数字。', 59); return false }
                if (isNaN($("#PriceSellOff1").val())) { $.jvalidate('PriceSellOff1', '请输入数字。', 59); return false }
                if (isNaN($("#PriceSellOff2").val())) { $.jvalidate('PriceSellOff2', '请输入数字。', 59); return false }
                if (isNaN($("#PriceSellOff3").val())) { $.jvalidate('PriceSellOff3', '请输入数字。', 59); return false }
                if ($("#Brief").val().length > 2000) { $.jvalidate('Brief', '您输入的内容太多。', 59); return false }
                if ($("#ImagePath").val().length > 50) { $.jvalidate('ImagePath', '您输入的内容太多。', 59); return false }
                if ($("#Code").val().length > 50) { $.jvalidate('Code', '您输入的内容太多。', 59); return false }
                if (isNaN($("#ProductType").val())) { $.jvalidate('ProductType', '请输入数字。', 59); return false }
                if (isNaN($("#Model2").val())) { $.jvalidate('Model2', '请输入数字。', 59); return false }
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
                        <select id="BrandID" name="BrandID">
                            <option value="0">选择品牌</option>
                            <%=ViewData["BrandIDOptions"]%>
                        </select>
                        <%= Html.TextBox("Title", Model.Title, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        编码：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Code", Model.Code, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        价格：
                    </td>
                    <td class="tc" colspan="3">
                        进：<%= Html.TextBox("PriceStock", Model.PriceStock, new { @class = "text w100" })%>
                        标：<%= Html.TextBox("PriceList", Model.PriceList, new { @class = "text w100" })%>
                        出：<%= Html.TextBox("PriceSelling", Model.PriceSelling, new { @class = "text w100" })%>
                        <script type="text/javascript">
                            $(function () {
                                $("#PriceList").blur(function () {
                                    $("#PriceSelling").val($("#PriceList").val());
                                });
                            });
                        </script>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        单位：
                    </td>
                    <td class="tc" colspan="3">
                        商品：<select id="UnitID" name="UnitID">
                            <%=ViewData["UnitIDOptions"]%>
                        </select>
                        货币：<select id="Currency" name="Currency">
                            <%=ViewData["MoneyOptions"] %>
                        </select>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        打折价：
                    </td>
                    <td class="tc" colspan="3">
                        一、<%= Html.TextBox("PriceSellOff1", Model.PriceSellOff1, new { @class = "text w100" })%>
                        二、<%= Html.TextBox("PriceSellOff2", Model.PriceSellOff2, new { @class = "text w100" })%>
                        三、<%= Html.TextBox("PriceSellOff3", Model.PriceSellOff3, new { @class = "text w100" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        型号：
                    </td>
                    <td class="tc">
                        <%= Html.TextBox("Model1", Model.Model1, new { @class = "text w200" })%>
                    </td>
                    <td class="tt">                        
                    </td>
                    <td class="tc">
                    </td>
                    <td>
                    </td>
                </tr>

                <tr>
                    <td class="tt">
                        商品图片：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("ImagePath", Model.ImagePath, new { @class = "text w200", @readonly="readonly" })%>
                        <span id="uploadButton">选择文件上传</span> <span id="canelButton" style="display: none">取
                            消上传</span>
                        <div id='progressBar'>
                        </div>
                        <div id="divStatus">
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        商品简介：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextArea("Brief", Model.Brief, new { @class = "textarea w600", rows = "5" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        规格综述：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextArea("Specification", Model.Specification, new { @class = "textarea w600", rows = "5" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        商品介绍：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextArea("Content", Model.Content, new { @class = "textarea w600", rows = "10" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        产品类型：
                    </td>
                    <td class="tc" colspan="3">
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
