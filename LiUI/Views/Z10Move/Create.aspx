<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    创建调拨单
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var Li = {};
        Li.isSubmit = true;

        $(function () {
            $("#oItem tr td select").each(function () {
                var selectText = $(this).prev();

                var selectVal = $(this).next();

                var option = $(this).find("option");

                option.each(function () {
                    if ($(this).val() == selectVal.val()) {
                        selectText.text($(this).text());
                    }
                });
            });

            /*window.onbeforeunload = function(){
            if(!Li.isSubmit){
            return "点击“确认”未保存的数据将会丢失！";
            }
            };*/

            $(document).click(function () {
                Li.isSubmit = false;
            });

            $("#xsubmit").click(function () {
                if ($("#xaddProduct").attr("show") == "true") {
                    parent.$.jshowtip("您还没有保存正在编辑的产品信息！", "error", 2);
                    return;
                }

                var $depotin = $("#depotin");
                var $depotout = $("#depotout");
                var $Remark = $("#Remark");

                if ($depotout.val() == "0") {
                    $.jvalidate($depotout, "请选择调出仓库！", 58);
                    return false;
                }
                else if ($depotin.val() == "0") {
                    $.jvalidate($depotin, "请选择调入仓库！", 58);
                    return false;
                }

                else if ($depotout.val() == $depotin.val()) {
                    $.jvalidate($depotin, "调出和调入仓库不能相同！", 58);
                    return false;
                }

                $.post("/Z10Move/doCreate",
                    {
                        'Remark': $Remark.val()
                    }, function (data) {
                        if (data == 1) {
                            Li.isSubmit = true;
                            parent.$.jshowtip("调拨单已保存！", "success", 1);
                            location = "/Z10Move/MoveList";
                        } else {
                            parent.$.jshowtip("系统错误，请联系管理员！", "error", 1);
                        }
                    }
                );
            });

            $("form").submit(function () {
                if ($("#Remark").val().length > 2000) { $.jvalidate('Remark', '您输入的内容太多。', 59); return false }
            });

            $(".detail.readonly input").attr("readonly", true);

            $("#xaddProduct").click(function () {
                xdepotID = $("#depotout").val();
                if (xdepotID <= 0) {
                    $.jvalidate("depotout", "请先选择调出仓库！", 58);
                    return;
                }

                var doutOption = $("#depotout option:selected");
                var span = "<span>" + doutOption.text() + "</span>"

                $("#depotout").after(span).hide();

                if ($(this).attr("show") == "true")
                    parent.$.jshowtip("您还没有保存上一条产品信息！", "error");
                else {
                    $("#selTitle").html("选择产品");
                    $("#ifSel").attr("src", "/Z10Move/selectproduct/" + xdepotID);
                    $("#dSelPanel").show();
                }
                //$(this).hide();
            });

            $("#bQuery").click(function () {
                xdepotID = $("#depotout").val();
                if (xdepotID <= 0) {
                    parent.$.jshowtip("必须选择调出仓库！", "error");
                    return;
                }
                $("#ifSel").attr("src", "/Z10Move/selectproduct/" + xdepotID + "?qProductTitle=" + encodeURIComponent($("#qProductTitle").val()));

            });

        });
        function SetCustomerID(id, tt) {
            $("#CustomerID").val(id);
            $("#xselcustomer").html(tt);
            $("#dSelPanel").hide();
        }
        function SetProductID(id, tt, maxNum) {
            html = "<tr>";
            html += "<td><input type='hidden' value='" + id + "'/>" + tt + "</td>";
            html += "<td class='tr'>" + maxNum + "</td>";
            html += "<td class='tr' rel='item_countshould'><input class='text w100 tr' /></td>";
            html += "<td><a href='javascript:;' class='i_xok xok'>保存</a> <a href='javascript:;' class='i_xedit xedit none'>编辑</a><a href='javascript:;' class='i_xok2 xok2 none'>保存</a><a href='javascript:;' class='i_xdel xdel'>删除</a></td>";
            html += "</tr>";


            $("#oItem").append(html);
            $("#dSelPanel").hide();
        }

        $(".xdel").live("click", function () {
            var tr = $(this).parent().parent();
            var pid = tr.find("input[type=hidden]").val();
            var index = tr.index();
            $.post("/Z10Order/RemoveItem2Session", { 'index': index, 'productid': pid }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("删除成功！", "success", 0.6);
                    tr.remove();
                    $("#xaddProduct").attr("show", "false");
                }
            });
        });

        $(".xok").live("click", function () {
            var num = $(this).parent().prev().find("input");
            var maxNum = $(this).parent().prev().prev().text();

            var tr = $(this).parent().parent();
            var pid = tr.find("input[type=hidden]").val();

            if (num.val() == "") {
                $.jvalidate(num, "请填写商品调拨数量！", 58, 10);
                return false;
            }
            else if (isNaN(num.val())) {
                $.jvalidate(num, "商品数量应该为数字！", 58, 10);
                return false;
            } else if (Number(num.val()) > Number(maxNum)) {
                $.jvalidate(num, "调拨数量超出了库存数量！", 58, 10);
                return false;
            }

            var enter = $(this);
            var index = tr.index();
            $.post("/Z10Order/SaveItem2Session", { 'index': index, 'ProductID': pid, 'CountShould': num.val(), 'DepotID': $("#depotout").val(), 'DepotID2': $("#depotin").val() }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("保存成功", "success", 0.6);

                    tr.addClass("readonly").find("input").attr("readonly", true);

                    enter.next().show();
                    enter.remove();
                    $("#xaddProduct").attr("show", "false");
                } else {
                    parent.$.jshowtip(data, "error", 2);
                    return false;
                }
            });
        });

        $(".xok2").live("click", function () {
            var num = $(this).parent().prev().find("input");
            var maxNum = $(this).parent().prev().prev().text();

            var tr = $(this).parent().parent();
            var pid = tr.find("input[type=hidden]").val();

            if (num.val() == "") {
                $.jvalidate(num, "请填写商品调拨数量！", 58, 10);
                return false;
            }
            else if (isNaN(num.val())) {
                $.jvalidate(num, "商品数量应该为数字！", 58, 10);
                return false;
            } else if (Number(num.val()) > Number(maxNum)) {
                $.jvalidate(num, "调拨数量超出了库存数量！", 58, 10);
                return false;
            }

            var enter = $(this);
            var index = tr.index();
            $.post("/Z10Order/ModiItem2Session", { 'index': index, 'ProductID': pid, 'CountShould': num.val() }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("\u7f16\u8f91\u6210\u529f", "success", 0.6);

                    tr.addClass("readonly").find("input").attr("readonly", true);

                    enter.hide();
                    enter.prev().show();
                    $("#xaddProduct").attr("show", "false");
                } else {
                    parent.$.jshowtip(data, "error", 0.6);
                    return false;
                }
            });
        });

        $(".xedit").live("click", function () {
            $("#xaddProduct").attr("show", "true");
            var tr = $(this).parent().parent();

            tr.removeClass("readonly").find("input").removeAttr("readonly");

            $(this).hide();
            $(this).next().show();
        });
    </script>
    <div style="left: 261px; top: 10px; display: none" id="dSelPanel" class="pop-box w500">
        <h2 rel="title">
            <a title="关闭" class="close" href="javascript:;">关闭</a><span id="selTitle">选择供应商</span></h2>
        <div class="con">
            <div class="top">
                <div rel="msg_box" class="fl">
                    <input class="text w100" id='qProductTitle' />
                    <a href='javascript:;' class='btn img' id='bQuery'><i class='icon i_search'></i>查询<b></b></a>
                </div>
            </div>
            <div class="main" style="height: 300px;">
                <iframe id="ifSel" width="100%" height="100%" frameborder="0"></iframe>
            </div>
        </div>
    </div>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    <a href="javascript:;" class="btn img back" rel="/Z10Move/MoveList"><i class="icon i_back">
                    </i>返回<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <% using (Html.BeginForm())
               {%>
            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title">
                        创建调拨单</h3>
                </caption>
                <tr>
                    <td class="tt">
                        调出仓库：
                    </td>
                    <td class="tc">
                        <select id="depotout">
                            <option value='0'>-请选择仓库- </option>
                            <%=ViewData["DepotOptions"] %>
                        </select>
                    </td>
                    <td class="tt">
                        调入仓库：
                    </td>
                    <td class="tc">
                        <select id="depotin">
                            <option value='0'>-请选择仓库- </option>
                            <%=ViewData["DepotOptions"] %>
                        </select>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        备注：
                    </td>
                    <td class="tc" colspan="3">
                        <%= Html.TextArea("Remark", Model.Z10Order.Remark, new { @class = "text w400", @style = "height:50px" })%>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div class="top">
                            <div class="con clearfix">
                                <div class="fl">
                                    <a href="javascript:;" id="xaddProduct" class="btn img" onclick=""><i class="icon i_create">
                                    </i>添加产品<b></b></a>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table width="100%" id='oItem' class="list-table f12">
                            <tr>
                                <th>
                                    产品名称
                                </th>
                                <th class="w100">
                                    库存数量
                                </th>
                                <th style="width: 120px">
                                    调拨数量
                                </th>
                                <th class="w100">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </td>
                </tr>
                <% if (ViewData["IsDetail"] == null)
                   { %>
                <tr class="action">
                    <td>
                    </td>
                    <td class="tc">
                        <input type="button" value="创建调拨单" id="xsubmit" class="gbutton mr20" />
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
