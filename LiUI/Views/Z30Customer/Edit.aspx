<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Z01Beetle.Entity.Z01Customer>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<%@ Import Namespace="Z01Beetle.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑 客户
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Content/Scripts/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/fileprogress.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/swfupload.queue.js"></script>
    <script type="text/javascript" src="/Content/Scripts/swfupload/jquery.XSWFUpload.js"></script>
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
                    $.post("/upload/GetOriImageName", { f: (new Date) * 1 }, function (data) { $("#Avatar").val(data); $("#progressBar").html(""); });

                }
            });

        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("form").submit(function () {
                if ($("#Title").val() == "") { $.jvalidate('Title', '必须输入标题', 59); return false }
                if ($("#Title").val().length > 300) { $.jvalidate('Title', '您输入的内容太多。', 59); return false }
                if ($("#Tel1").val().length > 50) { $.jvalidate('Tel1', '您输入的内容太多。', 59); return false }
                if ($("#Fax").val().length > 50) { $.jvalidate('Fax', '您输入的内容太多。', 59); return false }
                if ($("#Email").val().length > 500) { $.jvalidate('Email', '您输入的内容太多。', 59); return false }
                if ($("#PostCode").val().length > 30) { $.jvalidate('PostCode', '您输入的内容太多。', 59); return false }
                if ($("#Address").val().length > 500) { $.jvalidate('Address', '您输入的内容太多。', 59); return false }
                if ($("#Avatar").val().length > 200) { $.jvalidate('Avatar', '您输入的内容太多。', 59); return false }
                if ($("#IM").val().length > 3000) { $.jvalidate('IM', '您输入的内容太多。', 59); return false }
                if ($("#Url").val().length > 500) { $.jvalidate('Url', '您输入的内容太多。', 59); return false }
                if (isNaN($("#CustomerType").val())) { $.jvalidate('CustomerType', '请输入数字。', 59); return false }
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
                        客户名称：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Title", Model.Title, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        电话：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Tel1", Model.Tel1, new { @class = "text w300" })%>
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
                        头像：
                    </td>
                    <td class="tc" colspan="3">
                        <%if (ViewBag.act == "detail")
                          {
                              if (Model.Avatar.IsNotNullOrEmpty())
                              { 
                        %>
                        <img src="/TenantFiles/Image/<%=Model.Avatar %>" />
                        <%
                              }
                          }
                          else
                          {%>
                        <%= Html.TextBox("Avatar", Model.Avatar, new { @class = "text w200", @readonly = "readonly" })%>
                        <span id="uploadButton">选择文件上传</span> <span id="canelButton" style="display: none">取
                            消上传</span>请上传合适大小的图片，此处服务器不帮助处理。
                        <div id='progressBar'>
                        </div>
                        <div id="divStatus">
                        </div>
                        <%} %>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        即时通信：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("IM", Model.IM, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        网址：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextBox("Url", Model.Url, new { @class = "text w300" })%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        客户类型：
                    </td>
                    <td class="tc" colspan="3">
                        <%=(typeof(EAP.Logic.Z01.CustomerTyps)).ToHtmlControlList("CustomerType","checkbox", typeof(Resources.X), Model.CustomerType) %>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        备注：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextArea("Remark", Model.Remark, new { @class = "textarea w500", rows="10" })%>
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
