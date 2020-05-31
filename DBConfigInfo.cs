using System;
using System.Data;
using System.IO;

namespace CodeHelper
{
    [Serializable]
    public class DbConfigInfo
    {
        #region Properties

        /// <summary>
        /// ���������ƣ�
        /// ������IP��ַ��Ҳ�����ǻ�����
        /// </summary>
        public string ServerName  { get; set; }
        /// <summary>
        /// ���ݿ���ʵ��ʺ�
        /// </summary>
        public string UserName  { get; set; }
        /// <summary>
        /// ���ݿ��������
        /// </summary>
        public string UserPassword  { get; set; }
        /// <summary>
        /// ���ݿ���ʶ˿�
        /// </summary>
        public int Port  { get; set; }
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public string DataBase  { get; set; }
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public SQLDbTypes DbType  { get; set; }

        /// <summary>
        /// ���ӵ�����
        /// </summary>
        public string ConnectionName { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// ���ݿ����ø�����
    /// </summary>
    public class DbConfigUtil
    {
        /// <summary>
        /// ��ȡ���ݿ�����
        /// </summary>
        /// <returns></returns>
        public static DbConfigInfo GetDbConfig()
        {
            DbConfigInfo dbConfigInfo = new DbConfigInfo();
            string config = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "db.xml";
            if (!File.Exists(config))
            {
                DataTable dt = new DataTable("DBConfig");
                dt.Columns.Add("ServerName", Type.GetType("System.String") ?? throw new InvalidOperationException());
                dt.Columns.Add("UserName", Type.GetType("System.String") ?? throw new InvalidOperationException());
                dt.Columns.Add("UserPassword", Type.GetType("System.String") ?? throw new InvalidOperationException());
                dt.Columns.Add("Port", Type.GetType("System.Int32") ?? throw new InvalidOperationException());
                dt.Columns.Add("DataBase", Type.GetType("System.String") ?? throw new InvalidOperationException());
                dt.Columns.Add("DbType", Type.GetType("System.String") ?? throw new InvalidOperationException());
                dt.Columns.Add("ConnectionName", Type.GetType("System.String") ?? throw new InvalidOperationException());
                dbConfigInfo.ServerName = "127.0.0.1";
                dbConfigInfo.UserName = "root";
                dbConfigInfo.UserPassword = "1234";
                dbConfigInfo.Port = 3306;
                dbConfigInfo.DataBase = "test";
                dbConfigInfo.DbType =  SQLDbTypes.MySQL;
                dbConfigInfo.ConnectionName = "test";

                DataRow dr = dt.NewRow();
                dr["ServerName"] = dbConfigInfo.ServerName;
                dr["UserName"] = dbConfigInfo.UserName;
                dr["UserPassword"] = dbConfigInfo.UserPassword;
                dr["Port"] = dbConfigInfo.Port;
                dr["DataBase"] = dbConfigInfo.DataBase;
                dr["DbType"] = dbConfigInfo.DbType;
                dr["ConnectionName"] = dbConfigInfo.ConnectionName;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.WriteXml(config, XmlWriteMode.WriteSchema);
            }
            else
            {
                DataSet ds = new DataSet();
                ds.ReadXml(config, XmlReadMode.ReadSchema);
                DataRow dr = ds.Tables[0].Rows[0];
                dbConfigInfo.ServerName = dr["ServerName"].ToString();
                dbConfigInfo.UserName = dr["UserName"].ToString();
                dbConfigInfo.UserPassword = dr["UserPassword"].ToString();
                try
                {
                    dbConfigInfo.Port = int.Parse(dr["Port"].ToString());
                }
                catch
                {
                    dbConfigInfo.Port = 3306;
                }
                dbConfigInfo.DataBase = dr["DataBase"].ToString();
                Enum.TryParse(dr["DbType"].ToString(), true, out SQLDbTypes dbType);
                dbConfigInfo.DbType = dbType;
            }
            return dbConfigInfo;
        }

        public static void SaveDbConfig(DbConfigInfo dbConfigInfo)
        {
            string config = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "db.xml";
            DataSet ds = new DataSet();
            ds.ReadXml(config, XmlReadMode.ReadSchema);
            DataRow dr = ds.Tables[0].Rows[0];
            dr["ServerName"] = dbConfigInfo.ServerName;
            dr["UserName"] = dbConfigInfo.UserName;
            dr["UserPassword"] = dbConfigInfo.UserPassword;
            dr["Port"] = dbConfigInfo.Port;
            dr["DataBase"] = dbConfigInfo.DataBase;
            dr["DbType"] = dbConfigInfo.DbType;
            ds.Tables[0].AcceptChanges();
            ds.WriteXml(config, XmlWriteMode.WriteSchema);
        }

         

    }
}