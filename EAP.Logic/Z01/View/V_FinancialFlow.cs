using System;
using Zippy.Data.Mapping;

namespace EAP.Logic.Z01.View
{
    [DataTable(Name = "V_FinancialFlow", Title = "财务流水")]
    public class V_FinancialFlow
    {
        [DataField(Name = "FlowID", Title = "编号", IsPrimaryKey = true, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? FlowID { get; set; }

        [DataField(Name = "Amount", Title = "金额", DbType = System.Data.DbType.Currency, SqlDbType = System.Data.SqlDbType.Money)]
        public virtual Decimal? Amount { get; set; }

        [DataField(Name = "Currency", Title = "币种", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.Char)]
        public virtual String Currency { get; set; }

        [DataField(Name = "BankID", Title = "银行", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? BankID { get; set; }

        [DataField(Name = "CategoryID", Title = "类目", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? CategoryID { get; set; }

        [DataField(Name = "OrderID", Title = "订单", DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? OrderID { get; set; }

        [DataField(Name = "Remark", Title = "备注", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Remark { get; set; }

        [DataField(Name = "TenantID", Title = "租户", AllowNull = false, DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? TenantID { get; set; }

        [DataField(Name = "FlowType", Title = "支付流水类型", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? FlowType { get; set; }

        [DataField(Name = "FlowStatus", Title = "支付流水状态", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? FlowStatus { get; set; }

        [DataField(Name = "CreateDate", Title = "创建时间", DbType = System.Data.DbType.DateTime, SqlDbType = System.Data.SqlDbType.DateTime)]
        public virtual DateTime? CreateDate { get; set; }

        [DataField(Name = "Creator", Title = "创建人", DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? Creator { get; set; }

        [DataField(Name = "UpdateDate", Title = "更新时间", DbType = System.Data.DbType.DateTime, SqlDbType = System.Data.SqlDbType.DateTime)]
        public virtual DateTime? UpdateDate { get; set; }

        [DataField(Name = "Updater", Title = "更新人", DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? Updater { get; set; }


        [DataField(Name = "BankTitle", Title = "标题", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String BankTitle { get; set; }


        [DataField(Name = "CategoryTitle", Title = "标题", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String CategoryTitle { get; set; }
    }
}
