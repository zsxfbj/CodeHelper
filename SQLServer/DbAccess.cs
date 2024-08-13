using System;
using System.Collections.Generic;
using CodeHelper.IDAL;
using CodeHelper.Model.VO;

namespace CodeHelper.SQLServer
{
    /// <summary>
    /// 
    /// </summary>
    public class DbAccess : IDbAccess
    {
        /// <summary>
        /// 获取数据表的查询语句
        /// </summary>
        private const string SQL_GET_TABLES = "SELECT [Name] FROM {0}..SysObjects Where XType='U' ORDER BY Name";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connString"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<FieldVO> GetFields(string tableName, string connString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<TableVO> GetTables(string connString)
        {
            throw new NotImplementedException();
        }
    }
}
