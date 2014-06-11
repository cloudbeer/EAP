namespace EAP.Logic.Z30
{
    public enum VisitWays
    {
        Tel = 1, //电话
        Email = 2,
        IM = 3, //即时通讯
        ToDoor = 4 //上门
    }

    public enum Wishs
    {
        NotConsider = 1, //暂不考虑
        ContactLater = 2, //稍后联系
        NotPresent = 4, //客户不在现场
        Maybe = 8, //有意向
        DateToDoor = 0x10, //约定上门拜访
        DateToOrder = 0x20, //约定签单
        Ordered = 0x40  //已签单
    }

    public enum SuccessRatioes
    {
        SR00,
        SR20,
        SR40,
        SR80
    }
    /// <summary>
    /// 联系人决策类型
    /// </summary>
    public enum BuyerPersonas
    {
        UserBuyer = 0x01,
        TechnicalBuyer = 0x2,
        EconomicBuyer = 0x04,
        Coach = 0x10,
        Champion = 0x20,
        FOX = 0x40
    }
    /// <summary>
    /// 售前角色
    /// </summary>
    public enum BuyerTypes
    {
        Unknown = 1,
        Buyer = 2,
        NotBuyer
    }

    public enum WebSiteStatus
    {
        NoSite = 1,
        HadSite = 2,
        MaybeBetter = 4,
        PlatformSite = 8
    }
}