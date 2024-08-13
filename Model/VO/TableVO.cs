using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CodeHelper.Model.Serialize;

namespace CodeHelper.Model.VO
{
    /// <summary>
    /// 表结构视图
    /// </summary>
    [Serializable]
    public class TableVO
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonConverter(typeof(DefaultDateTimeConverter))]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 字段类型
        /// </summary>
        public List<FieldVO> Fields { get; set; } = new List<FieldVO>();
    }
}
