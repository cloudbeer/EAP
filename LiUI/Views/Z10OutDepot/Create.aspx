<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    新建出库单</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var Li = {};
        Li.isSubmit = true;

        function hideQuery() {
            $("#dSelPanel .top").hide();
        }
        function showQuery() {
            $("#dSelPanel .top").show();
        }

        $(function () {
        
            $("#btnCalFee").click(function(){
                var orderID = <%=Model.Z10Order.OrderID??0%>;
                var feeShip = parseFloat($("#FeeShip").val());
                $.post("/z10order/calfee/",{id:orderID,ship:feeShip},function(res){
                    feeTotal =  parseFloat(res); 
                    $("#FeeShould").val(feeShip+feeTotal);
                });
            });

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

                var $CustomerID = $("#CustomerID");
                var $DateOrder = $("#DateOrder");
                var $DateShip = $("#DateShip");
                var $Currency = $("#Currency");
                var $Remark = $("#Remark");

                if ($CustomerID.val() == "0" || $CustomerID.val() == "") {
                    $.jvalidate("xselcustomer", "请选择客户！", 58);
                    return false;
                }
                else if ($DateOrder.val() == "") {
                    $.jvalidate($DateOrder, "请输入下单日期！", 58);
                    return false;
                }
                else if ($DateShip.val() == "") {
                    $.jvalidate($DateShip, "请输入交货日期！", 58);
                    return false;
                }
                else if ($Currency.val() == "0") {
                    $.jvalidate($Currency, "请选择交易币种！", 54);
                    return false;
                }

                $.post("/Z10OutDepot/SaveOutDepot",
                    { 'CustomerID': $CustomerID.val(),
                        'DateOrder': $DateOrder.val(),
                        'DateShip': $DateShip.val(),
                        'Currency': $Currency.val(),
                        'Remark': $Remark.val(),
                     FeeShip:$("#FeeShip").val(),
                     FeeShould:$("#FeeShould").val(),
                    }, function (data) {
                        if (data == 1) {
                            Li.isSubmit = true;
                            parent.$.jshowtip("出库单已保存！", "success", 1);
                            location = "/Z10OutDepot/OutDepotList";
                        } else {
                            parent.$.jshowtip("系统错误，请联系管理员！", "error", 1);
                        }
                    }
                );
            });

            $("#DateOrder").val('<%=ViewData["CurrentDate"] %>');

            $("form").submit(function () {
                if ($("#Remark").val().length > 2000) { $.jvalidate('Remark', '您输入的内容太多。', 59); return false }
            });

            $(".detail.readonly input").attr("readonly", true);

            $("#xaddProduct").click(function () {
                if ($(this).attr("show") == "true")
                    parent.$.jshowtip("您还没有保存上一条产品信息！", "error");
                else {
                    $("#selTitle").html("选择产品");
                    $("#ifSel").attr("src", "/z10order/selectproduct_s1/");
                    $("#dSelPanel").show();
                }
                //$(this).hide();
            });

            $("#xselcustomer").click(function () {
                $("#selTitle").html("选择客户");
                $("#ifSel").attr("src", "/z10order/selectcustomer/?qCustomerType=<%=(int)EAP.Logic.Z01.CustomerTyps.Customer%>");
                $("#dSelPanel").show();
            });
            $("#bQuery").click(function () {
                var xurl = $("#ifSel").attr("src");
                if (xurl.toLocaleLowerCase().indexOf("/z10order/selectproduct_s1") >= 0) {
                    $("#ifSel").attr("src", "/z10order/selectproduct_s1?qTitle=" + encodeURIComponent($("#qTitle").val()));
                } else {
                    $("#ifSel").attr("src", "/z10order/selectcustomer?qTitle=" + encodeURIComponent($("#qTitle").val()));
                }
            });

            $("#DateOrder").bind("focus click", function () {
                WdatePicker({ maxDate: '#F{$dp.$D(\'DateShip\')||\'2100-10-01\'}', firstDayOfWeek: 1 })
            });

            $("#DateShip").bind("focus click", function () {
                WdatePicker({ minDate: '#F{$dp.$D(\'DateOrder\')}', maxDate: '2100-10-01', firstDayOfWeek: 1 });
            });
        });
        function SetCustomerID(id, tt) {
            $("#CustomerID").val(id);
            $("#xselcustomer").html(tt);
            $("#dSelPanel").hide();
        }
        function SetProductID(_pid, _ptitle, _price, _itemid, _color, _size, _depot) {
            html = "<tr>";
            html += "<td>";
            html += _ptitle + " " + _color + " " + _size + "  - 单价：" + _price;
            html += "<input type='hidden' class='_pid' value='" + _pid + "'/>";
            html += "<input type='hidden' class='_price' value='" + _price + "'/>";
            html += "<input type='hidden' class='_itemid' value='" + _itemid + "'/>";
            html += "<input type='hidden' class='_depot' value='" + _depot + "'/>";
            html += "</td>";

            html += "<td class='tr'><input class='text w100 tr item_price' value='" + _price + "' /></td>";
            html += "<td class='tr'><input class='text w100 tr item_countshould' value='1' /></td>";
            html += "<td>" + _depot + "</td>";
            html += "<td class='tr priceCount' rel='priceCount'></td>";
            html += "<td><a href='javascript:;' class='i_xok xok'>保存</a><a href='javascript:;' class='i_xdel xdel none'>删除</a><a href='javascript:;' class='i_xdel xcancel'>取消</a></td>";
            html += "</tr>";
            $("#oItem tr[rel=count]").before(html);
            $("#dSelPanel").hide();
        }

        $(".xdel").live("click", function () {
            if (!confirm("是否删除？"))
                return;
            var tr = $(this).parent().parent();
            var index = tr.index();
            $.post("/Z10Order/RemoveOutOrderItem/", { 'index': index }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("删除成功！", "success", 0.6);
                    tr.remove();
                }
            });
        });
        $(".xcancel").live("click", function () {
            if (!confirm("是否取消？"))
                return;
            var tr = $(this).parent().parent();
            parent.$.jshowtip("删除成功！", "success", 0.6);
            tr.remove();
        });


        $(".xok").live("click", function () {
            var _mum = $(this).parent().parent();

            _price = _mum.find(".item_price").val();
            _itemid = _mum.find("._itemid").val();
            _ptitle = _mum.find("._ptitle").val();
            _pid = _mum.find("._pid").val();
            _color = _mum.find("._color").val();
            _size = _mum.find("._size").val();
            _depot = _mum.find("._depot").val();
            _num = _mum.find(".item_countshould").val();
            
            if (_price == "") {
                $.jvalidate(_mum.find(".item_price"), "请填写商品价格！", 58);
                return false;
            }
            else if (isNaN(_price)) {
                $.jvalidate(_mum.find(".item_price"), "商品价格应该为数字！", 58);
                return false;
            }
            else if (_num == "") {
                $.jvalidate(_mum.find(".item_countshould"), "请填写商品数量！", 58);
                return false;
            }
            else if (isNaN(_num)) {
                $.jvalidate(_mum.find(".item_countshould"), "商品数量应该为数字！", 58);
                return false;
            }

            $.post("/Z10Order/SaveOutOrderItem/", {'ProductID': _pid, 'itemid': _itemid, 'Price': _price, 'CountShould': _num }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("保存成功", "success", 0.6);

                    _mum.addClass("readonly").find("input").attr("readonly", true);
                    _mum.find(".xok").hide();
                    _mum.find(".xcancel").hide();
                    _mum.find(".xdel").show();
                    $("#xaddProduct").attr("show", "false");
                } else {
                    parent.$.jshowtip(data, "error", 2);
                    return false;
                }
            });
        });


    </script>
    <div style="left: 261px; top: 10px; display: none" id="dSelPanel" class="pop-box w500">
        <h2 rel="title">
            <a title="关闭" class="close" href="javascript:;">关闭</a><span id="selTitle">选择客户</span></h2>
        <div class="con">
            <div class="top">
                <div rel="msg_box" class="fl">
                    <input class="text w100" id='qTitle' />
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
                    <a href="javascript:;" class="btn img back" rel="/Z10OutDepot/OutDepotList"><i class="icon i_back">
                    </i>返回<b></b></a> <a href="/Z10Order/ClearSessionOrder/?bUrl=/Z10OutDepot/OutDepot/"
                        class="btn img"><i class="icon i_create"></i>清空缓存并新建<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <% using (Html.BeginForm())
               {%>
            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title">
                        <%=ViewData["VTitle"]%></h3>
                </caption>
                <tr>
                    <td class="tt">
                        客户：
                    </td>
                    <td class="tc">
                        <%= Html.Hidden("CustomerID", Model.Z10Order.CustomerID) %>
                        <a href='javascript:;' id='xselcustomer' class="a2select w100">请选择客户</a>
                    </td>
                    <td class="tt">
                        下单日期 ~ 交货日期：
                    </td>
                    <td class="tc">
                        <%= Html.TextBox("DateOrder", Model.Z10Order.DateOrder, new { @class = "text w100"})%>
                        ~
                        <%= Html.TextBox("DateShip", Model.Z10Order.DateShip, new { @class = "text w100" })%>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        交易币种：
                    </td>
                    <td class="tc">
                        <select id="Currency" name="Currency" class="w100">
                            <option value="0">请选择交易币种</option>
                            <%=ViewData["MoneyOptions"]%>
                        </select>
                    </td>
                    <td class="tt">
                        备注：
                    </td>
                    <td class="tc">
                        <%= Html.TextBox("Remark", Model.Z10Order.Remark, new { @class = "text w300" })%>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td class="tt">
                        费用：
                    </td>
                    <td class="tc">
                        运费：<%= Html.TextBox("FeeShip", (Model.Z10Order.FeeShip??0).ToString("0.##"), new { @class = "text w100" })%>
                        合同额：<%= Html.TextBox("Total", (Model.Z10Order.Total??0).ToString("0.##"), new { @class = "text w100 readonly" })%>
                    </td>
                    <td class="tt">
                        应付：
                    </td>
                    <td class="tc">
                        <%= Html.TextBox("FeeShould", (Model.Z10Order.FeeShould??0).ToString("0.##"), new { @class = "text w100" })%>
                        <input type="button" value="计算" id="btnCalFee" />
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
                                <th>
                                    出售价格
                                </th>
                                <th>
                                    数量
                                </th>
                                <th class="w200">
                                    仓库
                                </th>
                                <th>
                                    小计
                                </th>
                                <th>
                                    操作
                                </th>
                            </tr>
                            <% var tempOrder = EAP.Logic.Z10.Order.CreateWithSession();
                               foreach (var item in tempOrder.Items)
                               { %>
                            <tr class="readonly">
                                <td>
                                    <%=Zippy.Data.StaticDB.DB.FindUnique<Z01Beetle.Entity.Z01Product>("ProductID="+(item.ProductID??0), "Title")%>
                                    <input type="hidden" value="<%=item.ProductID %>" class='productID' readonly="readonly" />
                                </td>
                                <td class="tr">
                                    <input value="<%=(item.Price??0).ToString("0.##") %>" class="text w100 tr item_price"
                                        readonly="readonly" />
                                </td>
                                <td class="tr">
                                    <input value="<%=(item.CountShould??0).ToString("0.##") %>" class="text w100 tr item_countshould"
                                        readonly="readonly" />
                                </td>
                                <td>
                                    大浪店
                                </td>
                                <td rel="priceCount" class="tr priceCount">
                                </td>
                                <td>
                                    <a class="i_xdel xdel none" href="javascript:;" style="display: inline;">删除</a>
                                </td>
                            </tr>
                            <%} %>
                            <tr class="focus readonly" rel="count">
                                <td class="count">
                                    合计
                                </td>
                                <td class="tr" rel="pCount">
                                    <input class='text w100 tr' value="" />
                                </td>
                                <td class="tr" rel="sCount">
                                    <input class='text w100 tr' value="" />
                                </td>
                                <td>
                                </td>
                                <td class="tr" rel="cCount">
                                </td>
                                <td>
                                </td>
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
                        <input type="button" value="保存" id="xsubmit" class="gbutton mr20" />
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
