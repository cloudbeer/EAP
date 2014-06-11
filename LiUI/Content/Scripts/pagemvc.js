/*
此js需要配合页面元素：
pageSize controller sortUrl
这些在页面中生命
*/

$(function () {
    $(".xdel").click(function () {
        var xthis = $(this);
        var id = xthis.attr("rel");
        $.jconfirmDel(function () {
            $.post(controller + "/Delete/" + id, null, function (res) {
                if (res == "401") {
                    parent.$.jshowtip("您没有执行此操作的权限！", "error");
                    return;
                }
                else if (res == "701") {
                    parent.$.jshowtip("此记录被引用，无法删除！", "error");
                    return;
                }
                else if (res == "1") {
                    xthis.parent().parent().parent().parent().parent().hide(0, function () {
                        $(this).remove();
                        parent.$.jshowtip('数据删除成功！', 'success');
                        if ($("tr[rel=item]").length == 0)
                            window.location.reload(true);
                    });
                }
            });
        });
    });
    $(".xedit").click(function () {
        var id = $(this).attr("rel");
        parent.go2(controller + "/Edit/" + id + "?ReturnUrl=" + encodeURIComponent(window.location.href));

    });
    $(".xdetail").click(function () {
        var id = $(this).attr("rel");
        parent.go2(controller + "/Edit/" + id + "?act=detail");
    });

    $("#bReload").click(function () {
        parent.$.jshowtip("数据加载中，请稍候...", "loading");
        window.location.reload(true);
    });
    $("#bShowAll").click(function () {
        var currentUrl = window.location.href;
        if (currentUrl.indexOf("?") > 0)
            currentUrl = currentUrl.substring(0, currentUrl.indexOf("?"));
        parent.go2(currentUrl + '?PageSize=' + pageSize);

    });
    $("#bDelete").click(function () {
        var allID = "";
        $(".xID").each(function () {
            if ($(this).attr("checked"))
                allID += $(this).val() + ",";
        });
        if (allID == "") {
            parent.$.jshowtip("请选择至少一行数据。", "error");
            return;
        }

        $.jconfirmDel(function () {
            $.post(controller + "/DeleteBatch", "ids=" + encodeURIComponent(allID), function (res) {
                if (res == "401") {
                    parent.$.jshowtip("您没有执行此操作的权限！", "error");
                    return;
                }
                if (res == "701") {
                    parent.$.jshowtip("此记录被引用，无法删除！", "error");
                    return;
                }
                if (res == "1") {
                    parent.$.jshowtip('数据删除成功！', 'success');
                    $("tr.focus").hide(0, function () {
                        $(this).remove();
                        if ($("tr[rel=item]").length == 0) {
                            window.location.reload(true);
                        }
                    });
                }
            });
        });
    });

    $(".i_sort").click(function () {
        var currentUrl = window.location.href;
        var xTemp = sortUrl + $(this).attr("rel");

        var xsortUrl = "";
        if (currentUrl.indexOf("?") > 0)
            currentUrl = currentUrl.substring(0, currentUrl.indexOf("?"));

        xsortUrl = currentUrl + "?" + xTemp;

        parent.go2(xsortUrl);
    });
});