using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP.Logic
{
    public class __UserController : Zippy.SaaS.Mvc.__UserController
    {
        protected override Zippy.SaaS.Entity.CRUD QueryPermission()
        {
            ViewData["TenantID"] = _tenant.TenantID;

            if ((_user.UserType ?? 0 & (int)Zippy.SaaS.UserTypes.SystemAdministrator) == (int)Zippy.SaaS.UserTypes.SystemAdministrator ||
                (_user.UserType ?? 0 & (int)Zippy.SaaS.UserTypes.HostAdministrator) == (int)Zippy.SaaS.UserTypes.HostAdministrator)
                return Zippy.SaaS.Entity.CRUD.Create |
                    Zippy.SaaS.Entity.CRUD.Delete |
                    Zippy.SaaS.Entity.CRUD.Read |
                    Zippy.SaaS.Entity.CRUD.Update |
                     Zippy.SaaS.Entity.CRUD.Check;


            List<EAP.Logic.Bus.View.V_UserPermission> mePers = 
                db.Take<EAP.Logic.Bus.View.V_UserPermission>("[UserID]=@UserID and [Flag]=@Flag", db.CreateParameter("UserID", _user.UserID),
                db.CreateParameter("Flag", _ContollerName));
            int myPermission = 0;
            
            mePers.ForEach(s =>
            {
                myPermission |= s.PermissionType ?? 0;
            });
            //throw new Exception(((Zippy.SaaS.Entity.CRUD)myPermission).ToString("F"));
            return (Zippy.SaaS.Entity.CRUD)myPermission;
        }
    }
}
