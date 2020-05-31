using System.Collections.Generic;

namespace CodeHelper
{
    public interface ISqlFuction
    {
        /// <summary>
        /// 获取数据库表列表及其结构
        /// </summary>
        /// <returns></returns>
        List<TableInfo> GetTables();

        /// <summary>
        /// 获取SQL语句及对应的SQL参数表
        /// </summary>
        /// <param name="tableInfo"></param>
        
        /// <returns></returns>
        string GetDalFields(TableInfo tableInfo);
        
        /// <summary>
        /// 获取新增函数
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string GetInsertFunc(string className, TableInfo tableInfo);

        /// <summary>
        /// 获取批量新增函数
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string GetBulkInsertFunc(string className, TableInfo tableInfo);
        /// <summary>
        /// 获取更新函数
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string GetUpdateFunc(string className, TableInfo tableInfo);

        /// <summary>
        /// 获取删除函数
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string GetDeleteFunc(string className, TableInfo tableInfo);

        /// <summary>
        /// 获取读取一条函数
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string GetSelectFunc(string className, TableInfo tableInfo);
        /// <summary>
        /// 获取所有记录函数
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string GetAllSelectFunc(string className, TableInfo tableInfo);
        /// <summary>
        /// 获取私有方式函数集
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        string GetPrviateFuncs(string className, TableInfo tableInfo);
    }
}