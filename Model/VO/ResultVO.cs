using CodeHelper.Enum;

namespace CodeHelper.Model.VO
{
    /// <summary>
    /// 处理结果
    /// </summary>
    public class ActionResultVO<T>
    {
        /// <summary>
        /// 错误类型
        /// </summary>
        public ExceptionTypes ExceptionType { get; set; } = ExceptionTypes.Success;

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 输出的结果
        /// </summary>
        public T Data { get; set; } = default!;
    }
}
