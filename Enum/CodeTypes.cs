using System.ComponentModel;
using System.Runtime.Serialization;

namespace CodeHelper.Enum
{
    /// <summary>
    /// 代码类型
    /// </summary>
    public enum CodeTypes
    {
        /// <summary>
        /// C#代码
        /// </summary>
        [EnumMember(Value = "10"), Description("C#代码")]
        CSharp = 10,
 
        /// <summary>
        /// Java代码
        /// </summary>
        [EnumMember(Value = "20"), Description("Java代码")]
        Java = 20
    }
}
