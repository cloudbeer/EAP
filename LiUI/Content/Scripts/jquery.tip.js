$.extend({
    jalert: function (content, title, icon, isModal, _Callback) {
        $.jtip(content, title, icon, 'alert', isModal, _Callback);
    },
    jconfirm: function (content, title, icon, isModal, _Callback) {
        $.jtip(content, title, icon, 'confirm', isModal, _Callback);

    },
    jconfirmDel: function (_Callback) {
        $.jtip("此操作将不可恢复，确认删除吗？", "确认删除", "warn", 'confirm', true, _Callback);
    },
    jtip: function (content, title, state, button, isModal, _Callback) {
        var alertBox = "";
        alertBox += '<div style="top: 100px;" class="pop-box pop-min">';
        alertBox += '<h2><a rel="close" class="close" href="javascript:;">关闭</a>' + title + '</h2>';
        alertBox += '<div class="con">';
        alertBox += '	<div class="msg-box">';

        if (state) {
            alertBox += '    	<i class="icon i_' + state + '"></i>';
        }
        //        if (state == 'warn')
        //            alertBox += '    	<i class="icon i_warn"></i>';
        //        else if (state == 'success')
        //            alertBox += '    	<i class="icon i_success"></i>';
        //        else if (state == 'error')
        //            alertBox += '    	<i class="icon i_error"></i>';
        //        else
        //            alert('您输入的状态图标不正确\r\n\r\nwarn      ---     警告图标\r\nsuccess  ---     正确图标\r\nerror      ---     错误图标');
        alertBox += '<span class="msg-text">' + content + '</span>';
        alertBox += '    </div>';
        alertBox += '    <div class="bottom">';
        if (button == 'alert')
            alertBox += '    	<button id="jenter" rel="enter" class="active">\u786e\u5b9a</button> ';
        else if (button == 'confirm')
            alertBox += '    	<button id="jenter" rel="enter" class="active">\u786e\u5b9a</button>&nbsp;<button id="jclose" rel="close">\u53d6\u6d88</button>';
        alertBox += '    </div>';
        alertBox += '</div>';
        alertBox += '</div>';

        $("body").append(alertBox);

        if (isModal)
            $.jmask();

        $(".close").live("click", function () {
            try {
                $("#mask").remove();
                $(this).parents(".pop-box").remove();
            } catch (e) {
                $(this).parents(".pop-box").remove();
            }
        });

        $("#jenter").live("click", function () {
            try {
                $("#mask").remove();
                $(this).parents(".pop-box").remove();
            } catch (e) {
                $(this).parents(".pop-box").remove();
            }

            if (_Callback) {
                _Callback();
            }
        });

        $("#jclose").live("click", function () {
            try {
                $("#mask").remove();
                $(this).parents(".pop-box").remove();
            } catch (e) {
                $(this).parents(".pop-box").remove();
            }
        });

    },
    jshowtip: function (msg, icon, timeout, focusId, _callback) {
        timeout = timeout * 1000 || 1000;

        $.jhidetip();

        var tip = '';
        tip += '<div id="jtip" class="stat-msg">';
        if (icon) {
            tip += '	<span class="icon i_t' + icon + '"></span>';
        }
        //        if (icon == 'success')
        //            tip += '	<span class="icon i_tsuccess"></span>';
        //        else if (icon == 'warn')
        //            tip += '	<span class="icon i_twarn"></span>';
        //        else if (icon == 'error')
        //            tip += '	<span class="icon i_terror"></span>';
        //        else if (icon == 'loading')
        //            tip += '	<span class="icon i_tloading"></span>';
        //        else
        //            alert('您输入的状态图标不正确\r\n\r\nerror       ---     错误图标\r\nsuccess     ---     正确图标\r\nsuccess     ---     警告图标\r\nloading     ---     加载图标');
        //        
        tip += '	<span rel="con">' + msg + '</span>';
        tip += '</div>';

        $("body").append(tip);
        $("#jtip").css('margin-left', -($("#jtip").width() / 2) + 'px');

        setTimeout(function () {
            $("#jtip").remove();

            if (focusId)
                $("#" + focusId).focus();

            if (_callback)
                _callback();
        }, timeout);
    },
    jhidetip: function () {
        $("#jtip").remove();
    },
    jpop: function (txt, _callback) {
        var popHTML = "";
        popHTML += '<div class="right_bottom_tip">';
        popHTML += '<h2><a rel="close" class="close" href="javascript:;">关闭</a></h2>';
        popHTML += '	<div class="con">' + txt;
        if (_callback)
            popHTML += '<div class="tr pt10 action"><a href="javascript:;" class="close" onclick="(' + _callback + ')();">我知道了</a></div>';
        else
            popHTML += '<div class="tr pt10 action"><a href="javascript:;" class="close">我知道了</a></div>';
        popHTML += '</div>';
        popHTML += '</div>';

        $("body").append(popHTML);

        $(".right_bottom_tip").slideDown(1000, function () {
            $(this).find(">h2 a.close").show('fast');
        });

        $(".right_bottom_tip a.close").live("click", function () {
            $(this).parents(".right_bottom_tip").slideUp(1000, function () {
                $(this).remove();
            });
        });
    },
    jvalidate: function (oid, txt, mt, ml) {
        mt = mt || 24;
        ml = ml || 0;
        if (txt) {
            var msg_box = '<div id="js_min_msg_box" class="hint-war">' + txt + '<b></b></div>';
            var $msg_box;
            if ($("#js_min_msg_box")[0] != undefined) { return false };
            $("body").append(msg_box);

            $msg_box = $("#js_min_msg_box");

            var $o;
            if (typeof (oid) == "object")
                $o = oid;
            else
                $o = $("#" + oid);
            var position = $o.position();

            $o.focus(function () {
                $("#js_min_msg_box").remove();
            });

            $msg_box.css({ 'top': position.top + mt, 'left': position.left + ml });
        }

        return false;
    },
    jmask: function () {
        if ($("#mask")[0] == undefined) {
            var mask = "<div id='mask' style='z-index:99; position:absolute; top:0; left:0; background:#93AFDB;width:" + $(window).width() + "px;height:" + $(window).height() + "px;filter:alpha(opacity=30);-moz-opacity:0.3;opacity: 0.3'></div>";

            $("body").append(mask);
        }
    }
});