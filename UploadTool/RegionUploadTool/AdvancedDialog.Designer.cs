namespace RegionUploadTool
{
    partial class AdvancedDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedDialog));
            this.lbParent = new System.Windows.Forms.Label();
            this.Coordinates = new System.Windows.Forms.TextBox();
            this.lbCoordinates = new System.Windows.Forms.Label();
            this.ParentSelector = new System.Windows.Forms.ComboBox();
            this.DialogBack = new System.Windows.Forms.Button();
            this.NewChild = new System.Windows.Forms.Button();
            this.FormatCoordinates = new System.Windows.Forms.Button();
            this.CoordinatesOrder = new System.Windows.Forms.ComboBox();
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.MainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbParent
            //
            this.lbParent.Location = new System.Drawing.Point(3, 7);
            this.lbParent.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lbParent.Name = "lbParent";
            this.lbParent.Size = new System.Drawing.Size(84, 20);
            this.lbParent.TabIndex = 0;
            this.lbParent.Text = "Omliggend";
            // 
            // Coordinates
            // 
            this.Coordinates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Coordinates.Location = new System.Drawing.Point(113, 38);
            this.Coordinates.Multiline = true;
            this.Coordinates.Name = "Coordinates";
            this.MainLayout.SetRowSpan(this.Coordinates, 4);
            this.Coordinates.Size = new System.Drawing.Size(194, 216);
            this.Coordinates.TabIndex = 1;
            // 
            // lbCoordinates
            //
            this.lbCoordinates.Location = new System.Drawing.Point(3, 42);
            this.lbCoordinates.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lbCoordinates.Name = "lbCoordinates";
            this.lbCoordinates.Size = new System.Drawing.Size(91, 20);
            this.lbCoordinates.TabIndex = 2;
            this.lbCoordinates.Text = "Coördinaten";
            // 
            // ParentSelector
            // 
            this.ParentSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParentSelector.FormattingEnabled = true;
            this.ParentSelector.Location = new System.Drawing.Point(113, 3);
            this.ParentSelector.Name = "ParentSelector";
            this.ParentSelector.Size = new System.Drawing.Size(194, 28);
            this.ParentSelector.TabIndex = 3;
            // 
            // DialogBack
            // 
            this.DialogBack.Location = new System.Drawing.Point(313, 3);
            this.DialogBack.Name = "DialogBack";
            this.DialogBack.Size = new System.Drawing.Size(94, 29);
            this.DialogBack.TabIndex = 5;
            this.DialogBack.Text = "OK";
            this.DialogBack.UseVisualStyleBackColor = true;
            this.DialogBack.Click += new System.EventHandler(this.DialogBack_Click);
            // 
            // NewChild
            // 
            this.NewChild.Location = new System.Drawing.Point(313, 108);
            this.NewChild.Name = "NewChild";
            this.NewChild.Size = new System.Drawing.Size(94, 29);
            this.NewChild.TabIndex = 5;
            this.NewChild.Text = "Nieuw";
            this.NewChild.UseVisualStyleBackColor = true;
            this.NewChild.Click += new System.EventHandler(this.NewChild_Click);
            // 
            // FormatCoordinates
            // 
            this.FormatCoordinates.Location = new System.Drawing.Point(313, 73);
            this.FormatCoordinates.Name = "FormatCoordinates";
            this.FormatCoordinates.Size = new System.Drawing.Size(94, 29);
            this.FormatCoordinates.TabIndex = 5;
            this.FormatCoordinates.Text = "Lijn uit";
            this.FormatCoordinates.UseVisualStyleBackColor = true;
            this.FormatCoordinates.Click += new System.EventHandler(this.FormatCoordinates_Click);
            // 
            // CoordinatesOrder
            // 
            this.CoordinatesOrder.FormattingEnabled = true;
            this.CoordinatesOrder.Items.AddRange(new object[] {
            "Long, lat",
            "Lat, long"});
            this.CoordinatesOrder.Location = new System.Drawing.Point(313, 38);
            this.CoordinatesOrder.Name = "CoordinatesOrder";
            this.CoordinatesOrder.Size = new System.Drawing.Size(94, 28);
            this.CoordinatesOrder.TabIndex = 6;
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 3;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.MainLayout.Controls.Add(this.lbParent, 0, 0);
            this.MainLayout.Controls.Add(this.Coordinates, 1, 1);
            this.MainLayout.Controls.Add(this.ParentSelector, 1, 0);
            this.MainLayout.Controls.Add(this.FormatCoordinates, 2, 2);
            this.MainLayout.Controls.Add(this.CoordinatesOrder, 2, 1);
            this.MainLayout.Controls.Add(this.lbCoordinates, 0, 1);
            this.MainLayout.Controls.Add(this.DialogBack, 2, 0);
            this.MainLayout.Controls.Add(this.NewChild, 2, 3);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 5;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
            this.MainLayout.Size = new System.Drawing.Size(413, 257);
            this.MainLayout.TabIndex = 7;
            // 
            // AdvancedDialog
            // 
            this.ClientSize = new System.Drawing.Size(413, 257);
            this.Controls.Add(this.MainLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "AdvancedDialog";
            this.Text = "Geadvanceerde opties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdvancedDialog_FormClosing);
            this.Load += new System.EventHandler(this.AdvancedDialog_Load);
            this.MainLayout.ResumeLayout(false);
            this.MainLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbParent;
        private System.Windows.Forms.TextBox Coordinates;
        private System.Windows.Forms.Label lbCoordinates;
        private System.Windows.Forms.ComboBox ParentSelector;
        private System.Windows.Forms.Button DialogBack;
        private System.Windows.Forms.Button NewChild;
        private System.Windows.Forms.Button FormatCoordinates;
        private System.Windows.Forms.ComboBox CoordinatesOrder;
        private System.Windows.Forms.TableLayoutPanel MainLayout;
    }
}