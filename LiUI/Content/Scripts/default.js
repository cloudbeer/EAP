$(function () {
    var content = $("#content");
    var iframe = '<iframe id="js_main_frame" name="main_frame" src="/cabbage/desk" style="width:100%;height:' + (content.height() - 24) + 'px" frameborder="0"></iframe>'
    content.append(iframe);

    (function () {
        var menu = "";
        if (mymenu) {
            $.each(mymenu, function () {
                menu += '<h3><a href="javascript:;" rel="' + $(this)[0][0] + '"><i class="ico i_' + $(this)[0][2] + '"></i>' + $(this)[0][1] + '<b></b></a></h3>';
            });

            var js_sidbar_menu = $("#js_sidbar_menu");

            js_sidbar_menu.html(menu);

            var menuH = $("#siderbar").height() - js_sidbar_menu.height();

            $("#js_sidbar_zmenu").height(menuH);

        } else {
            alert('您没有权限执行任何权限！');
        }
    })();

    $("#continer #siderbar .subnav h3").hover(function () {
        if ($(this).attr("class") != "current")
            $(this).find("a").css("color", "#F76515");

        $(this).find("b").css('display', 'inline')
    }, function () {
        if ($(this).attr("class") != "current")
            $(this).find("b").css('display', 'none');

        $(this).find("a").css("color", "#2D52A5");
    });

    $("#continer #siderbar .menu a").live("click", function () {
        $("#continer #siderbar .menu a").removeClass("selected");
        $(this).addClass("selected");
    });

    $("#continer #siderbar .subnav h3").live("click", function () {
        $("#continer #siderbar .subnav h3").removeClass("current").find("b").css('display', 'none');
        $(this).addClass("current").find("a").css('color', '#2D52A5').find("b").css('display', 'inline');

        var rel = $(this).find("a").attr("rel");
        if (mymenu) {
            $.each(mymenu, function () {
                if ($(this)[0][0] == rel) {
                    var m = $(this)[0];
                    var zm = $(this)[1];

                    var zmenu = "";
                    zmenu += '<h3 id="js_sidbar_title_box" class="title">' + m[1] + '</h3>';
                    zmenu += '<ul id="js_sidbar_tree" class="menu">';

                    $.each(zm, function (i) {
                        zmenu += '	<li>';
                        zmenu += '		<a href="javascript:;" onclick="go2(\'' + zm[i][1] + '\',true)">';
                        zmenu += '			<i class="icon i_' + zm[i][2] + '"></i><span rel="text">' + zm[i][0] + '</span>';
                        zmenu += '		</a>';
                        zmenu += '	</li>';
                    });

                    zmenu += '</ul>';

                    var js_sidbar_zmenu = $("#js_sidbar_zmenu");
                    js_sidbar_zmenu.html(zmenu);
                    var zH = js_sidbar_zmenu.height() - 45;
                    $("#js_sidbar_tree").height(zH);

                    return false;
                }
            });
        }
    });

    $("#js_sidbar_tree li a").live("click", function () {
        $("#js_location").html($("#js_sidbar_title_box").text() + " &raquo; " + $(this).find("span[rel=text]").text());
    });

    $("#js_exit").click(function () {
        $.jconfirm('确认退出励惠进销存管理系统吗？', '励惠企业管理', 'warn', true, function () {
            window.location = "/account/Logout";
        });
    });
    $("#js_changpwd").click(function () {
        go2("/account/changepassword");
    });

    //$("#weather").text(__minisite__weather__)

    //$(document).jconfirm('这里是内容','这是标题','success',true);
    //$(document).jshowtip('这是内容这是内容这是内容这是内容','loading');
    $("#js_sidbar_menu h3:first-child").click(); //模拟点击第一个菜显示子菜单

    calPageSize();

    var oFrm = $('#js_main_frame');

    function hideTip() {
        if (this.readyState && this.readyState != 'complete') return;
        else $.jhidetip();
    }

    oFrm.load(hideTip);
    oFrm[0].onreadystatechange = hideTip;

    //$.jpop('&nbsp;&nbsp;&nbsp;&nbsp;产品“液晶屏 X-53232”库存已不足预设的警戒数量，请您及时处理！');
});

$(window).resize(function () {
    var content = $("#content");
    $("#js_main_frame").height(content.height() - 24);

    calPageSize();
});

function go2(url, addPageSize) {
    if (addPageSize)
        url += "?pagesize=" + pageSize;

    $("#js_main_frame").attr("src", function () {
        $.jshowtip("数据加载中，请稍候...", "loading", 60);
        return url;
    });
}

function calPageSize() {
    var winHeight = $(window).height();
    srcHeight = winHeight - 220;
    pageSize = srcHeight / 30;
    pageSize = parseInt(pageSize);
}