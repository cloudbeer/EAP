using System;
using Zippy.Data.Mapping;


namespace EAP.Logic.Z10.View
{
    [DataTable(Name = "V_DepotFlow", Title = "库存流水")]
    public class V_DepotFlow
    {
        [DataField(Name = "FlowID", Title = "库存流水编号", IsPrimaryKey = true, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? FlowID { get; set; }

        [DataField(Name = "DepotID", Title = "仓库", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? DepotID { get; set; }

        [DataField(Name = "OrderID", Title = "订单", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? OrderID { get; set; }

        [DataField(Name = "ProductID", Title = "产品", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? ProductID { get; set; }

        [DataField(Name = "Count", Title = "数量", DbType = System.Data.DbType.Decimal, SqlDbType = System.Data.SqlDbType.Decimal)]
        public virtual Decimal? Count { get; set; }

        [DataField(Name = "CreateDate", Title = "创建时间", DbType = System.Data.DbType.DateTime, SqlDbType = System.Data.SqlDbType.DateTime)]
        public virtual DateTime? CreateDate { get; set; }

        [DataField(Name = "DepotTitle", Title = "标题", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String DepotTitle { get; set; }

        [DataField(Name = "ProductTitle", Title = "标题", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String ProductTitle { get; set; }
    }
}
