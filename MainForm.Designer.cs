namespace CodeHelper
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenuStrips = new System.Windows.Forms.MenuStrip();
            this.topMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.topMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTables = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.rbCSharp2 = new System.Windows.Forms.RadioButton();
            this.btnCreateModel = new System.Windows.Forms.Button();
            this.btnCreateDAL = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbJava = new System.Windows.Forms.RadioButton();
            this.rbDooPHP = new System.Windows.Forms.RadioButton();
            this.rbCSharp4 = new System.Windows.Forms.RadioButton();
            this.mainMenuStrips.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrips
            // 
            this.mainMenuStrips.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topMenuItem1,
            this.topMenuItem2});
            this.mainMenuStrips.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrips.Name = "mainMenuStrips";
            this.mainMenuStrips.Size = new System.Drawing.Size(992, 25);
            this.mainMenuStrips.TabIndex = 0;
            this.mainMenuStrips.Text = "主菜单";
            // 
            // topMenuItem1
            // 
            this.topMenuItem1.Name = "topMenuItem1";
            this.topMenuItem1.Size = new System.Drawing.Size(80, 21);
            this.topMenuItem1.Text = "数据库设置";
            this.topMenuItem1.Click += new System.EventHandler(this.topMenuItem1_Click);
            // 
            // topMenuItem2
            // 
            this.topMenuItem2.Name = "topMenuItem2";
            this.topMenuItem2.Size = new System.Drawing.Size(80, 21);
            this.topMenuItem2.Text = "连接数据库";
            this.topMenuItem2.Click += new System.EventHandler(this.topMenuItem2_Click);
            // 
            // dataTables
            // 
            this.dataTables.Location = new System.Drawing.Point(0, 27);
            this.dataTables.Name = "dataTables";
            this.dataTables.Size = new System.Drawing.Size(231, 539);
            this.dataTables.TabIndex = 1;
            this.dataTables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.dataTables_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(239, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "类名";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(273, 28);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(159, 21);
            this.txtClassName.TabIndex = 3;
            // 
            // txtCode
            // 
            this.txtCode.AcceptsReturn = true;
            this.txtCode.AcceptsTab = true;
            this.txtCode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(236, 106);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCode.Size = new System.Drawing.Size(755, 459);
            this.txtCode.TabIndex = 4;
            // 
            // rbCSharp2
            // 
            this.rbCSharp2.AutoSize = true;
            this.rbCSharp2.Location = new System.Drawing.Point(8, 20);
            this.rbCSharp2.Name = "rbCSharp2";
            this.rbCSharp2.Size = new System.Drawing.Size(77, 16);
            this.rbCSharp2.TabIndex = 5;
            this.rbCSharp2.Tag = "Code";
            this.rbCSharp2.Text = "C#（2.0）";
            this.rbCSharp2.UseVisualStyleBackColor = true;
            this.rbCSharp2.CheckedChanged += new System.EventHandler(this.rbCSharp2_CheckedChanged);
            // 
            // btnCreateModel
            // 
            this.btnCreateModel.Location = new System.Drawing.Point(443, 27);
            this.btnCreateModel.Name = "btnCreateModel";
            this.btnCreateModel.Size = new System.Drawing.Size(107, 23);
            this.btnCreateModel.TabIndex = 8;
            this.btnCreateModel.Text = "生成Model类";
            this.btnCreateModel.UseVisualStyleBackColor = true;
            this.btnCreateModel.Click += new System.EventHandler(this.btnCreateModel_Click);
            // 
            // btnCreateDAL
            // 
            this.btnCreateDAL.Location = new System.Drawing.Point(559, 27);
            this.btnCreateDAL.Name = "btnCreateDAL";
            this.btnCreateDAL.Size = new System.Drawing.Size(101, 23);
            this.btnCreateDAL.TabIndex = 9;
            this.btnCreateDAL.Text = "生成DAL类";
            this.btnCreateDAL.UseVisualStyleBackColor = true;
            this.btnCreateDAL.Click += new System.EventHandler(this.btnCreateDAL_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbJava);
            this.groupBox1.Controls.Add(this.rbDooPHP);
            this.groupBox1.Controls.Add(this.rbCSharp4);
            this.groupBox1.Controls.Add(this.rbCSharp2);
            this.groupBox1.Location = new System.Drawing.Point(241, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(730, 44);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "代码类型";
            // 
            // rbJava
            // 
            this.rbJava.AutoSize = true;
            this.rbJava.Location = new System.Drawing.Point(228, 20);
            this.rbJava.Name = "rbJava";
            this.rbJava.Size = new System.Drawing.Size(47, 16);
            this.rbJava.TabIndex = 8;
            this.rbJava.Tag = "Code";
            this.rbJava.Text = "Java";
            this.rbJava.UseVisualStyleBackColor = true;
            this.rbJava.CheckedChanged += new System.EventHandler(this.rbJava_CheckedChanged);
            // 
            // rbDooPHP
            // 
            this.rbDooPHP.AutoSize = true;
            this.rbDooPHP.Location = new System.Drawing.Point(163, 20);
            this.rbDooPHP.Name = "rbDooPHP";
            this.rbDooPHP.Size = new System.Drawing.Size(59, 16);
            this.rbDooPHP.TabIndex = 7;
            this.rbDooPHP.Tag = "Code";
            this.rbDooPHP.Text = "DooPHP";
            this.rbDooPHP.UseVisualStyleBackColor = true;
            this.rbDooPHP.CheckedChanged += new System.EventHandler(this.rbDooPHP_CheckedChanged);
            // 
            // rbCSharp4
            // 
            this.rbCSharp4.AutoSize = true;
            this.rbCSharp4.Checked = true;
            this.rbCSharp4.Location = new System.Drawing.Point(86, 20);
            this.rbCSharp4.Name = "rbCSharp4";
            this.rbCSharp4.Size = new System.Drawing.Size(77, 16);
            this.rbCSharp4.TabIndex = 6;
            this.rbCSharp4.TabStop = true;
            this.rbCSharp4.Tag = "Code";
            this.rbCSharp4.Text = "C#（4.0）";
            this.rbCSharp4.UseVisualStyleBackColor = true;
            this.rbCSharp4.CheckedChanged += new System.EventHandler(this.rbCSharp4_CheckedChanged);
            // 
            // MainForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 566);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCreateDAL);
            this.Controls.Add(this.btnCreateModel);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataTables);
            this.Controls.Add(this.mainMenuStrips);
            this.MainMenuStrip = this.mainMenuStrips;
            this.Name = "MainForm";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成器";
            this.mainMenuStrips.ResumeLayout(false);
            this.mainMenuStrips.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrips;
        private System.Windows.Forms.ToolStripMenuItem topMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem topMenuItem2;
        private System.Windows.Forms.TreeView dataTables;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.RadioButton rbCSharp2;
        private System.Windows.Forms.Button btnCreateModel;
        private System.Windows.Forms.Button btnCreateDAL;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbCSharp4;
        private System.Windows.Forms.RadioButton rbDooPHP;
        private System.Windows.Forms.RadioButton rbJava;
    }
}

