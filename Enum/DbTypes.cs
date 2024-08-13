using System.Runtime.Serialization;

namespace CodeHelper.Enum
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbTypes
    {
        /// <summary>
        /// SQLServer2005及以上版本数据库
        /// </summary>
        [EnumMember(Value ="10")]
        SQLServer = 10,

        /// <summary>
        /// MySQL数据库
        /// </summary>
        [EnumMember(Value = "20")]
        MySQL = 20
    }
}
