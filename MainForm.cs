using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeHelper
{
    public partial class MainForm : Form
    {
        private Dictionary<String, List<FieldInfo>> tableFields = new Dictionary<String, List<FieldInfo>>();
        private string tableName;

        public MainForm()
        {
            InitializeComponent();
            switch (Global.GetInstance().CodeType)
            {
                   case CodeTypes.CSharp4:
                    rbCSharp4.Checked = true;
                    break;
                case CodeTypes.CSharp:
                    rbCSharp2.Checked = true;
                    break;
                case CodeTypes.Java:

                    rbJava.Checked = true;
                    break;
                case CodeTypes.DooPHP :
                    rbDooPHP.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// 配置数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topMenuItem1_Click(object sender, EventArgs e)
        {
            DbConfig dbFrm = new DbConfig();
            if (dbFrm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(@"数据库设置成功！", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void topMenuItem2_Click(object sender, EventArgs e)
        {
            DbConfigInfo dbConfigInfo = DbConfigUtil.GetDbConfig();
            if (dbConfigInfo != null)
            {
                Global.GetInstance().DB = dbConfigInfo;
            }

            try
            {
                Global.GetInstance().Tables = BSqlFunction.GetInstance().GetTables();
                CreateTreeNode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataTables_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                txtClassName.Text = e.Node.Text;
                tableName = e.Node.Text;
            }
        }

        #region private void btnCreateModel_Click(object sender, EventArgs e)

        private void btnCreateModel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtClassName.Text) && !string.IsNullOrEmpty(tableName))
            {
                txtCode.Text = BSqlFunction.GetInstance().GetModelClass(txtClassName.Text.Trim(), tableName);
            }
            else
            {
                MessageBox.Show(@"请选择要生成代码的数据表或者填写要生成的类名", @"提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion  private void btnCreateModel_Click(object sender, EventArgs e)

        private void btnCreateDAL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClassName.Text))
            {
                MessageBox.Show(@"必须填写类名", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string className = txtClassName.Text.Substring(0, 1).ToUpper() + txtClassName.Text.Substring(1);

            //MessageBox.Show(className);

            txtCode.Text = BSqlFunction.GetInstance().GetDalClass(className, tableName);
        }

        #region 生成代码类型的选择点击事件

        /// <summary>
        /// 选择生成C#4代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCSharp4_CheckedChanged(object sender, EventArgs e)
        {
            Global.GetInstance().CodeType = CodeTypes.CSharp4;
        }

        private void rbCSharp2_CheckedChanged(object sender, EventArgs e)
        {
            Global.GetInstance().CodeType = CodeTypes.CSharp;
        }

        private void rbDooPHP_CheckedChanged(object sender, EventArgs e)
        {
            Global.GetInstance().CodeType = CodeTypes.DooPHP;
        }

        private void rbJava_CheckedChanged(object sender, EventArgs e)
        {
            Global.GetInstance().CodeType = CodeTypes.Java;
        }

        #endregion 生成代码类型的选择点击事件


        #region Private Methods

        #region  private void CreateTreeNode()
        /// <summary>
        /// 创建表单树
        /// </summary>
        private void CreateTreeNode()
        {
            tableFields.Clear();
            dataTables.Nodes.Clear();
            if (Global.GetInstance().Tables.Count > 0)
            {
                foreach (TableInfo tableInfo in Global.GetInstance().Tables)
                {
                    TreeNode parentNode = new TreeNode();
                    parentNode.Name = "table_" + tableInfo.TableName;
                    parentNode.Text = tableInfo.TableName;

                    foreach (FieldInfo fieldInfo in tableInfo.Fields)
                    {
                        TreeNode node = new TreeNode();
                        node.Name = "table_" + tableInfo.TableName + "_field_" + fieldInfo.FieldName;

                        node.Text = fieldInfo.FieldName + @" [" + fieldInfo.FieldType + @"(" + fieldInfo.FieldLength + @")";
                        if (fieldInfo.IsPrimaryKey)
                        {
                            node.Text = node.Text + @", pk";
                        }
                        if (fieldInfo.IsIdentity)
                        {
                            node.Text = node.Text + @", identity";
                        }
                        if (fieldInfo.IsNullAble)
                        {
                            node.Text = node.Text + @",null";
                        }
                        else
                        {
                            node.Text = node.Text + @",not null";
                        }
                        node.Text = node.Text + @"]";
                        parentNode.Nodes.Add(node);
                    }
                    dataTables.Nodes.Add(parentNode);
                }
            }
        }
        #endregion  private void CreateTreeNode()


        #endregion Private Methods

        
        #region unused
        private void btnCreateJava_Click(object sender, EventArgs e)
        {
            //txtCode.Text = "";
            //if(txtClassName.Text.Trim() != string.Empty)
            //{
            //    DBConfigInfo dbConfigInfo = DBConfigUtil.GetDBConfigInfo();
            //    StringBuilder sb = new StringBuilder();

            //    //生成java model部分
            //    StringBuilder fieldString = new StringBuilder();
            //    StringBuilder getString = new StringBuilder();
            //    StringBuilder setString = new StringBuilder();
            //    StringBuilder constructorString = new StringBuilder();
            //    StringBuilder parameterString = new StringBuilder();
            //    //java Sql部分
            //    string insertSql = "INSERT INTO " + dbConfigInfo.DataBase + "." + dataTables.SelectedNode.Text + " ( {0} ) VALUES ( {1} )";
            //    string updateSql = "UPDATE " + dbConfigInfo.DataBase + "." + dataTables.SelectedNode.Text + " SET {0} WHERE {1}";
            //    string deleteSql = "DELETE FROM " + dbConfigInfo.DataBase + "." + dataTables.SelectedNode.Text + " WHERE {0}";
            //    string selectSql = "SELECT {0} FROM " + dbConfigInfo.DataBase + "." + dataTables.SelectedNode.Text + " WHERE {1}";

            //    StringBuilder insertFields = new StringBuilder();
            //    StringBuilder selectFields = new StringBuilder();
            //    StringBuilder valuesString = new StringBuilder();
            //    StringBuilder updateFields = new StringBuilder();
            //    StringBuilder whereParams = new StringBuilder();
            //    StringBuilder rsString = new StringBuilder();


            //    sb.Append("public class " + txtClassName.Text.Trim() + " {" + Environment.NewLine);
            //    sb.Append(Environment.NewLine);

            //    constructorString.Append("\tpublic " + txtClassName.Text + "() {}" + Environment.NewLine);
            //    sb.Append(Environment.NewLine);
            //    constructorString.Append("\tpublic " + txtClassName.Text + "(@parms) { " + Environment.NewLine);


            //    List<FieldInfo> fields = tableFields[tableName];
            //    bool hasAutoIncrement = false;
            //    foreach (FieldInfo info in fields)
            //    {
            //        selectFields.Append(info.FieldName + ", ");
            //        if (info.Extra.Equals("auto_increment"))
            //        {
            //            whereParams.Append(info.FieldName + "=?");
            //            hasAutoIncrement = true;
            //        }
            //        else if (hasAutoIncrement == false && info.FieldKey.Equals("PRI"))
            //        {
            //            whereParams.Append(info.FieldName + "=? AND ");
            //            insertFields.Append(info.FieldName + ", ");
            //            valuesString.Append("?,");
            //        }
            //        else
            //        {
            //            insertFields.Append(info.FieldName + ", ");
            //            updateFields.Append(info.FieldName + "=?, ");
            //            valuesString.Append("?,");
            //        }

            //        string firstAlphabet = info.FieldName.Substring(0, 1);

            //        if (info.Comment.Length > 0)
            //        {
            //            fieldString.Append("\t/**" + Environment.NewLine);
            //            fieldString.Append("\t*" + info.Comment + Environment.NewLine);
            //            fieldString.Append("\t*/" + Environment.NewLine);
            //        }
            //        fieldString.Append("\tprivate ");
            //        getString.Append("\tpublic ");
            //        setString.Append("\tpublic void set" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + " (");

            //        if (info.FieldTye.IndexOf("bigint", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("long ");
            //            getString.Append("long get" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "(){" + Environment.NewLine);

            //            setString.Append(" long " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + " ){" + Environment.NewLine);

            //            parameterString.Append(" long " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + ",");

            //            rsString.Append("rs.getLong(\"" + info.FieldName + "\"),");
            //        }
            //        else if (info.FieldTye.IndexOf("tinyint", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("byte ");
            //            getString.Append("byte get" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "(){" + Environment.NewLine);
            //            setString.Append(" byte " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + " ){" + Environment.NewLine);

            //            parameterString.Append(" byte " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + ",");

            //            rsString.Append("rs.getByte(\"" + info.FieldName + "\"),");
            //        }
            //        else if (info.FieldTye.IndexOf("int", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("int ");
            //            getString.Append("int get" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "(){" + Environment.NewLine);

            //            setString.Append(" int " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + " ){" + Environment.NewLine);

            //            parameterString.Append(" int " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + ",");
            //            rsString.Append("rs.getInt(\"" + info.FieldName + "\"),");
            //        }
            //        else if (info.FieldTye.IndexOf("char", StringComparison.OrdinalIgnoreCase) != -1 || info.FieldTye.IndexOf("blob", StringComparison.OrdinalIgnoreCase) != -1 || info.FieldTye.IndexOf("text", StringComparison.OrdinalIgnoreCase) != -1 )
            //        {
            //            fieldString.Append("String ");
            //            getString.Append("String get" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "(){" + Environment.NewLine);

            //            setString.Append(" String " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + " ){" + Environment.NewLine);

            //            parameterString.Append(" String " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + ",");

            //            rsString.Append("rs.getString(\"" + info.FieldName + "\"),");
            //        }
            //        else if (info.FieldTye.IndexOf("float", StringComparison.OrdinalIgnoreCase) != -1 )
            //        {
            //            fieldString.Append("double ");
            //            getString.Append("double get" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "(){" + Environment.NewLine);

            //            setString.Append(" double " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + " ){" + Environment.NewLine);

            //            parameterString.Append(" double " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + ",");

            //            rsString.Append("rs.getDouble(\"" + info.FieldName + "\"),");
            //        }

            //        else if (info.FieldTye.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("java.math.BigDecimal ");
            //            getString.Append("java.math.BigDecimal get" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "(){" + Environment.NewLine);

            //            setString.Append(" java.math.BigDecimal " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + " ){" + Environment.NewLine);

            //            parameterString.Append(" java.math.BigDecimal " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + ",");

            //            rsString.Append("rs.getBigDecimal(\"" + info.FieldName + "\"),");
            //        }
            //        else if (info.FieldTye.IndexOf("datetime", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("Date ");

            //            getString.Append("Date get" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "(){" + Environment.NewLine);

            //            setString.Append(" Date " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + " ){" + Environment.NewLine);

            //            parameterString.Append(" Date " + firstAlphabet.ToLower() + info.FieldName.Substring(1) + ",");

            //            rsString.Append("rs.getTimestamp(\"" + info.FieldName + "\"),");
            //        }
            //        rsString.Append(Environment.NewLine);

            //        fieldString.Append(firstAlphabet.ToUpper() + info.FieldName.Substring(1) + ";" + Environment.NewLine);

            //        getString.Append(Environment.NewLine);
            //        getString.Append("\t\treturn " + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + ";" + Environment.NewLine);
            //        getString.Append("\t}" + Environment.NewLine);

            //        setString.Append("\t\t" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + " = " + firstAlphabet.ToLower() +
            //                         info.FieldName.Substring(1) + ";" + Environment.NewLine);
            //        setString.Append("\t}" + Environment.NewLine);

            //        constructorString.Append("\t\t" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + " = " + firstAlphabet.ToLower() +
            //                         info.FieldName.Substring(1) + ";" + Environment.NewLine);

            //    }

            //    constructorString.Append("\t}" + Environment.NewLine);

            //    sb.Append(fieldString.ToString());
            //    sb.Append(Environment.NewLine);
            //    string parmString = parameterString.Remove(parameterString.Length - 1, 1).ToString();
            //    sb.Append(constructorString);
            //    sb.Replace("@parms", parmString);
            //    sb.Append(Environment.NewLine);
            //    sb.Append(getString.ToString());
            //    sb.Append(Environment.NewLine);
            //    sb.Append(setString.ToString());


            //    sb.Append("}");
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);


            //    //生成java DAL部分

            //    sb.Append("//DAL部分");

            //    sb.Append(Environment.NewLine);


            //    sb.Append("\tprivate final static String SQL_INSERT_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(insertSql, insertFields.Remove(insertFields.Length - 2, 2), valuesString.Remove(valuesString.Length - 1, 1)) + "\";");
            //    sb.Append(Environment.NewLine);
            //    /*
            //    if(whereParams.Length > 0)
            //    {
            //        whereParams.Remove(whereParams.Length - 4, 4);
            //    }
            //    */
            //    sb.Append("\tprivate final static String SQL_UPDATE_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(updateSql, updateFields.Remove(updateFields.Length - 2, 2), whereParams) + "\";");
            //    sb.Append(Environment.NewLine);

            //    sb.Append("\tprivate final static String SQL_DELETE_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(deleteSql, whereParams) + "\";");
            //    sb.Append(Environment.NewLine);

            //    sb.Append("\tprivate final static String SQL_SELECT_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(selectSql, selectFields.Remove(selectFields.Length - 2, 2), whereParams) + "\";");
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);

            //    sb.Append(rsString.ToString());
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);

            //    txtCode.Text = sb.ToString();

            //}
        }

        private void btnCreatePHP_Click(object sender, EventArgs e)
        {
            //txtCode.Text = "";
            //if (txtClassName.Text.Trim() != string.Empty)
            //{
            //    DBConfigInfo dbConfigInfo = DBConfigUtil.GetDBConfigInfo();
            //    StringBuilder sb = new StringBuilder();

            //    //生成java model部分
            //    StringBuilder fieldString = new StringBuilder();

            //    StringBuilder constructorString = new StringBuilder();
            //    StringBuilder parameterString = new StringBuilder();
            //    //java Sql部分
            //    string insertSql = "INSERT INTO " + dbConfigInfo.DataBase + "." + dataTables.SelectedNode.Text + " ( {0} ) VALUES ( {1} )";
            //    string updateSql = "UPDATE " + dbConfigInfo.DataBase + "." + dataTables.SelectedNode.Text + " SET {0} WHERE {1}";
            //    string deleteSql = "DELETE FROM " + dbConfigInfo.DataBase + "."  + dataTables.SelectedNode.Text + " WHERE {0}";
            //    string selectSql = "SELECT {0} FROM " + dbConfigInfo.DataBase + "." + dataTables.SelectedNode.Text + " WHERE {1}";

            //    StringBuilder insertFields = new StringBuilder();
            //    StringBuilder selectFields = new StringBuilder();
            //    StringBuilder valuesString = new StringBuilder();
            //    StringBuilder updateFields = new StringBuilder();
            //    StringBuilder whereParams = new StringBuilder();
            //    StringBuilder rsString = new StringBuilder();

            //    sb.Append("class " + txtClassName.Text.Trim() + " {" + Environment.NewLine);
            //    sb.Append(Environment.NewLine);

            //    constructorString.Append("\tpublic function __construct(@parms) { " + Environment.NewLine);


            //    List<FieldInfo> fields = tableFields[tableName];
            //    bool hasAutoIncrement = false;
            //    foreach (FieldInfo info in fields)
            //    {
            //        selectFields.Append(info.FieldName + ", ");
            //        rsString.Append("$row[\""+info.FieldName+"\"],");

            //        string firstAlphabet = info.FieldName.Substring(0, 1);


            //        if (info.Comment.Length > 0)
            //        {
            //            fieldString.Append("\t/**" + Environment.NewLine);
            //            fieldString.Append("\t*" + info.Comment + Environment.NewLine);
            //            fieldString.Append("\t*/" + Environment.NewLine);
            //        }
            //        fieldString.Append("\tprivate ");

            //        fieldString.Append("$" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) );
            //        if (info.FieldTye.IndexOf("bigint", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("=0;" + Environment.NewLine);
            //            parameterString.Append(" $in_" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "=0,");
            //            if (info.Extra.Equals("auto_increment"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0,1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName );
            //                hasAutoIncrement = true;
            //            }
            //            else if (hasAutoIncrement == false && info.FieldKey.Equals("PRI"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0,1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName+" AND ");
            //                insertFields.Append(info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0,1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName+",");
            //            }
            //            else
            //            {
            //                insertFields.Append(info.FieldName + ", ");
            //                updateFields.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ",");
            //            }
            //        }
            //        else if (info.FieldTye.IndexOf("tinyint", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("=0;" + Environment.NewLine);
            //            parameterString.Append(" $in_" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "=0,");
            //            if (info.Extra.Equals("auto_increment"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName);
            //                hasAutoIncrement = true;
            //            }
            //            else if (hasAutoIncrement == false && info.FieldKey.Equals("PRI"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + " AND ");
            //                insertFields.Append(info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ",");
            //            }
            //            else
            //            {
            //                insertFields.Append(info.FieldName + ", ");
            //                updateFields.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ",");
            //            }
            //        }
            //        else if (info.FieldTye.IndexOf("int", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("=0;" + Environment.NewLine);
            //            parameterString.Append(" $in_" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "=0,");
            //            if (info.Extra.Equals("auto_increment"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName);
            //                hasAutoIncrement = true;
            //            }
            //            else if (hasAutoIncrement == false && info.FieldKey.Equals("PRI"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + " AND ");
            //                insertFields.Append(info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ",");
            //            }
            //            else
            //            {
            //                insertFields.Append(info.FieldName + ", ");
            //                updateFields.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ",");
            //            }
            //        }
            //        else if (info.FieldTye.IndexOf("char", StringComparison.OrdinalIgnoreCase) != -1 || info.FieldTye.IndexOf("blob", StringComparison.OrdinalIgnoreCase) != -1 || info.FieldTye.IndexOf("text", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("='';" + Environment.NewLine);
            //            parameterString.Append(" $in_" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "='',");
            //            if (info.Extra.Equals("auto_increment"))
            //            {
            //                whereParams.Append(info.FieldName + "='$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "'");
            //                hasAutoIncrement = true;
            //            }
            //            else if (hasAutoIncrement == false && info.FieldKey.Equals("PRI"))
            //            {
            //                whereParams.Append(info.FieldName + "='$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "' AND ");
            //                insertFields.Append(info.FieldName + ", ");
            //                valuesString.Append("'$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "',");
            //            }
            //            else
            //            {
            //                insertFields.Append(info.FieldName + ", ");
            //                updateFields.Append(info.FieldName + "='$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "', ");
            //                valuesString.Append("'$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "',");
            //            }
            //        }
            //        else if (info.FieldTye.IndexOf("float", StringComparison.OrdinalIgnoreCase) != -1 || info.FieldTye.IndexOf("decimal", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("=0.0;" + Environment.NewLine);
            //            parameterString.Append(" $in_" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "=0.0,");
            //            if (info.Extra.Equals("auto_increment"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName);
            //                hasAutoIncrement = true;
            //            }
            //            else if (hasAutoIncrement == false && info.FieldKey.Equals("PRI"))
            //            {
            //                whereParams.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + " AND ");
            //                insertFields.Append(info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ",");
            //            }
            //            else
            //            {
            //                insertFields.Append(info.FieldName + ", ");
            //                updateFields.Append(info.FieldName + "=$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ", ");
            //                valuesString.Append("$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + ",");
            //            }
            //        }
            //        else if (info.FieldTye.IndexOf("datetime", StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            fieldString.Append("='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "';" + Environment.NewLine);
            //            parameterString.Append(" $in_" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + "='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
            //            if (info.Extra.Equals("auto_increment"))
            //            {
            //                whereParams.Append(info.FieldName + "='$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "'");
            //                hasAutoIncrement = true;
            //            }
            //            else if (hasAutoIncrement == false && info.FieldKey.Equals("PRI"))
            //            {
            //                whereParams.Append(info.FieldName + "='$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "' AND ");
            //                insertFields.Append(info.FieldName + ", ");
            //                valuesString.Append("'$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "',");
            //            }
            //            else
            //            {
            //                insertFields.Append(info.FieldName + ", ");
            //                updateFields.Append(info.FieldName + "='$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "', ");
            //                valuesString.Append("'$" + txtClassName.Text.Substring(0, 1).ToLower() + txtClassName.Text.Substring(1) + "->" + info.FieldName + "',");
            //            }
            //        }
            //        rsString.Append(Environment.NewLine);

            //        constructorString.Append("\t\t$this->" + firstAlphabet.ToUpper() + info.FieldName.Substring(1) + " = $in_" + firstAlphabet.ToUpper() +
            //                         info.FieldName.Substring(1) + ";" + Environment.NewLine);

            //    }
            //    constructorString.Append("\t}" + Environment.NewLine);

            //    sb.Append(fieldString.ToString());
            //    sb.Append(Environment.NewLine);
            //    string parmString = parameterString.Remove(parameterString.Length - 1, 1).ToString();
            //    sb.Append(constructorString);
            //    sb.Replace("@parms", parmString);

            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t//__get()：获取属性值");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\tpublic function __get($property_name){ ");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t\tif(isset($this->$property_name)){");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t\t\treturn($this->$property_name);");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t\t}");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t\telse {");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t\t\treturn NULL;");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t\t}");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t}");
            //    sb.Append(Environment.NewLine);

            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t//__set()：设置单个私有数据属性值，用于少量的修改数据");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\tpublic function __set($property_name, $value) {");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t\t$this->$property_name = $value;");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("\t}");
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);
            //    sb.Append("}");
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);

            //    //生成java sql部分

            //    sb.Append("//MySQL 操作部分");

            //    sb.Append(Environment.NewLine);
            //    sb.Append("public class " + txtClassName.Text.Replace("Info", "").Trim() + " {" + Environment.NewLine);

            //    sb.Append("//private static final String SQL_INSERT_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(insertSql, insertFields.Remove(insertFields.Length - 2, 2), valuesString.Remove(valuesString.Length - 1, 1)) + "\";");
            //    sb.Append(Environment.NewLine);
            //    /*
            //    if(whereParams.Length > 0)
            //    {
            //        whereParams.Remove(whereParams.Length - 4, 4);
            //    }
            //    */
            //    sb.Append("//private final static String SQL_UPDATE_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(updateSql, updateFields.Remove(updateFields.Length - 2, 2), whereParams) + "\";");
            //    sb.Append(Environment.NewLine);

            //    sb.Append("//private final static String SQL_DELETE_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(deleteSql, whereParams) + "\";");
            //    sb.Append(Environment.NewLine);

            //    sb.Append("//private final static String SQL_SELECT_" + dataTables.SelectedNode.Text.ToUpper() + " = \"" + string.Format(selectSql, selectFields.Remove(selectFields.Length - 2, 2), whereParams) + "\";");
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);

            //    sb.Append(rsString.ToString());
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);

            //    sb.Append("}");
            //    txtCode.Text = sb.ToString();

            //}
        }

        #endregion unused

   
        

    }
}