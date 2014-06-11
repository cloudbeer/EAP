using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP.Logic.Z30
{
    /// <summary>
    /// 仅能保存的一个实体类
    /// </summary>
    public class QuickCommunication
    {
        public string CorpName { get; set; }
        public string Tel { get; set; }
        public string LinkName { get; set; }
        public string Content { get; set; }
        public int? VisitWay { get; set; }
        public int? SuccessRatio { get; set; }
        public int? BuyerPersonas { get; set; }
        public int? WebSiteStatus { get; set; }

        public void Save(Guid tenantID, Guid me)
        {
            Z01Beetle.Entity.Z01Customer cus = new Z01Beetle.Entity.Z01Customer();
            cus.Title = CorpName + "[" + LinkName + "]";
            cus.Tel1 = Tel;
            cus.TenantID = tenantID;
            cus.CustomerStatus = WebSiteStatus;            
            cus.Principal = me;

            int cusID = Zippy.Data.StaticDB.DB.Insert(cus);

            if (cusID > 0)
            {
                Z01Beetle.Entity.Z01CustomerPerson cusPer = new Z01Beetle.Entity.Z01CustomerPerson();
                cusPer.Name = LinkName;
                cusPer.Tel1 = Tel;
                cusPer.CustomerID = cusID;
                cusPer.TenantID = tenantID;
                cusPer.Creator = me;
                cusPer.UserType = BuyerPersonas;
                int perID = Zippy.Data.StaticDB.DB.Insert(cusPer);

                Z30CRM.Entity.Z30Communication com = new Z30CRM.Entity.Z30Communication();
                com.VisitDate = DateTime.Now;
                com.VisitWay = (int)VisitWay;
                com.SuccessRatio = (int)SuccessRatio;
                com.CustomerID = cusID;
                com.PersonID = perID;
                com.Content = Content;
                com.TenantID = tenantID;
                com.Creator = me;
                Zippy.Data.StaticDB.DB.Insert(com);

            }

        }
    }
}
