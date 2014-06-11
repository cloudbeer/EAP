<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
销售退货
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var Li = {};
        Li.isSubmit = true;

        $(function () {
            $("#oItem tr td select").each(function(){
                var selectText = $(this).prev();
                
                var selectVal = $(this).next();
                
                var option = $(this).find("option");

                option.each(function(){
                    if($(this).val() == selectVal.val()){
                        selectText.text($(this).text());
                    }
                });
            });

            /*window.onbeforeunload = function(){
                if(!Li.isSubmit){
                    return "点击“确认”未保存的数据将会丢失！";
                }
            };*/

            $(document).click(function(){
                Li.isSubmit = false;
            });

            $("#xsubmit").click(function(){
                if ($("#xaddProduct").attr("show") == "true"){
                    parent.$.jshowtip("您还没有保存正在编辑的产品信息！", "error", 2);
                    return;
                }

                var $CustomerID = $("#CustomerID");
                var $DateOrder = $("#DateOrder");
                var $Currency = $("#Currency");
                var $Remark = $("#Remark");

                if ($CustomerID.val() == "0" || $CustomerID.val() == "") {
                    $.jvalidate("xselcustomer", "请选择供应商！",58);
                    return false;
                }
                else if ($DateOrder.val() == "") {
                    $.jvalidate($DateOrder, "请输入退货日期！", 58);
                    return false;
                }
                else if ($Currency.val() == "0") {
                    $.jvalidate($Currency, "请选择交易币种！", 54);
                    return false;
                }

                $.post("/Z10SaleReturned/doReturned", 
                    {'CustomerID': $CustomerID.val(),
                     'DateOrder': $DateOrder.val(),
                     'Currency' : $Currency.val(),
                     'Remark' : $Remark.val()
                    }, function (data) {
                        if(data == 1){
                            Li.isSubmit = true;
                            parent.$.jshowtip("退货单已保存！", "success", 1);
                            location = "/Z10SaleReturned/SaleReturnedList";
                        }else{
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
                    $("#ifSel").attr("src", "/Z10Order/selectproduct");
                    $("#dSelPanel").show();
                }
                //$(this).hide();
            });

            $("#xselcustomer").click(function () {
                $("#selTitle").html("选择客户");
                $("#ifSel").attr("src", "/z10order/selectcustomer?qCustomerType=<%=(int)EAP.Logic.Z01.CustomerTyps.Customer %>");
                $("#dSelPanel").show();
            });
            $("#bQuery").click(function () {
                var xurl = $("#ifSel").attr("src");
                if (xurl.toLocaleLowerCase().indexOf("/z10order/selectproduct") >= 0) {
                    $("#ifSel").attr("src", "/z10order/selectproduct?qTitle=" + encodeURIComponent($("#qTitle").val()));
                } else {
                    $("#ifSel").attr("src", "/z10order/selectcustomer?qCustomerType=<%=(int)EAP.Logic.Z01.CustomerTyps.Customer %>&qTitle=" + encodeURIComponent($("#qTitle").val()));
                }
            });

            $("#DateOrder").bind("focus click", function(){
                WdatePicker({maxDate:'#F{\'2100-10-01\'}',firstDayOfWeek : 1})
            });
        });
        function SetCustomerID(id, tt) {
            $("#CustomerID").val(id);
            $("#xselcustomer").html(tt);
            $("#dSelPanel").hide();
        }
        function SetProductID(id, tt) {
            html = "<tr>";
            html += "<td><input type='hidden' value='" + id + "'/>" + tt + "</td>";
            html += "<td class='tr' rel='item_price'><input class='text w100 tr' /></td>";
            html += "<td class='tr' rel='item_countshould'><input class='text w100 tr' /></td>";
            html += "<td><span class='select none'></span><select><option value='0'> -请选择仓库- </option><%=ViewData["DepotOptions"] %></select><input type='hidden' value=''/></td>";
            html += "<td class='tr priceCount' rel='priceCount'></td>";
            html += "<td><a href='javascript:;' class='i_xok xok'>保存</a> <a href='javascript:;' class='i_xedit xedit none'>编辑</a><a href='javascript:;' class='i_xok2 xok2 none'>保存</a><a href='javascript:;' class='i_xdel xdel'>删除</a></td>";
            html += "</tr>";
            $("#oItem tr[rel=count]").before(html);
            $("#dSelPanel").hide();
        }

        $(".xdel").live("click",function(){
            var tr = $(this).parent().parent();
            var pid = tr.find("input[type=hidden]").val();
            var index = tr.index();
            $.post("/Z10Order/RemoveItem2Session", {'index': index,'productid' : pid }, function (data) {
                if(data == 1){
                    parent.$.jshowtip("删除成功！", "success", 0.6);
                    tr.remove();
                    $("#xaddProduct").attr("show", "false");
                }
            });
        });

        $(".xok").live("click", function () {
            var price = $(this).parent().prev().prev().prev().prev().find("input");
            var num = $(this).parent().prev().prev().prev().find("input");

            var tr = $(this).parent().parent();
            var pid = tr.find("input[type=hidden]").val();
            var select = tr.find("select option:selected");
            var selectText = select.parent().prev();
            var selectVal = select.parent().next();

            if (price.val() == "") {
                $.jvalidate(price, "请填写商品价格！", 58);
                return false;
            }
            else if (isNaN(price.val())) {
                $.jvalidate(price, "商品价格应该为数字！", 58);
                return false;
            }
            else if (num.val() == "") {
                $.jvalidate(num, "请填写商品数量！", 58);
                return false;
            }
            else if (isNaN(num.val())) {
                $.jvalidate(num, "商品数量应该为数字！", 58);
                return false;
            }
            else if (select.val() == 0) {
                $.jvalidate(select.parent(), "您还没有选择仓库！", 58);
                return false;
            }

            $(this).parent().prev().text(price.val() * num.val());

            var pcount = 0;
            $("td[rel=item_price] input").each(function(){
                pcount += Number($(this).val());
            });

            $("td[rel=pCount] input").val(pcount);

            var scount = 0;
            $("td[rel=item_countshould] input").each(function(){
                scount += Number($(this).val());
            });

            $("td[rel=sCount] input").val(scount);

            var ccount = 0;
            $("td[rel=priceCount]").each(function(){
                ccount += Number($(this).text());
            });
            $("td[rel=cCount]").text(ccount);

            var enter = $(this);
            var index = tr.index();
            $.post("/Z10Order/SaveItem2Session", {'index': index, 'ProductID': pid, 'DepotID': select.val(),'Price': price.val(), 'CountShould': num.val() }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("保存成功", "success", 0.6);

                    tr.addClass("readonly").find("input").attr("readonly", true);
                    selectText.text(select.text());
                    selectVal.val(select.val());
                    selectText.show();
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
            var price = $(this).parent().prev().prev().prev().prev().find("input");
            var num = $(this).parent().prev().prev().prev().find("input");

            var tr = $(this).parent().parent();
            var pid = tr.find("input[type=hidden]").val();
            var title = tr.find(">td:first").text();
            var select = tr.find("select option:selected");
            var selectText = select.parent().prev();
            var selectVal = select.parent().next();

            if (price.val() == "") {
                $.jvalidate(price, "请填写商品价格！", 58);
                return false;
            }
            else if (isNaN(price.val())) {
                $.jvalidate(price, "商品价格应该为数字！", 58);
                return false;
            }
            else if (num.val() == "") {
                $.jvalidate(num, "请填写商品数量！", 58);
                return false;
            }
            else if (isNaN(num.val())) {
                $.jvalidate(num, "商品数量应该为数字！", 58);
                return false;
            }
            else if (select.val() == 0) {
                $.jvalidate(select.parent(), "您还没有选择仓库！", 58);
                return false;
            }

            $(this).parent().prev().text(price.val() * num.val());

            var pcount = 0;
            $("td[rel=item_price] input").each(function(){
                pcount += Number($(this).val());
            });

            $("td[rel=pCount] input").val(pcount);

            var scount = 0;
            $("td[rel=item_countshould] input").each(function(){
                scount += Number($(this).val());
            });

            $("td[rel=sCount] input").val(scount);

            var ccount = 0;
            $("td[rel=priceCount]").each(function(){
                ccount += Number($(this).text());
            });
            $("td[rel=cCount]").text(ccount);

            var enter = $(this);
            var index = tr.index();
            $.post("/Z10Order/ModiItem2Session", {'index': index, 'ProductID': pid, 'Title': title,'DepotID': select.val(),'Price': price.val(), 'CountShould': num.val() }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("\u7f16\u8f91\u6210\u529f", "success", 0.6);

                    tr.addClass("readonly").find("input").attr("readonly", true);
                    selectText.text(select.text());
                    selectVal.val(select.val());
                    selectText.show();
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
            $("#xaddProduct").attr("show","true");
            var tr = $(this).parent().parent();

            tr.removeClass("readonly").find("input").removeAttr("readonly");

            var select = tr.find("select");
            var selectText = select.prev();
            var selectVal = select.next();
            select.val(selectVal.val());
            selectText.hide();
            $(this).hide();
            $(this).next().show();
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
                    <a href="javascript:;" class="btn img back" rel="/Z10Returned/ReturnedList"><i class="icon i_back"></i>返回<b></b></a>
                </div>
            </div>
        </div>
        <div id="main">
            <% using (Html.BeginForm())
               {%>
            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title">销售退货</h3>
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
                        退货日期：
                    </td>
                    <td class="tc">
                        <%= Html.TextBox("DateOrder", Model.Z10Order.DateOrder, new { @class = "text w100"})%>
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
                    <td colspan="5">
                        <div class="top">
                            <div class="con clearfix">
                                <div class="fl">
                                    <a href="javascript:;" id="xaddProduct" class="btn img" onclick=""><i class="icon i_create">
                                    </i>添加退货产品<b></b></a>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table width="100%" id='oItem' class="list-table f12">
                            <tr>
                                <th>产品名称</th>
                                <th>单价</th>
                                <th>数量</th>
                                <th class="w200">仓库</th>
                                <th>小计</th>
                                <th>操作</th>
                            </tr>
                            <tr class="focus readonly" rel="count">
                                <td class="count">合计</td>
                                <td class="tr" rel="pCount"><input class='text w100 tr' value="" /></td>
                                <td class="tr" rel="sCount"><input class='text w100 tr' value="" /></td>
                                <td></td>
                                <td class="tr" rel="cCount"></td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <% if (ViewData["IsDetail"] == null)
                   { %>
                <tr class="action">
                    <td></td>
                    <td class="tc">
                        <input type="button" value="创建退货单" id="xsubmit" class="gbutton mr20" />
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <%} %>
            </table>
            <%} %>
        </div>
    </div>
</asp:Content>
