using System;
using System.Web;
using System.Web.Mvc;
using Zippy.SaaS.Entity;
using Zippy.SaaS.Helper;

namespace EAP.Controllers
{
    public class AccountController : Zippy.SaaS.Mvc.Controller
    {
        public ActionResult Logon()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Logon(FormCollection cols)
        {
            string UserName = cols["UserName"];
            ViewData["UserName"] = UserName;

            string ValidateCode = cols["ValidateCode"];

            if (Session["vcode"] == null)
            {
                this.ModelState.AddModelError("ValidateCode Timeout", "验证码过期，请重新刷新。");
                return View(cols);
            }
            if (Session["vcode"].ToString() != ValidateCode)
            {
                this.ModelState.AddModelError("ValidateCode Error", "验证码不正确。");
                return View();
            }
            Session.Remove("vcode");

            Zippy.Data.IDalProvider db = Zippy.Data.DalFactory.CreateProvider();

            string Flag = cols["Flag"];
            ViewData["Flag"] = Flag;
            string Password = cols["Password"];
            string RememberMe = cols["RememberMe"];


            int cookieMinutes = 30;
            if (RememberMe.IsNotNullOrEmpty() && RememberMe.StartsWith("true"))
            {
                cookieMinutes = 60 * 3;
            }

            Tenant tenant = null;
            User user = null;


            tenant = TenantHelper.Get(Flag, db);

            if (tenant == null)
            {
                this.ModelState.AddModelError("Tenant not found", "租户未找到");
                return View();
            }

            user = UserHelper.Get(UserName, tenant.TenantID.Value, db);
            if (user == null)
            {
                this.ModelState.AddModelError("User not found", "用户未找到");
                return View();
            }
            if (Password.Md6() != user.Password)
            {
                this.ModelState.AddModelError("Password not right", "密码不正确");
                return View();
            }

            if (ModelState.IsValid)
            {
                Session["tenant"] = tenant;
                Session["user"] = user;

                tenant.TenantID.ToString().Save2Cookie("TenantID", DateTime.Now.AddMinutes(cookieMinutes));
                user.UserName.Save2Cookie("UserName", DateTime.Now.AddMinutes(cookieMinutes));

                return Return();

            }
            else
            {
                return View();
            }


        }

        public ActionResult Logout()
        {
            Session.Clear();
            //Session.Remove("tenant");
            //Session.Remove("user");
            "TenantID".RemoveCookie();
            "UserName".RemoveCookie();
            return Redirect("Logon");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePassword(FormCollection cols)
        {
            string opwd = cols["OPassword"];
            string npwd = cols["NPassword"];
            User _sUser = Session["user"] as User;
            if (_sUser == null)
            {
                return Content("请登录后修改密码。");
            }

            if (opwd.Md6() != _sUser.Password)
            {
                return Content("原密码错误，请重试。");
            }
            string mdpwd = npwd.Md6();
            _sUser.Password = mdpwd;
            var dataUser = new User();
            dataUser.UserID = _sUser.UserID;
            dataUser.Password = mdpwd;
            db.Update(dataUser);
            return Content("1");
        }

        public ActionResult UpdateIE()
        {
            return View();
        }
    }
}
