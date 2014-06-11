$(function () {
    $("#xsubmit").click(function () {
        var payAccount = $("#PayAccount");
        var feePaid = $("#FeePaid");

        if (payAccount.val() == "0") {
            $.jvalidate(payAccount, "请选择支出帐户！", 52);
            return;
        }

        if (feePaid.val() == "") {
            $.jvalidate(feePaid, "请填写当前支付金额！", 56);
            return;
        }

        var orderStatus = $("input[type=radio]:checked").val();
        if (orderStatus == undefined) {
            $.jvalidate($("tr.focus td.tc"), '您还没有选择单据完成进度！', 58);
            return;
        }


        $.post("/Z10Returned/PayAction", {
            'bank': payAccount.val(),
            'feePaid': feePaid.val(),
            'orderStatus': orderStatus
        }, function (data) {
            if (data == 1) {
                parent.$.jshowtip("支付成功！", "success");
                parent.go2("/Z10SaleReturned/SaleReturnedList");
            }
            else {
                parent.$.jshowtip(data, "error");
            }
        });
    });
});