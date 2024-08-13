using System;
using System.Collections.Generic;

namespace CodeHelper.Model
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
        /// 字段类型
        /// </summary>
        public List<FieldVO> Fields { get; set; } = new List<FieldVO>();
    }
}
