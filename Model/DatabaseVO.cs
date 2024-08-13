using System;
using System.Collections.Generic;

namespace CodeHelper.Model
{
    /// <summary>
    /// 数据库视图
    /// </summary>
    [Serializable]
    public class DatabaseVO
    {
        /// <summary>
        /// 访问地址
        /// </summary>
        public string Address {  get; set; } = string.Empty;

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
        /// 数据库名称
        /// </summary>
        public string DbName {  get; set; } = string.Empty; 

        /// <summary>
        /// 配置的名称
        /// </summary>
        public string ConfigName {  get; set; } = string.Empty;

        /// <summary>
        /// 数据库里的数据表
        /// </summary>
        public List<TableVO> Tables { get; set; } = new List<TableVO>();
    }
}
