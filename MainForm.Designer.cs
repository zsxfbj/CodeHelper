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
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 349);
            this.Name = "MainForm";
            this.ResumeLayout(false);

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
        private System.Windows.Forms.RadioButton rbJava;
        private System.Windows.Forms.Button button1;
    }
}

