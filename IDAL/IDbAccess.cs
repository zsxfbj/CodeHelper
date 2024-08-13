using System.Collections.Generic;
using CodeHelper.Model.VO;

namespace CodeHelper.IDAL
{
    /// <summary>
    /// 数据库相关的操作方法定义
    /// </summary>
    public interface IDbAccess
    {
        

        /// <summary>
        /// 获取数据库表列表及其结构
        /// </summary>
        /// <returns></returns>
        List<TableVO> GetTables(string connString);

        /// <summary>
        /// 根据表名获取字段列表信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        List<FieldVO> GetFields(string tableName, string connString);
    }
}
