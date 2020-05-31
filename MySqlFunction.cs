using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CodeHelper
{
    public class MySqlFunction : ISqlFuction
    {
        private static MySqlFunction _instance;
        private static readonly object LockObj = new object();

        private MySqlFunction(){}

        #region Public Methods

        #region public static MySqlFunction GetInstance()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MySqlFunction GetInstance()
        {
            if (_instance != null) return _instance;
            lock (LockObj)
            {
                if (_instance == null)
                {
                    _instance = new MySqlFunction();
                }
                return _instance;
            }
        }
        #endregion public static MySqlFunction GetInstance()
        
        #region public List<TableInfo> GetTables()

        /// <summary>
        ///     获取数据库表名
        /// </summary>
        /// <returns></returns>
        public List<TableInfo> GetTables()
        {
            List<TableInfo> tables = new List<TableInfo>();
            if (Global.GetInstance().MySQLConn() != null && Global.GetInstance().DB != null)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(string.Format("USE {0};SHOW TABLES", Global.GetInstance().DB.DataBase), Global.GetInstance().MySQLConn());

                    cmd.Connection.Open();

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            TableInfo tableInfo = new TableInfo {TableName = rdr.IsDBNull(0) ? string.Empty : rdr.GetString(0)};

                            tableInfo.Fields = GetFields(tableInfo.TableName);
                            
                            tables.Add(tableInfo);
                        }
                        rdr.Close();
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), @"警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return tables;
        }
        #endregion public List<TableInfo> GetTables()

        #region public string GetDalFields(TableInfo tableInfo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        public string GetDalFields(TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL :
                {
                    switch (Global.GetInstance().CodeType)
                    {
                        case CodeTypes.CSharp:
                        case CodeTypes.CSharp4 :
                            return GetCSharpMySqlDalStr(tableInfo);
                        case CodeTypes.Java:
                            return GetJavaMysqlDalStr(tableInfo);
                        default :
                            return string.Empty;
                    }
                }
                default:
                    return string.Empty;
            }
        }
        #endregion public string GetDalFields(TableInfo tableInfo)

        #region public string GetInsertFunc(string className, TableInfo tableInfo)

        public string GetInsertFunc(string className, TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpInsertFunc(className, tableInfo);
                            case CodeTypes.Java:
                                return GetJavaInsertFunc(className, tableInfo);
                            default:
                                return string.Empty;
                        }
                    }
                default:
                    return string.Empty;
            }
        }
        
        #endregion public string GetInsertFunc(string className, TableInfo tableInfo)

        #region public string GetBulkInsertFunc(string className, TableInfo tableInfo)
        /// <summary>
        /// 新增批量添加DAL层方法
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        public string GetBulkInsertFunc(string className, TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpBulkInsertFunc(className, tableInfo);
                            case CodeTypes.Java:
                                return GetJavaBulkInsertFunc(className, tableInfo);
                            default:
                                return string.Empty;
                        }
                    }
                default:
                    return string.Empty;
            }
        }
        #endregion public string GetBulkInsertFunc(string className, TableInfo tableInfo)

        #region  public string GetUpdateFunc(string className, TableInfo tableInfo)
        public string GetUpdateFunc(string className, TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpUpdateFunc(className, tableInfo);
                            case CodeTypes.Java:
                                return GetJavaUpdateFunc(className, tableInfo);
                            default:
                                return string.Empty;
                        }
                    }
                default:
                    return string.Empty;
            }
        }
        #endregion  public string GetUpdateFunc(string className, TableInfo tableInfo)

        #region public string GetDeleteFunc(string className, TableInfo tableInfo)
        /// <summary>
        /// 删除函数
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        public string GetDeleteFunc(string className, TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpDeleteFunc(tableInfo);
                            case CodeTypes.Java:
                                return GetJavaDeleteFunc(tableInfo);
                            default:
                                return string.Empty;
                        }
                    }
                default:
                    return string.Empty;
            }
        }
        #endregion public string GetDeleteFunc(string className, TableInfo tableInfo)

        #region public string GetSelectFunc(string className, TableInfo tableInfo)
        public string GetSelectFunc(string className, TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpSelectFunc(className, tableInfo);
                            case CodeTypes.Java:
                                return GetJavaSelectFunc(className, tableInfo);
                            default:
                                return string.Empty;
                        }
                    }
                default:
                    return string.Empty;
            }
        }
        #endregion public string GetSelectFunc(string className, TableInfo tableInfo)

        #region public string GetAllSelectFunc(string className, TableInfo tableInfo)
        public string GetAllSelectFunc(string className, TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpSelectAllFunc(className, tableInfo);
                            case CodeTypes.Java:
                                return GetJavaSelectAllFunc(className, tableInfo);
                            default:
                                return string.Empty;
                        }
                    }
                default:
                    return string.Empty;
            }
        }
        #endregion public string GetAllSelectFunc(string className, TableInfo tableInfo)

        #region public string GetPrviateFuncs(string className, TableInfo tableInfo)
        public string GetPrviateFuncs(string className, TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpPrivateFuncs(className, tableInfo);
                            case CodeTypes.Java:
                                return GetJavaPrivateFuncs(className, tableInfo);
                            default:
                                return string.Empty;
                        }
                    }
                default:
                    return string.Empty;
            }
        }
        #endregion public string GetPrviateFuncs(string className, TableInfo tableInfo)

        #endregion Public Methods

        #region Private Methods

        #region private static List<FieldInfo> GetFields(string tableName)

        /// <summary>
        ///     获取数据表的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static List<FieldInfo> GetFields(string tableName)
        {
            var fields = new List<FieldInfo>();
            if (Global.GetInstance().MySQLConn() != null && Global.GetInstance().DB != null)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(string.Format("SHOW FULL FIELDS FROM `{0}`", tableName),Global.GetInstance().MySQLConn());
                    cmd.Connection.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var fieldInfo = new FieldInfo
                            {
                                FieldName = rdr.IsDBNull(0) ? string.Empty : rdr.GetString(0),
                                IsIdentity = !rdr.IsDBNull(6) &&
                                             rdr.GetString(6)
                                                 .Equals("auto_increment", StringComparison.OrdinalIgnoreCase),
                                IsPrimaryKey = !rdr.IsDBNull(4) &&
                                               rdr.GetString(4).Equals("PRI", StringComparison.OrdinalIgnoreCase)
                            };

                            //求字段长度
                            int length = 0;
                            string fieldType = rdr.IsDBNull(1) ? "" : rdr.GetString(1);

                            if (fieldType.IndexOf("(", StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                //int(11) left=3; right=6
                                int left = fieldType.IndexOf("(", StringComparison.OrdinalIgnoreCase);
                                int right = fieldType.IndexOf(")", StringComparison.OrdinalIgnoreCase);
                                int.TryParse(fieldType.Substring(left + 1, right - left - 1), out length);
                                fieldInfo.FieldType = fieldType.Substring(0, left);
                            }
                            else
                            {
                                fieldInfo.FieldType = fieldType;
                            }
                            fieldInfo.FieldLength = length;


                            fieldInfo.IsNullAble = !rdr.IsDBNull(3) &&
                                                   rdr.GetString(3).Equals("YES", StringComparison.OrdinalIgnoreCase);
                            fieldInfo.Description = rdr.IsDBNull(8) ? string.Empty : rdr.GetString(8);
                            fields.Add(fieldInfo);
                        }
                        rdr.Close();
                    }
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), @"警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return fields;
        }

        #endregion private static List<FieldInfo> GetFields(string tableName)
        
        #region CSharp代码部分

        #region private string GetCSharpMySqlDalStr(TableInfo tableInfo)
        /// <summary>
        /// 获取CSharp代码的MySQL数据库的DAL访问层SQL语句部分
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetCSharpMySqlDalStr(TableInfo tableInfo)
        {
            //定义输出的内容
            StringBuilder sb = new StringBuilder();
            //定义参数输出
            StringBuilder parmsStr = new StringBuilder();

            //定义插入语句字段输出
            StringBuilder insertFieldStr = new StringBuilder();
            StringBuilder bulkInsertFieldStr = new StringBuilder();
            StringBuilder insertParamStr = new StringBuilder();

            //定义更新语句字段输出
            StringBuilder updateParamStr = new StringBuilder();

            //定义标准唯一参数
            StringBuilder keyParamStr = new StringBuilder();

            //定义选取的参数
            StringBuilder selectParamStr = new StringBuilder();

            //定义排序规则
            StringBuilder orderParamStr = new StringBuilder();

            //遍历字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                parmsStr.Append("\t");
                parmsStr.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_PARMSTRING, fieldInfo.FieldName.ToUpper().Replace("_", ""), fieldInfo.FieldName));


                //不是自增，必然要主动插入
                if (!fieldInfo.IsIdentity)
                {
                    //新增
                    insertFieldStr.AppendFormat("`{0}`,", fieldInfo.FieldName);
                    insertParamStr.AppendFormat("@{0},", fieldInfo.FieldName);
                    //批量添加
                    bulkInsertFieldStr.AppendFormat("`{0}`,", fieldInfo.FieldName);

                    if (!fieldInfo.IsPrimaryKey)
                    {
                        //更新
                        updateParamStr.AppendFormat("`{0}`=@{0},", fieldInfo.FieldName);
                    }
                }

                selectParamStr.AppendFormat("`{0}`,", fieldInfo.FieldName);

            }
            //设置关键key参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                keyParamStr.AppendFormat("`{0}`=@{0}", autoIncr.FieldName);
                orderParamStr.AppendFormat("`{0}`", autoIncr.FieldName);
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    foreach (FieldInfo priField in priFields)
                    {
                        if (keyParamStr.Length > 0)
                        {
                            keyParamStr.Append(" AND ");
                            orderParamStr.Append(" DESC, ");
                        }
                        keyParamStr.AppendFormat("`{0}`=@{0}", priField.FieldName);
                        orderParamStr.AppendFormat("`{0}`", priField.FieldName);
                    }
                }
            }

            sb.AppendLine("\t//SQL语句部分");
            //添加添加语句
            sb.Append("\tprivate const string " + string.Format(ConstVars.INSERT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_INSERT_SQL, tableInfo.TableName,
                insertFieldStr.Remove(insertFieldStr.Length - 1, 1),
                insertParamStr.Remove(insertParamStr.Length - 1, 1), Global.GetInstance().DB.DataBase));

            //添加添加语句
            sb.Append("\tprivate const string " + string.Format(ConstVars.BULK_INSERT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_BULK_INSERT_SQL, tableInfo.TableName,
                bulkInsertFieldStr.Remove(bulkInsertFieldStr.Length - 1, 1), Global.GetInstance().DB.DataBase));

            //添加更新语句
            sb.Append("\tprivate const string " + string.Format(ConstVars.UPDATE_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_UPDATE_SQL, tableInfo.TableName,
               updateParamStr.Remove(updateParamStr.Length - 1, 1), keyParamStr, Global.GetInstance().DB.DataBase));

            //添加删除语句
            sb.Append("\tprivate const string " + string.Format(ConstVars.DELETE_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_DELETE_SQL, tableInfo.TableName, keyParamStr, Global.GetInstance().DB.DataBase));

            //添加单选语句
            sb.Append("\tprivate const string " + string.Format(ConstVars.SELECT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_SELECT_SQL, tableInfo.TableName,
                selectParamStr.Remove(selectParamStr.Length - 1, 1), keyParamStr, Global.GetInstance().DB.DataBase));


            //添加全选语句
            sb.Append("\tprivate const string " + string.Format(ConstVars.SELECT_ALL_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_SELECT_ALL_SQL, tableInfo.TableName,
                selectParamStr, Global.GetInstance().DB.DataBase));

            //添加分页语句
            sb.Append("\tprivate const string " + string.Format(ConstVars.SELECT_PAGED_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_SELECT_PAGED_SQL, tableInfo.TableName,
                selectParamStr, orderParamStr, "{0},{1}", Global.GetInstance().DB.DataBase));

            sb.Append("\tprivate const string " + string.Format(ConstVars.SELECT_COUNT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.CSHARP_MYSQL_COUNT_SQL, tableInfo.TableName, Global.GetInstance().DB.DataBase));

            //添加SQL参数
            sb.AppendLine("");
            sb.AppendLine("\t//SQL使用的参数");
            sb.AppendLine(parmsStr.ToString());
            return sb.ToString();
        }
        #endregion private string GetCSharpMySqlDalStr(TableInfo tableInfo)

        #region private static string GetCSharpInsertFunc(string className, TableInfo tableInfo)

        private static string GetCSharpInsertFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1) + "Info";
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1) + "Info";
            //定义函数名称
            string funcName = string.Format(ConstVars.CSHARP_INSERT_FUNCTION, "bool", modelName, modelObjName);
 
            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            sqlParamStr.AppendLine("\t\tMySqlParameter[] parms = new MySqlParameter[]");
            sqlParamStr.AppendLine("\t\t{");
            //定义参数复制
            StringBuilder sqlParamValueStr = new StringBuilder();

            int i = 0;
            //遍历数据表字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(fieldInfo.FieldName);
                
                if (!fieldInfo.IsIdentity)
                {
                    sqlParamStr.AppendLine(string.Format("\t\t\tnew MySqlParameter({0}, MySqlDbType.{1}),", string.Format(ConstVars.PARM_NAME, fieldInfo.FieldName.ToUpper().Replace("_", "")), Global.GetInstance().GetMySqlDbType(fieldInfo)));

                    sqlParamValueStr.AppendLine(string.Format("\t\tparms[{0}].Value = {1}.{2};", i, modelObjName, fieldPropertyName));
                   
                    i++;
                }
            }
            sqlParamStr.Remove(sqlParamStr.Length - 3, 3);
            sqlParamStr.AppendLine("");
            sqlParamStr.AppendLine("\t\t};");
            
            //添加折块开始
            sb.AppendLine("\t#region " + funcName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 新增一条记录");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine("\t/// <param name=\"" + modelObjName + "\"></param>");
            sb.AppendLine("\t/// <returns>bool 是否添加成功</returns>");
            //添加函数定义
            sb.AppendLine("\t" + funcName);
            //添加函数体开始
            sb.AppendLine("\t{");
            //添加输出结果变量定义
            sb.AppendLine("\t\tbool result = false;");

            //添加MySqlParamter参数定义
            sb.AppendLine(sqlParamStr.ToString());
            //添加MySqlParamter参数赋值
            sb.AppendLine(sqlParamValueStr.ToString());

            //添加数据库操作部分
            sb.AppendLine(string.Format("\t\tresult = Database.ExecuteNonQuery(connString, CommandType.Text, SQL_INSERT_{0}, parms) > 0;", tableInfo.TableName.ToUpper().Replace("_", "")));
          
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + funcName);

            return sb.ToString();
        }
        #endregion private string GetCSharpInsertFunc(string className, TableInfo tableInfo)

        #region private static string GetCSharpBulkInsertFunc(string className, TableInfo tableInfo)
        private static string GetCSharpBulkInsertFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1) + "Info";
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1) + "Info";
            //定义函数名称
            string funcName = string.Format(ConstVars.CSHARP_BULK_INSERT_FUNCTION, modelName, modelObjName);
            //插入的语句名称
            string insertSqlName = string.Format(ConstVars.BULK_INSERT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", ""));
            
            StringBuilder insertParamStr = new StringBuilder();
            //遍历数据表字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (!fieldInfo.IsIdentity)
                {
                    insertParamStr.Append("@" + fieldInfo.FieldName + "{0},");
                }
            }
            insertParamStr.Remove(insertParamStr.Length - 1, 1);

            
            //添加折块开始
            sb.AppendLine("\t#region " + funcName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 批量增加记录");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine("\t/// <param name=\"" + modelObjName + "s\">对象集合</param>");
            sb.AppendLine("\t/// <returns>bool 是否添加成功</returns>");
            //添加函数定义
            sb.AppendLine("\t" + funcName);
            //添加函数体开始
            sb.AppendLine("\t{");
            //添加输出结果变量定义
            sb.AppendLine("\t\tbool result = false;");
            //定义要拼接的SQL
            sb.AppendLine("\t\tStringBuilder insertSql = new StringBuilder();");
            sb.AppendLine("\t\tinsertSql.Append(" + insertSqlName+");");

            //添加数据库操作部分
            //定义循环计数
            sb.AppendLine("\t\tint i=1;");
            sb.AppendLine("\t\t//定义SQL参数");
            sb.AppendLine("\t\tList<MySqlParameter> parms = new List<MySqlParameter>();");
            //遍历循环
            sb.AppendLine("\t\t//遍历对象进行入库操作");
            sb.AppendLine("\t\tforeach ("+modelName+" "+ modelObjName + " in "+modelObjName+"s)");
            sb.AppendLine("\t\t{");
            
            //先添加扩展参数 
            sb.AppendLine("\t\t\tinsertSql.AppendFormat(\"(" + insertParamStr + ");\",i);");
            sb.AppendLine("\t\t\t//把参数添加到参数队列里");
            //遍历添加对应的参数值
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (!fieldInfo.IsIdentity)
                {
                    string fieldPropertyName = BSqlFunction.GetPropertyName(fieldInfo.FieldName);
                    sb.AppendLine("\t\t\tMySqlParameter parm" + fieldPropertyName + " = new MySqlParameter(string.Format(" + "\"@" + fieldInfo.FieldName + "{0}\",i), MySqlDbType." + Global.GetInstance().GetMySqlDbType(fieldInfo) + "),");
                    sb.AppendLine("\t\t\tparm" + fieldPropertyName + ".Value = " + modelObjName + "." + fieldPropertyName + ";");
                    sb.AppendLine("\t\t\tparms.Add(parm" + fieldPropertyName + ");");
                }
            }
            //增加行数判断，操作500行，执行批量入库操作，防止一次提交的数据太多，造成大规模死锁
            sb.AppendLine("\t\t\t//增加行数判断，操作500行，执行批量入库操作，防止一次提交的数据太多，造成大规模死锁");
            sb.AppendLine("\t\t\tif(i % 500 == 0)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tinsertSql.Remove(insertSql.Length - 1, 1);");
            sb.AppendLine("\t\t\t\t//执行SQL插入");
            sb.AppendLine("\t\t\t\tresult = Database.ExecuteNonQuery(connString, CommandType.Text, insertSql.ToString(), parms.ToArray()) > 0;");
            sb.AppendLine("\t\t\t\t//清理SQL语句");
            sb.AppendLine("\t\t\t\tinsertSql.Clear();");
            sb.AppendLine("\t\t\t\tparms.Clear();");
            sb.AppendLine("\t\t\t\t//重新加载SQL语句");
            sb.AppendLine("\t\t\t\tinsertSql.Append(" + insertSqlName + ");");
            sb.AppendLine("\t\t\t}");
            //行数自增
            sb.AppendLine("\t\t\t//行数自增");
            sb.AppendLine("\t\t\ti++;");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t\tif(params.Count > 0 )");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tinsertSql.Remove(insertSql.Length - 1, 1);");
            sb.AppendLine("\t\t\t//执行SQL插入");
            sb.AppendLine("\t\t\tresult = Database.ExecuteNonQuery(connString, CommandType.Text, insertSql.ToString(), parms.ToArray()) > 0;");
            sb.AppendLine("\t\t\t//清理SQL语句");
            sb.AppendLine("\t\t\tinsertSql.Clear();");
            sb.AppendLine("\t\t\tparms.Clear();");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + funcName);

            return sb.ToString();
        }

        #endregion private static string GetCSharpBulkInsertFunc(string className, TableInfo tableInfo)

        #region private static string GetCSharpUpdateFunc(string className, TableInfo tableInfo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private static string GetCSharpUpdateFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1) + "Info";
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1) + "Info";
            //定义函数名称
            string funcName = string.Format(ConstVars.CSHARP_UPDATE_FUNCTION, modelName, modelObjName);

            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            sqlParamStr.AppendLine("\t\tMySqlParameter[] parms = new MySqlParameter[]");
            sqlParamStr.AppendLine("\t\t{");
            //定义参数复制
            StringBuilder sqlParamValueStr = new StringBuilder();

            int i = 0;
            //遍历数据表字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(fieldInfo.FieldName);
                sqlParamStr.AppendLine(string.Format("\t\t\tnew MySqlParameter({0}, MySqlDbType.{1}),", string.Format(ConstVars.PARM_NAME, fieldInfo.FieldName.ToUpper().Replace("_", "")), Global.GetInstance().GetMySqlDbType(fieldInfo)));
                sqlParamValueStr.AppendLine(string.Format("\t\tparms[{0}].Value = {1}.{2};", i, modelObjName, fieldPropertyName));

                i++;
                
            }
            sqlParamStr.Remove(sqlParamStr.Length - 3, 3);
            sqlParamStr.AppendLine("");
            sqlParamStr.AppendLine("\t\t};");

            //添加折块开始
            sb.AppendLine("\t#region " + funcName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 更新记录");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine("\t/// <param name=\"" + modelObjName + "\">对象</param>");
            sb.AppendLine("\t/// <returns>bool 是否更新成功</returns>");
            //添加函数定义
            sb.AppendLine("\t" + funcName);
            //添加函数体开始
            sb.AppendLine("\t{");
            //添加输出结果变量定义
            sb.AppendLine("\t\tbool result = false;");

            //添加MySqlParamter参数定义
            sb.AppendLine(sqlParamStr.ToString());
            //添加MySqlParamter参数赋值
            sb.AppendLine(sqlParamValueStr.ToString());

            //添加数据库操作部分
            sb.AppendLine(string.Format("\t\tresult = Database.ExecuteNonQuery(connString, CommandType.Text, SQL_UPDATE_{0}, parms) > 0;", tableInfo.TableName.ToUpper().Replace("_", "")));

           
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + funcName);

            return sb.ToString();
        }
        #endregion private static string GetCSharpUpdateFunc(string className, TableInfo tableInfo)

        #region private string GetCSharpDeleteFunc(TableInfo tableInfo)

        private string GetCSharpDeleteFunc(TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义参数列表
            StringBuilder argListStr = new StringBuilder();
            StringBuilder argStr = new StringBuilder();

            int argCount = 0;

            //设置关键key参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                argListStr.Append(BSqlFunction.GetCSharpTypeString(autoIncr.FieldType) + " " + BSqlFunction.GetPropertyName(autoIncr.FieldName, true));
                argStr.Append(BSqlFunction.GetPropertyName(autoIncr.FieldName, true));
                argCount ++;
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    foreach (FieldInfo priField in priFields)
                    {
                        argCount++;
                        if (argListStr.Length > 0)
                        {
                            argListStr.Append(", ");
                            argStr.Append(", ");
                        }
                        argListStr.Append(BSqlFunction.GetCSharpTypeString(priField.FieldType) + " " + BSqlFunction.GetPropertyName(priField.FieldName, true));
                        argStr.Append(BSqlFunction.GetPropertyName(priField.FieldName, true));
                    }
                }
            }

            //定义函数名称
            string funcName = string.Format(ConstVars.CSHARP_DELETE_FUNCTION, argListStr);

            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            if (argCount == 1)
            {
                sqlParamStr.AppendLine(string.Format("\t\tMySqlParameter parm = getMySqlParameter({0});", argStr));
            }
            else
            {
                sqlParamStr.AppendLine(string.Format("\t\tMySqlParameter[] parms = getMySqlParameters({0});", argStr));
            }

            //添加折块开始
            sb.AppendLine("\t#region " + funcName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 删除一条记录");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine("\t/// <returns>bool 是否删除成功</returns>");
            //添加函数定义
            sb.AppendLine("\t" + funcName);
            //添加函数体开始
            sb.AppendLine("\t{");
            //添加输出结果变量定义
            sb.AppendLine("\t\tbool result = false;");

            //添加MySqlParamter参数定义
            sb.AppendLine(sqlParamStr.ToString());
            if (argCount == 1)
            {
                //添加数据库操作部分
                sb.AppendLine(string.Format("\t\tresult = Database.ExecuteNonQuery(connString, CommandType.Text, SQL_DELETE_{0}, parm) > 0;", tableInfo.TableName.ToUpper().Replace("_", "")));
            }
            else
            {
                //添加数据库操作部分
                sb.AppendLine(string.Format("\t\tresult = Database.ExecuteNonQuery(connString, CommandType.Text, SQL_DELETE_{0}, parms) > 0;", tableInfo.TableName.ToUpper().Replace("_", "")));
            }
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + funcName);

            return sb.ToString();
        }
        #endregion private string GetCSharpDeleteFunc(TableInfo tableInfo)

        #region private string GetCSharpSelectFunc(string className, TableInfo tableInfo)
        private static string GetCSharpSelectFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

             //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1) + "Info";
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1) + "Info";

            //定义参数列表
            StringBuilder argListStr = new StringBuilder();
            StringBuilder argStr = new StringBuilder();

            int argCount = 0;

            //设置关键key参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                argListStr.Append(BSqlFunction.GetCSharpTypeString(autoIncr.FieldType) + " " + BSqlFunction.GetPropertyName(autoIncr.FieldName, true));
                argStr.Append(BSqlFunction.GetPropertyName(autoIncr.FieldName, true));
                argCount ++;
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    foreach (FieldInfo priField in priFields)
                    {
                        argCount++;
                        if (argListStr.Length > 0)
                        {
                            argListStr.Append(", ");
                            argStr.Append(", ");
                        }
                        argListStr.Append(BSqlFunction.GetCSharpTypeString(priField.FieldType) + " " + BSqlFunction.GetPropertyName(priField.FieldName, true));
                        argStr.Append(BSqlFunction.GetPropertyName(priField.FieldName, true));
                    }
                }
            }

            //定义函数名称
            string funcName = string.Format(ConstVars.CSHARP_SELECT_FUNCTION, BSqlFunction.GetPropertyName(className), argListStr);

            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            if (argCount == 1)
            {
                sqlParamStr.AppendLine(string.Format("\t\tMySqlParameter parm = getMySqlParameter({0});", argStr));
            }
            else
            {
                sqlParamStr.AppendLine(string.Format("\t\tMySqlParameter[] parms = getMySqlParameters({0});", argStr));
            }

            //添加折块开始
            sb.AppendLine("\t#region " + funcName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 获取一条记录");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine(string.Format("\t/// <returns>{0},具体对象或者null</returns>", modelName));
            //添加函数定义
            sb.AppendLine("\t" + funcName);
            //添加函数体开始
            sb.AppendLine("\t{");
            //添加输出结果变量定义
            sb.AppendLine(string.Format("\t\t{0} {1} = null;", modelName, modelObjName));

            //添加MySqlParamter参数定义
            sb.AppendLine(sqlParamStr.ToString());

            //添加SqlDataReader方式
            if (argCount == 1)
            {
                //添加数据库操作部分
                sb.AppendLine(string.Format("\t\tusing(MySqlDataReader rdr = Database.ExecuteReader(connString, CommandType.Text, SQL_SELECT_{0}, parm))", tableInfo.TableName.ToUpper().Replace("_", "")));
            }
            else
            {
                //添加数据库操作部分
                sb.AppendLine(string.Format("\t\tusing(MySqlDataReader rdr = Database.ExecuteReader(connString, CommandType.Text, SQL_SELECT_{0}, parms))", tableInfo.TableName.ToUpper().Replace("_", "")));
            }
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif(rdr.Read())");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine(string.Format("\t\t\t\t{0} = get{1}(rdr);",modelObjName, modelName));
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine(string.Format("\t\treturn {0};", modelObjName));
            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + funcName);

            return sb.ToString();
        }
        #endregion private string GetCSharpSelectFunc(string className, TableInfo tableInfo)

        #region private string GetCSharpSelectAllFunc(string className, TableInfo tableInfo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private static string GetCSharpSelectAllFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1) + "Info";
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1) + "Info";

            //定义函数名称
            string funcName = string.Format(ConstVars.CSHARP_SELECT_ALL_FUNCTION, BSqlFunction.GetPropertyName(className));
            
            //添加折块开始
            sb.AppendLine("\t#region " + funcName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 获取所有记录");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine(string.Format("\t/// <returns>List<{0}>,对象列表</returns>", modelName));
            //添加函数定义
            sb.AppendLine("\t" + funcName);
            //添加函数体开始
            sb.AppendLine("\t{");
            //添加输出结果变量定义
            sb.AppendLine(string.Format("\t\tList<{0}> {1}s = new List<{0}>();", modelName, modelObjName));
            
            //添加数据库操作部分
            sb.AppendLine(string.Format("\t\tusing(MySqlDataReader rdr = Database.ExecuteReader(connString, CommandType.Text, SQL_SELECT_{0}S))", tableInfo.TableName.ToUpper().Replace("_", "")));
            
            
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\twhile(rdr.Read())");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine(string.Format("\t\t\t\t{0}s.Add(get{1}(rdr));", modelObjName, modelName));
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine(string.Format("\t\treturn {0}s;", modelObjName));
            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + funcName);

            return sb.ToString();
        }
        #endregion private string GetCSharpSelectAllFunc(string className, TableInfo tableInfo)

        #region private string GetCSharpPrivateFuncs(string className, TableInfo tableInfo)
        private static string GetCSharpPrivateFuncs(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1) + "Info";
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1) + "Info";

            //定义参数列表
            StringBuilder argListStr = new StringBuilder();
            StringBuilder argStr = new StringBuilder();

            //定义获取默认SQL参数的方法
            StringBuilder getParamFuncName = new StringBuilder();
            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            StringBuilder sqlParamValueStr = new StringBuilder();

            int argCount = 0;

            //设置关键key参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                argListStr.Append(BSqlFunction.GetCSharpTypeString(autoIncr.FieldType) + " " + BSqlFunction.GetPropertyName(autoIncr.FieldName, true));
                argStr.Append(BSqlFunction.GetPropertyName(autoIncr.FieldName, true));
                argCount++;
                sqlParamStr.AppendLine(string.Format("\t\tMySqlParameter parm = new MySqlParameter({0}, MySqlDbType.{1});", string.Format(ConstVars.PARM_NAME, autoIncr.FieldName.ToUpper().Replace("_", "")), Global.GetInstance().GetMySqlDbType(autoIncr)));
                sqlParamValueStr.AppendLine(string.Format("\t\tparm.Value = {0};", BSqlFunction.GetPropertyName(autoIncr.FieldName, true)));
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    if (priFields.Count == 1)
                    {
                        argListStr.Append(BSqlFunction.GetCSharpTypeString(priFields[0].FieldType) + " " +
                                          BSqlFunction.GetPropertyName(priFields[0].FieldName, true));
                        argStr.Append(BSqlFunction.GetPropertyName(priFields[0].FieldName, true));
                       
                        sqlParamStr.AppendLine(
                            string.Format("\t\tMySqlParameter parm = new MySqlParameter({0}, MySqlDbType.{1});",
                                string.Format(ConstVars.PARM_NAME, priFields[0].FieldName.ToUpper().Replace("_", "")),
                                Global.GetInstance().GetMySqlDbType(priFields[0])));
                        sqlParamValueStr.AppendLine(string.Format("\t\tparm.Value = {0};",
                            BSqlFunction.GetPropertyName(priFields[0].FieldName, true)));
                        argCount++;
                    }
                    else
                    {
                        sqlParamStr.AppendLine("\t\tMySqlParameter[] parms = new MySqlParameter[]");
                        sqlParamStr.AppendLine("\t\t{");
                        foreach (FieldInfo priField in priFields)
                        {
                            if (argCount > 0)
                            {
                                argListStr.Append(", ");
                                argStr.AppendLine(", ");
                                sqlParamStr.Append(",");
                                sqlParamStr.AppendLine("");
                            }

                            sqlParamStr.AppendFormat("\t\t\tnew MySqlParameter({0}, MySqlDbType.{1})", string.Format(ConstVars.PARM_NAME, priField.FieldName.ToUpper().Replace("_", "")), Global.GetInstance().GetMySqlDbType(priField));
                            sqlParamValueStr.AppendLine(string.Format("\t\tparms[{0}].Value = {1};", argCount, BSqlFunction.GetPropertyName(priField.FieldName, true)));
                           
                            argListStr.Append(BSqlFunction.GetCSharpTypeString(priField.FieldType) + " " + BSqlFunction.GetPropertyName(priField.FieldName, true));
                            argStr.Append(BSqlFunction.GetPropertyName(priField.FieldName, true));

                            argCount++;
                        }
                        sqlParamStr.AppendLine("");
                        sqlParamStr.AppendLine("\t\t};");
                    }
                }
            }
            
            //定义参数表
            if (argCount == 1)
            {
                getParamFuncName.AppendLine(string.Format("private static MySqlParameter getMySqlParameter({0})", argListStr));
            }
            else
            {
                getParamFuncName.AppendLine(string.Format("private static MySqlParameter[] getMySqlParameters({0})", argListStr));
            }

            //添加折块开始
            sb.AppendLine("\t#region " + getParamFuncName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 获取主键参数");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine("\t/// <returns></returns>");
            //添加函数定义
            sb.AppendLine("\t" + getParamFuncName);
            //添加函数体开始
            sb.AppendLine("\t{");
            //添加输出结果变量定义
            sb.AppendLine(sqlParamStr.ToString());
            //添加赋值
            sb.AppendLine(sqlParamValueStr.ToString());
            //添加结果输出
            if (argCount == 1)
            {
                sb.AppendLine("\t\treturn parm;");
            }
            else
            {
                sb.AppendLine("\t\treturn parms");
            }
            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + getParamFuncName);

            sb.AppendLine("");
            //开始新增获取SqlDataReader的model对象
            string funcName = string.Format("private static {0} get{0}(MySqlDataReader rdr)", modelName);

            //添加折块开始
            sb.AppendLine("\t#region " + funcName);
            //添加函数说明
            sb.AppendLine("\t/// <summary>");
            sb.AppendLine("\t/// 根据MyDataReader获取对象");
            sb.AppendLine("\t/// </summary>");
            sb.AppendLine(string.Format("\t/// <returns>{0},对象</returns>", modelName));
            //添加函数定义
            sb.AppendLine("\t" + funcName);
            //添加函数体开始
            sb.AppendLine("\t{");

            sb.AppendLine(string.Format("\t\t{0} {1} = new {0}(", modelName, modelObjName));

            int i = 0;
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                sb.AppendLine(string.Format("\t\t\t{0}", GetCSharpMySqlDataReader(fieldInfo.FieldType, i)));
                i++;
            }
            sb.Remove(sb.Length - 3, 3);
            sb.AppendLine("");
            sb.AppendLine("\t\t);");
            //输出返回值
            sb.AppendLine(string.Format("\t\treturn {0};", modelObjName));

            //添加函数体结束
            sb.AppendLine("\t}");
            //添加折块结束
            sb.AppendLine("\t#endregion " + funcName);

            return sb.ToString();
        }
        #endregion private static string GetCSharpPrivateFuncs(string className, TableInfo tableInfo)

        #region private static string GetCSharpMySqlDataReader(string fieldType, int index)

        private static string GetCSharpMySqlDataReader(string fieldType, int index)
        {
            switch (fieldType.ToLower())
            {
                default:
                    return string.Format("rdr.IsDBNull({0}) ? string.Empty : rdr.GetString({0}),", index);
                case "int":
                    return string.Format("rdr.IsDBNull({0}) ? 0 : rdr.GetInt32({0}),", index);
                case "short":
                    return string.Format("rdr.IsDBNull({0}) ? (short)0 : rdr.GetInt16({0}),", index);
                case "tinyint":
                    return string.Format("rdr.IsDBNull({0}) ? (byte)0 : rdr.GetByte({0}),", index);
                case "bigint":
                    return string.Format("rdr.IsDBNull({0}) ? 0L : rdr.GetInt64({0}),", index);
                case "decimal":
                    return string.Format("rdr.IsDBNull({0}) ? decimal.Zero : rdr.GetDecimal({0}),", index);
                case "float":
                    return string.Format("rdr.IsDBNull({0}) ? (double)0 : rdr.GetDouble({0}),", index);
                case "datetime":
                case "date":
                case "time":
                    return string.Format("rdr.IsDBNull({0}) ? new DateTime(1900,1,1) : rdr.GetDateTime({0}),", index);
            }
        }
        #endregion private static string GetCSharpMySqlDataReader(string fieldType, int index)

        #endregion CSharp代码部分

        #region Java代码部分

        #region private static string GetJavaMysqlDalStr(TableInfo tableInfo)
        /// <summary>
        /// 构造SQL
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private static string GetJavaMysqlDalStr(TableInfo tableInfo)
        {
            //定义输出的内容
            StringBuilder sb = new StringBuilder();
          
            //定义插入语句字段输出
            StringBuilder insertFieldStr = new StringBuilder();
            StringBuilder insertParamStr = new StringBuilder();
            //定义更新语句字段输出
            StringBuilder updateParamStr = new StringBuilder();
            //定义标准唯一参数
            StringBuilder keyParamStr = new StringBuilder();
            //定义选取的参数
            StringBuilder selectParamStr = new StringBuilder();

            //定义排序规则
            StringBuilder orderParamStr = new StringBuilder();

            //遍历字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                //不是自增，必然要主动插入
                if (!fieldInfo.IsIdentity)
                {
                    //新增
                    insertFieldStr.AppendFormat("`{0}`,", fieldInfo.FieldName);
                    insertParamStr.AppendFormat("?,");
                   
                    if (!fieldInfo.IsPrimaryKey)
                    {
                        //更新
                        updateParamStr.AppendFormat("`{0}`=?,", fieldInfo.FieldName);
                    }
                }
                selectParamStr.AppendFormat("`{0}`,", fieldInfo.FieldName);

            }
            //设置关键key参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                keyParamStr.AppendFormat("`{0}`=?", autoIncr.FieldName);
                orderParamStr.AppendFormat("`{0}`", autoIncr.FieldName);
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    foreach (FieldInfo priField in priFields)
                    {
                        if (keyParamStr.Length > 0)
                        {
                            keyParamStr.Append(" AND ");
                            orderParamStr.Append(" DESC, ");
                        }
                        keyParamStr.AppendFormat("`{0}`=?", priField.FieldName);
                        orderParamStr.AppendFormat("`{0}`", priField.FieldName);
                    }
                }
            }

            sb.AppendLine("\t//SQL语句部分");
            //添加添加语句
            sb.Append("\tprivate final static String " + string.Format(ConstVars.INSERT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.JAVA_MYSQL_INSERT_SQL, tableInfo.TableName,
                insertFieldStr.Remove(insertFieldStr.Length - 1, 1), insertParamStr.Remove(insertParamStr.Length - 1, 1)));
            
            //添加更新语句
            sb.Append("\tprivate final static String " + string.Format(ConstVars.UPDATE_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.JAVA_MYSQL_UPDATE_SQL, tableInfo.TableName,
               updateParamStr.Remove(updateParamStr.Length - 1, 1), keyParamStr));

            //添加删除语句
            sb.Append("\tprivate final static String " + string.Format(ConstVars.DELETE_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.JAVA_MYSQL_DELETE_SQL, tableInfo.TableName, keyParamStr));

            //添加单选语句
            sb.Append("\tprivate final static String " + string.Format(ConstVars.SELECT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.JAVA_MYSQL_SELECT_SQL, tableInfo.TableName,
                selectParamStr.Remove(selectParamStr.Length - 1, 1), keyParamStr));

            //添加全选语句
            sb.Append("\tprivate final static String " + string.Format(ConstVars.SELECT_ALL_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.JAVA_MYSQL_SELECT_ALL_SQL, tableInfo.TableName, selectParamStr));

            //添加分页语句
            sb.Append("\tprivate final static String " + string.Format(ConstVars.SELECT_PAGED_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.JAVA_MYSQL_SELECT_PAGED_SQL, tableInfo.TableName, selectParamStr, orderParamStr, "%d,%d"));

            sb.Append("\tprivate final static String " + string.Format(ConstVars.SELECT_COUNT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + " = ");
            sb.AppendLine(string.Format(ConstVars.JAVA_MYSQL_COUNT_SQL, tableInfo.TableName));
            
            
            return sb.ToString();
        }

        #endregion private static string GetJavaMysqlDalStr(TableInfo tableInfo)

        #region private static string GetJavaInsertFunc(string className, TableInfo tableInfo)

        private static string GetJavaInsertFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1);
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1);
            //定义函数名称
            string funcName = string.Format(ConstVars.JAVA_INSERT_FUNCTION, "boolean", modelName, modelObjName);

            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            sqlParamStr.AppendLine("\t\t\tObject[] params = new Object[] {");
            
            //遍历数据表字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(fieldInfo.FieldName);

                if (!fieldInfo.IsIdentity && !fieldInfo.IsPrimaryKey)
                {
                    sqlParamStr.AppendLine(string.Format("\t\t\t\t{0}.get{1}(),", modelObjName, fieldPropertyName));
                }
            }
            
            sqlParamStr.Remove(sqlParamStr.Length - 3, 3);
            sqlParamStr.AppendLine("");
            sqlParamStr.AppendLine("\t\t\t};");

           
            //添加函数说明
            sb.AppendLine("\t/**");
            sb.AppendLine("\t * 新增一条记录");
            sb.AppendLine("\t * @param " + modelObjName + " 具体对象");
            sb.AppendLine("\t * @return boolean 是否新增成功");
            sb.AppendLine("\t */");
            //添加函数定义
            sb.Append("\t" + funcName);
            //添加函数体开始
            sb.AppendLine(" throws SQLException {");
            //添加输出结果变量定义
            sb.AppendLine("\t\tboolean result = false;");
            //定义数据库连接字符串
            sb.AppendLine("\t\tConnection conn = null;");

            sb.AppendLine("\t\ttry {");
            sb.AppendLine(string.Format("\t\t\tconn = DBManager.getInstance().getConnection(\"{0}\");", Global.GetInstance().DB.DataBase));

            //添加SQL语句的参数
            sb.AppendLine(sqlParamStr.ToString());

            sb.AppendLine(string.Format("\t\t\tresult = DataAccess.executeCommand(conn, {0}, params) >0;", string.Format(ConstVars.INSERT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", ""))));

            sb.AppendLine("\t\t} catch (SQLException ex) {");
            sb.AppendLine("\t\t\tthrow ex;");
            sb.AppendLine("\t\t} finally {");
            sb.AppendLine("\t\t\ttry {");
            sb.AppendLine("\t\t\t\tif(conn != null && !conn.isClosed()) {");
            sb.AppendLine("\t\t\t\t\tconn.close();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t} catch (SQLException e) {");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            return sb.ToString();
        }
        #endregion private string GetJavaInsertFunc(string className, TableInfo tableInfo)

        #region private static string GetJavaBulkInsertFunc(string className, TableInfo tableInfo)
        private static string GetJavaBulkInsertFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1);
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1);
            //定义函数名称
            string funcName = string.Format(ConstVars.JAVA_BULK_INSERT_FUNCTION, modelName, modelObjName);
            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            int i = 1;
            //遍历数据表字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (!fieldInfo.IsIdentity)
                {
                    sqlParamStr.AppendLine("\t\t\t\t" + BSqlFunction.GetJavaPreparedStatement(fieldInfo, i, modelObjName));
                    i++;
                }
            }
            sqlParamStr.Remove(sqlParamStr.Length - 2, 2);
            sqlParamStr.AppendLine("");


            //添加函数说明
            sb.AppendLine("\t/**");
            sb.AppendLine("\t * 批量添加记录");
            sb.AppendLine("\t * @param " + modelObjName + "s 一组对象信息");
            sb.AppendLine("\t * @return boolean 是否添加成功");
            sb.AppendLine("\t */");
            //添加函数定义
            sb.Append("\t" + funcName);
            //添加函数体开始
            sb.AppendLine(" throws SQLException {");
            //添加输出结果变量定义
            sb.AppendLine("\t\tboolean result = false;");
            //定义数据库连接字符串
            sb.AppendLine("\t\tConnection conn = null;");

            sb.AppendLine("\t\ttry {");
            sb.AppendLine(string.Format("\t\t\tconn = DBManager.getInstance().getConnection(\"{0}\");", Global.GetInstance().DB.DataBase));
            sb.AppendLine("\t\t\tconn.setAutoCommit(false);");

            //添加SQL语句的参数
            sb.AppendLine("\t\t\tPreparedStatement ps = conn.prepareStatement(" + string.Format(ConstVars.INSERT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", "")) + ",ResultSet.TYPE_SCROLL_SENSITIVE,ResultSet.CONCUR_READ_ONLY);");
          
            //开始循环
            sb.Append(string.Format("\t\t\tfor({0} {1} : {1}s)", modelName, modelObjName));
            sb.AppendLine("{");
            sb.AppendLine(sqlParamStr.ToString());
            sb.AppendLine("\t\t\t\tps.addBatch();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\t\tps.executeBatch();");
            sb.AppendLine("\t\t\tconn.commit();");
            sb.AppendLine("\t\t\tresult = true;");
            sb.AppendLine("\t\t} catch (SQLException ex) {");
            sb.AppendLine("\t\t\tthrow ex;");
            sb.AppendLine("\t\t} finally {");
            sb.AppendLine("\t\t\ttry {");
            sb.AppendLine("\t\t\t\tif(conn != null && !conn.isClosed()) {");
            sb.AppendLine("\t\t\t\t\tconn.close();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t} catch (SQLException e) {");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            return sb.ToString();
        }

        #endregion private static string GetJavaBulkInsertFunc(string className, TableInfo tableInfo)

        #region private static string GetJavaUpdateFunc(string className, TableInfo tableInfo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private static string GetJavaUpdateFunc(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1);
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1);
            //定义函数名称
            string funcName = string.Format(ConstVars.JAVA_UPDATE_FUNCTION, modelName, modelObjName);

            //定义参数表
            StringBuilder sqlParamStr = new StringBuilder();
            sqlParamStr.AppendLine("\t\t\tObject[] params = new Object[] {");

            //遍历数据表字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(fieldInfo.FieldName);

                if (!fieldInfo.IsIdentity && !fieldInfo.IsPrimaryKey)
                {
                    sqlParamStr.AppendLine(string.Format("\t\t\t\t{0}.get{1}(),", modelObjName, fieldPropertyName));
                }
            }
            //根据自增字段或者主键字段添加相应的参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(autoIncr.FieldName);
                sqlParamStr.AppendLine(string.Format("\t\t\t\t{0}.get{1}()", modelObjName, fieldPropertyName));
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    foreach (FieldInfo priField in priFields)
                    {
                        string fieldPropertyName = BSqlFunction.GetPropertyName(priField.FieldName);
                        sqlParamStr.AppendLine(string.Format("\t\t\t\t{0}.get{1}()", modelObjName, fieldPropertyName));
                    }
                }
            }

            sqlParamStr.Remove(sqlParamStr.Length - 2, 2);
            sqlParamStr.AppendLine("");
            sqlParamStr.AppendLine("\t\t\t};");


            //添加函数说明
            sb.AppendLine("\t/**");
            sb.AppendLine("\t * 更新记录");
            sb.AppendLine("\t * @param " + modelObjName + " 具体对象");
            sb.AppendLine("\t * @return boolean 是否更新成功");
            sb.AppendLine("\t */");
            //添加函数定义
            sb.Append("\t" + funcName);
            //添加函数体开始
            sb.AppendLine(" throws SQLException {");
            //添加输出结果变量定义
            sb.AppendLine("\t\tboolean result = false;");
            //定义数据库连接字符串
            sb.AppendLine("\t\tConnection conn = null;");

            sb.AppendLine("\t\ttry {");
            sb.AppendLine(string.Format("\t\t\tconn = DBManager.getInstance().getConnection(\"{0}\");", Global.GetInstance().DB.DataBase));

            //添加SQL语句的参数
            sb.AppendLine(sqlParamStr.ToString());

            sb.AppendLine(string.Format("\t\t\tresult = DataAccess.executeCommand(conn, {0}, params) >0;", string.Format(ConstVars.UPDATE_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", ""))));

            sb.AppendLine("\t\t} catch (SQLException ex) {");
            sb.AppendLine("\t\t\tthrow ex;");
            sb.AppendLine("\t\t} finally {");
            sb.AppendLine("\t\t\ttry {");
            sb.AppendLine("\t\t\t\tif(conn != null && !conn.isClosed()) {");
            sb.AppendLine("\t\t\t\t\tconn.close();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t} catch (SQLException e) {");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            return sb.ToString();
        }
        #endregion private string GetJavaUpdateFunc(string className, TableInfo tableInfo)

        #region private static string GetJavaDeleteFunc(TableInfo tableInfo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private static string GetJavaDeleteFunc(TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();
            
            //定义函数参数表
            StringBuilder argBuilder = new StringBuilder();
            //定义SQL参数表
            StringBuilder sqlParamStr = new StringBuilder();
            sqlParamStr.AppendLine("\t\t\tObject[] params = new Object[] {");

            //根据自增字段或者主键字段添加相应的参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(autoIncr.FieldName, true);
                argBuilder.Append(BSqlFunction.GetJavaTypeString(autoIncr.FieldType) + " " + fieldPropertyName + ", ");
                sqlParamStr.AppendLine("\t\t\t\t" + fieldPropertyName + ",");
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    foreach (FieldInfo priField in priFields)
                    {
                        string fieldPropertyName = BSqlFunction.GetPropertyName(priField.FieldName, true);
                        argBuilder.Append(BSqlFunction.GetJavaTypeString(priField.FieldType) + " " + fieldPropertyName + ", ");
                        sqlParamStr.AppendLine("\t\t\t\t" + fieldPropertyName + ",");
                    }
                }
            }
            argBuilder.Remove(argBuilder.Length - 2, 2);
            sqlParamStr.Remove(sqlParamStr.Length - 3, 3);
            sqlParamStr.AppendLine("");
            sqlParamStr.AppendLine("\t\t\t};");

            //定义函数名称
            string funcName = string.Format(ConstVars.JAVA_DELETE_FUNCTION, argBuilder);

            //添加函数说明
            sb.AppendLine("\t/**");
            sb.AppendLine("\t * 删除一条记录");
            sb.AppendLine("\t * @param ");
            sb.AppendLine("\t * @return boolean 是否删除成功");
            sb.AppendLine("\t */");
            //添加函数定义
            sb.Append("\t" + funcName);
            //添加函数体开始
            sb.AppendLine(" throws SQLException {");
            //添加输出结果变量定义
            sb.AppendLine("\t\tboolean result = false;");
            //定义数据库连接字符串
            sb.AppendLine("\t\tConnection conn = null;");

            sb.AppendLine("\t\ttry {");
            sb.AppendLine(string.Format("\t\t\tconn = DBManager.getInstance().getConnection(\"{0}\");", Global.GetInstance().DB.DataBase));

            //添加SQL语句的参数
            sb.AppendLine(sqlParamStr.ToString());

            sb.AppendLine($"\t\t\tresult = DataAccess.executeCommand(conn, {string.Format(ConstVars.DELETE_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", ""))}, params) >0;");
            
            sb.AppendLine("\t\t} catch (SQLException ex) {");
            sb.AppendLine("\t\t\tthrow ex;");
            sb.AppendLine("\t\t} finally {");
            sb.AppendLine("\t\t\ttry {");
            sb.AppendLine("\t\t\t\tif(conn != null && !conn.isClosed()) {");
            sb.AppendLine("\t\t\t\t\tconn.close();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t} catch (SQLException e) {");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine("\t\treturn result;");
            //添加函数体结束
            sb.AppendLine("\t}");
            return sb.ToString();
        }
        #endregion private static string GetJavaDeleteFunc(TableInfo tableInfo)

        #region private static string GetJavaSelectFunc(string className, TableInfo tableInfo)
        private static string GetJavaSelectFunc(string className, TableInfo tableInfo)
        {
            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1);
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1);

            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义函数参数表
            StringBuilder argBuilder = new StringBuilder();
            //定义SQL参数表
            StringBuilder sqlParamStr = new StringBuilder();
            sqlParamStr.AppendLine("\t\t\tObject[] params = new Object[] {");

            //根据自增字段或者主键字段添加相应的参数
            FieldInfo autoIncr = tableInfo.Fields.FirstOrDefault(c => c.IsIdentity);
            if (autoIncr != null)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(autoIncr.FieldName, true);
                argBuilder.Append(BSqlFunction.GetJavaTypeString(autoIncr.FieldType) + " " + fieldPropertyName + ", ");
                sqlParamStr.AppendLine("\t\t\t\t" + fieldPropertyName + ",");
            }
            else
            {
                List<FieldInfo> priFields = tableInfo.Fields.FindAll(c => c.IsPrimaryKey);
                if (priFields.Count > 0)
                {
                    foreach (FieldInfo priField in priFields)
                    {
                        string fieldPropertyName = BSqlFunction.GetPropertyName(priField.FieldName, true);
                        argBuilder.Append(BSqlFunction.GetJavaTypeString(priField.FieldType) + " " + fieldPropertyName + ", ");
                        sqlParamStr.AppendLine("\t\t\t\t" + fieldPropertyName + ",");
                    }
                }
            }
            
            argBuilder.Remove(argBuilder.Length - 2, 2);
            sqlParamStr.Remove(sqlParamStr.Length - 3, 3);
            sqlParamStr.AppendLine("");
            sqlParamStr.AppendLine("\t\t\t};");

            //定义函数名称
            string funcName = string.Format(ConstVars.JAVA_SELECT_FUNCTION, modelName, argBuilder);

            //添加函数说明
            sb.AppendLine("\t/**");
            sb.AppendLine("\t * 获取一条记录");
            sb.AppendLine("\t * @param ");
            sb.AppendLine($"\t * @return {modelName} or NULL");
            sb.AppendLine("\t */");
            //添加函数定义
            sb.Append("\t" + funcName);
            //添加函数体开始
            sb.AppendLine(" throws SQLException {");
            //添加输出结果变量定义
            sb.AppendLine(string.Format("\t\t{0} {1} = null;", modelName, modelObjName));
            //定义数据库连接字符串
            sb.AppendLine("\t\tConnection conn = null;");

            sb.AppendLine("\t\ttry {");
            sb.AppendLine(string.Format("\t\t\tconn = DBManager.getInstance().getConnection(\"{0}\");", Global.GetInstance().DB.DataBase));

            //添加SQL语句的参数
            sb.AppendLine(sqlParamStr.ToString());

            sb.AppendLine($"\t\t\tResultSet rs = DataAccess.getResultSet(conn, {string.Format(ConstVars.SELECT_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", ""))}, params);");

            sb.AppendLine("\t\t\tif(rs != null && rs.next()) {");
            sb.AppendLine($"\t\t\t\t{modelObjName}=get{modelName}(rs);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\t//关闭结果集");
            sb.AppendLine("\t\t\tif(rs != null){");
            sb.AppendLine("\t\t\t\trs.close();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t} catch (SQLException ex) {");
            sb.AppendLine("\t\t\tthrow ex;");
            sb.AppendLine("\t\t} finally {");
            sb.AppendLine("\t\t\ttry {");
            sb.AppendLine("\t\t\t\tif(conn != null && !conn.isClosed()) {");
            sb.AppendLine("\t\t\t\t\tconn.close();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t} catch (SQLException e) {");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine($"\t\treturn {modelObjName};");
            //添加函数体结束
            sb.AppendLine("\t}");
            return sb.ToString();
        }
        #endregion private string GetJavaSelectFunc(string className, TableInfo tableInfo)

        #region private static string GetJavaSelectAllFunc(string className, TableInfo tableInfo)
        private static string GetJavaSelectAllFunc(string className, TableInfo tableInfo)
        {
            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1);
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1);

            //定义总的输出
            StringBuilder sb = new StringBuilder();
            
            //定义函数名称
            string funcName = string.Format(ConstVars.JAVA_SELECT_ALL_FUNCTION, modelName, BSqlFunction.GetPropertyName(className));

            //添加函数说明
            sb.AppendLine("\t/**");
            sb.AppendLine("\t * 获取所有记录");
            sb.AppendLine("\t * @return ArrayList实例对象数组");
            sb.AppendLine("\t */");
            //添加函数定义
            sb.Append("\t" + funcName);
            //添加函数体开始
            sb.AppendLine(" throws SQLException {");
            //添加输出结果变量定义
            sb.AppendLine(string.Format("\t\tList<{0}> {1}s = new ArrayList<{0}>();", modelName, modelObjName));
            //定义数据库连接字符串
            sb.AppendLine("\t\tConnection conn = null;");

            sb.AppendLine("\t\ttry {");
            sb.AppendLine(string.Format("\t\t\tconn = DBManager.getInstance().getConnection(\"{0}\");", Global.GetInstance().DB.DataBase));
            
            sb.AppendLine(string.Format("\t\t\tResultSet rs = DataAccess.getResultSet(conn, {0});", string.Format(ConstVars.SELECT_ALL_SQL_NAME, tableInfo.TableName.ToUpper().Replace("_", ""))));

            sb.AppendLine("\t\t\twhile(rs != null && rs.next()) {");
            sb.AppendLine($"\t\t\t\t{modelObjName}s.add(get{modelName}(rs));");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\t//关闭结果集");
            sb.AppendLine("\t\t\tif(rs != null){");
            sb.AppendLine("\t\t\t\trs.close();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t} catch (SQLException ex) {");
            sb.AppendLine("\t\t\tthrow ex;");
            sb.AppendLine("\t\t} finally {");
            sb.AppendLine("\t\t\ttry {");
            sb.AppendLine("\t\t\t\tif(conn != null && !conn.isClosed()) {");
            sb.AppendLine("\t\t\t\t\tconn.close();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t} catch (SQLException e) {");
            sb.AppendLine("\t\t\t\tthrow e;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //添加结果输出
            sb.AppendLine($"\t\treturn {modelObjName}s;");
            //添加函数体结束
            sb.AppendLine("\t}");
            return sb.ToString();
        }
        #endregion private string GetJavaSelectAllFunc(string className, TableInfo tableInfo)

        #region private static string GetJavaPrivateFuncs(string className, TableInfo tableInfo)
        private static string GetJavaPrivateFuncs(string className, TableInfo tableInfo)
        {
            //定义总的输出
            StringBuilder sb = new StringBuilder();

            //定义model类型名
            string modelName = className.Substring(0, 1).ToUpper() + className.Substring(1);
            //定义model对象名
            string modelObjName = className.Substring(0, 1).ToLower() + className.Substring(1);

            //添加函数说明
            sb.AppendLine("\t/** ");
            sb.AppendLine("\t * 根据ResultSet获取实例对象");
            sb.AppendLine("\t * params rs 结果集");
            sb.AppendLine("\t * @return " + modelName);
            sb.AppendLine("\t */");
            //开始新增获取SqlDataReader的model对象
            string funcName = string.Format("private static {0} get{0}(ResultSet rs) throws SQLException", modelName);

            //添加函数定义
            sb.Append("\t" + funcName);
            //添加函数体开始
            sb.AppendLine(" {");

            sb.AppendLine(string.Format("\t\t{0} {1} = new {0}();", modelName, modelObjName));

            //遍历读取Field记录
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                sb.AppendLine($"\t\t{modelObjName}.set{fieldInfo.FieldName}({BSqlFunction.GetJavaResultSet(fieldInfo)});");
            }
           
           
            //输出返回值
            sb.AppendLine($"\t\treturn {modelObjName};");
            //添加函数体结束
            sb.AppendLine("\t}");
            return sb.ToString();
        }
        #endregion private static string GetJavaPrivateFuncs(string className, TableInfo tableInfo)

        #endregion Java代码部分

        #endregion Private Methods
    }
}