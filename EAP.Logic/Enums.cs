using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAP.Logic
{
    public enum DeleteFlags
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 回收站
        /// </summary>
        Trashed = 2,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 3,
    }

}
