using System;

namespace CodeHelper
{
    /// <summary>
    /// ���ݱ���ֶ�����
    /// </summary>
    [Serializable]
    public class FieldInfo
    {
        /// <summary>
        /// �ֶ�����
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// �ֶ���������
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// �Ƿ�������
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// �ֶγ���
        /// </summary>
        public int FieldLength { get; set; }
        /// <summary>
        /// �ֶ�˵��
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// �ɷ�Ϊ��
        /// </summary>
        public bool IsNullAble { get; set; }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}