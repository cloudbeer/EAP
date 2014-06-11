<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<Z01Beetle.Entity.Z01Title>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    排序
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#xcontent").sortable();

            $("#btnSaveOrder").click(function () {
                result = $("#xcontent").sortable('toArray');
                url = '/Z01Title/Sort';
                para = 'result=' + result;

                $.post(url, para, function (res) {
                    if (res == '1') {
                        window.document.location.href = "/Z01Title";
                    } else {
                        $.jshowtip("保存失败。");
                    }

                });
            });
        });
    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a href="/Z01Title" class="btn img"><i class="icon i_back"></i>返回<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <table cellspacing="0" cellpadding="0" border="0" class="detail">
                <caption>
                    <h3 id="title">
                        拖动鼠标排序</h3>
                </caption>
                <tr>
                    <td>
                    </td>
                    <td>
                        <div id="xcontent" style="padding: 30px">
                            <%
                                if (Model.Count > 0)
                                {
                                    foreach (var item in Model)
                                    {
                            %>
                            <div id="d_<%=item.TitleID %>" style="cursor: move; padding: 10px">
                                <%=item.Title%></div>
                            <%
                                }
                                } %>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr class="action">
                    <td>
                    </td>
                    <td class="tc">
                        <input type="button" value="保存" id="btnSaveOrder" class="gbutton mr20" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
