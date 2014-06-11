<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Z01CustomerPerson>" %>

<%@ Import Namespace="Z01Beetle.Entity" %>
<table cellspacing="0" cellpadding="0" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
    <caption>
        <h3 id="title">
            <%=ViewData["VTitle"]%></h3>
    </caption>
    <tr>
        <td class="tt">
            名字：
        </td>
        <td class="tc" colspan="3">
            <%= Model.Name%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            Tel1：
        </td>
        <td class="tc" colspan="3">
            <%=Model.Tel1%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            Tel2：
        </td>
        <td class="tc" colspan="3">
            <%=Model.Tel2%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            昵称：
        </td>
        <td class="tc" colspan="3">
            <%= Model.Nickname%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            Email：
        </td>
        <td class="tc" colspan="3">
            <%=Model.Email%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            QQ：
        </td>
        <td class="tc" colspan="3">
            <%=Model.QQ%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            MSN：
        </td>
        <td class="tc" colspan="3">
            <%=Model.MSN%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            Skype：
        </td>
        <td class="tc" colspan="3">
            <%=Model.Skype%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            旺旺：
        </td>
        <td class="tc" colspan="3">
            <%=Model.WangWang%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            飞信：
        </td>
        <td class="tc" colspan="3">
            <%=Model.Fetion%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            Yahoo IM：
        </td>
        <td class="tc" colspan="3">
            <%=Model.YahooIM%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            其他IM：
        </td>
        <td class="tc" colspan="3">
            <%=Model.OtherIM%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            头像：
        </td>
        <td class="tc" colspan="3">
            <%=Model.Avatar%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            地址：
        </td>
        <td class="tc" colspan="3">
            <%=Model.Address%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            邮编：
        </td>
        <td class="tc" colspan="3">
            <%=Model.PostCode%>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="tt">
            头衔/职务：
        </td>
        <td class="tc" colspan="3">
            <%=Model.TitleID%>
        </td>
        <td>
        </td>
    </tr>
</table>
