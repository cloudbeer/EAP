
namespace EAP.Logic.Z10
{
    public enum OrderTypes
    {
        /// <summary>
        /// 采购入库
        /// </summary>
        Purchase = 1,
        /// <summary>
        /// 采购退货
        /// </summary>
        PurchaseReturn = 2,
        /// <summary>
        /// 生产入库
        /// </summary>
        Produce = 4,
        /// <summary>
        /// 生产退料
        /// </summary>
        ProduceReturn = 8,
        /// <summary>
        /// 销售出库
        /// </summary>
        Sale = 16,
        /// <summary>
        /// 销售退货
        /// </summary>
        SaleReturn = 32,
        /// <summary>
        /// 调货单(低利润单)
        /// </summary>
        SaleLowProfit = 64,
        /// <summary>
        /// 盘点
        /// </summary>
        Check = 256,
        /// <summary>
        /// 调拨单
        /// </summary>
        Transfer = 1024,
        /// <summary>
        /// 盘库亏
        /// </summary>
        InventoryOut=2048,
        /// <summary>
        /// 盘库盈
        /// </summary>
        InventoryIn=4096
    }

    public enum OrderStatus
    {
        Ordered = 0x1,  //已下单
        OuttedSome = 0x10,  //部分出库
        InnedSome = 0x20,  //部分入库
        Outted = 0x40,//出库
        Inned = 0x80, //入库
        Paid = 0x100, //已付款
        PaidSome = 0x200, //已部分付款
        Checked = 0x1000, //审批
        Finished = 0x10000,   //已结束，由用户标记
        Archived = 0x20000,   //存档
        BossChecked = 0x40000 //老板已清算
    }
}
