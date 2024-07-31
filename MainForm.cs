using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeHelper
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<String, List<FieldInfo>> tableFields = new Dictionary<String, List<FieldInfo>>();
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TopMenuItem1_Click(object sender, EventArgs e)
        {
            DbConfig dbFrm = new DbConfig();
            if (dbFrm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(@"数据库配置成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TopMenuItem2_Click(object sender, EventArgs e)
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
                MessageBox.Show(ex.Message, "数据库访问失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DataTables_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                txtClassName.Text = e.Node.Text;
                tableName = e.Node.Text;
            }
        }

        #region private void btnCreateModel_Click(object sender, EventArgs e)

        private void BtnCreateModel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtClassName.Text) && !string.IsNullOrEmpty(tableName))
            {
                txtCode.Text = BSqlFunction.GetInstance().GetModelClass(txtClassName.Text.Trim(), tableName);
            }
            else
            {
                MessageBox.Show(@"Model类生成成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion  private void btnCreateModel_Click(object sender, EventArgs e)

        private void BtnCreateDAL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClassName.Text))
            {
                MessageBox.Show(@"DAL类生成成功", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string className = txtClassName.Text.Substring(0, 1).ToUpper() + txtClassName.Text.Substring(1);

            //MessageBox.Show(className);

            txtCode.Text = BSqlFunction.GetInstance().GetDalClass(className, tableName);
        }

       
        /// <summary>
        ///  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbCSharp4_CheckedChanged(object sender, EventArgs e)
        {
            Global.GetInstance().CodeType = CodeTypes.CSharp4;
        }

        private void RbCSharp2_CheckedChanged(object sender, EventArgs e)
        {
            Global.GetInstance().CodeType = CodeTypes.CSharp;
        }     

        private void RbJava_CheckedChanged(object sender, EventArgs e)
        {
            Global.GetInstance().CodeType = CodeTypes.Java;
        }

        


        #region Private Methods

        #region  private void CreateTreeNode()
        /// <summary>
        ///  
        /// </summary>
        private void CreateTreeNode()
        {
            tableFields.Clear();
            dataTables.Nodes.Clear();
            if (Global.GetInstance().Tables.Count > 0)
            {
                foreach (TableInfo tableInfo in Global.GetInstance().Tables)
                {
                    TreeNode parentNode = new TreeNode
                    {
                        Name = "table_" + tableInfo.TableName,
                        Text = tableInfo.TableName
                    };

                    foreach (FieldInfo fieldInfo in tableInfo.Fields)
                    {
                        TreeNode node = new TreeNode
                        {
                            Name = "table_" + tableInfo.TableName + "_field_" + fieldInfo.FieldName,

                            Text = fieldInfo.FieldName + @" [" + fieldInfo.FieldType + @"(" + fieldInfo.FieldLength + @")"
                        };
                        if (fieldInfo.IsPrimaryKey)
                        {
                            node.Text += @", pk";
                        }
                        if (fieldInfo.IsIdentity)
                        {
                            node.Text += @", identity";
                        }
                        if (fieldInfo.IsNullAble)
                        {
                            node.Text += @",null";
                        }
                        else
                        {
                            node.Text += @",not null";
                        }
                        node.Text += @"]";
                        parentNode.Nodes.Add(node);
                    }
                    dataTables.Nodes.Add(parentNode);
                }
            }
        }
        #endregion  private void CreateTreeNode()


        #endregion Private Methods

      
    }
}