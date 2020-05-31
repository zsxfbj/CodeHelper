namespace CodeHelper
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class ConstVars
    {
        #region SQL语句名称
        
        public const string INSERT_SQL_NAME = "SQL_INSERT_{0}";
        public const string BULK_INSERT_SQL_NAME = "SQL_BULK_INSERT_{0}";
        public const string UPDATE_SQL_NAME = "SQL_UPDATE_{0}";
        public const string DELETE_SQL_NAME = "SQL_DELETE_{0}";
        public const string SELECT_SQL_NAME = "SQL_SELECT_{0}";
        public const string SELECT_ALL_SQL_NAME = "SQL_SELECT_{0}S";
        public const string SELECT_PAGED_SQL_NAME = "SQL_SELECT_PAGED_{0}S";
        public const string SELECT_COUNT_SQL_NAME = "SQL_COUNT_{0}S";
        public const string PARM_NAME = "PARM_{0}";

        #endregion SQL语句名称

        #region CSharp SQL语句部分
        //CSharp下SQLServer的SQL语句
        public const string CSHARP_SQLSERVER_INSERT_SQL = "INSERT INTO [{0}] ({1}) VALUES ({2})";
        public const string CSHARP_SQLSERVER_UPDATE_SQL = "UPDATE [{0}] SET {1} WHERE {2}";
        public const string CSHARP_SQLSERVER_DELETE_SQL = "DELETE FROM [{0}] WHERE {1}";
        public const string CSHARP_SQLSERVER_SELECT_SQL = "SELECT {1} FROM [{0}] WHERE {2}";
        public const string CSHARP_SQLSERVER_SELECT_ALL_SQL = "SELECT {1} FROM [{0}]";
        public const string CSHARP_SQLSERVER_SELECT_PAGED_SQL = @"SELECT {1} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {2}) AS rowNum, * FROM [{0}]) AS t WHERE rowNum BETWEEN @StartIndex AND @EndIndex";
        public const string CSHARP_SQLSERVER_SELECT_COUNT_ALL = "SELECT COUNT(*) AS RecorderCount FROM {0}";
        public const string CSHARP_SQLSERVER_PARMSTRING = "private const string PARM_{0} = \"@{1}\";";

        //CSharp下MySQL的SQL语句
        public const string CSHARP_MYSQL_INSERT_SQL = "\"INSERT INTO `{3}`.`{0}` ({1}) VALUES ({2})\";";

        public const string CSHARP_MYSQL_BULK_INSERT_SQL = "\"INSERT INTO `{2}`.`{0}` ({1}) VALUES \";";
        //public const string CSHARP_MYSQL_BULK_INSERT_SQL_VALUES = "\"({0}),\"";
        public const string CSHARP_MYSQL_UPDATE_SQL = "\"UPDATE `{3}`.`{0}` SET {1} WHERE {2}\";";
        public const string CSHARP_MYSQL_DELETE_SQL = "\"DELETE FROM `{2}`.`{0}` WHERE {1}\";";
        public const string CSHARP_MYSQL_SELECT_SQL = "\"SELECT {1} FROM `{3}`.`{0}` WHERE {2}\";";
        public const string CSHARP_MYSQL_SELECT_ALL_SQL = "\"SELECT {1} FROM `{2}`.`{0}`\";";

        public const string CSHARP_MYSQL_SELECT_PAGED_SQL = "\"SELECT {1} FROM `{4}`.`{0}` ORDER BY {2} DESC LIMIT {3}\";";
        public const string CSHARP_MYSQL_COUNT_SQL = "\"SELECT COUNT(*) AS RecorderCount FROM `{1}`.`{0}`\";";

        /// <summary>
        /// CSharp下的MySQL表字段参数
        /// </summary>
        public const string CSHARP_MYSQL_PARMSTRING = "private const string PARM_{0} = \"?{1}\";";

        #endregion CSharp SQL语句部分

        #region CSharp DAL层函数

        //CSharp代码部分

        /// <summary>
        /// 新增一条记录的函数定义
        /// </summary>
        public const string CSHARP_INSERT_FUNCTION = @"public {0} Insert({1} {2})";
        /// <summary>
        /// 批量新增函数定义
        /// </summary>
        public const string CSHARP_BULK_INSERT_FUNCTION = @"public bool Insert(List<{0}> {1}s)";
        /// <summary>
        /// 更新函数定义
        /// </summary>
        public const string CSHARP_UPDATE_FUNCTION = @"public bool Update({0} {1})";
        /// <summary>
        /// 删除函数定义
        /// </summary>
        public const string CSHARP_DELETE_FUNCTION = @"public bool Delete({0})";
        /// <summary>
        /// 选取单一的记录
        /// </summary>
        public const string CSHARP_SELECT_FUNCTION = @"public {0}Info Get{0}({1})";

        /// <summary>
        /// 选取所有的记录
        /// </summary>
        public const string CSHARP_SELECT_ALL_FUNCTION = @"public List<{0}Info> Get{0}s()";

        /// <summary>
        /// 选取分页的记录
        /// </summary>
        public const string CSHARP_SELECT_PAGED_FUNCTION = @"public List<{0}Info> Get{0}s(int startIndex, int pageSize)";

        /// <summary>
        /// 获取MySqlParameter方法
        /// </summary>
        public const string CSHARP_GET_MYSQL_PARAM_FUNCTION = @"private static MySqlParameter getParameter({0})";
        /// <summary>
        /// 从MySqlDataReader读取数据获取对象的方法
        /// </summary>
        public const string CSHARP_GET_MYSQL_MODEL_FUNCTION = @"private static {0}Info get{0}(MySqlDataReader rdr)";

        #endregion CSharp DAL层函数

        #region SQL连接字符串
        /// <summary>
        /// SQLServer数据库连接字符串
        /// </summary>
        public const string SQLSERVER_CONNECT_STRING = "Data Source={0},{3};User Id={1};Password={2};Database={4}";
        /// <summary>
        /// MySQL数据库连接字符串
        /// </summary>
        public const string MYSQL_CONNECT_STRING = "server={0};uid={1};pwd={2};port={3};database={4};sslmode=none";

        #endregion SQL连接字符串
        
        #region JAVA 语句部分
        //JAVA MYSQL下的SQL
        public const string JAVA_MYSQL_INSERT_SQL = "\"INSERT INTO `{0}` ({1}) VALUES ({2})\";";
        public const string JAVA_MYSQL_UPDATE_SQL = "\"UPDATE `{0}` SET {1} WHERE {2}\";";
        public const string JAVA_MYSQL_DELETE_SQL = "\"DELETE FROM `{0}` WHERE {1}\";";
        public const string JAVA_MYSQL_SELECT_SQL = "\"SELECT {1} FROM `{0}` WHERE {2}\";";
        public const string JAVA_MYSQL_SELECT_ALL_SQL = "\"SELECT {1} FROM `{0}`\";";
        public const string JAVA_MYSQL_SELECT_PAGED_SQL = "\"SELECT {1} FROM `{0}` ORDER BY {2} DESC LIMIT {3}\";";
        public const string JAVA_MYSQL_COUNT_SQL = "\"SELECT COUNT(*) AS RecorderCount FROM `{0}`\";";

        /// <summary>
        /// 新增一条记录的函数定义
        /// </summary>
        public const string JAVA_INSERT_FUNCTION = @"public {0} insert({1} {2})";
        /// <summary>
        /// 批量新增函数定义
        /// </summary>
        public const string JAVA_BULK_INSERT_FUNCTION = @"public boolean insert(List<{0}> {1}s)";

        /// <summary>
        /// 更新函数定义
        /// </summary>
        public const string JAVA_UPDATE_FUNCTION = @"public boolean update({0} {1})";
        /// <summary>
        /// 删除函数定义
        /// </summary>
        public const string JAVA_DELETE_FUNCTION = @"public boolean delete({0})";
        /// <summary>
        /// 选取单一的记录
        /// </summary>
        public const string JAVA_SELECT_FUNCTION = @"public {0} get{0}({1})";

        /// <summary>
        /// 选取所有的记录
        /// </summary>
        public const string JAVA_SELECT_ALL_FUNCTION = @"public List<{0}> get{1}s()";
        #endregion

        public const string TRY = "try{";
        public const string CATCH2 = "} catch (Exception e){ System.out.print(e.getMessage()); } finally {if(conn != null && !conn.isClosed()){conn.close();}  }";

        public const string CATCH3 = "} catch (Exception e){ System.out.print(e.getMessage()); } finally {if(conn != null && !conn.isClosed()){conn.close();} \r if(stmt != null){stmt.close();}if(rs != null){rs.close();} }";

    }
}