using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeHelper
{
    public class SqlServerFunction : ISqlFuction
    {
        private SqlServerFunction()
        {
        }

        #region Fields

        private static SqlServerFunction _instance;

        /// <summary>
        /// 获取数据表的查询语句
        /// </summary>
        private const string SQL_GET_TABLES = "Select Name FROM {0}..SysObjects Where XType='U' ORDER BY Name";

        /// <summary>
        /// 获取字段的查询语句
        /// </summary>
        private const string SQL_GET_FIELDS = @"SELECT      
a.colorder  as FieldNo  
,a.name  as FieldName  
,(case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then 1 else 0 end) as IsIdentity  
,(case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (SELECT name FROM sysindexes WHERE indid in(SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then 1 else 0 end) as IsPrimaryKey  
, b.name as FieldType  
, COLUMNPROPERTY(a.id,a.name,'PRECISION') as FieldLength  
, (case when a.isnullable=1 then 1 else 0 end) as IsNullAble  
,isnull(e.text,'') as FieldDefault  
,isnull(g.[value],'')  as Decription  
FROM    syscolumns a    
left join    systypes b  on    a.xusertype=b.xusertype    
inner join    sysobjects d on    a.id=d.id and d.xtype='U' and d.name<>'dtproperties'    
left join    syscomments e on    a.cdefault=e.id left join    
sys.extended_properties g on    a.id=G.major_id and a.colid=g.minor_id    
left join sys.extended_properties f on    d.id=f.major_id and f.minor_id=0    
where   d.name='{0}'     
order by  a.id,a.colorder";

        #endregion Fields

        #region Public Methods

        #region public static SQLServerFunction GetInstance()

        public static SqlServerFunction GetInstance()
        {
            if (_instance != null) return _instance;
            _instance = new SqlServerFunction();
            return _instance;
        }

        #endregion public static SQLServerFunction GetInstance()

        #region public List<TableInfo> GetTables()

        /// <summary>
        /// 获取数据库表名
        /// </summary>
        /// <returns></returns>
        public List<TableInfo> GetTables()
        {
            List<TableInfo> tables = new List<TableInfo>();
            if (Global.GetInstance().SQLServerConn() != null && Global.GetInstance().DB != null)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(string.Format(SQL_GET_TABLES, Global.GetInstance().DB.DataBase),
                        Global.GetInstance().SQLServerConn());

                    cmd.Connection.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            TableInfo tableInfo = new TableInfo
                            {
                                TableName = rdr.IsDBNull(0) ? string.Empty : rdr.GetString(0)
                            };

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
        public string GetDalFields(TableInfo tableInfo)
        {
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    {
                        switch (Global.GetInstance().CodeType)
                        {
                            case CodeTypes.CSharp:
                            case CodeTypes.CSharp4:
                                return GetCSharpSqlDalStr(tableInfo);
                            case CodeTypes.Java:
                                return GetJavaMysqlDalStr(tableInfo);
                            default:
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

        #region private List<FieldInfo> GetFields(string tableName)

        /// <summary>
        /// 获取数据表的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private List<FieldInfo> GetFields(string tableName)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            if (Global.GetInstance().SQLServerConn() != null && Global.GetInstance().DB != null)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(string.Format(SQL_GET_FIELDS, tableName),
                        Global.GetInstance().SQLServerConn());
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            FieldInfo fieldInfo = new FieldInfo
                            {
                                FieldName = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                                IsIdentity = (rdr.GetInt32(2) == 1),
                                IsPrimaryKey = (rdr.GetInt32(3) == 1),
                                FieldType = rdr.GetString(4),
                                FieldLength = rdr.GetInt32(5),
                                IsNullAble = (rdr.GetInt32(6) == 1),
                                Description = rdr.IsDBNull(8) ? string.Empty : rdr.GetString(8)
                            };
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

        #endregion private List<FieldInfo> GetFields(string tableName)

        #region private string getCSharpSqlDalStr(TableInfo tableInfo)
        /// <summary>
        /// 获取CSharp代码的MySQL数据库的DAL访问层SQL语句部分
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetCSharpSqlDalStr(TableInfo tableInfo)
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
        #endregion private string getCSharpSqlDalStr(TableInfo tableInfo)

        #region private string getJavaMysqlDalStr(TableInfo tableInfo)
        /// <summary>
        /// 构造SQL
        /// </summary>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetJavaMysqlDalStr(TableInfo tableInfo)
        {
            //定义输出的内容
            StringBuilder sb = new StringBuilder();
            //SQL
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("\tprivate static final String " + string.Format(ConstVars.INSERT_SQL_NAME, tableInfo.TableName.ToUpper()) + " = \" INSERT INTO `" + tableInfo.TableName + "` (");
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (!fieldInfo.IsIdentity)
                {
                    sqlBuilder.Append(fieldInfo.FieldName + ",");
                }
            }
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            sqlBuilder.Append(") VALUES (");
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (!fieldInfo.IsIdentity)
                {
                    sqlBuilder.Append("?,");
                }
            }
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            sqlBuilder.Append(")\";");
            sqlBuilder.AppendLine();

            sqlBuilder.Append("\tprivate static final String " + string.Format(ConstVars.DELETE_SQL_NAME, tableInfo.TableName.ToUpper()) + " = \" DELETE  FROM `" + tableInfo.TableName + "` WHERE ");
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (fieldInfo.IsPrimaryKey)
                {
                    sqlBuilder.Append("" + fieldInfo.FieldName);
                }
            }
            sqlBuilder.AppendLine(" = ?\";");
            sqlBuilder.AppendLine("\tprivate static final String " + string.Format(ConstVars.SELECT_SQL_NAME, tableInfo.TableName.ToUpper()) + " = \" SELECT * FROM `" + tableInfo.TableName + "` \";");
            sqlBuilder.Append("\tprivate static final String " + string.Format(ConstVars.UPDATE_SQL_NAME, tableInfo.TableName.ToUpper()) + " = \" UPDATE `" + tableInfo.TableName + "` SET ? = ? WHERE ");
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (fieldInfo.IsPrimaryKey)
                {
                    sqlBuilder.Append("" + fieldInfo.FieldName);
                }
            }
            sqlBuilder.AppendLine(" = ?\";");
            sb.Append(sqlBuilder);
            sb.AppendLine();

            //insert方法
            StringBuilder insertBuilder = new StringBuilder();
            insertBuilder.AppendLine("");
            insertBuilder.AppendLine("\t//insert");

            insertBuilder.Append("\tpublic boolean insert(" + BSqlFunction.GetPropertyName(tableInfo.TableName) + " " + BSqlFunction.GetPropertyName(tableInfo.TableName, true));
            insertBuilder.AppendLine(")throws Exception{");
            insertBuilder.AppendLine("\t\tboolean result = false;");
            insertBuilder.AppendLine("\t\tConnection conn = null;");
            insertBuilder.AppendLine("\t\t" + ConstVars.TRY);

            insertBuilder.AppendLine("\t\t\tconn = DBManager.getConnection(\"myconn\");");
            insertBuilder.AppendLine("\t\t\tif(" + BSqlFunction.GetPropertyName(tableInfo.TableName, true) + " != null){");

            insertBuilder.Append("\t\t\tObject[] params = new Object[]{");

            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                if (!fieldInfo.IsIdentity)
                {
                    insertBuilder.Append(BSqlFunction.GetPropertyName(tableInfo.TableName, true) + ".get" +
                                         BSqlFunction.GetPropertyName(fieldInfo.FieldName) + "(),");
                }
            }
            insertBuilder.Remove(insertBuilder.Length - 1, 1);
            insertBuilder.AppendLine("};");
            insertBuilder.AppendLine("\t\t\tint intResult = DataAccess.executeCommand(conn, " + string.Format(ConstVars.INSERT_SQL_NAME, tableInfo.TableName.ToUpper()) + ", params);");
            insertBuilder.AppendLine("\t\t\tif(intResult>0){");
            insertBuilder.AppendLine("\t\t\t\tresult = true;");
            insertBuilder.AppendLine("\t\t\t}");
            insertBuilder.AppendLine("\t\t}");
            insertBuilder.AppendLine("\t\t" + ConstVars.CATCH2);
            insertBuilder.AppendLine("\t\treturn result;");
            insertBuilder.AppendLine("\t}");
            sb.Append(insertBuilder);


            //delete方法
            StringBuilder deleteBuilder = new StringBuilder();
            deleteBuilder.AppendLine("");
            deleteBuilder.AppendLine("\t//delete");
            deleteBuilder.AppendLine("\tpublic boolean delete(int id)throws Exception{");

            deleteBuilder.AppendLine("\t\tboolean result = false;");
            deleteBuilder.AppendLine("\t\tConnection conn = null;");
            deleteBuilder.AppendLine("\t\t" + ConstVars.TRY);
            deleteBuilder.AppendLine("\t\t\tconn = DBManager.getConnection(\"myconn\");");
            deleteBuilder.AppendLine("\t\t\tif(id >0){");
            deleteBuilder.AppendLine("\t\t\t\tObject[] params = new Object[]{id};");
            deleteBuilder.AppendLine("\t\t\t\tint intResult = DataAccess.executeCommand(conn, " + string.Format(ConstVars.DELETE_SQL_NAME, tableInfo.TableName.ToUpper()) + ", params);");
            deleteBuilder.AppendLine("\t\t\tif(intResult>0){");
            deleteBuilder.AppendLine("\t\t\t\tresult = true;");
            deleteBuilder.AppendLine("\t\t\t}");
            deleteBuilder.AppendLine("\t\t}");
            deleteBuilder.AppendLine("\t\t" + ConstVars.CATCH2);
            deleteBuilder.AppendLine("\t\treturn result;");
            deleteBuilder.AppendLine("\t}");
            sb.Append(deleteBuilder);

            //select
            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.AppendLine("");
            selectBuilder.AppendLine("\t//select");
            selectBuilder.AppendLine("\tpublic List<" + BSqlFunction.GetPropertyName(tableInfo.TableName) + "> get" + BSqlFunction.GetPropertyName(tableInfo.TableName) + "s()throws Exception{");
            selectBuilder.AppendLine("\t\tList<" + BSqlFunction.GetPropertyName(tableInfo.TableName) + "> " + BSqlFunction.GetPropertyName(tableInfo.TableName, true) + " = new ArrayList<" + BSqlFunction.GetPropertyName(tableInfo.TableName) + ">();");
            selectBuilder.AppendLine("\t\tConnection conn = null;");
            selectBuilder.AppendLine("\t\t" + ConstVars.TRY);
            selectBuilder.AppendLine("\t\t\tconn = DBManager.getConnection(\"myconn\");");
            selectBuilder.AppendLine("\t\t\tResultSet rs = DataAccess.getResultSet(conn, " + string.Format(ConstVars.SELECT_SQL_NAME, tableInfo.TableName.ToUpper()) + ");");
            selectBuilder.AppendLine("\t\t\twhile(rs != null && rs.next()){");
            selectBuilder.AppendLine("\t\t\t\t" + BSqlFunction.GetPropertyName(tableInfo.TableName, true) + ".add(get" + BSqlFunction.GetPropertyName(tableInfo.TableName) + "(rs));");
            selectBuilder.AppendLine("\t\t\t}");
            selectBuilder.AppendLine("\t\t" + ConstVars.CATCH2);
            selectBuilder.AppendLine("\t\treturn " + BSqlFunction.GetPropertyName(tableInfo.TableName, true) + ";");
            selectBuilder.AppendLine("\t\t}");
            sb.Append(selectBuilder);

            //update
            StringBuilder updateBuilder = new StringBuilder();
            updateBuilder.AppendLine("");
            updateBuilder.AppendLine("\t//update");
            updateBuilder.AppendLine("\tpublic boolean update(String param1,String param2,int id)throws Exception{");

            updateBuilder.AppendLine("\t\tboolean result = false;");
            updateBuilder.AppendLine("\t\tConnection conn = null;");
            updateBuilder.AppendLine("\t\t" + ConstVars.TRY);
            updateBuilder.AppendLine("\t\t\tconn = DBManager.getConnection(\"myconn\");");
            updateBuilder.AppendLine("\t\t\tif(id >0){");
            updateBuilder.AppendLine("\t\t\t\tObject[] params = new Object[]{param1,param2,id};");
            updateBuilder.AppendLine("\t\t\t\tint intResult = DataAccess.executeCommand(conn, " + string.Format(ConstVars.UPDATE_SQL_NAME, tableInfo.TableName.ToUpper()) + ", params);");
            updateBuilder.AppendLine("\t\t\tif(intResult>0){");
            updateBuilder.AppendLine("\t\t\t\tresult = true;");
            updateBuilder.AppendLine("\t\t\t}");
            updateBuilder.AppendLine("\t\t}");
            updateBuilder.AppendLine("\t\t" + ConstVars.CATCH2);
            updateBuilder.AppendLine("\t\treturn result;");
            updateBuilder.AppendLine("\t}");

            sb.Append(updateBuilder);


            //从ResultSet转换成对象
            StringBuilder rsBuilder = new StringBuilder();
            rsBuilder.AppendLine("");
            rsBuilder.AppendLine("\t//从ResultSet转换成对象");
            rsBuilder.AppendLine("\tprivate static " + BSqlFunction.GetPropertyName(tableInfo.TableName) + " get" + BSqlFunction.GetPropertyName(tableInfo.TableName) + "(ResultSet rs) throws Exception{");
            rsBuilder.Append("\t\t" + BSqlFunction.GetPropertyName(tableInfo.TableName) + " " + BSqlFunction.GetPropertyName(tableInfo.TableName, true) + " = new " + BSqlFunction.GetPropertyName(tableInfo.TableName) + "(");
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                string type = BSqlFunction.GetJavaTypeString(fieldInfo.FieldType);
                rsBuilder.AppendLine("rs.get" + type.Substring(0, 1).ToUpper() + type.Substring(1, type.Length - 1) + "(\"" + fieldInfo.FieldName + "\"),");
            }
            rsBuilder.Remove(rsBuilder.Length - 1, 1);
            rsBuilder.AppendLine(");");
            rsBuilder.AppendLine("\t\treturn " + BSqlFunction.GetPropertyName(tableInfo.TableName, true) + ";");
            rsBuilder.Append("}");
            sb.Append(rsBuilder);

            return sb.ToString();
        }

        #endregion

        #region private string getCSharpInsertFunc(string className, TableInfo tableInfo)
        private string GetCSharpInsertFunc(string className, TableInfo tableInfo)
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
            sqlParamStr.AppendLine("\t\tSqlParameter[] parms = new SqlParameter[]");
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
                    sqlParamStr.AppendLine(string.Format("\t\t\tnew SqlParameter({0}, SqlDbType.{1}),", string.Format(ConstVars.PARM_NAME, fieldInfo.FieldName.ToUpper().Replace("_", "")), GetSqlDbType(fieldInfo)));

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

            //添加SqlParamter参数定义
            sb.AppendLine(sqlParamStr.ToString());
            //添加SqlParamter参数赋值
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
        #endregion private string getCSharpInsertFunc(string className, TableInfo tableInfo)

        #region private string getCSharpBulkInsertFunc(string className, TableInfo tableInfo)
        private string GetCSharpBulkInsertFunc(string className, TableInfo tableInfo)
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
            sb.AppendLine("\t\tinsertSql.Append(" + insertSqlName + ");");

            //添加数据库操作部分
            //定义循环计数
            sb.AppendLine("\t\tint i=1;");
            sb.AppendLine("\t\t//定义SQL参数");
            sb.AppendLine("\t\tList<SqlParameter> parms = new List<SqlParameter>();");
            //遍历循环
            sb.AppendLine("\t\t//遍历对象进行入库操作");
            sb.AppendLine("\t\tforeach (" + modelName + " " + modelObjName + " in " + modelObjName + "s)");
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
                    sb.AppendLine("\t\t\tSqlParameter parm" + fieldPropertyName + " = new SqlParameter(string.Format(" + "\"@" + fieldInfo.FieldName + "{0}\",i), SqlDbType." + GetSqlDbType(fieldInfo) + "),");
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

        #endregion private string getCSharpBulkInsertFunc(string className, TableInfo tableInfo)

        #region private string getCSharpUpdateFunc(string className, TableInfo tableInfo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetCSharpUpdateFunc(string className, TableInfo tableInfo)
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
            sqlParamStr.AppendLine("\t\tSqlParameter[] parms = new SqlParameter[]");
            sqlParamStr.AppendLine("\t\t{");
            //定义参数复制
            StringBuilder sqlParamValueStr = new StringBuilder();

            int i = 0;
            //遍历数据表字段
            foreach (FieldInfo fieldInfo in tableInfo.Fields)
            {
                string fieldPropertyName = BSqlFunction.GetPropertyName(fieldInfo.FieldName);
                sqlParamStr.AppendLine(string.Format("\t\t\tnew SqlParameter({0}, SqlDbType.{1}),", string.Format(ConstVars.PARM_NAME, fieldInfo.FieldName.ToUpper().Replace("_", "")), GetSqlDbType(fieldInfo)));
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

            //添加SqlParamter参数定义
            sb.AppendLine(sqlParamStr.ToString());
            //添加SqlParamter参数赋值
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
        #endregion private string getCSharpUpdateFunc(string className, TableInfo tableInfo)

        #region private string getCSharpDeleteFunc(TableInfo tableInfo)

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
                argCount++;
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
                sqlParamStr.AppendLine(string.Format("\t\tSqlParameter parm = getSqlParameter({0});", argStr));
            }
            else
            {
                sqlParamStr.AppendLine(string.Format("\t\tSqlParameter[] parms = getSqlParameters({0});", argStr));
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

            //添加SqlParamter参数定义
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
        #endregion private string getCSharpDeleteFunc(TableInfo tableInfo)

        #region private string getCSharpSelectFunc(string className, TableInfo tableInfo)
        private string GetCSharpSelectFunc(string className, TableInfo tableInfo)
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
                argCount++;
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
                sqlParamStr.AppendLine(string.Format("\t\tSqlParameter parm = getSqlParameter({0});", argStr));
            }
            else
            {
                sqlParamStr.AppendLine(string.Format("\t\tSqlParameter[] parms = getSqlParameters({0});", argStr));
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

            //添加SqlParamter参数定义
            sb.AppendLine(sqlParamStr.ToString());

            //添加SqlDataReader方式
            if (argCount == 1)
            {
                //添加数据库操作部分
                sb.AppendLine(string.Format("\t\tusing(SqlDataReader rdr = Database.ExecuteReader(connString, CommandType.Text, SQL_SELECT_{0}, parm))", tableInfo.TableName.ToUpper().Replace("_", "")));
            }
            else
            {
                //添加数据库操作部分
                sb.AppendLine(string.Format("\t\tusing(SqlDataReader rdr = Database.ExecuteReader(connString, CommandType.Text, SQL_SELECT_{0}, parms))", tableInfo.TableName.ToUpper().Replace("_", "")));
            }
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif(rdr.Read())");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine(string.Format("\t\t\t\t{0} = get{1}(rdr);", modelObjName, modelName));
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
        #endregion private string getCSharpSelectFunc(string className, TableInfo tableInfo)

        #region private string getCSharpSelectAllFunc(string className, TableInfo tableInfo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetCSharpSelectAllFunc(string className, TableInfo tableInfo)
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
            sb.AppendLine(string.Format("\t\tusing(SqlDataReader rdr = Database.ExecuteReader(connString, CommandType.Text, SQL_SELECT_{0}S))", tableInfo.TableName.ToUpper().Replace("_", "")));


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
        #endregion private string getCSharpSelectAllFunc(string className, TableInfo tableInfo)

        #region private string getCSharpPrivateFuncs(string className, TableInfo tableInfo)
        private string GetCSharpPrivateFuncs(string className, TableInfo tableInfo)
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
                sqlParamStr.AppendLine(string.Format("\t\tSqlParameter parm = new SqlParameter({0}, SqlDbType.{1});", string.Format(ConstVars.PARM_NAME, autoIncr.FieldName.ToUpper().Replace("_", "")), GetSqlDbType(autoIncr)));
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
                            string.Format("\t\tSqlParameter parm = new SqlParameter({0}, SqlDbType.{1});",
                                string.Format(ConstVars.PARM_NAME, priFields[0].FieldName.ToUpper().Replace("_", "")),
                                GetSqlDbType(priFields[0])));
                        sqlParamValueStr.AppendLine(string.Format("\t\tparm.Value = {0};",
                            BSqlFunction.GetPropertyName(priFields[0].FieldName, true)));
                        argCount++;
                    }
                    else
                    {
                        sqlParamStr.AppendLine("\t\tSqlParameter[] parms = new SqlParameter[]");
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

                            sqlParamStr.AppendFormat("\t\t\tnew SqlParameter({0}, SqlDbType.{1})", string.Format(ConstVars.PARM_NAME, priField.FieldName.ToUpper().Replace("_", "")), GetSqlDbType(priField));
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
                getParamFuncName.AppendLine(string.Format("private static SqlParameter getSqlParameter({0})", argListStr));
            }
            else
            {
                getParamFuncName.AppendLine(string.Format("private static SqlParameter[] getSqlParameters({0})", argListStr));
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
            string funcName = string.Format("private static {0} get{0}(SqlDataReader rdr)", modelName);

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
                sb.AppendLine(string.Format("\t\t\t{0}", GetCSharpSqlDataReader(fieldInfo.FieldType, i)));
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
        #endregion private string getCSharpPrivateFuncs(string className, TableInfo tableInfo)

        #region private static string getSqlDbType(FieldInfo fieldInfo)
        private static string GetSqlDbType(FieldInfo fieldInfo)
        {
            switch (fieldInfo.FieldType.ToLower())
            {
                case "varchar":
                    return "VarChar," + fieldInfo.FieldLength;
                case "int":
                    return "Int32";
                case "datetime":
                    return "DateTime";
                case "tinyint":
                    return "Byte";
                case "bigint":
                    return "Int64";
                case "short":
                    return "Int16";
                default:
                    return "VarChar";

            }
        }
        #endregion private static string getSqlDbType(FieldInfo fieldInfo)

        #region private static string getCSharpSqlDataReader(string fieldType, int index)
        private static string GetCSharpSqlDataReader(string fieldType, int index)
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
                    return string.Format("rdr.IsDBNull({0}) ? new DateTime(1900,1,1) : rdr.GetDateTime({0}),", index);
            }
        }
        #endregion private static string getCSharpSqlDataReader(string fieldType, int index)

        #endregion Private Methods
    }
}