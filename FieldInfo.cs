using System;

namespace CodeHelper
{
    /// <summary>
    /// 数据表的字段属性
    /// </summary>
    [Serializable]
    public class FieldInfo
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段数据类型
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        public int FieldLength { get; set; }
        /// <summary>
        /// 字段说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 可否为空
        /// </summary>
        public bool IsNullAble { get; set; }
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}