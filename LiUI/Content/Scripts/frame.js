$(function () {
    setwindow();

    $(".list").bind("click", function () {
        var tbtn = $(this); //当前的点击的按钮
        var tid = tbtn.attr("id");
        var opt_mmenu = $(".opt-mmenu,.pop-query"); //缓存jquery对象

        opt_mmenu.hide();
        opt_mmenu.each(function (i, d) {
            var tp = tbtn.position();

            var current = $(this);
            current.css({ "left": tp.left, "top": tp.top + tbtn.height() })

            if (current.attr("rel") == tid) {
                current.slideDown('fast').bind("click", function () {
                    if (current.attr('class').indexOf("pop-query") > 1) {
                        return false;
                    }
                    else {
                        current.hide();
                    }
                })

                $(document).one("click", function () {
                    opt_mmenu.hide();
                });
            }
        });
        return false; //阻止冒泡
    });

    $("table tr th input[type=checkbox]").bind("click", function () {
        $("table tr input[type=checkbox]").attr("checked", $(this).attr("checked"));

        if ($(this).attr("checked")) {
            $("table tr").each(function () {
                if ($(this).attr("rel") == "item")
                    $(this).addClass("focus");
            });
        }
        else
            $("table tr").removeClass("focus");
    });

    $("table tr td input[type=checkbox]").bind("click", function () {
        if ($(this).attr("checked")) {
            $(this).parents("tr").addClass("focus");
        }
        else {
            $(this).parents("tr").removeClass("focus");
        }
    });

    $(".pop-box .close").click(function () {
        $(".pop-box").slideUp('fast');
        $("#frame_content").remove();
        $("#mask").hide();
    });

    $(".back").click(function () {
        var url = $(this).attr("rel")
        if (url)
            parent.go2(url);
    });
});

function setwindow() {
    var h = $(document).height() - $("#bottom").height();
    $("#contents").height(h);
    $("#main").height(h - 35);
}

  
$(window).resize(setwindow);

$(document).ajaxStart(function () { parent.$.jshowtip("\u6570\u636e\u5904\u7406\u4e2d\uff0c\u8bf7\u7a0d\u5019\u002e\u002e\u002e", "loading") });
$(document).ajaxError(function () { parent.$.jshowtip("\u7cfb\u7edf\u6b63\u5fd9\uff0c\u8bf7\u7a0d\u5019\u518d\u6267\u884c\u6b64\u64cd\u4f5c\uff01", "error") });



//处理键盘事件 禁止后退键（Backspace）密码或单行、多行文本框除外  
function banBackSpace(e) {
    var ev = e || window.event; //获取event对象     
    var obj = ev.target || ev.srcElement; //获取事件源     

    var t = obj.type || obj.getAttribute('type'); //获取事件源类型    

    //获取作为判断条件的事件类型  
    var vReadOnly = obj.getAttribute('readonly');
    var vEnabled = obj.getAttribute('enabled');
    //处理null值情况  
    vReadOnly = (vReadOnly == null) ? false : vReadOnly;
    vEnabled = (vEnabled == null) ? true : vEnabled;

    //当敲Backspace键时，事件源类型为密码或单行、多行文本的，  
    //并且readonly属性为true或enabled属性为false的，则退格键失效  
    var flag1 = (ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea")
                && (vReadOnly == true || vEnabled != true)) ? true : false;

    //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效  
    var flag2 = (ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea")
                ? true : false;

    //判断  
    if (flag2) {
        return false;
    }
    if (flag1) {
        return false;
    }
}

//禁止后退键 作用于Firefox、Opera  
document.onkeypress = banBackSpace;
//禁止后退键  作用于IE、Chrome  
document.onkeydown = banBackSpace;  