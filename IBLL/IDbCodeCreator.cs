using System.Collections.Generic;
using CodeHelper.Model.VO;

namespace CodeHelper.IBLL
{
    /// <summary>
    /// 代码创建方法
    /// </summary>
    public interface IDbCodeCreator
    {
        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        List<string> GetParms(TableVO table);

        /// <summary>
        /// 根据表结构获取新增SQL语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetInsertSQLString(TableVO table);

        /// <summary>
        /// 根据表结构获取更新的SQL语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetUpdateSQLString(TableVO table);

        /// <summary>
        /// 根据表结构获取删除的SQL语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetDeleteSQLString(TableVO table);

        /// <summary>
        /// 根据主键获取单条记录语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetSelectSQLString(TableVO table);

        /// <summary>
        /// 获取分页查询时，总记录数的语句
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetPagedCountSQLString(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetPagedSelectSQLString(TableVO table);

        /// <summary>
        /// 获取新增方法
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetInsertFunction(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetUpdateFunction(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetDeleteFunction(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetSelectFunction(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetPagedFunction(TableVO table);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetToVOFunction(TableVO table);
    }
}
