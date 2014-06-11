$(function () {
    $(".xok").click(function () {
        var $this = $(this);
        var countHappendTr = $this.parent().prev();
        var countHappend = countHappendTr.find("input[type=text]");
        var itemID = $this.prev().val();

        if (countHappend.val() == "") {
            $.jvalidate(countHappend, '请输入实际出库数量！', 58, 10);
            return;
        }
        else if (isNaN(countHappend.val())) {
            $.jvalidate(countHappend, '您还没有数据没保存！', 58, 10);
            return;
        }

        $.post("/Z10Order/ModiItemCountHappend2Session",
            { 'ItemID': itemID,
              'CountHappend': countHappend.val()
            }, function (data) {
                if (data == 1) {
                    parent.$.jshowtip("已保存！", "success");

                    countHappendTr.addClass("readonly");
                    countHappend.attr("readonly", true);
                    $this.next().show();
                    $this.hide();
                } else {
                    parent.$.jshowtip(data, "error", 2);
                    return;
                }
            }
        );
    });

    $(".xedit").click(function () {
        var countHappendTr = $(this).parent().prev();
        var countHappend = countHappendTr.find("input[type=text]");

        countHappendTr.removeClass("readonly");
        countHappend.removeAttr("readonly");
        $(this).prev().show();
        $(this).hide();
    });

    $("#xsubmit").click(function () {
        var readonly = false;
        var noreadonlytxt = null;
        $("#oItem input.text").each(function () {
            readonly = $(this).attr("readonly");

            if (!readonly) {
                noreadonlytxt = $(this);
                return false;
            }
        });

        if (!readonly) {
            $.jvalidate(noreadonlytxt, '您还有未保存的数据！', 58, 10);
            return;
        }

        var orderStatus = $("input[type=radio]:checked").val();
        if (orderStatus == undefined) {
            $.jvalidate($("tr.focus td.tc"), '您还没有选择单据完成进度！', 58);
            return;
        }

        $.post("/Z10SaleReturned/SavePutOut", { 'orderStatus': orderStatus },
        function (data) {
            if (data == 1) {
                parent.$.jshowtip("出库处理完成！", "success");
                parent.go2("/Z10SaleReturned/SaleReturnedList");
            }
            else {
                parent.$.jshowtip(data, "error", 1);
            }
        }
        );
    });
});