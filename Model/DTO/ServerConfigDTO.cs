using System;
using CodeHelper.Model.Serialize;
using System.Text.Json.Serialization;
using CodeHelper.Enum;

namespace CodeHelper.Model.DTO
{
    /// <summary>
    /// 数据库服务器配置信息
    /// </summary>
    public class ServerConfigDTO
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName { get; set; } = string.Empty;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbTypes DbType { get; set; }

        /// <summary>
        /// 服务器Ip或者信息
        /// </summary>
        public string Server { get; set; } = string.Empty;

        /// <summary>
        /// 访问的用户账号
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 访问的密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 访问的端口（默认端口，可以不填写）
        /// </summary>
        public string Port { get; set; } = string.Empty;

        /// <summary>
        /// 可不配置
        /// </summary>
        public string Database { get; set; } = string.Empty;

        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonConverter(typeof(DefaultDateTimeConverter))]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新日期
        /// </summary>
        [JsonConverter(typeof(DefaultDateTimeConverter))]
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
