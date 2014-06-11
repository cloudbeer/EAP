using System;
using Zippy.Data.Mapping;

namespace EAP.Logic.Z10.View
{
    [DataTable(Name = "V_DepotProduct", Title = "库存商品")]
    public class V_DepotProduct
    {
        [DataField(Name = "DepotProductID", Title = "编号", IsPrimaryKey = true, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? DepotProductID { get; set; }

        [DataField(Name = "DepotID", Title = "仓库编号", IsPrimaryKey = false, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? DepotID { get; set; }
        [DataField(Name = "ProductID", Title = "产品编号", IsPrimaryKey = false, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? ProductID { get; set; }
        [DataField(Name = "BrandID", Title = "品牌", IsPrimaryKey = false, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? BrandID { get; set; }

        [DataField(Name = "DepotTitle", Title = "仓库标题", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String DepotTitle { get; set; }

        [DataField(Name = "Code", Title = "编码", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Code { get; set; }

        [DataField(Name = "Manager", Title = "负责人", DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? Manager { get; set; }

        [DataField(Name = "TenantID", Title = "租户", AllowNull = false, DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? TenantID { get; set; }


        [DataField(Name = "CountAlarm", Title = "报警数量", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? CountAlarm { get; set; }

        [DataField(Name = "InSum", Title = "总进货", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? InSum { get; set; }

        [DataField(Name = "OutSum", Title = "总出货", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? OutSum { get; set; }

        [DataField(Name = "StockSum", Title = "库存", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? StockSum { get; set; }


        [DataField(Name = "ProductTitle", Title = "商品名称", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ProductTitle { get; set; }


        [DataField(Name = "UnitID", Title = "标准计量单位", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? UnitID { get; set; }

        [DataField(Name = "PriceList", Title = "标价", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? PriceList { get; set; }

        [DataField(Name = "PriceStock", Title = "进货价", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? PriceStock { get; set; }

        [DataField(Name = "PriceSelling", Title = "出售价格", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? PriceSelling { get; set; }

        [DataField(Name = "PriceSellOff1", Title = "打折价1", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? PriceSellOff1 { get; set; }

        [DataField(Name = "PriceSellOff2", Title = "打折价2", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? PriceSellOff2 { get; set; }

        [DataField(Name = "PriceSellOff3", Title = "打折价3", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? PriceSellOff3 { get; set; }
    }
}
