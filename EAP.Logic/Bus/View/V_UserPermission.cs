using System;
using Zippy.Data.Mapping;


namespace EAP.Logic.Bus.View
{
    [DataTable(Name = "V_UserPermission", Title = "用户角色")]
    public class V_UserPermission
    {
        [DataField(Name = "UserID", Title = "用户编号", IsPrimaryKey = true, AllowNull = false, DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? UserID { get; set; }

        [DataField(Name = "UserName", Title = "用户名", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String UserName { get; set; }

        [DataField(Name = "Email", Title = "Email", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Email { get; set; }

        [DataField(Name = "Name", Title = "名字", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Name { get; set; }

        [DataField(Name = "Nickname", Title = "昵称", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Nickname { get; set; }

        [DataField(Name = "Title", Title = "标题", AllowNull = false, DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Title { get; set; }

        [DataField(Name = "Url", Title = "链接地址", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Url { get; set; }

        [DataField(Name = "Flag", Title = "标识", DbType = System.Data.DbType.String, SqlDbType = System.Data.SqlDbType.NVarChar)]
        public virtual String Flag { get; set; }

        [DataField(Name = "PermissionType", Title = "权限表类型", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? PermissionType { get; set; }

        [DataField(Name = "PermissionStatus", Title = "权限表状态", DbType = System.Data.DbType.Int32, SqlDbType = System.Data.SqlDbType.Int)]
        public virtual Int32? PermissionStatus { get; set; }

        [DataField(Name = "TenantID", Title = "租户", AllowNull = false, DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? TenantID { get; set; }

        [DataField(Name = "PermissionID", Title = "权限表编号", IsPrimaryKey = true, AutoIncrement = true, AllowNull = false, DbType = System.Data.DbType.Int64, SqlDbType = System.Data.SqlDbType.BigInt)]
        public virtual Int64? PermissionID { get; set; }
    }
}
