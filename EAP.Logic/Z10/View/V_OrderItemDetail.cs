
using System;
using Zippy.Data.Mapping;

namespace EAP.Logic.Z10.View
{
    [DataTable(Name = "V_OrderItemDetail", Title = "订单产品详情")]
    public class V_OrderItemDetail
    {
        [DataField(Name = "ItemID", Title = "订单产品编号", IsPrimaryKey = true, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? ItemID { get; set; }

        [DataField(Name = "ProductID", Title = "产品", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? ProductID { get; set; }

        [DataField(Name = "Title", Title = "名称", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Title { get; set; }

        [DataField(Name = "OrderID", Title = "订单", AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? OrderID { get; set; }

        [DataField(Name = "Price", Title = "价格", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? Price { get; set; }

        [DataField(Name = "CountChecked", Title = "盘点数量", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? CountChecked { get; set; }

        [DataField(Name = "CountShould", Title = "应该发生数量", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? CountShould { get; set; }

        [DataField(Name = "CountHappend", Title = "实际发生数量", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? CountHappend { get; set; }

        [DataField(Name = "CountHappend2", Title = "实际发生数量2", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? CountHappend2 { get; set; }

        [DataField(Name = "DepotID", Title = "主仓库", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? DepotID { get; set; }

        [DataField(Name = "DepotID2", Title = "相关仓库", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? DepotID2 { get; set; }

        [DataField(Name = "ExtColor", Title = "颜色", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtColor { get; set; }

        [DataField(Name = "ExtSize", Title = "尺寸", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtSize { get; set; }

        [DataField(Name = "ExtSpecification", Title = "规格", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtSpecification { get; set; }

        [DataField(Name = "ExtModel1", Title = "其他扩展", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtModel1 { get; set; }

        [DataField(Name = "ExtModel2", Title = "其他扩展2", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtModel2 { get; set; }

        [DataField(Name = "ExtModel3", Title = "其他扩展3", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtModel3 { get; set; }

        [DataField(Name = "ExtModel4", Title = "其他扩展4", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtModel4 { get; set; }

        [DataField(Name = "ExtModel5", Title = "其他扩展5", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ExtModel5 { get; set; }

        [DataField(Name = "DepotProductDetailID", Title = "库存详情编号", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? DepotProductDetailID { get; set; }

        [DataField(Name = "Remark", Title = "备注", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Remark { get; set; }

        [DataField(Name = "Total", Title = "小计", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? Total { get; set; }

        [DataField(Name = "TenantID", Title = "租户", AllowNull = false, DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? TenantID { get; set; }

        [DataField(Name = "ItemType", Title = "订单产品类型", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? ItemType { get; set; }

        [DataField(Name = "ItemStatus", Title = "订单产品状态", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? ItemStatus { get; set; }

        [DataField(Name = "CreateDate", Title = "创建时间", DbType = System.Data.DbType.DateTime, SqlDbType = System.Data.SqlDbType.DateTime)]
        public virtual DateTime? CreateDate { get; set; }

        [DataField(Name = "Creator", Title = "创建人", DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? Creator { get; set; }

        [DataField(Name = "UpdateDate", Title = "更新时间", DbType = System.Data.DbType.DateTime, SqlDbType = System.Data.SqlDbType.DateTime)]
        public virtual DateTime? UpdateDate { get; set; }

        [DataField(Name = "Updater", Title = "更新人", DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? Updater { get; set; }

        //以下来自订单表

        [DataField(Name = "DateOrder", Title = "下单日期", DbType = System.Data.DbType.DateTime, SqlDbType = System.Data.SqlDbType.DateTime)]
        public virtual DateTime? DateOrder { get; set; }

        [DataField(Name = "CustomerID", Title = "客户", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? CustomerID { get; set; }
		
        [DataField(Name = "DeleteFlag", Title = "删除标记", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? DeleteFlag { get; set; }
        [DataField(Name = "OrderType", Title = "订单类型", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? OrderType { get; set; }

        [DataField(Name = "OrderStatus", Title = "订单状态", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? OrderStatus { get; set; }


    }
}


