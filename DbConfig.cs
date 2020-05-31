using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CodeHelper
{
    public partial class DbConfig : Form
    {
        private DbConfigInfo _dbConfigInfo = null;

        public DbConfig()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            _dbConfigInfo = DbConfigUtil.GetDbConfig();
            txtServerName.Text = _dbConfigInfo.ServerName;
            txtUserName.Text = _dbConfigInfo.UserName;
            txtPwd.Text = _dbConfigInfo.UserPassword;
            txtPort.Text = _dbConfigInfo.Port.ToString("D");
            txtDataBase.Text = _dbConfigInfo.DataBase;
            if (_dbConfigInfo.DbType == SQLDbTypes.MySQL)
            {
                rbMySQL.Checked = true;
            }
            else
            {
                rbSQLServer.Checked = true;
            }
            //cmbbSQLType.SelectedValue = dbConfigInfo.DbType;
            //cmbbSQLType.SelectedIndex = cmbbSQLType.Items.IndexOf(dbConfigInfo.DbType);
            //cmbbSQLType.SelectedText = dbConfigInfo.DbType;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
             
            string connectionString = "";
            if (rbMySQL.Checked)
            {
                connectionString = string.Format(ConstVars.MYSQL_CONNECT_STRING, txtServerName.Text, txtUserName.Text.Trim(), txtPwd.Text.Trim(), txtPort.Text.Trim(), txtDataBase.Text.Trim());
                try
                {
                    MySqlConnection conn = new MySqlConnection(connectionString);
                    conn.Open();
                    MessageBox.Show(@"数据打开正常！", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                connectionString = string.Format(ConstVars.SQLSERVER_CONNECT_STRING, txtServerName.Text, txtUserName.Text.Trim(), txtPwd.Text.Trim(), txtPort.Text.Trim(), txtDataBase.Text.Trim());
                MessageBox.Show(connectionString, @"提示", MessageBoxButtons.OK);
                try
                {
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    MessageBox.Show(@"数据打开正常！", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtPort.Text, out var port))
            {
                _dbConfigInfo.ServerName = txtServerName.Text.Trim();
                _dbConfigInfo.UserName = txtUserName.Text.Trim();
                _dbConfigInfo.UserPassword = txtPwd.Text.Trim();
                _dbConfigInfo.Port = port;
                _dbConfigInfo.DataBase = txtDataBase.Text.Trim();
                if (rbMySQL.Checked)
                {
                    _dbConfigInfo.DbType = SQLDbTypes.MySQL;
                }
                else
                {
                    _dbConfigInfo.DbType = SQLDbTypes.SQLServer;
                }
                DbConfigUtil.SaveDbConfig(_dbConfigInfo);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(@"端口必须是整数！", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}