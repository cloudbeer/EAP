<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EAP.Logic.Z10.Order>" %>

<%@ Import Namespace="Z10Cabbage.Entity" %>
<%@ Import Namespace="Z10Cabbage.Entity.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    创建进销存订单
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var Li = {};
        Li.isSubmit = true;

        
        $(function () {        
            $("#xsubmit").hide();
            $("#btnCalFee").click(function(){
                if ($("#oItem tr.readonly").length+1!=$("#oItem tr").length){
                    parent.$.jshowtip("您还没有保存上一条产品信息！" , "error");
                    return false;
                }
                var orderID = <%=Model.Z10Order.OrderID??0%>;
                var feeShip = parseFloat($("#FeeShip").val());
                $.post("/z10order/calfee/",{id:orderID,ship:feeShip},function(res){
                    feeTotal =  parseFloat(res); 
                    $("#FeeShould").val(feeShip+feeTotal);
                    $("#xsubmit").show();
                });
            });
            
            $("#btnSaveFee").click(function(){
                var orderID = <%=Model.Z10Order.OrderID??0%>;
                var feeShip = parseFloat($("#FeeShip").val());
                var feeShould = parseFloat($("#FeeShould").val());
                $.post("/z10order/savefee/",{id:orderID,ship:feeShip,feeShould:feeShould},function(res){
                    if (res=="1"){
                            parent.$.jshowtip("费用已经保存！", "success", 1);                        
                    }
                });
            });


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
                if ($(".xaddProduct").attr("show") == "true"){
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

                $.post("/Z10Order/PurchaseSave", 
                    {'CustomerID': $CustomerID.val(),
                     'DateOrder': $DateOrder.val(),
                     'DateShip' : $DateShip.val(),
                     'Currency' : $Currency.val(),
                     'Remark' : $Remark.val(),
                     FeeShip:$("#FeeShip").val(),
                     FeeShould:$("#FeeShould").val(),
                    }, function (data) {
                        if(data == 1){
                            Li.isSubmit = true;
                            parent.$.jshowtip("采购单已保存！", "success", 1);
                            location = "/Z10Order/PurchaseList";
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


            $(".xaddProduct").click(function () {
                if ($("#oItem tr.readonly").length+1!=$("#oItem tr").length){
                    parent.$.jshowtip("您还没有保存上一条产品信息！" , "error");
                    return;
                }
                $("#xsubmit").hide();
                if ($(this).attr("show") == "true")
                    parent.$.jshowtip("您还没有保存上一条产品信息！", "error");
                else {
                    $("#selTitle").html("选择产品");
                    $("#ifSel").attr("src", "/z10order/selectproduct/");
                    $("#dSelPanel").show();
                }
                //$(this).hide();
            });

            $("#xselcustomer").click(function () {
                $("#selTitle").html("选择供应商");
                $("#ifSel").attr("src", "/z10order/selectcustomer?qCustomerType=<%=(int)EAP.Logic.Z01.CustomerTyps.Supplier %>");
                $("#dSelPanel").show();
            });
            $("#bQuery").click(function () {
                var xurl = $("#ifSel").attr("src");
                if (xurl.toLocaleLowerCase().indexOf("/z10order/selectproduct") >= 0) {
                    $("#ifSel").attr("src", "/z10order/selectproduct?qTitle=" + encodeURIComponent($("#qTitle").val()));
                } else {
                    $("#ifSel").attr("src", "/z10order/selectcustomer?qCustomerType=<%=(int)EAP.Logic.Z01.CustomerTyps.Supplier %>&qTitle=" + encodeURIComponent($("#qTitle").val()));
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
        function SetProductID(id, tt,price) {
            html = "<tr>";
            html += "<td><input type='hidden' class='productID' value='" + id + "'/>" + tt + "</td>";
            html += "<td  class='item_color'><input class='text w100' /></td>";
            html += "<td  class='item_size'><input class='text w100' /></td>";
            html += "<td class='tr item_price' rel='item_price'><input class='text w100 tr' value='"+ price +"' /></td>";
            html += "<td><span class='select none'></span><select><option value='0'> -请选择仓库- </option><%=ViewData["DepotOptions"] %></select><input type='hidden' value=''/></td>";
            html += "<td class='tr item_countshould' rel='item_countshould'><input class='text w100 tr' value='1' /></td>";
            html += "<td class='tr priceCount' rel='priceCount'></td>";
            html += "<td><a href='javascript:;' class='i_xok xok'>保存</a><a href='javascript:;' class='i_xok2 xok2 none'>保存</a><a href='javascript:;' class='i_xdel xdel'>删除</a></td>";
            html += "</tr>";
            $("#oItem tr[rel=count]").before(html);
            $("#dSelPanel").hide();
        }

        $(".xdel").live("click",function(){
            var delbutton = $(this);
            var tr = $(this).parent().parent();
            var pid = tr.find("input[type=hidden]").val();
            var index = tr.index();
            $.post("/Z10Order/RemoveItem2Session", {'index': index,'productid' : pid }, function (data) {
                if(data == 1){
                    parent.$.jshowtip("删除成功！", "success", 0.6);
                    tr.remove();
                    $(".xaddProduct").attr("show", "false");
                    
                    var scount = 0;
                    $("td.item_countshould input").each(function(){
                        scount += Number($(this).val());
                    });

                    $("td.sCount input").val(scount);

                    var ccount = 0;
                    $("td.priceCount").each(function(){
                        ccount += Number($(this).text());
                    });
                    $("td.cCount").text(ccount);
                }
            });
        });

        $(".xok").live("click", function () {
            var tr = $(this).parent().parent();
            var color =tr.find(".item_color>input");
            var size =tr.find(".item_size>input");
            var price =tr.find(".item_price>input");
            var num =tr.find(".item_countshould>input");
            var pid = tr.find(".productID").val();
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


            var scount = 0;
            $("td.item_countshould input").each(function(){
                scount += Number($(this).val());
            });

            $("td.sCount input").val(scount);

            var ccount = 0;
            $("td.priceCount").each(function(){
                ccount += Number($(this).text());
            });
            $("td.cCount").text(ccount);

            var enter = $(this);
            var index = tr.index();
            $.post("/Z10Order/SaveItem2Session", {'index': index, 'ProductID': pid, 'DepotID': select.val(),'Price': price.val(), 'CountShould': num.val(),ExtColor:color.val(),ExtSize:size.val() }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("保存成功", "success", 0.6);

                    tr.addClass("readonly").find("input").attr("readonly", true);
                    selectText.text(select.text());
                    selectVal.val(select.val());
                    selectText.show();
                    enter.next().show();
                    enter.remove();
                    $(".xaddProduct").attr("show", "false");
                    tr.find(".xok2").hide();
                } else {
                    parent.$.jshowtip(data, "error", 5);
                    return false;
                }
            });
        });

        $(".xok2").live("click", function () {
                    
            var tr = $(this).parent().parent();
            var color =tr.find(".item_color>input");
            var size =tr.find(".item_size>input");
            var price =tr.find(".item_price>input");
            var num =tr.find(".item_countshould>input");
            var pid = tr.find(".productID").val();
            var select = tr.find("select option:selected");
            var title = tr.find(">td:first").text();
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
            

            var scount = 0;
            $("td.item_countshould input").each(function(){
                scount += Number($(this).val());
            });

            $("td.sCount input").val(scount);

            var ccount = 0;
            $("td.priceCount").each(function(){
                ccount += Number($(this).text());
            });
            $("td.cCount").text(ccount);

            var enter = $(this);
            var index = tr.index();
            $.post("/Z10Order/ModiItem2Session", {'index': index, 'ProductID': pid, 'Title': title,'DepotID': select.val(),'Price': price.val(), 'CountShould': num.val(),ExtColor:color.val(),ExtSize:size.val() }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("\u7f16\u8f91\u6210\u529f", "success", 0.6);

                    tr.addClass("readonly").find("input").attr("readonly", true);
                    selectText.text(select.text());
                    selectVal.val(select.val());
                    selectText.show();
                    enter.hide();
                    enter.prev().show();
                    $(".xaddProduct").attr("show", "false");
                } else {
                    parent.$.jshowtip(data, "error", 5);
                    return false;
                }
            });
        });

        $(".xedit").live("click", function () {
            $(".xaddProduct").attr("show","true");
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
    <%
        
    %>
    <div style="left: 261px; top: 10px; display: none" id="dSelPanel" class="pop-box w500">
        <h2 rel="title">
            <a title="关闭" class="close" href="javascript:;">关闭</a><span id="selTitle">选择供应商</span></h2>
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
                    <a href="javascript:;" class="btn img back" rel="/Z10Order/PurchaseList/"><i class="icon i_back">
                    </i>返回<b></b></a> <a href="/Z10Order/ClearSessionOrder/?bUrl=/Z10Order/Purchase/"
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
                        供应商：
                    </td>
                    <td class="tc">
                        <%= Html.Hidden("CustomerID", Model.Z10Order.CustomerID) %>
                        <%
                   var customerLabel = "请选择供应商";
                   if ((Model.Z10Order.CustomerID ?? 0) > 0)
                   {
                       var xLabel = Zippy.Data.StaticDB.DB.FindUnique<Z01Beetle.Entity.Z01Customer>("CustomerID=" + Model.Z10Order.CustomerID.Value, "Title");
                       if (xLabel != null)
                           customerLabel = xLabel.ToString();
                   }
                        %>
                        <a href='javascript:;' id='xselcustomer' class="a2select w100">
                            <%=customerLabel %></a>
                    </td>
                    <td class="tt">
                        下单日期 ~ 交货日期：
                    </td>
                    <td class="tc">
                        <%= Html.TextBox("DateOrder", Model.Z10Order.DateOrder.HasValue ? Model.Z10Order.DateOrder.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"), new { @class = "text w100" })%>
                        ~
                        <%= Html.TextBox("DateShip", Model.Z10Order.DateShip.HasValue?Model.Z10Order.DateShip.Value.ToString("yyyy-MM-dd"):"", new { @class = "text w100" })%>
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
                        <%--<input type="button" value="保存" id="btnSaveFee" />--%>
                    </td>
                    <td width="*">
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div class="top">
                            <div class="con clearfix">
                                <div class="fl">
                                    <a href="javascript:;" class="btn img xaddProduct"><i class="icon i_create"></i>添加产品<b></b></a>
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
                                    颜色
                                </th>
                                <th>
                                    尺寸
                                </th>
                                <th>
                                    单价
                                </th>
                                <th class="w200">
                                    仓库
                                </th>
                                <th>
                                    数量
                                </th>
                                <th>
                                    小计
                                </th>
                                <th>
                                    操作
                                </th>
                            </tr>
                            <% var tempOrder = EAP.Logic.Z10.Order.CreateWithSession();
                               
                            %>
                            <% foreach (var item in tempOrder.Items)
                               { %>
                            <tr class="readonly">
                                <td>
                                    <input type="hidden" value="<%=item.ProductID %>" class='productID' readonly="readonly" />
                                    <%=Zippy.Data.StaticDB.DB.FindUnique<Z01Beetle.Entity.Z01Product>("ProductID="+(item.ProductID??0), "Title")%>
                                </td>
                                <td class='item_color'>
                                    <input class="text w100" value="<%=item.ExtColor%>" readonly="readonly" />
                                </td>
                                <td>
                                    <input class="text w100" value="<%=item.ExtSize%>" readonly="readonly" />
                                </td>
                                <td class='tr item_price' rel='item_price'>
                                    <input class="text w100 tr" value="<%=(item.Price??0).ToString("0.##")%>" readonly="readonly" />
                                </td>
                                <td>
                                    <%
                                   long depotID = item.DepotID ?? 0;
                                    %>
                                    <span class="select none">
                                        <%=Zippy.Data.StaticDB.DB.FindUnique<Z10Cabbage.Entity.Z10Depot>("DepotID=" + depotID, "Title")%></span>
                                    <select>
                                        <option value='0'>-请选择仓库- </option>
                                        <%=ViewData["DepotOptions"] %>
                                    </select>
                                    <input type="hidden" value="<%=item.DepotID%>" readonly="readonly" />
                                </td>
                                <td class="tr item_countshould" rel="item_countshould">
                                    <input class="text w100 tr" value="<%=(item.CountShould??0).ToString("0.##")%>" readonly="readonly" />
                                </td>
                                <td class='tr priceCount'>
                                    <%=((item.Price ?? 0) * (item.CountShould ?? 0)).ToString("0.##")%>
                                </td>
                                <td>
                                    <a class="i_xdel xdel" href="javascript:;">删除</a>
                                </td>
                            </tr>
                            <%} %>
                            <tr class="focus readonly" rel="count">
                                <td class="count">
                                    合计
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="tr sCount" rel="sCount">
                                    <input class='text w100 tr' value=" <%=(tempOrder.Items.Sum(s=>s.CountShould)??0).ToString("0.##") %>" />
                                </td>
                                <td class="tr cCount" rel="cCount">
                                    <%=(tempOrder.Items.Sum(s=>s.CountShould*s.Price)??0).ToString("0.##") %>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div class="top">
                            <div class="con clearfix">
                                <div class="fl">
                                    <a href="javascript:;" class="btn img xaddProduct"><i class="icon i_create"></i>添加产品<b></b></a>
                                </div>
                            </div>
                        </div>
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
