namespace Mms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.showDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.DebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
      this.textBoxMailer = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxMail = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.buttonSend = new System.Windows.Forms.Button();
      this.listViewStat = new System.Windows.Forms.ListView();
      this.columnHeaderAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeaderStat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.buttonStop = new System.Windows.Forms.Button();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // timer1
      // 
      this.timer1.Enabled = true;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // notifyIcon1
      // 
      resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
      this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
      // 
      // showDebugToolStripMenuItem
      // 
      this.showDebugToolStripMenuItem.Name = "showDebugToolStripMenuItem";
      resources.ApplyResources(this.showDebugToolStripMenuItem, "showDebugToolStripMenuItem");
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DebugToolStripMenuItem});
      resources.ApplyResources(this.menuStrip1, "menuStrip1");
      this.menuStrip1.Name = "menuStrip1";
      // 
      // DebugToolStripMenuItem
      // 
      this.DebugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
      this.DebugToolStripMenuItem.Name = "DebugToolStripMenuItem";
      resources.ApplyResources(this.DebugToolStripMenuItem, "DebugToolStripMenuItem");
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
      this.toolStripMenuItem1.Click += new System.EventHandler(this.showDebugToolStripMenuItem_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
      this.toolStripMenuItem2.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // textBoxMailer
      // 
      resources.ApplyResources(this.textBoxMailer, "textBoxMailer");
      this.textBoxMailer.Name = "textBoxMailer";
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // textBoxMail
      // 
      resources.ApplyResources(this.textBoxMail, "textBoxMail");
      this.textBoxMail.Name = "textBoxMail";
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // buttonSend
      // 
      resources.ApplyResources(this.buttonSend, "buttonSend");
      this.buttonSend.Name = "buttonSend";
      this.buttonSend.UseVisualStyleBackColor = true;
      this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
      // 
      // listViewStat
      // 
      this.listViewStat.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAddr,
            this.columnHeaderStat});
      this.listViewStat.GridLines = true;
      this.listViewStat.HideSelection = false;
      resources.ApplyResources(this.listViewStat, "listViewStat");
      this.listViewStat.Name = "listViewStat";
      this.listViewStat.UseCompatibleStateImageBehavior = false;
      this.listViewStat.View = System.Windows.Forms.View.Details;
      // 
      // columnHeaderAddr
      // 
      resources.ApplyResources(this.columnHeaderAddr, "columnHeaderAddr");
      // 
      // columnHeaderStat
      // 
      resources.ApplyResources(this.columnHeaderStat, "columnHeaderStat");
      // 
      // buttonStop
      // 
      resources.ApplyResources(this.buttonStop, "buttonStop");
      this.buttonStop.Name = "buttonStop";
      this.buttonStop.UseVisualStyleBackColor = true;
      this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
      // 
      // Form1
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.buttonStop);
      this.Controls.Add(this.listViewStat);
      this.Controls.Add(this.buttonSend);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.textBoxMail);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBoxMailer);
      this.Controls.Add(this.menuStrip1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "Form1";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem showDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    private System.Windows.Forms.TextBox textBoxMailer;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxMail;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button buttonSend;
    private System.Windows.Forms.ListView listViewStat;
    private System.Windows.Forms.ColumnHeader columnHeaderAddr;
    private System.Windows.Forms.ColumnHeader columnHeaderStat;
    private System.Windows.Forms.Button buttonStop;
  }
}

