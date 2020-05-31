using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using MySql.Data.MySqlClient;

namespace CodeHelper
{
    /// <summary>
    /// 全局变量
    /// 
    /// </summary>
    public class Global
    {
        #region Fields

        private static Global _instance;

        #endregion Fields

        private Global()
        {
            CodeType = CodeTypes.Java;
        }

        #region Properties 
        /// <summary>
        /// 数据库配置信息
        /// </summary>
        public DbConfigInfo DB { get; set; }

        /// <summary>
        /// 生成的代码类型
        /// </summary>
        public CodeTypes CodeType { get; set; }

        /// <summary>
        /// 数据库数据表
        /// </summary>
        public List<TableInfo> Tables { get; set; } 

        #endregion Properties

        #region Internal Methods
        
        internal string GetCharpSQLDAL(string className, string tableName, List<TableInfo> tables)
        {
        
            string objName = className.Substring(0, 1).ToLower() + className.Substring(1);
            string getPriParamFuncName = "getSqlParameter";
            string getObjFuncName = "get" + className;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine("using System.Data;");
            stringBuilder.AppendLine("using System.Data.SqlClient;");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("public class " + className);
            stringBuilder.AppendLine("{");

            //先写私有变量
            TableInfo tableInfo = tables.Find(c => c.TableName == tableName);
            if (tableInfo != null)
            {
                string insertSQLName = "SQL_INSERT_" + tableName.ToUpper();
                string updateSQLName = "SQL_UPDATE_" + tableName.ToUpper();
                string deleteSQLName = "SQL_DELETE_" + tableName.ToUpper();
                string selectSQLName = "SQL_SELECT_" + tableName.ToUpper();
            
                StringBuilder fieldsStr = new StringBuilder();
                fieldsStr.AppendLine("\t#region Fields");
                fieldsStr.AppendLine("");
                StringBuilder fields = new StringBuilder();

                //增加INSERT
                //添加字段
                StringBuilder insert1 = new StringBuilder();
                StringBuilder insert2 = new StringBuilder();

                StringBuilder update1 = new StringBuilder();
                StringBuilder funcArgs = new StringBuilder();
                StringBuilder funcValues = new StringBuilder();
                StringBuilder keyParms = new StringBuilder();

                StringBuilder insertValueIf = new StringBuilder();
                StringBuilder insertParmElse = new StringBuilder();
                StringBuilder insertValueElse = new StringBuilder();

                StringBuilder updateValueIf = new StringBuilder();
                StringBuilder updateParmElse = new StringBuilder();
                StringBuilder updateValueElse = new StringBuilder();

                StringBuilder pkValueIf = new StringBuilder();
                StringBuilder pkParmElse = new StringBuilder();
                StringBuilder pkValueElse = new StringBuilder();

                StringBuilder rdrStr = new StringBuilder();

                int i = 0;
                int j = 0;
                int k = 0;
                foreach (FieldInfo field in tableInfo.Fields)
                {
                    fieldsStr.AppendLine("\t" + string.Format(ConstVars.CSHARP_SQLSERVER_PARMSTRING, field.FieldName.ToUpper(),
                                                      field.FieldName));
                    fields.Append("[" + field.FieldName + "],");

                    rdrStr.AppendLine("\t\t\t" + Global.GetInstance().GetSqlDataReaderString(field.FieldType, k) + ",");
                    updateParmElse.Append("\t\t\t\tnew SqlParameter(PARM_" + field.FieldName.ToUpper() + "," + Global.GetInstance().GetSQLServerDBTypeString(field.FieldType));

                    if (BSqlFunction.GetCSharpTypeString(field.FieldType).Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        updateParmElse.Append("," + field.FieldLength);
                    }

                    updateParmElse.AppendLine("),");
                    if (field.IsPrimaryKey)
                    {
                        keyParms.Append(string.Format("[{0}]=@{0} AND ", field.FieldName));
                        string argName = field.FieldName.Substring(0, 1).ToLower() + field.FieldName.Substring(1);
                        funcArgs.Append(BSqlFunction.GetCSharpTypeString(field.FieldType) + " " + argName + ",");
                        funcValues.Append(argName + ",");

                        updateValueIf.AppendLine("\t\t\t\t" + argName + ",");
                        pkValueIf.AppendLine("\t\t\t\t" + argName + ",");

                        pkParmElse.Append("\t\t\t\tnew SqlParameter(PARM_" + field.FieldName.ToUpper() + "," + Global.GetInstance().GetSQLServerDBTypeString(field.FieldType));

                        if (BSqlFunction.GetCSharpTypeString(field.FieldType)
                                  .Equals("string", StringComparison.OrdinalIgnoreCase))
                        {

                            updateParmElse.Append("," + field.FieldLength);
                            pkParmElse.Append("," + field.FieldLength);
                        }

                        updateParmElse.AppendLine("),");
                        pkParmElse.AppendLine("),");

                        updateValueElse.AppendLine("\t\t\tparms[" + j + "].Value = " + argName + ";");
                        pkValueElse.AppendLine("\t\t\tparms[" + j + "].Value = " + argName + ";");

                        j++;
                    }
                    else if (!field.IsIdentity)
                    {
                        insert1.Append("[" + field.FieldName + "],");
                        insert2.Append("@" + field.FieldName + ",");

                        insertValueIf.AppendLine("\t\t\t\t" + objName + "Info." + field.FieldName + ",");
                        // updateValueIf.AppendLine("\t\t\t\t" + objName + "Info." + field.FieldName + ",");
                        insertParmElse.Append("\t\t\t\tnew SqlParameter(PARM_" + field.FieldName.ToUpper() + "," + Global.GetInstance().GetSQLServerDBTypeString(field.FieldType));
                        // updateParmElse.Append("\t\t\t\tnew SqlParameter(PARM_" + field.FieldName.ToUpper() + "," + Global.GetInstance().GetSQLServerDBTypeString(field.FieldType));


                        if (BSqlFunction.GetCSharpTypeString(field.FieldType).Equals("string", StringComparison.OrdinalIgnoreCase))
                        {
                            insertParmElse.Append("," + field.FieldLength);
                            //updateParmElse.Append("," + field.FieldLength);
                        }
                        insertParmElse.AppendLine("),");
                        //updateParmElse.AppendLine("),");

                        insertValueElse.AppendLine("\t\t\tparms[" + i + "].Value = " + objName + "Info." + field.FieldName + ";");
                        //updateValueElse.AppendLine("\t\t\tparms[" + j + "].Value = " + objName + "Info." + field.FieldName + ";");

                        update1.Append(string.Format("[{0}] = @{0},", field.FieldName));
                        i++;
                        j++;
                    }
                    k++;

                }

                insert1.Remove(insert1.Length - 1, 1);
                insert2.Remove(insert2.Length - 1, 1);
                update1.Remove(insert1.Length - 1, 1);
                fields.Remove(fields.Length - 1, 1);
                keyParms.Remove(keyParms.Length - 5, 5);
                funcArgs.Remove(funcArgs.Length - 1, 1);
                funcValues.Remove(funcValues.Length - 1, 1);
                insertValueIf.Remove(insertValueIf.Length - 3, 3);
                insertParmElse.Remove(insertParmElse.Length - 3, 3);
                insertValueElse.Remove(insertValueElse.Length - 2, 2);
                updateValueIf.Remove(updateValueIf.Length - 3, 3);
                updateParmElse.Remove(updateParmElse.Length - 3, 3);
                updateValueElse.Remove(updateValueElse.Length - 2, 2);
                pkValueIf.Remove(pkValueIf.Length - 3, 3);
                pkParmElse.Remove(pkParmElse.Length - 3, 3);
                pkValueElse.Remove(pkValueElse.Length - 2, 2);
                rdrStr.Remove(rdrStr.Length - 3, 3);

                fieldsStr.AppendLine("\tprivate const string PARM_STARTINDEX=\"@StartIndex\";");
                fieldsStr.AppendLine("\tprivate const string PARM_ENDINDEX=\"@EndIndex\";");

                #region SQL语句部分

                fieldsStr.AppendLine("\tprivate const string SQL_INSERT_" + tableName.ToUpper() + " = \"" + string.Format(ConstVars.CSHARP_SQLSERVER_INSERT_SQL, tableName, insert1, insert2) + "\";");
                fieldsStr.AppendLine("\tprivate const string SQL_UPDATE_" + tableName.ToUpper() + " = \"" + string.Format(ConstVars.CSHARP_SQLSERVER_UPDATE_SQL, tableName, update1, keyParms) + "\";");
                fieldsStr.AppendLine("\tprivate const string SQL_DELETE_" + tableName.ToUpper() + " = \"" + string.Format(ConstVars.CSHARP_SQLSERVER_DELETE_SQL, tableName, keyParms) + "\";");
                fieldsStr.AppendLine("\tprivate const string SQL_SELECT_" + tableName.ToUpper() + " = \"" + string.Format(ConstVars.CSHARP_SQLSERVER_SELECT_SQL, tableName, fields, keyParms) + "\";");
                fieldsStr.AppendLine("\tprivate const string SQL_SELECT_" + tableName.ToUpper() + "S = \"" + string.Format(ConstVars.CSHARP_SQLSERVER_SELECT_ALL_SQL, tableName, fields) + "\";");
                fieldsStr.AppendLine("\tprivate const string SQL_SELECT_PAGED_" + tableName.ToUpper() + "S = \"" + string.Format(ConstVars.CSHARP_SQLSERVER_SELECT_PAGED_SQL, tableName, fields, funcValues) + "\";");
                fieldsStr.AppendLine("\tprivate const string SQL_COUNT_" + tableName.ToUpper() + "S = \"" + string.Format(ConstVars.CSHARP_SQLSERVER_SELECT_COUNT_ALL, tableName) + "\";");

                #endregion SQL语句部分

                fieldsStr.AppendLine("");
                fieldsStr.AppendLine("\t#endregion Fields");
                stringBuilder.Append(fieldsStr);
                stringBuilder.AppendLine("");

                #region 公共方法

                //开始拼接函数
                stringBuilder.AppendLine("\t#region Public Methods");
                stringBuilder.AppendLine("");

                #region Insert Function

                stringBuilder.AppendLine("\t#region Insert");
                stringBuilder.AppendLine("\t/// <summary>");
                stringBuilder.AppendLine("\t/// 新增记录");
                stringBuilder.AppendLine("\t/// </summary>");
                //函数声明
                stringBuilder.Append("\tpublic bool Insert(");
                stringBuilder.Append(className + "Info ");
                stringBuilder.Append(objName);
                stringBuilder.AppendLine("Info)");
                //函数体
                stringBuilder.AppendLine("\t{");
                stringBuilder.AppendLine("\t\tbool result = false;");
                stringBuilder.AppendLine("\t\tSqlParameter[] parms;");
                stringBuilder.AppendLine("\t\tif (ParamsCache.GetCachedParameterSet(Database.CONN_STRING_NON_DTC, \"" + insertSQLName + "\", out parms))");
                stringBuilder.AppendLine("\t\t{");
                stringBuilder.AppendLine("\t\t\tDatabase.AssignParameterValues(parms,");
                stringBuilder.Append(insertValueIf);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\t\t\t);");
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine("\t\telse");
                stringBuilder.AppendLine("\t\t{");
                stringBuilder.AppendLine("\t\t\tparms = new SqlParameter[]");
                stringBuilder.AppendLine("\t\t\t{");
                stringBuilder.Append(insertParmElse);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\t\t};");
                stringBuilder.Append(insertValueElse);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\t\tParamsCache.CacheParameterSet(Database.CONN_STRING_NON_DTC, \"" + insertSQLName + "\", parms);");
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\tusing (SqlConnection conn = new SqlConnection(Database.CONN_STRING_NON_DTC))");
                stringBuilder.AppendLine("\t\t{");
                stringBuilder.AppendLine("\t\t\tconn.Open();");
                stringBuilder.AppendLine("\t\t\tusing (SqlTransaction tran = conn.BeginTransaction())");
                stringBuilder.AppendLine("\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\ttry");
                stringBuilder.AppendLine("\t\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\t\tint ret = Database.ExecuteNonQuery(tran, CommandType.Text, " + insertSQLName + ", parms);");
                stringBuilder.AppendLine("\t\t\t\t\tif(ret > 0)");
                stringBuilder.AppendLine("\t\t\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\t\t\tresult = true;");
                stringBuilder.AppendLine("\t\t\t\t\t}");
                stringBuilder.AppendLine("\t\t\t\t\ttran.Commit();");
                stringBuilder.AppendLine("\t\t\t\t}");
                stringBuilder.AppendLine("\t\t\t\tcatch");
                stringBuilder.AppendLine("\t\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\t\ttran.Rollback();");
                stringBuilder.AppendLine("\t\t\t\t\tthrow;");
                stringBuilder.AppendLine("\t\t\t\t}");
                stringBuilder.AppendLine("\t\t\t}");
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine("\t\treturn result;");
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine("\t#endregion Insert");

                stringBuilder.AppendLine();

                #endregion Insert Function

                #region Update Function

                stringBuilder.AppendLine("\t#region Update");
                stringBuilder.AppendLine("\t/// <summary>");
                stringBuilder.AppendLine("\t/// 更新记录");
                stringBuilder.AppendLine("\t/// </summary>");
                stringBuilder.Append("\tpublic bool Update(");
                stringBuilder.Append(className + "Info ");
                stringBuilder.Append(objName + "Info, ");
                stringBuilder.Append(funcArgs);
                stringBuilder.AppendLine(")");
                //函数体
                stringBuilder.AppendLine("\t{");
                stringBuilder.AppendLine("\t\tbool result = false;");
                stringBuilder.AppendLine("\t\tSqlParameter[] parms;");
                stringBuilder.AppendLine("\t\tif (ParamsCache.GetCachedParameterSet(Database.CONN_STRING_NON_DTC, \"" + updateSQLName + "\", out parms))");
                stringBuilder.AppendLine("\t\t{");
                stringBuilder.AppendLine("\t\t\tDatabase.AssignParameterValues(parms,");
                stringBuilder.Append(updateValueIf);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\t\t\t);");
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine("\t\telse");
                stringBuilder.AppendLine("\t\t{");
                stringBuilder.AppendLine("\t\t\tparms = new SqlParameter[]");
                stringBuilder.AppendLine("\t\t\t{");
                stringBuilder.Append(updateParmElse);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\t\t};");
                stringBuilder.Append(updateValueElse);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\t\tParamsCache.CacheParameterSet(Database.CONN_STRING_NON_DTC, \"" + updateSQLName + "\", parms);");
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("\t\tusing (SqlConnection conn = new SqlConnection(Database.CONN_STRING_NON_DTC))");
                stringBuilder.AppendLine("\t\t{");
                stringBuilder.AppendLine("\t\t\tconn.Open();");
                stringBuilder.AppendLine("\t\t\tusing (SqlTransaction tran = conn.BeginTransaction())");
                stringBuilder.AppendLine("\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\ttry");
                stringBuilder.AppendLine("\t\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\t\tint ret = Database.ExecuteNonQuery(tran, CommandType.Text, " + updateSQLName + ", parms);");
                stringBuilder.AppendLine("\t\t\t\t\tif(ret > 0)");
                stringBuilder.AppendLine("\t\t\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\t\t\tresult = true;");
                stringBuilder.AppendLine("\t\t\t\t\t}");
                stringBuilder.AppendLine("\t\t\t\t\ttran.Commit();");
                stringBuilder.AppendLine("\t\t\t\t}");
                stringBuilder.AppendLine("\t\t\t\tcatch");
                stringBuilder.AppendLine("\t\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\t\ttran.Rollback();");
                stringBuilder.AppendLine("\t\t\t\t\tthrow;");
                stringBuilder.AppendLine("\t\t\t\t}");
                stringBuilder.AppendLine("\t\t\t}");
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine("\t\treturn result;");
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine("\t#endregion Update");
                stringBuilder.AppendLine();


                #endregion Update Function

                #region Delete Function

                StringBuilder deleteFunc = new StringBuilder();
                deleteFunc.AppendLine("\t#region Delete");
                deleteFunc.AppendLine("\t/// <summary>");
                deleteFunc.AppendLine("\t/// 删除记录");
                deleteFunc.AppendLine("\t/// </summary>");
                deleteFunc.Append("\tpublic bool Delete(");
                deleteFunc.Append(funcArgs);
                deleteFunc.AppendLine(")");
                //函数体
                deleteFunc.AppendLine("\t{");
                deleteFunc.AppendLine("\t\tbool result = false;");
                deleteFunc.AppendLine("\t\tSqlParameter[] parms = " + getPriParamFuncName + "(" + funcArgs + ");");
                deleteFunc.AppendLine();
                deleteFunc.AppendLine("\t\tusing (SqlConnection conn = new SqlConnection(Database.CONN_STRING_NON_DTC))");
                deleteFunc.AppendLine("\t\t{");
                deleteFunc.AppendLine("\t\t\tconn.Open();");
                deleteFunc.AppendLine("\t\t\tusing (SqlTransaction tran = conn.BeginTransaction())");
                deleteFunc.AppendLine("\t\t\t{");
                deleteFunc.AppendLine("\t\t\t\ttry");
                deleteFunc.AppendLine("\t\t\t\t{");
                deleteFunc.AppendLine("\t\t\t\t\tint ret = Database.ExecuteNonQuery(tran, CommandType.Text, " + deleteSQLName + ", parms);");
                deleteFunc.AppendLine("\t\t\t\t\tif(ret > 0)");
                deleteFunc.AppendLine("\t\t\t\t\t{");
                deleteFunc.AppendLine("\t\t\t\t\t\tresult = true;");
                deleteFunc.AppendLine("\t\t\t\t\t}");
                deleteFunc.AppendLine("\t\t\t\t\ttran.Commit();");
                deleteFunc.AppendLine("\t\t\t\t}");
                deleteFunc.AppendLine("\t\t\t\tcatch");
                deleteFunc.AppendLine("\t\t\t\t{");
                deleteFunc.AppendLine("\t\t\t\t\ttran.Rollback();");
                deleteFunc.AppendLine("\t\t\t\t\tthrow;");
                deleteFunc.AppendLine("\t\t\t\t}");
                deleteFunc.AppendLine("\t\t\t}");
                deleteFunc.AppendLine("\t\t}");
                deleteFunc.AppendLine("\t\treturn result;");

                deleteFunc.AppendLine("\t}");
                deleteFunc.AppendLine("\t#endregion Delete");
                stringBuilder.Append(deleteFunc);
                stringBuilder.AppendLine();

                #endregion Delete Function

                #region Select Functions

                #region 单条读取
                string selectFuncName = "Get" + className;
                stringBuilder.AppendLine("\t#reion " + selectFuncName);
                stringBuilder.AppendLine("\t/// <summary>");
                stringBuilder.AppendLine("\t/// 获取单个记录");
                stringBuilder.AppendLine("\t/// </summary>");
                stringBuilder.Append("\tpublic " + className + "Info " + selectFuncName + "(");
                stringBuilder.Append(funcArgs);
                stringBuilder.AppendLine(")");
                stringBuilder.AppendLine("\t{");
                //函数题体
                stringBuilder.AppendLine("\t\t" + className + "Info " + objName + "Info = null;");
                stringBuilder.AppendLine("\t\tSqlParameter[] parms = " + getPriParamFuncName + "(" + funcValues + ");");
                stringBuilder.AppendLine();

                stringBuilder.AppendLine("\t\tusing (SqlDataReader rdr = Database.ExecuteReader(Database.CONN_STRING_NON_DTC, CommandType.Text, " + selectSQLName + ", parms))");
                stringBuilder.AppendLine("\t\t{");

                stringBuilder.AppendLine("\t\t\tif(rdr.Read())");
                stringBuilder.AppendLine("\t\t\t{");
                stringBuilder.AppendLine("\t\t\t\t" + objName + "Info = " + getObjFuncName + "(rdr);");
                stringBuilder.AppendLine("\t\t\t}");
                stringBuilder.AppendLine("\t\t}");
                stringBuilder.AppendLine("\t\treturn " + objName + "Info;");
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine("\t#endregion " + selectFuncName);
                stringBuilder.AppendLine();

                #endregion 单条读取

                #region 全部读取

//                string selectAllFuncName = "Get" + className + "s";
//                stringBuilder.AppendLine("\t#reion " + selectAllFuncName);
//                stringBuilder.AppendLine("\t/// <summary>");
//                stringBuilder.AppendLine("\t/// 获取所有记录");
//                stringBuilder.AppendLine("\t/// </summary>");
//                stringBuilder.AppendLine("\tpublic List<" + className + "Info> " + selectAllFuncName + "()");
//                stringBuilder.AppendLine("\t{");
//                //函数题体
//                stringBuilder.AppendLine("\t\tList<" + className + "Info> " + objName + "s = new List<" + className + "Info>();");
//                stringBuilder.AppendLine();
//
//                stringBuilder.AppendLine("\t\tusing (SqlDataReader rdr = Database.ExecuteReader(Database.CONN_STRING_NON_DTC, CommandType.Text, " + selectAllSQLName + "S))");
//                stringBuilder.AppendLine("\t\t{");
//
//                stringBuilder.AppendLine("\t\t\twhile(rdr.Read())");
//                stringBuilder.AppendLine("\t\t\t{");
//                stringBuilder.AppendLine("\t\t\t\t" + objName + "s.Add(" + getObjFuncName + "(rdr));");
//                stringBuilder.AppendLine("\t\t\t}");
//                stringBuilder.AppendLine("\t\t}");
//                stringBuilder.AppendLine("\t\treturn " + objName + "s;");
//                stringBuilder.AppendLine("\t}");
//                stringBuilder.AppendLine("\t#endregion " + selectFuncName);
//                stringBuilder.AppendLine();
//
//
//                stringBuilder.AppendLine("\t#reion " + selectAllFuncName + "(int pageIndex, int pageSize, out int recorderCount)");
//                stringBuilder.AppendLine("\t/// <summary>");
//                stringBuilder.AppendLine("\t/// 获取分页记录");
//                stringBuilder.AppendLine("\t/// </summary>");
//                stringBuilder.AppendLine("\tpublic List<" + className + "Info> " + selectAllFuncName + "(int pageIndex, int pageSize, out int recorderCount)");
//                stringBuilder.AppendLine("\t{");
//                //函数题体
//                stringBuilder.AppendLine("\t\tList<" + className + "Info> " + objName + "s = new List<" + className + "Info>();");
//
//                stringBuilder.AppendLine("\t\trecorderCount = 0;");
//                stringBuilder.AppendLine("\t\tusing (SqlDataReader rdr = Database.ExecuteReader(Database.CONN_STRING_NON_DTC, CommandType.Text, " + selectCountSQLName + "))");
//                stringBuilder.AppendLine("\t\t{");
//
//                stringBuilder.AppendLine("\t\t\tif(rdr.Read())");
//                stringBuilder.AppendLine("\t\t\t{");
//                stringBuilder.AppendLine("\t\t\t\trecorderCount = rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);");
//                stringBuilder.AppendLine("\t\t\t}");
//                stringBuilder.AppendLine("\t\t}");
//
//
//                stringBuilder.AppendLine("\t\tif(pageIndex < 1)");
//                stringBuilder.AppendLine("\t\t\tpageIndex = 1;");
//
//                stringBuilder.AppendLine("\t\tint startIndex = (pageIndex - 1 ) * pageSize + 1;");
//                stringBuilder.AppendLine("\t\tint endIndex = pageIndex * pageSize;");
//
//                stringBuilder.AppendLine("\t\tSqlParameter[] parms;");
//                stringBuilder.AppendLine("\t\tif (ParamsCache.GetCachedParameterSet(Database.CONN_STRING_NON_DTC, \"SqlParamters_StartIndex_EndIndex\", out parms))");
//
//                stringBuilder.AppendLine("\t\t{");
//                stringBuilder.AppendLine("\t\t\tDatabase.AssignParameterValues(parms,startIndex, endIndex);");
//                stringBuilder.AppendLine("\t\t}");
//                stringBuilder.AppendLine("\t\telse");
//                stringBuilder.AppendLine("\t\t{");
//                stringBuilder.AppendLine("\t\t\tparms = new SqlParameter[]");
//                stringBuilder.AppendLine("\t\t\t\t{");
//                stringBuilder.AppendLine("\t\t\t\t\tnew SqlParameter(PARM_STARTINDEX, SqlDbType.Int),");
//                stringBuilder.AppendLine("\t\t\t\t\tnew SqlParameter(PARM_ENDINDEX, SqlDbType.Int)");
//                stringBuilder.AppendLine("\t\t\t\t};");
//                stringBuilder.AppendLine("\t\t\tparms[0].Value = startIndex;");
//                stringBuilder.AppendLine("\t\t\tparms[1].Value = endIndex;");
//                stringBuilder.AppendLine("\t\t\tParamsCache.CacheParameterSet(Database.CONN_STRING_NON_DTC, \"SqlParamters_StartIndex_EndIndex\", parms);");
//                stringBuilder.AppendLine("\t\t}");
//                stringBuilder.AppendLine();
//
//                stringBuilder.AppendLine("\t\tusing (SqlDataReader rdr = Database.ExecuteReader(Database.CONN_STRING_NON_DTC, CommandType.Text, " + selectPagedSQLName + ", parms))");
//                stringBuilder.AppendLine("\t\t{");
//
//                stringBuilder.AppendLine("\t\t\twhile(rdr.Read())");
//                stringBuilder.AppendLine("\t\t\t{");
//                stringBuilder.AppendLine("\t\t\t\t" + objName + "s.Add(" + getObjFuncName + "(rdr));");
//                stringBuilder.AppendLine("\t\t\t}");
//                stringBuilder.AppendLine("\t\t}");
//                stringBuilder.AppendLine("\t\treturn " + objName + "s;");
//                stringBuilder.AppendLine("\t}");
//                stringBuilder.AppendLine("\t#endregion " + selectFuncName);
//                stringBuilder.AppendLine();

                #endregion 全部读取

                #endregion  Select Functions

                stringBuilder.AppendLine("\t#endregion Public Methods");
                stringBuilder.AppendLine();

                #endregion 公共方法

                #region 私有方法

                stringBuilder.AppendLine("\t#region Private Methods");
                stringBuilder.AppendLine();

                #region getSqlParameters
                //获取主键SqlParameters
                StringBuilder getPriParamsStr = new StringBuilder();
                getPriParamsStr.AppendLine("\t#region " + getPriParamFuncName);
                getPriParamsStr.AppendLine("\t/// <summary>");
                getPriParamsStr.AppendLine("\t/// 获取主键SqlParameters参数");
                getPriParamsStr.AppendLine("\t/// </summary>");
                getPriParamsStr.AppendLine("\tprivate static SqlParameter[] " + getPriParamFuncName + "(" + funcArgs + ")");
                getPriParamsStr.AppendLine("\t{");
                getPriParamsStr.AppendLine("\t\tSqlParameter[] parms;");
                getPriParamsStr.AppendLine("\t\tif (ParamsCache.GetCachedParameterSet(Database.CONN_STRING_NON_DTC, \"" + getPriParamFuncName + "_" + className + "\", out parms))");

                getPriParamsStr.AppendLine("\t\t{");
                getPriParamsStr.AppendLine("\t\t\tDatabase.AssignParameterValues(parms,");
                getPriParamsStr.Append(pkValueIf);
                getPriParamsStr.AppendLine();
                getPriParamsStr.AppendLine("\t\t\t\t);");
                getPriParamsStr.AppendLine("\t\t}");
                getPriParamsStr.AppendLine("\t\telse");
                getPriParamsStr.AppendLine("\t\t{");
                getPriParamsStr.AppendLine("\t\t\tparms = new SqlParameter[]");
                getPriParamsStr.AppendLine("\t\t\t{");
                getPriParamsStr.Append(pkParmElse);
                getPriParamsStr.AppendLine();
                getPriParamsStr.AppendLine("\t\t\t};");
                getPriParamsStr.Append(pkValueElse);
                getPriParamsStr.AppendLine();
                getPriParamsStr.AppendLine("\t\t\tParamsCache.CacheParameterSet(Database.CONN_STRING_NON_DTC, \"" + getPriParamFuncName + "_" + className + "\", parms);");
                getPriParamsStr.AppendLine("\t\t}");
                getPriParamsStr.AppendLine();
                getPriParamsStr.AppendLine("\t\treturn parms;");
                getPriParamsStr.AppendLine("\t}");
                getPriParamsStr.AppendLine("\t#endregion " + getPriParamFuncName);
                stringBuilder.Append(getPriParamsStr);
                stringBuilder.AppendLine();

                #endregion getSqlParameters

                #region GetObject Function
                StringBuilder getObjectStr = new StringBuilder();

                getObjectStr.AppendLine("\t#region " + getObjFuncName);
                getObjectStr.AppendLine("\t/// <summary>");
                getObjectStr.AppendLine("\t/// 获取对象");
                getObjectStr.AppendLine("\t/// </summary>");
                getObjectStr.AppendLine("\tprivate static " + className + "Info get" + className + "(SqlDataReader rdr)");

                //函数体
                getObjectStr.AppendLine("\t{");
                getObjectStr.Append("\t\t");
                getObjectStr.AppendLine(className + "Info " + objName + "Info = new " + className + "Info(");
                getObjectStr.Append(rdrStr);
                getObjectStr.AppendLine();
                getObjectStr.AppendLine("\t\t);");
                getObjectStr.AppendLine("\t\treturn " + objName + "Info;");
                getObjectStr.AppendLine("\t}");

                getObjectStr.AppendLine("\t#endregion " + getObjFuncName);
                stringBuilder.Append(getObjectStr);
                stringBuilder.AppendLine();
                #endregion GetObject Function

                stringBuilder.AppendLine("\t#endregion Private Methods");

                #endregion 私有方法

            }
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
        
        #region internal string GetSQLServerDBTypeString(string fieldType)

        /// <summary>
        /// 获取SQLServer的数据类型
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        internal string GetSQLServerDBTypeString(string fieldType)
        {
            if (fieldType.Equals("bigint", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.BigInt";
            }
            if (fieldType.Equals("int", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Int";
            }
            if (fieldType.Equals("tinyint", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.TinyInt";
            }
            if (fieldType.Equals("smallint", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.SmallInt";
            }
            if (fieldType.Equals("bit", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Bit";
            }
            if (fieldType.Equals("float", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Float";
            }
            if (fieldType.Equals("money", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Money";
            }
            if (fieldType.Equals("numeric", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Decimal";
            }
            if (fieldType.Equals("varchar", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.VarChar";
            }
            if (fieldType.Equals("nvarchar", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.NVarChar";
            }
            if (fieldType.Equals("char", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Char";
            }
            if (fieldType.Equals("text", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Text";
            }
            if (fieldType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.DateTime";
            }
            if (fieldType.Equals("date", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Date";
            }
            if (fieldType.Equals("datetime2", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.DateTime2";
            }
            if (fieldType.Equals("time", StringComparison.OrdinalIgnoreCase))
            {
                return "SqlDbType.Time";
            }
            return "unknown";
        }

        #endregion internal string GetSQLServerDBTypeString(string fieldType)

        #region  internal string GetSqlDataReaderString(string fieldType, int index)

        internal string GetSqlDataReaderString(string fieldType, int index)
        {
            if (fieldType.Equals("bigint", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? 0L : rdr.GetInt64({0})", index);
            }
            if (fieldType.Equals("int", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? 0 : rdr.GetInt32({0})", index);
            }
            if (fieldType.Equals("tinyint", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? (byte)0 : rdr.GetByte({0})", index);
            }
            if (fieldType.Equals("smallint", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? (short)0 : rdr.GetInt16({0})", index);
            }
            if (fieldType.Equals("bit", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? false : rdr.GetBoolean({0})", index);
            }
            if (fieldType.Equals("float", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? 0.0 : rdr.GetDouble({0})", index);
            }
            if (fieldType.Equals("money", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("numeric", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? decimal(0) : rdr.GetDecimal({0})", index); //"decimal";
            }
            if (fieldType.Equals("varchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("nvarchar", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("char", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("text", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? string.Empty : rdr.GetString({0})", index);
            }
            if (fieldType.Equals("datetime", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("date", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("datetime2", StringComparison.OrdinalIgnoreCase) ||
                fieldType.Equals("time", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("rdr.IsDBNull({0}) ? DateTime.Now : rdr.GetDateTime({0})", index); //"DateTime";
            }
            return string.Format("rdr.IsDBNull({0}) ? string.Empty : rdr.GetString({0})", index);
            ;
        }

        #endregion  internal string GetSqlDataReaderString(string fieldType, int index)

        #region internal string GetMySqlDbType(FieldInfo fieldInfo)
        internal string GetMySqlDbType(FieldInfo fieldInfo)
        {
            switch (fieldInfo.FieldType.ToLower())
            {
                case "varchar":
                    return "VarChar," + fieldInfo.FieldLength;
                case "int":
                    return "Int32";
                case "datetime":
                case "date":
                case "time":
                    return "DateTime";
                case "tinyint":
                    return "Byte";
                case "bigint":
                    return "Int64";
                case "short":
                    return "Int16";
                case "float":
                    return "Double";
                default:
                    return "VarChar";

            }
        }
        #endregion internal string GetMySqlDbType(FieldInfo fieldInfo)


        #endregion Internal Methods

        #region Public Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static Global GetInstance()
        {
            if (_instance != null) return _instance;

            _instance = new Global();
            return _instance;
        }

        #region  public MySqlConnection MySQLConn()
        /// <summary>
        ///     MySQL连接
        /// </summary>
        public MySqlConnection MySQLConn()
        {
            return
                new MySqlConnection(string.Format(ConstVars.MYSQL_CONNECT_STRING, DB.ServerName, DB.UserName,
                    DB.UserPassword,
                    DB.Port, DB.DataBase));
        }
        #endregion  public MySqlConnection MySQLConn()

        #region public SqlConnection SQLServerConn()
        /// <summary>
        ///     SQL连接
        /// </summary>
        public SqlConnection SQLServerConn()
        {
            return
                new SqlConnection(string.Format(ConstVars.SQLSERVER_CONNECT_STRING, DB.ServerName, DB.UserName,
                    DB.UserPassword,
                    DB.Port, DB.DataBase));
        }

        #endregion public SqlConnection SQLServerConn()
        
        #endregion Public Methods
        
    }
}