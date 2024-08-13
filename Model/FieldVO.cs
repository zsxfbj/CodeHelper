using System;

namespace CodeHelper.Model
{
    /// <summary>
    /// 字段属性描述视图
    /// </summary>
    [Serializable]
    public class FieldVO
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; } = string.Empty;
        /// <summary>
        /// 字段数据类型
        /// </summary>
        public string FieldType { get; set; } = string.Empty;
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey { get; set; } = false;
        /// <summary>
        /// 字段长度
        /// </summary>
        public int FieldLength { get; set; } = 0;
        /// <summary>
        /// 字段说明
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 可否为空
        /// </summary>
        public bool IsNullAble { get; set; } = true;
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { get; set; } = false;
    }
}
