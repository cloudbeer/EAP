<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Z01Beetle.Entity.Z01Brand>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 品牌
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Content/Scripts/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/fileprogress.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/swfupload.queue.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/jquery.XSWFUpload.js"></script>
    <link rel="stylesheet" type="text/css" href="/Content/Scripts/swfupload/default.css" />
    <script type="text/javascript">

        $(function () {
            $(document).XSWFUpload({
                button_placeholder_id: 'uploadButton',
                upload_url: '/upload/OriImage',
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
                    $.post("/upload/GetOriImageName?f=" + (new Date) * 1, null, function (data) { $("#ImagePath").val(data); $("#progressBar").html(""); });

                }
            });

        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("form").submit(function () {
                if ($("#Title").val() == "") { $.jvalidate('Title', '必须输入名称', 59); return false }
                if ($("#Title").val().length > 300) { $.jvalidate('Title', '您输入的内容太多。', 59); return false }
                if ($("#ImagePath").val().length > 500) { $.jvalidate('ImagePath', '您输入的内容太多。', 59); return false }
                if (isNaN($("#Producer").val())) { $.jvalidate('Producer', '请输入数字。', 59); return false }
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
                        名称：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Title", Model.Title, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        Logo：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("ImagePath", Model.ImagePath, new { @class = "text w200", @readonly = "readonly" })%>
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
                        生产商：
                    </td>
                    <td class="tc" colspan="3">
                    (未实现)
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
