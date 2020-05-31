using System.Collections.Generic;

namespace CodeHelper
{
    /// <summary>
    /// 数据表结构<br />
    /// author: ShengXiongFeng
    /// </summary>
    [System.Serializable]
    public sealed class TableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public List<FieldInfo> Fields { get; set; }

    }
}