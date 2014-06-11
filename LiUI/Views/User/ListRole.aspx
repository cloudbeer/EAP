<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Role>>" %>

<%@ Import Namespace="EAP.Bus.Entity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    用户角色设定
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
    var userid = "<%=ViewData["UserID"] %>";
    $(function () {
        $(".xuserrole").click(function () {
            $("#xmain input").attr("disabled","disabled");
            ischeck = $(this).attr("checked");
            xval = $(this).val();
            if (ischeck) {
                $.post("/User/SetRole/"+userid, "roleid="+xval, function(){
                    $("#xmain input").attr("disabled","");
                })
            }
            else {
                $.post("/User/RemoveRole/"+userid, "roleid="+xval, function(){
                    $("#xmain input").attr("disabled","");
                })
            }
        });

    });
    </script>
    <div id="xmain">
        <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
            <tr rel="title">
                <th>
                    角色名称
                </th>
                <th class="w30">
                    请选择
                </th>
            </tr>
            <%
                if (Model.Count > 0)
                {
                    List<UserRole> myRoles = ViewData["myRoles"] as List<UserRole>;

                    foreach (var item in Model)
                    {
                        bool isChecked = myRoles.Exists(s => s.RoleID == item.RoleID); 
            %>
            <tr rel="item"<%=isChecked?" class='focus'":"" %>>
                <td>
                    <%=item.Title%>
                </td>
                <td class="tc">
                    <input type="checkbox" class='xuserrole' value="<%=item.RoleID %>" <% if (isChecked) Response.Write(" checked='checked'"); %> />
                </td>
            </tr>
            <%
                }
                }
                else
                { %>
            <tr rel="noitem">
                <td colspan="100" class="msg-box h200">
                    没有任何数据
                </td>
            </tr>
            <%} %>
        </table>
    </div>
</asp:Content>
