<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    编辑出库单
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

            /*$(window).unload(function(){
                if(!Li.isSubmit){
                    return "您的数据已做出修改，离开页面将无法保存您的修改！";
                }else{
                    return false;
                }
            });*/

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
                var $DateShip = $("#DateShip");
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
                
                $.post("/Z10Returned/EditSave", {
                 'id': $("#OrderID").val(),
                 'CustomerID': $CustomerID.val(),
                 'DateOrder': $DateOrder.val(),
                 'DateShip' : $DateShip.val(),
                 'Currency' : $Currency.val(),
                 'Remark' : $Remark.val()
                }, function (data) {
                        if(data == 1){
                            Li.isSubmit = true;
                            parent.$.jshowtip("采购单已保存！", "success", 1,null,function(){
                                parent.go2("/Z10Returned/ReturnedList");
                            });
                        }else{
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
                if ($(this).attr("show") == "true")
                    parent.$.jshowtip("您还没有保存上一条产品信息！", "error", 2);
                else {
                    $("#selTitle").html("选择产品");
                    $("#ifSel").attr("src", "/z10order/selectproduct");
                    $("#dSelPanel").show();
                }
            });

            $("#xselcustomer").click(function () {
                $("#selTitle").html("选择客户");
                $("#ifSel").attr("src", "/z10order/selectcustomer");
                $("#dSelPanel").show();
            });
            $("#bQuery").click(function () {
                var xurl = $("#ifSel").attr("src");
                if (xurl.toLocaleLowerCase().indexOf("/z10order/selectproduct") >= 0) {
                    $("#ifSel").attr("src", "/z10order/selectproduct?qTitle=" + encodeURIComponent($("#qTitle").val()));
                } else {
                    $("#ifSel").attr("src", "/z10order/selectcustomer?qTitle=" + encodeURIComponent($("#qTitle").val()));
                }
            });

            $("#DateOrder").bind("focus click", function(){
                WdatePicker({maxDate:'#F{$dp.$D(\'DateShip\')||\'2100-10-01\'}',firstDayOfWeek : 1})
            });

            $("#DateShip").bind("focus click", function(){
                WdatePicker({minDate:'#F{$dp.$D(\'DateOrder\')}',maxDate:'2100-10-01',firstDayOfWeek : 1});
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
            var itemId = tr.find("input[type=hidden]").attr("rel");
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
            $.post("/Z10Order/ModiItem2Session", {'index': index,'ItemID': itemId, 'ProductID': pid, 'Title': title,'DepotID': select.val(),'Price': price.val(), 'CountShould': num.val() }, function (data) {
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
    <% Zippy.Data.IDalProvider db = ViewData["db"] as Zippy.Data.IDalProvider; %>
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
            <%=Html.Hidden("OrderID", Model.Z10Order.OrderID)%>
            <table cellspacing="0" cellpadding="0" width="100%" border="0" class="detail <% if (ViewData["IsDetail"]!=null){ Response.Write("readonly");}%>">
                <caption>
                    <h3 id="title">修改出库单</h3>
                </caption>
                <tr>
                    <td class="tt">
                        供应商：
                    </td>
                    <td class="tc">
                        <%= Html.Hidden("CustomerID", Model.Z10Order.CustomerID) %>
                        <% var customer = Model.Z10Order.GetCustomer(db); %>
                        <a href='javascript:;' id='xselcustomer' class="a2select w100"><%= customer != null ? customer.Title : "请选择供应商"%></a>
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
                        <script type="text/javascript">
                            $("#Currency").val('<%=Model.Z10Order.Currency %>');
                        </script>
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
                                <th class="tc">产品名称</th>
                                <th class="tc">单价</th>
                                <th class="tc">数量</th>
                                <th class="w200 tc">仓库</th>
                                <th class="tc">小计</th>
                                <th class="tc">操作</th>
                            </tr>
                            <%
                               decimal sCount = 0;
                               decimal tCount = 0;
                               decimal cCount = 0;
                            %>
                            <% foreach (Z10Cabbage.Entity.Z10OrderItem item in Model.Items)
                               { %>
                            <tr class="readonly">
                                <% var product = item.GetProduct(db); %>
                                <td><input type='hidden' value='<%=item.ProductID %>' rel='<%=item.ItemID %>'/><%=product != null ? product.Title : ""%></td>
                                <td class="tr" rel="item_price"><input class='text w100 tr' value="<%=(item.Price??0).ToString("0.##") %>" /></td>
                                <td class="tr priceCount" rel="item_countshould"><input class='text w100 tr' value="<%=(item.CountShould??0).ToString("0.##") %>"/></td>
                                <% sCount += item.CountShould ?? 0;%>
                                <td>
                                    <span class="select"></span>
                                    <select>
                                        <option value="0"> -请选择仓库- </option>
                                        <%=ViewData["DepotOptions"] %>
                                    </select>
                                    <input type="hidden" value="<%=item.DepotID %>"/>
                                </td>
                                <td class="tr" rel="priceCount"><% tCount = (item.Price * item.CountShould) ?? 0; Response.Write(tCount.ToString("0.##")); %></td>
                                <% cCount += tCount; %>
                                <td>
                                <a href='javascript:;' class='i_xedit xedit'>编辑</a>
                                <a href='javascript:;' class='i_xok2 xok2 none'>保存</a>
                                <a href='javascript:;' class='i_xdel xdel'>删除</a>
                                </td>
                            </tr>
                            <%} %>
                            <tr class="focus readonly" rel="count">
                                <td class="count">合计</td>
                                <td class="tr" rel="pCount"></td>
                                <td class="tr" rel="sCount"><input class='text w100 tr' value="<%=sCount.ToString("0.##") %>" /></td>
                                <td></td>
                                <td class="tr" rel="cCount"><%=cCount.ToString("0.##")%></td>
                                <td></td>
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
