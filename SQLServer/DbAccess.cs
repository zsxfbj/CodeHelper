using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CodeHelper.IDAL;
using CodeHelper.Model.DTO;
using CodeHelper.Model.VO;

namespace CodeHelper.SQLServerDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DbAccess : IDbAccess
    {
        /// <summary>
        /// 获取数据表的查询语句
        /// </summary>
        private const string SQL_SELECT_ALL_DATABASES = "SELECT * FROM master.dbo.SysDatabases";

        /// <summary>
        /// 获取指定库的所有数据表
        /// </summary>
        private const string SQL_SELECT_ALL_TABLES = "SELECT obj.name AS [name], se.value AS [description], obj.create_date FROM sys.objects obj LEFT JOIN [sys].[extended_properties] se ON obj.object_id = se.major_id AND se.minor_id = 0 WHERE obj.type = 'U'";

        /// <summary>
        /// 获取字段的查询语句
        /// </summary>
        private const string SQL_GET_FIELDS = @"SELECT      
a.colorder  as FieldNo  
,a.name  as FieldName  
,(CASE WHEN COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 THEN 1 ELSE 0 END) AS IsIdentity  
,(CASE WHEN exists(SELECT 1 FROM sysobjects where xtype='PK' AND parent_obj=a.id and name in (SELECT name FROM sysindexes WHERE indid IN (SELECT indid FROM sysindexkeys WHERE id=a.id AND colid=a.colid))) THEN 1 ELSE 0 END) AS IsPrimaryKey  
, b.name AS FieldType  
, COLUMNPROPERTY(a.id,a.name,'PRECISION') AS FieldLength  
, (CASE WHEN a.isnullable=1 THEN 1 ELSE 0 END) AS IsNullAble  
,isnull(e.text,'') AS FieldDefault  
,isnull(g.[value],'')  AS Decription  
FROM syscolumns AS a    
LEFT JOIN systypes AS b ON a.xusertype=b.xusertype    
INNER JOIN sysobjects AS d ON a.id=d.id AND d.xtype='U' AND d.name<>'dtproperties'    
LEFT JOIN syscomments AS e ON a.cdefault=e.id 
LEFT JOIN sys.extended_properties AS g ON a.id=G.major_id AND a.colid=g.minor_id    
LEFT JOIN sys.extended_properties AS f ON d.id=f.major_id AND f.minor_id=0    
WHERE d.name='{0}' ORDER BY a.id,a.colorder";

        /// <summary>
        /// 获取数据库链接字符串
        /// </summary>
        /// <param name="serverConfig"></param>
        /// <returns></returns>
        private static string GetConnString(ServerConfigDTO serverConfig)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=");
            sb.Append(serverConfig.Server.Trim());
            if (!string.IsNullOrWhiteSpace(serverConfig.Port))
            {
                sb.Append(",");
                sb.Append(serverConfig.Port.Trim());
            }
            if (!string.IsNullOrWhiteSpace(serverConfig.Database))
            {
                sb.Append(";Database=");
                sb.Append(serverConfig.Database.Trim());
            }
            sb.Append(";User ID=");
            sb.Append(serverConfig.UserId.Trim());
            sb.Append(";Password=");
            sb.Append(serverConfig.Password.Trim());
            sb.Append(";Trusted_Connection=True");
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverConfig"></param>
        /// <returns></returns>
        public List<DatabaseVO> GetDatabases(ServerConfigDTO serverConfig)
        {
            List<DatabaseVO> databases = new List<DatabaseVO> ();
            using(SqlDataReader rdr = Database.ExecuteReader(GetConnString(serverConfig), System.Data.CommandType.Text, SQL_SELECT_ALL_DATABASES))
            {
                while (rdr.Read())
                {
                    DatabaseVO vo = new DatabaseVO
                    {
                        DbName = rdr.GetString(0),
                        Tables = new List<TableVO>(),
                        CreateTime = rdr.GetDateTime(6)
                    };

                    databases.Add(vo);
                }
                rdr.Close();
            }
            return databases;
        }
          
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="serverConfig"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<FieldVO> GetFields(string tableName, ServerConfigDTO serverConfig, string db)
        {
            List<FieldVO> fields = new List<FieldVO>();
            serverConfig.Database = db;
            string connString = GetConnString(serverConfig);
         
            using (SqlDataReader rdr = Database.ExecuteReader(connString, System.Data.CommandType.Text, string.Format(SQL_GET_FIELDS, tableName)))
            {
                while (rdr.Read())
                {
                    FieldVO vo = new FieldVO
                    {
                        FieldName = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                        IsIdentity = (rdr.GetInt32(2) == 1),
                        IsPrimaryKey = (rdr.GetInt32(3) == 1),
                        FieldType = rdr.GetString(4),
                        FieldLength = rdr.GetInt32(5),
                        IsNullAble = (rdr.GetInt32(6) == 1),
                        DefaultValue = rdr.IsDBNull(7) ? string.Empty : rdr.GetString(7),
                        Description = rdr.IsDBNull(8) ? string.Empty : rdr.GetString(8)
                    };
                    fields.Add(vo);
                }
                rdr.Close();
            }         
            return fields;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverConfig"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<TableVO> GetTables(ServerConfigDTO serverConfig, string db)
        {
            List<TableVO> tables = new List<TableVO>();
            serverConfig.Database = db;
            string connString = GetConnString(serverConfig);            
            using (SqlDataReader rdr = Database.ExecuteReader(connString, System.Data.CommandType.Text, SQL_SELECT_ALL_TABLES))
            {
                while (rdr.Read())
                {
                    TableVO vo = new TableVO
                    {
                        TableName = rdr.GetString(0),
                        Description =rdr.GetString(1),
                        CreateTime = rdr.GetDateTime(2), 
                        Fields = new List<FieldVO>()
                    };
                    tables.Add(vo);                                       
                }
                rdr.Close();
            }
            return tables;
        }
    }
}
