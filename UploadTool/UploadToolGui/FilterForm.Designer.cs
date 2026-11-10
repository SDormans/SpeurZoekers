namespace UploadToolGui
{
    partial class FilterForm
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
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.MenuOk = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReset = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuConcatenation = new System.Windows.Forms.ToolStripComboBox();
            this.MenuSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuSort = new System.Windows.Forms.ToolStripLabel();
            this.MenuSortProp = new System.Windows.Forms.ToolStripComboBox();
            this.MenuSortDir = new System.Windows.Forms.ToolStripComboBox();
            this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Menu
            // 
            this.Menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOk,
            this.MenuReset,
            this.MenuConcatenation,
            this.MenuSeparator0,
            this.MenuAdd,
            this.MenuSeparator1,
            this.MenuSort,
            this.MenuSortProp,
            this.MenuSortDir});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(592, 32);
            this.Menu.TabIndex = 1;
            this.Menu.Text = "menuStrip1";
            // 
            // MenuOk
            // 
            this.MenuOk.Name = "MenuOk";
            this.MenuOk.Size = new System.Drawing.Size(43, 28);
            this.MenuOk.Text = "OK";
            this.MenuOk.Click += new System.EventHandler(this.MenuOk_Click);
            // 
            // MenuReset
            // 
            this.MenuReset.Name = "MenuReset";
            this.MenuReset.Size = new System.Drawing.Size(59, 28);
            this.MenuReset.Text = "Reset";
            this.MenuReset.Click += new System.EventHandler(this.MenuReset_Click);
            // 
            // MenuConcatenation
            // 
            this.MenuConcatenation.Items.AddRange(new object[] {
            "En",
            "Of"});
            this.MenuConcatenation.Name = "MenuConcatenation";
            this.MenuConcatenation.Size = new System.Drawing.Size(75, 28);
            this.MenuConcatenation.SelectedIndexChanged += new System.EventHandler(this.Menu_SelectedIndexChanged);
            // 
            // MenuSeparator0
            // 
            this.MenuSeparator0.Name = "MenuSeparator0";
            this.MenuSeparator0.Size = new System.Drawing.Size(6, 28);
            // 
            // MenuAdd
            // 
            this.MenuAdd.Name = "MenuAdd";
            this.MenuAdd.Size = new System.Drawing.Size(83, 28);
            this.MenuAdd.Text = "Voeg toe";
            this.MenuAdd.Click += new System.EventHandler(this.MenuAdd_Click);
            // 
            // MenuSeparator1
            // 
            this.MenuSeparator1.Name = "MenuSeparator1";
            this.MenuSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // MenuSort
            // 
            this.MenuSort.Name = "MenuSort";
            this.MenuSort.Size = new System.Drawing.Size(57, 25);
            this.MenuSort.Text = "Sorteer";
            // 
            // MenuSortProp
            // 
            this.MenuSortProp.Name = "MenuSortProp";
            this.MenuSortProp.Size = new System.Drawing.Size(170, 28);
            this.MenuSortProp.SelectedIndexChanged += new System.EventHandler(this.Menu_SelectedIndexChanged);
            // 
            // MenuSortDir
            // 
            this.MenuSortDir.Items.AddRange(new object[] {
            "Op",
            "Neer"});
            this.MenuSortDir.Name = "MenuSortDir";
            this.MenuSortDir.Size = new System.Drawing.Size(75, 28);
            this.MenuSortDir.SelectedIndexChanged += new System.EventHandler(this.Menu_SelectedIndexChanged);
            // 
            // MainPanel
            // 
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.MainPanel.Location = new System.Drawing.Point(0, 32);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(592, 271);
            this.MainPanel.TabIndex = 2;
            // 
            // FilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 303);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.Menu);
            this.MinimumSize = new System.Drawing.Size(610, 350);
            this.Name = "FilterForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FilterForm_Load);
            this.SizeChanged += new System.EventHandler(this.FilterForm_SizeChanged);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem MenuOk;
        private System.Windows.Forms.ToolStripMenuItem MenuReset;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator0;
        private System.Windows.Forms.ToolStripComboBox MenuConcatenation;
        private System.Windows.Forms.ToolStripMenuItem MenuAdd;
        private System.Windows.Forms.ToolStripSeparator MenuSeparator1;
        private System.Windows.Forms.ToolStripLabel MenuSort;
        private System.Windows.Forms.ToolStripComboBox MenuSortProp;
        private System.Windows.Forms.ToolStripComboBox MenuSortDir;
        private System.Windows.Forms.FlowLayoutPanel MainPanel;
    }
}