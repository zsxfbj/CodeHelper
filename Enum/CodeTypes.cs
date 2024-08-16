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
        /// C#原生代码
        /// </summary>
        [EnumMember(Value = "10"), Description("C#原生代码")]
        CSharp = 10,
        /// <summary>
        /// C#基于EF6.0的代码
        /// </summary>
        [EnumMember(Value = "11"), Description("C# EntityFramework6.0代码")]
        CSharpEF = 11,
        /// <summary>
        /// Java原生代码
        /// </summary>
        [EnumMember(Value = "20"), Description("Java原生代码")]
        Java = 20
    }
}
