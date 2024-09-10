using System;
using CodeHelper.Enum;


namespace CodeHelper.Model.Exception
{
    /// <summary>
    /// 服务异常类
    /// </summary>
    [Serializable]
    public class ServiceException : System.Exception
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ExceptionTypes ErrorCode { get; set; } = ExceptionTypes.Success;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {            
            return "ErrorCode:" + (int) ErrorCode + ", ErrorMessage: " + (string.IsNullOrEmpty(ErrorMessage) ? ErrorCode.GetDescription() : ErrorMessage);
        }

    }
}
