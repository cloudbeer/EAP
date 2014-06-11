<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>踏骐销售管理系统</title>
    <link rel="stylesheet" type="text/css" href="/content/styles/base.css" />
    <link rel="stylesheet" type="text/css" href="/content/styles/login.css" />
    <link rel="stylesheet" type="text/css" href="/content/Styles/icon.css" />
    <link rel="stylesheet" type="text/css" href="/content/Styles/common.css" />
    <script type="text/javascript" src="/content/scripts/jquery.js"></script>
    <script type="text/javascript" src="/content/Scripts/jquery.tip.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#login_frm").submit(function () {
                if ($("#UserName").val() == "") { $.jvalidate('UserName', '请输入您的登录帐号。'); return false }
                if ($("#Password").val() == "") { $.jvalidate('Password', '请输入您的登录密码。'); return false };
                if ($("#ValidateCode").val() == "") { $.jvalidate('ValidateCode', '请输入右侧的验证码。'); return false };
            });


            $.jvalidate('UserName', '<%= Html.ValidationMessage("User not found", "用户未找到。")%>');
            $.jvalidate('Password', '<%= Html.ValidationMessage("Password not right", "密码不正确。")%>');
            $.jvalidate('ValidateCode', '<%= Html.ValidationMessage("ValidateCode Timeout", "验证码过期，请重新填写。")%>');
            $.jvalidate('ValidateCode', '<%= Html.ValidationMessage("ValidateCode Error", "验证码不正确。")%>');

        });
    </script>
</head>
<body>
    <div id="page">
        <div id="wrap" class="bc">
            <div id="header" class="clearfix">
                <span class="fr mt30"><a href="/Z10Public/QueryDepot/" target="_blank">库存查询</a>
                </span>
                <h1>
                    <img src="/content/images/logo.gif" alt="踏骐销售管理系统" /></h1>
            </div>
            <div id="loginbox" class="bc">
                <div id="logincontent" class="clearfix">
                    <div id="login_l" class="fl">
                        <img src="/content/images/banner.png" alt="Banner" />
                    </div>
                    <div id="login_r" class="fl">
                        <form id="login_frm" action="/Account/Logon" method="post">
                        <table>
                            <caption>
                                <h3>
                                    欢迎登陆踏骐销售管理系统</h3>
                            </caption>
                            <tr>
                                <th>
                                    帐号：
                                </th>
                                <td>
                                    <input type="text" class="text" id="UserName" name="UserName" value="<%=ViewData["UserName"]%>" />
                                    <input type="hidden" name="Flag" value="TouchBike" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    密码：
                                </th>
                                <td>
                                    <input type="password" class="text" id="Password" name="Password" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    验证码：
                                </th>
                                <td>
                                    <input type="text" class="text" style="width: 100px" id="ValidateCode" name="ValidateCode" />
                                    <img style="cursor: pointer;" src="/helper/vimage?w=75&h=26" title="点击更换验证码" onclick="this.src=this.src+'?'" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                </th>
                                <td>
                                    <input type="checkbox" id="RememberMe" name="RememberMe" value="true" />
                                    <label for="RememberMe" style="color: #666">
                                        记住我</label>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                </th>
                                <td>
                                    <button type="submit" class="button mt10">
                                        登录</button>
                                </td>
                            </tr>
                        </table>
                        </form>
                    </div>
                </div>
            </div>
            <div id="footer">
                <div class="fl">
                    <span style="color: #069">深圳市踏骐商贸有限公司</span><br />
                    &copy; 2009-2012 TouchBike. All Rights Reserved.
                </div>
                <div class="fr">
                    地址：深圳市龙华新区锦绣江南四期1140<br />
                    电话：0755-29552685&nbsp;&nbsp;&nbsp;&nbsp;传真：0755-29552685</div>
            </div>
        </div>
    </div>
</body>
</html>
