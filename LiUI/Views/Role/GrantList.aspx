<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Zippy.Data.Collections.PaginatedList<EAP.Bus.Entity.Permission>>" %>

<%@ Import Namespace="EAP.Bus.Entity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    授权列表
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
    var roleid = "<%=ViewData["RoleID"] %>";
    $(function(){
        $("input[name='xPermission']").click(function(){
            xthis = $(this);
            pt = xthis.val();
            pid = xthis.attr("rel");
            ischeck = $(this).attr("checked");
            if (ischeck) {
                $.post("/Role/SetPermission/"+roleid, "pid="+pid+"&pt="+ pt, function(){
                    $("#xmain input").attr("disabled","");
                    parent.$.jshowtip("授权操作成功！","success");
                })
            }
            else {
                $.post("/Role/RemovePermission/"+roleid, "pid="+pid+"&pt="+ pt, function(){
                    $("#xmain input").attr("disabled","");
                    parent.$.jshowtip("已取消授权！","success");
                })
            }
        });
    });
    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    给角色授权
                </div>
            </div>
        </div>
        <div id="main">
            <table cellspacing="0" cellpadding="0" border="0" rel="main" class="list-table">
                <tr rel="title">
                    <th>
                        标题
                    </th>
                    <th>
                        链接地址
                    </th>
                    <th>
                        授权
                    </th>
                </tr>
                <%
                    if (Model.Count > 0)
                    {

                        Type enumType = typeof(Zippy.SaaS.Entity.CRUD);
                        System.Array values = Enum.GetValues(enumType);
                        string[] names = Enum.GetNames(enumType);
                        Type resourceType = typeof(Resources.X);
                        System.Resources.ResourceManager rm = new System.Resources.ResourceManager(resourceType);

                        List<RolePermission> myPers = ViewData["MyPermissions"] as List<RolePermission>; //当前角色所有的权限


                        foreach (var item in Model)
                        {
                        
                %>
                <tr rel="item">
                    <td>
                        <%=(item.ParentID>0?"├─":"") %>
                        <i class="icon i_<%=item.Icon %>"></i>
                        <%=item.Title%>
                    </td>
                    <td>
                        <%=item.Url%>
                    </td>
                    <td>
                        <%
int myPermissionType = 0; //当前角色对于当前url所具有的权限
RolePermission myrp = myPers.Where(s => s.PermissionID == item.PermissionID).FirstOrDefault();
if (myrp != null)
    myPermissionType = myrp.PermissionType ?? 0;
StringBuilder sb = new StringBuilder();
int validXX = item.PermissionType ?? 0;
long xpermission = item.PermissionID ?? 0;
foreach (int val in values)
{
    string name = Enum.GetName(enumType, val);
    string text = rm.GetString("Enum_" + enumType.Name + "_" + name);
    if ((validXX & val) == val)
    {
        string xchecked = ((myPermissionType & val) == val ? " checked='checked'" : "");
        sb.Append("<input type='checkbox' name='xPermission' value='" + val + "' id='xPermission" + val + "_" + xpermission + "' rel='" + xpermission + "'" + xchecked + " />");
        sb.Append(" <label for='xPermission" + val + "_" + xpermission + "''>" + text + "</label> ");
    }
    else
    {
        //sb.Append(" - ");
    }
}                            
                        %>
                        <%=sb %>
                    </td>
                </tr>
                <%
                    }
                    }
                    else
                    { %>
                <!-- 没有数据的时候显示 -->
                <tr rel="noitem">
                    <td colspan="100" class="msg-box h200">
                        没有任何数据
                    </td>
                </tr>
                <!-- 没有数据的时候显示 -->
                <%} %>
            </table>
        </div>
    </div>
</asp:Content>
