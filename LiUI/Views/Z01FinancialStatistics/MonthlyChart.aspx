<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Chart
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/Content/FusionCharts/FusionCharts.js" charset="utf-8"></script>
    <script type="text/javascript">
        $(function () {
            var xwidth, xheight;
            var myChart;

            xwidth = $(document).width() - 40;
            xheight = $(document).height() - $("#top").height() - 40;

            $("#bQuery").click(function () {
                xDateFrom = encodeURIComponent($("#qDateFrom").val());
                xDateTo = encodeURIComponent($("#qDateTo").val());
                xCurrency = encodeURIComponent($("#qCurrency").val());
                xIn = encodeURIComponent($("#qIn").val());
                xDuration = encodeURIComponent($("#qDuration").val());

                daysDiff = compareDate(xDateTo, xDateFrom, "-");

                if (xDuration == "1") { //day
                    if (daysDiff > 10) {
                        parent.$.jshowtip("间隔时间不允许超过 10 天。", "error");
                        return;
                    }
                }
                else {
                    if (daysDiff > 366) {
                        parent.$.jshowtip("间隔时间不允许超过 12 个月。", "error");
                        return;
                    }
                }
                url = "/Z01FinancialStatistics/GetMonthlyChartXml?qDateFrom=" + xDateFrom +
                "&qDateTo=" + xDateTo + "&qCurrency=" + xCurrency + "&qIn=" + xIn + "&qDuration=" + xDuration

                xswf = $("#qSwf").val();
                myChart = new FusionCharts("/Content/FusionCharts/" + xswf + ".swf", "mychart01", xwidth, xheight, "0", "0");
                myChart.setTransparent(true);
                myChart.setAttribute("lang", 'zh-cn');

                $("#xchart").html("");
                myChart.setDataURL(encodeURIComponent(url));
                myChart.render("xchart");
            });




            $("#qDateFrom").bind("focus click", function () {
                WdatePicker({ maxDate: '#F{$dp.$D(\'qDateTo\')}', firstDayOfWeek: 1 })
            });

            $("#qDateTo").bind("focus click", function () {
                WdatePicker({ minDate: '#F{$dp.$D(\'qDateFrom\')}', firstDayOfWeek: 1 });
            });



        });

        function compareDate(first, second, sign) {
            fArray = first.split(sign);
            sArray = second.split(sign);
            var fDate = new Date(fArray[0], fArray[1], fArray[2]);
            var sDate = new Date(sArray[0], sArray[1], sArray[2]);

            var t = Math.abs(fDate.getTime() - sDate.getTime());
            var days = t / (1000 * 60 * 60 * 24);
            return days;
        }

    </script>
    <div id="contents">
        <div id="top">
            <div class="con clearfix">
                <div class="fl">
                    时段：<%=Html.TextBox("qDateFrom", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"), new { @class="text w80"})%>
                    -
                    <%=Html.TextBox("qDateTo", DateTime.Now.ToString("yyyy-MM-dd"), new { @class = "text w80" })%>
                    币种：<select id="qCurrency">
                        <%=ViewData["CurrencyOptions"]%>
                    </select>
                    类型：<select id="qIn">
                        <option value="1">收入</option>
                        <option value="0">支出</option>
                    </select>
                    按：<select id="qDuration">
                        <option value="1">日</option>
                        <option value="0">月</option>
                    </select>
                    显示：<select id="qSwf">
                        <option value="FCF_Column3D">柱状图（3D）</option>
                        <option value="FCF_Column2D">柱状图（2D）</option>
                        <option value="FCF_Pie3D">饼图（3D）</option>
                        <option value="FCF_Pie2D">饼图（2D）</option>
                        <option value="FCF_Line">线性图</option>
                    </select>
                    <a href="javascript:;" id="bQuery">确认</a>
                </div>
            </div>
        </div>
    </div>
    <div id="main">
        <div id="xchart" style="margin: 20px">
        </div>
    </div>
</asp:Content>
