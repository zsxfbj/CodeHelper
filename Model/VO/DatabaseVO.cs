using System;
using System.Collections.Generic;
using CodeHelper.Model.Serialize;
using System.Text.Json.Serialization;

namespace CodeHelper.Model.VO
{
    /// <summary>
    /// 数据库视图
    /// </summary>
    [Serializable]
    public class DatabaseVO
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; } = string.Empty;

        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonConverter(typeof(DefaultDateTimeConverter))]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 数据库里的数据表
        /// </summary>
        public List<TableVO> Tables { get; set; } = new List<TableVO>();
    }
}
