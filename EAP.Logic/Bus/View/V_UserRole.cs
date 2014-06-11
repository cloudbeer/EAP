using System;
using Zippy.Data.Mapping;

namespace EAP.Logic.Bus.View
{
    [DataTable(Name = "V_UserRole", Title = "用户角色")]
    public class V_UserRole
    {
        [DataField(Name = "UserID", Title = "用户", DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? UserID { get; set; }

        [DataField(Name = "RoleID", Title = "角色", DbType = System.Data.DbType.Guid, SqlDbType = System.Data.SqlDbType.UniqueIdentifier)]
        public virtual Guid? RoleID { get; set; }

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
    }
}
