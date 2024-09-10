using System.ComponentModel;
using System.Runtime.Serialization;

namespace CodeHelper.Enum
{
    /// <summary>
    /// 异常类型
    /// </summary>
    public enum ExceptionTypes
    {
        /// <summary>
        /// 成功
        /// </summary>
        [EnumMember(Value = "200"), Description("成功")]
        Success = 200,

        /// <summary>
        /// 参数错误
        /// </summary>
        [EnumMember(Value = "400"), Description("参数错误")]
        ParameterError = 400,

        /// <summary>
        /// 数据库访问失败
        /// </summary>
        [EnumMember(Value = "405"), Description("数据库访问失败")]
        DatabaseAccessError = 405
    }
}
