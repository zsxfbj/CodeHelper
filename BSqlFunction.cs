using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeHelper
{
    /// <summary>
    ///     数据库的方法类
    /// </summary>
    public class BSqlFunction
    {
        private static BSqlFunction _instance;

        private BSqlFunction()
        {
        }

        #region Public Methods

        #region public static BSqlFunction GetInstance()

        public static BSqlFunction GetInstance()
        {
            if (_instance != null) return _instance;
            _instance = new BSqlFunction();
            return _instance;
        }

        #endregion public static BSqlFunction GetInstance()

        #region public List<TableInfo> GetTables()

        /// <summary>
        ///     获取数据库的数据表
        /// </summary>
        /// <returns></returns>
        public List<TableInfo> GetTables()
        {
            if (Global.GetInstance().DB != null)
            {
                if (Global.GetInstance().DB.DbType == SQLDbTypes.SQLServer)
                    return SqlServerFunction.GetInstance().GetTables();
                if (Global.GetInstance().DB.DbType == SQLDbTypes.MySQL) return MySqlFunction.GetInstance().GetTables();
            }

            return new List<TableInfo>();
        }

        #endregion public List<TableInfo> GetTables()

        #region public string GetModelClass(string className, string tableName)

        /// <summary>
        ///     获取函数的Model类
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetModelClass(string className, string tableName)
        {
            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(tableName))
            {
                var tableInfo = Global.GetInstance().Tables.FirstOrDefault(c =>
                    c.TableName.Equals(tableName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (tableInfo != null)
                    switch (Global.GetInstance().CodeType)
                    {
                        case CodeTypes.CSharp4:
                            return GetCSharp4Model(className.Trim(), tableInfo);
                        case CodeTypes.CSharp:
                            return GetCSharpModel(className.Trim(), tableInfo);                        
                        case CodeTypes.Java:
                            return GetJavaModel(className.Trim(), tableInfo);
                    }
            }

            return string.Empty;
        }

        #endregion public string GetModelClass(string className, string tableName)

        #region public string GetDALClass(string className, string tableName)

        public string GetDalClass(string className, string tableName)
        {
            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(tableName))
            {
                var tableInfo =
                    Global.GetInstance()
                        .Tables.FirstOrDefault(
                            c => c.TableName.Equals(tableName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (tableInfo != null)
                    switch (Global.GetInstance().CodeType)
                    {
                        case CodeTypes.CSharp:
                        case CodeTypes.CSharp4:
                            return GetCharpDalClass(className.Trim(), tableInfo);
                        case CodeTypes.Java:
                            return GetJavaDalClass(className.Trim(), tableInfo);
                    }
            }

            return string.Empty;
        }

        #endregion public string GetDALClass(string className, string tableName)

        #endregion Public Methods

        #region Internal Methods

        #region internal static string GetPropertyName(string name, bool isFirstLower = false)

        /// <summary>
        ///     将属性名字转换为指定的标准格式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isFirstLower">首字母是否小写，默认false为小写</param>
        /// <returns></returns>
        internal static string GetPropertyName(string name, bool isFirstLower = false)
        {
            var sb = new StringBuilder();
            if (name.IndexOf('_', 0) > 0)
            {
                var strs = name.Split('_');
                var i = 0;
                foreach (var str in strs)
                {
                    if (i == 0 && isFirstLower)
                        sb.Append(str.Substring(0, 1).ToLower() + str.Substring(1));
                    else
                        sb.Append(str.Substring(0, 1).ToUpper() + str.Substring(1));
                    i++;
                }
            }
            else
            {
                if (isFirstLower)
                    sb.Append(name.Substring(0, 1).ToLower() + name.Substring(1));
                else
                    sb.Append(name.Substring(0, 1).ToUpper() + name.Substring(1));
            }

            return sb.ToString();
        }

        #endregion internal static string GetPropertyName(string name, bool isFirstLower = false)

        #region internal static string GetCSharpTypeString(string fieldType)

        /// <summary>
        ///     获取CSharp的数据类型
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        internal static string GetCSharpTypeString(string fieldType)
        {
            if (fieldType.Equals("bigint", StringComparison.OrdinalIgnoreCase)) return "long";
            if (fieldType.Equals("int", StringComparison.OrdinalIgnoreCase)) return "int";
            if (fieldType.Equals("tinyint", StringComparison.OrdinalIgnoreCase)) return "byte";
            if (fieldType.Equals("smallint", StringComparison.OrdinalIgnoreCase)) return "short";
            if (fieldType.Equals("bit", StringComparison.OrdinalIgnoreCase)) return "bool";
            if (fieldType.Equals("float", StringComparison.OrdinalIgnoreCase)) return "double";
            if (fieldType.Equals("money", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("decimal", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("numeric", StringComparison.OrdinalIgnoreCase))
                return "decimal";
            if (fieldType.Equals("varchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("nvarchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("char", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("longtext", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("text", StringComparison.OrdinalIgnoreCase))
                return "string";
            if (fieldType.Equals("datetime", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("date", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("datetime2", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("time", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("timestamp", StringComparison.OrdinalIgnoreCase))
                return "DateTime";
            return "unknown";
        }

        #endregion internal static string GetCSharpTypeString(string fieldType)

        #region internal static string GetCSharpMySqlReaderType(string fieldType, int index)

        internal static string GetCSharpMySqlReaderType(string fieldType, int index)
        {
            if (fieldType.Equals("bigint", StringComparison.OrdinalIgnoreCase))
                return string.Format("getInt64({0})", index);
            if (fieldType.Equals("int", StringComparison.OrdinalIgnoreCase))
                return string.Format("getInt32({0})", index);
            if (fieldType.Equals("tinyint", StringComparison.OrdinalIgnoreCase)) return "byte";
            if (fieldType.Equals("smallint", StringComparison.OrdinalIgnoreCase)) return "short";
            if (fieldType.Equals("bit", StringComparison.OrdinalIgnoreCase)) return "bool";
            if (fieldType.Equals("float", StringComparison.OrdinalIgnoreCase)) return "double";
            if (fieldType.Equals("money", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("numeric", StringComparison.OrdinalIgnoreCase))
                return "decimal";
            if (fieldType.Equals("varchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("nvarchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("char", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("text", StringComparison.OrdinalIgnoreCase))
                return "string";
            if (fieldType.Equals("datetime", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("date", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("datetime2", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("time", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("timestamp", StringComparison.OrdinalIgnoreCase))
                return "DateTime";
            return string.Format("getString({0})", index);
        }

        #endregion internal static string GetCSharpMySqlReaderType(string fieldType, int index)

        #region internal static string GetJavaResultSet(FieldInfo fieldInfo)

        internal static string GetJavaResultSet(FieldInfo fieldInfo)
        {
            switch (fieldInfo.FieldType.ToLower())
            {
                default:
                    return $"rs.getString(\"{fieldInfo.FieldName}\")";
                case "int":
                    return $"rs.getInt(\"{fieldInfo.FieldName}\")";
                case "short":
                case "smallint":
                    return $"rs.getShort(\"{fieldInfo.FieldName}\")";
                case "tinyint":
                    return $"rs.getByte(\"{fieldInfo.FieldName}\")";
                case "bigint":
                    return $"rs.getLong(\"{fieldInfo.FieldName}\")";
                case "money":
                case "numeric":
                case "decimal":
                    return $"rs.getBigDecimal(\"{fieldInfo.FieldName}\")";
                case "float":
                    return $"rs.getDouble(\"{fieldInfo.FieldName}\")";
                case "timestamp":
                case "datetime":
                case "date":
                case "time":
                    return $"rs.getTimestamp(\"{fieldInfo.FieldName}\")";
            }
        }

        #endregion internal string GetJavaResultSet(FieldInfo fieldInfo)

        #region internal string GetJavaPreparedStatement(FieldInfo fieldInfo)

        internal static string GetJavaPreparedStatement(FieldInfo fieldInfo, int i, string modelObjName)
        {
            var fieldPropertyName = GetPropertyName(fieldInfo.FieldName);
            switch (fieldInfo.FieldType.ToLower())
            {
                default:
                    return $"ps.setString({i},{modelObjName}.get{fieldPropertyName}());";
                case "int":
                    return $"ps.setInt({i},{modelObjName}.get{fieldPropertyName}());";
                case "short":
                case "smallint":
                    return $"ps.setShort({i},{modelObjName}.get{fieldPropertyName}());";
                case "tinyint":
                    return $"ps.setByte({i},{modelObjName}.get{fieldPropertyName}());";
                case "bigint":
                    return $"ps.setLong({i},{modelObjName}.get{fieldPropertyName}());";
                case "money":
                case "numeric":
                case "decimal":
                    return $"ps.setBigDecimal({i},{modelObjName}.get{fieldPropertyName}());";
                case "float":
                    return $"ps.setDouble({i},{modelObjName}.get{fieldPropertyName}());";
                case "timestamp":
                case "datetime":
                case "date":
                case "time":
                    return string.Format("ps.setTimestamp({0},new Timestamp({1}.get{2}().getTime()));", i, modelObjName,
                        fieldPropertyName);
            }
        }

        #endregion internal string GetJavaPreparedStatement(FieldInfo fieldInfo)

        #region internal string GetJavaTypeString(string fieldType)

        /// <summary>
        ///     获取CSharp的数据类型
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        internal static string GetJavaTypeString(string fieldType)
        {
            if (fieldType.StartsWith("bigint", StringComparison.OrdinalIgnoreCase)) return "long";
            if (fieldType.StartsWith("int", StringComparison.OrdinalIgnoreCase)) return "int";
            if (fieldType.StartsWith("tinyint", StringComparison.OrdinalIgnoreCase)) return "byte";
            if (fieldType.StartsWith("smallint", StringComparison.OrdinalIgnoreCase)) return "short";
            if (fieldType.StartsWith("bit", StringComparison.OrdinalIgnoreCase)) return "boolean";
            if (fieldType.StartsWith("float", StringComparison.OrdinalIgnoreCase)) return "double";
            if (fieldType.StartsWith("money", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("numeric", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("decimal", StringComparison.OrdinalIgnoreCase))
                return "BigDecimal";
            if (fieldType.StartsWith("varchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("nvarchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("char", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("text", StringComparison.OrdinalIgnoreCase))
                return "String";
            if (fieldType.StartsWith("datetime", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("date", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("datetime2", StringComparison.OrdinalIgnoreCase) ||
                fieldType.StartsWith("timestamp", StringComparison.OrdinalIgnoreCase))
                return "Date";

            //return "unknown";
            return fieldType;
        }

        #endregion internal string GetJavaTypeString(string fieldType)

        #endregion Internal Methods

        #region Private Methods

        #region  private string getCSharpModel(string className, TableInfo tableInfo)

        /// <summary>
        ///     生成CSharp2.0的Model类
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetCSharpModel(string className, TableInfo tableInfo)
        {
            //构造总输出
            var stringBuilder = new StringBuilder();

            //写类名
            stringBuilder.AppendLine("/// <summary>");
            stringBuilder.AppendLine("///");
            stringBuilder.AppendLine("/// </summary>");
            stringBuilder.AppendLine("[Serializable]");
            stringBuilder.AppendLine("public class " + className);
            stringBuilder.AppendLine("{");

            //构造函数输出
            var constructorStr = new StringBuilder();

            constructorStr.AppendLine("\t#region Constructors");
            constructorStr.AppendLine("");
            constructorStr.AppendLine("\tpublic " + className + "(){}");
            constructorStr.AppendLine();

            //私有变量输出
            var fieldsStr = new StringBuilder();
            fieldsStr.AppendLine("\t#region Fields");
            fieldsStr.AppendLine("");

            //属性输出
            var propertiesStr = new StringBuilder();
            propertiesStr.AppendLine("\t#region Properties");
            propertiesStr.AppendLine("");

            //构造函数参数
            var constructorArgsStr = new StringBuilder();
            //构造函数体
            var constructorBodyStr = new StringBuilder();

            //遍历表字段进行输出
            foreach (var field in tableInfo.Fields)
            {
                //处理字段
                fieldsStr.AppendLine("\t/// <summary>");
                fieldsStr.AppendLine("\t/// " + field.Description);
                fieldsStr.AppendLine("\t/// </summary>");
                fieldsStr.Append("\tprivate ");
                fieldsStr.Append(GetCSharpTypeString(field.FieldType));
                var fieldName = "_" + GetPropertyName(field.FieldName, true);
                fieldsStr.AppendLine(" " + fieldName + ";");
                fieldsStr.AppendLine("");

                //处理属性
                propertiesStr.AppendLine("");
                propertiesStr.AppendLine("\t/// <summary>");
                propertiesStr.AppendLine("\t/// " + field.Description);
                propertiesStr.AppendLine("\t/// </summary>");
                propertiesStr.Append("\tpublic ");
                propertiesStr.Append(GetCSharpTypeString(field.FieldType));
                propertiesStr.AppendLine(" " + GetPropertyName(field.FieldName));
                propertiesStr.AppendLine("\t{");
                propertiesStr.AppendLine("\t\tget { return " + fieldName + "; }");
                propertiesStr.AppendLine("\t\tset { " + fieldName + " = value; }");
                propertiesStr.AppendLine("\t}");
                propertiesStr.AppendLine("");

                //处理构造函数
                var argName = GetPropertyName(field.FieldName, true);
                constructorArgsStr.Append(GetCSharpTypeString(field.FieldType));
                constructorArgsStr.Append(" ");
                constructorArgsStr.Append(argName);
                constructorArgsStr.Append(", ");
                constructorBodyStr.Append("\t\t");
                constructorBodyStr.Append(fieldName);
                constructorBodyStr.Append(" = ");
                constructorBodyStr.AppendLine(argName + ";");
            }

            //字段结尾
            fieldsStr.AppendLine("\t#endregion Fields");
            //属性结尾
            propertiesStr.AppendLine("\t#endregion Properties");

            //添加字段输出
            stringBuilder.Append(fieldsStr);
            stringBuilder.AppendLine("");
            //添加属性输出
            stringBuilder.Append(propertiesStr);
            stringBuilder.AppendLine("");

            //添加构造函数输出
            if (constructorArgsStr.Length > 0)
            {
                var bodyStr = constructorArgsStr.Remove(constructorArgsStr.Length - 2, 2);
                constructorStr.AppendLine("\tpublic " + className + "(" + bodyStr + ")");
                constructorStr.AppendLine("\t{");
                constructorStr.Append(constructorBodyStr);
                constructorStr.AppendLine("\t}");
            }

            //构造函数结束
            constructorStr.AppendLine("\t#endregion Constructors");

            //添加构造函数
            stringBuilder.Append(constructorStr);
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("}");

            //输出结果
            return stringBuilder.ToString();
        }

        #endregion  private string getCSharpModel(string className, string tableName, List<TableInfo> tables)

        #region  private static string GetCSharp4Model(string className, TableInfo tableInfo)

        /// <summary>
        ///     生成CSharp4.0的Model类
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private static string GetCSharp4Model(string className, TableInfo tableInfo)
        {
            //构造总输出
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Data.Linq;");
            stringBuilder.AppendLine("using System.Data.Linq.Mapping;");
            stringBuilder.AppendLine("");
            //写类名
            stringBuilder.AppendLine("[Serializable]");
            stringBuilder.AppendLine(string.Format("[Table(\"{0}\")]", tableInfo.TableName));
            stringBuilder.AppendLine("public class " + className);
            stringBuilder.AppendLine("{");

            //构造函数输出
            var constructorStr = new StringBuilder();

            constructorStr.AppendLine("\t#region Constructors");
            constructorStr.AppendLine("");
            constructorStr.AppendLine("\tpublic " + className + "(){}");
            constructorStr.AppendLine();

            //属性输出
            var propertiesStr = new StringBuilder();
            propertiesStr.AppendLine("\t#region Properties");
            propertiesStr.AppendLine("");

            //构造函数参数
            var constructorArgsStr = new StringBuilder();
            //构造函数体
            var constructorBodyStr = new StringBuilder();

            //遍历表字段进行输出
            foreach (var field in tableInfo.Fields)
            {
                var argName = GetPropertyName(field.FieldName, true);
                var propertyName = GetPropertyName(field.FieldName);
                //处理属性
                propertiesStr.AppendLine("");
                propertiesStr.AppendLine("\t/// <summary>");
                propertiesStr.AppendLine("\t/// " + field.Description);
                propertiesStr.AppendLine("\t/// </summary>");

                if (field.IsPrimaryKey) propertiesStr.AppendLine("\t[key]");
                
                propertiesStr.AppendFormat("\t[Column(\"{0}\")]", field.FieldName);
                propertiesStr.AppendLine();
                if (field.IsIdentity) propertiesStr.AppendLine("\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");     

                propertiesStr.Append("\tpublic ");

                propertiesStr.Append(GetCSharpTypeString(field.FieldType));
                propertiesStr.AppendLine(" " + propertyName + " { get; set; }");
                propertiesStr.AppendLine("");

                //处理构造函数

                constructorArgsStr.Append(GetCSharpTypeString(field.FieldType));
                constructorArgsStr.Append(" ");
                constructorArgsStr.Append(argName);
                constructorArgsStr.Append(", ");
                constructorBodyStr.Append("\t\t");
                constructorBodyStr.Append(propertyName);
                constructorBodyStr.Append(" = ");
                constructorBodyStr.AppendLine(argName + ";");
            }

            //属性结尾
            propertiesStr.AppendLine("\t#endregion Properties");

            //添加属性输出
            stringBuilder.Append(propertiesStr);
            stringBuilder.AppendLine("");

            //添加构造函数输出
            if (constructorArgsStr.Length > 0)
            {
                var bodyStr = constructorArgsStr.Remove(constructorArgsStr.Length - 2, 2);
                constructorStr.AppendLine("\tpublic " + className + "(" + bodyStr + ")");
                constructorStr.AppendLine("\t{");
                constructorStr.Append(constructorBodyStr);
                constructorStr.AppendLine("\t}");
            }

            //构造函数结束
            constructorStr.AppendLine("\t#endregion Constructors");

            //添加构造函数
            stringBuilder.Append(constructorStr);

            //构造结束
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("}");

            //输出结果
            return stringBuilder.ToString();
        }

        #endregion private static string getCSharp4Model(string className, string tableName, List<TableInfo> tables)        

        #region private string getJavaModel(string className, TableInfo tableInfo)

        /// <summary>
        ///     生成Java的Model类
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetJavaModel(string className, TableInfo tableInfo)
        {
            //构造总输出
            var stringBuilder = new StringBuilder();

            //写类注释
            stringBuilder.AppendLine("/**");
            stringBuilder.AppendLine(" *  ");
            stringBuilder.AppendLine(" *  @since " + DateTime.Now.ToString("yyyy-MM-dd"));
            stringBuilder.AppendLine(" *  @author ");
            stringBuilder.AppendLine(" */");

            //写类名
            stringBuilder.Append("public class " + className);
            stringBuilder.AppendLine(" implements java.io.Serializable {");

            //构造函数输出
            var constructorStr = new StringBuilder();

            constructorStr.AppendLine("\t/**");
            constructorStr.AppendLine("\t *");
            constructorStr.AppendLine("\t */");
            constructorStr.AppendLine("\tpublic " + className + "(){}");
            constructorStr.AppendLine();

            //私有变量输出
            var fieldsStr = new StringBuilder();
            fieldsStr.AppendLine("");

            //属性输出
            var propertiesStr = new StringBuilder();
            propertiesStr.AppendLine("");

            //构造函数参数
            var constructorArgsStr = new StringBuilder();
            //构造函数体
            var constructorBodyStr = new StringBuilder();

            //遍历表字段进行输出
            foreach (var field in tableInfo.Fields)
            {
                var fieldName = GetPropertyName(field.FieldName, true);
                //处理字段
                fieldsStr.AppendLine("\t/**");
                fieldsStr.AppendLine("\t * " + field.Description);
                fieldsStr.AppendLine("\t */");
                fieldsStr.Append("\tprivate ");
                fieldsStr.Append(GetJavaTypeString(field.FieldType));
                fieldsStr.AppendLine(" " + fieldName + ";");
                fieldsStr.AppendLine("");

                //处理属性
                propertiesStr.AppendLine("");
                propertiesStr.AppendLine("\t/**");
                propertiesStr.AppendLine("\t * 获取" + field.Description);
                propertiesStr.AppendLine("\t */");
                propertiesStr.Append("\tpublic ");
                propertiesStr.Append(GetJavaTypeString(field.FieldType));
                propertiesStr.Append(" get" + GetPropertyName(field.FieldName));
                propertiesStr.AppendLine("(){");
                propertiesStr.AppendLine("\t\treturn " + fieldName + "; ");
                propertiesStr.AppendLine("\t}");
                propertiesStr.AppendLine("");

                propertiesStr.AppendLine("\t/**");
                propertiesStr.AppendLine("\t * 设置" + field.Description);
                propertiesStr.AppendLine("\t */");
                propertiesStr.Append("\tpublic void");
                propertiesStr.Append(" set" + GetPropertyName(field.FieldName));
                propertiesStr.Append("(" + GetJavaTypeString(field.FieldType) + " " + fieldName);
                propertiesStr.AppendLine("){");
                propertiesStr.AppendLine("\t\tthis." + fieldName + " = " + fieldName + "; ");
                propertiesStr.AppendLine("\t}");
                propertiesStr.AppendLine("");


                //处理构造函数
                constructorArgsStr.Append(GetJavaTypeString(field.FieldType));
                constructorArgsStr.Append(" ");
                constructorArgsStr.Append(fieldName);
                constructorArgsStr.Append(", ");
                constructorBodyStr.Append("\t\tthis.");
                constructorBodyStr.Append(fieldName);
                constructorBodyStr.Append(" = ");
                constructorBodyStr.AppendLine(fieldName + ";");
            }


            //添加字段输出
            stringBuilder.Append(fieldsStr);
            stringBuilder.AppendLine("");
            //添加属性输出
            stringBuilder.Append(propertiesStr);
            stringBuilder.AppendLine("");

            //添加构造函数输出
            if (constructorArgsStr.Length > 0)
            {
                var bodyStr = constructorArgsStr.Remove(constructorArgsStr.Length - 2, 2);
                constructorStr.AppendLine("\tpublic " + className + "(" + bodyStr + "){");
                constructorStr.Append(constructorBodyStr);
                constructorStr.AppendLine("\t}");
            }
            //构造函数结束


            //添加构造函数
            stringBuilder.Append(constructorStr);
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("}");

            //输出结果
            return stringBuilder.ToString();
        }

        #endregion private string getJavaModel(string className, TableInfo tableInfo)

        #region private string getCharpDalClass(string className, TableInfo tableInfo)

        private string GetCharpDalClass(string className, TableInfo tableInfo)
        {
            //构造总输出
            var stringBuilder = new StringBuilder();
            //写类名
            stringBuilder.AppendLine("public class " + className);
            stringBuilder.AppendLine("{");

            //写SQL语句部分
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("\t#region Fields");
            stringBuilder.AppendLine("");
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetDalFields(tableInfo));
                    break;
            }

            //添加字段块标识结束
            stringBuilder.AppendLine("\t#endregion Fields");
            stringBuilder.AppendLine("");

            //添加公共方法块开始
            stringBuilder.AppendLine("\t#region Public Methods");
            stringBuilder.AppendLine("");
            //添加方法
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                {
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetInsertFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetBulkInsertFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetUpdateFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetDeleteFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetSelectFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetAllSelectFunc(className, tableInfo));
                }
                    break;
            }

            //添加公开方法块结束
            stringBuilder.AppendLine("\t#endregion Public Methods");

            //添加公共方法块开始
            stringBuilder.AppendLine("\t#region Private Methods");
            stringBuilder.AppendLine("");
            //添加方法
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                {
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetPrviateFuncs(className, tableInfo));
                }
                    break;
            }

            //添加公开方法块结束
            stringBuilder.AppendLine("\t#endregion Private Methods");

            //构造结束
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }

        #endregion private string getCharpDalClass(string className, TableInfo tableInfo)

        #region private string getJavaDalClass(string className, TableInfo tableInfo)

        /// <summary>
        ///     构造dao层框架
        /// </summary>
        /// <param name="className"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private string GetJavaDalClass(string className, TableInfo tableInfo)
        {
            //构造总输出
            var stringBuilder = new StringBuilder();

            //引入常用包
            stringBuilder.AppendLine("import java.util.*;");
            stringBuilder.AppendLine("import java.sql.*;");
            stringBuilder.AppendLine("");

            //写类名
            stringBuilder.AppendLine("/**");
            stringBuilder.AppendLine(" * " + className + "数据访问层");
            stringBuilder.AppendLine(" * @since " + DateTime.Now.ToString("yyyy-MM-dd"));
            stringBuilder.AppendLine(" * @author ");
            stringBuilder.AppendLine(" */");
            stringBuilder.AppendLine("public class " + GetPropertyName(className) + "DAO {");
            stringBuilder.AppendLine("");

            //SQL
            stringBuilder.AppendLine("");

            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetDalFields(tableInfo));
                    break;
            }

            //公开方法
            stringBuilder.AppendLine("");
            //添加方法
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                {
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetInsertFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetBulkInsertFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetUpdateFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetDeleteFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetSelectFunc(className, tableInfo));
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetAllSelectFunc(className, tableInfo));
                }
                    break;
            }

            //私有方法
            stringBuilder.AppendLine("");
            //添加方法
            switch (Global.GetInstance().DB.DbType)
            {
                case SQLDbTypes.MySQL:
                {
                    stringBuilder.AppendLine(MySqlFunction.GetInstance().GetPrviateFuncs(className, tableInfo));
                }
                    break;
            }


            //构造结束
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("}");
            return stringBuilder.ToString();
        }

        #endregion private string getJavaDalClass(string className, TableInfo tableInfo)

        #endregion Private Methods
    }
}