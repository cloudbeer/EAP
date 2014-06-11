<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Z10Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
附加条款
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        if (Model == null)
        {
            Response.Write("错误的退货单编号");
            Response.End();
        }
	
    %>
    <script type="text/javascript" src="/Content/Scripts/kindeditor/kindeditor-min.js"></script>
    <script type="text/javascript">
        KE.show({
            id: 'Terms',
            allowPreviewEmoticons: false,
            allowUpload: false,
            width: "99%",
            items: [
				'fontname', 'fontsize', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
				'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
				'insertunorderedlist', '|', 'emoticons', 'image', 'link']
        });

        $(function () {
            $("#xok").click(function () {
                $.get("/Z01PaperTemplate/GetDoc/" + $("#TermsTemp").val(), null, function (res) {
                    KE.html("Terms", res);
                });
            });
        });

       
    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a href="javascript:;" rel="/Z10SaleReturned/SaleReturnedList" class="btn img back"><i class="icon i_back"></i>返回<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <% using (Html.BeginForm())
               {%><table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                   <caption>
                       <h3 id="title">附加条款</h3>
                   </caption>
                   <tr>
                       <td class="tt">
                           选择模板：
                       </td>
                       <td class="tc" colspan="3">
                           <select id="TermsTemp">
                               <%=ViewData["TempOptions"] %>
                           </select>
                           <input type="button" value="确认" id="xok" />（确认后，模板将覆盖原有内容。）
                       </td>
                       <td width="*"></td>
                   </tr>
                   <tr>
                       <td class="tt">
                           条款内容：
                       </td>
                       <td class="tc" colspan="3">
                           <%=Html.TextArea("Terms", Model.Terms, new { @class = "textarea w600",rows=15 }) %>
                       </td>
                   </tr>
                   <tr class="action">
                       <td>
                       </td>
                       <td class="tc">
                           <input type="submit" value="保存" id="xsubmit" class="gbutton mr20" />
                       </td>
                       <td width="*"></td>
                   </tr>
               </table>
            <%} %>
        </div>
    </div>
</asp:Content>
