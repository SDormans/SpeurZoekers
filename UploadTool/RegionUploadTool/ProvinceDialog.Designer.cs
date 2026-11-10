namespace RegionUploadTool
{
    partial class ProvinceListDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProvinceListDialog));
            this.ProvinceList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ProvinceList
            // 
            this.ProvinceList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ProvinceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProvinceList.ItemHeight = 20;
            this.ProvinceList.Location = new System.Drawing.Point(0, 0);
            this.ProvinceList.Name = "ProvinceList";
            this.ProvinceList.Size = new System.Drawing.Size(800, 450);
            this.ProvinceList.TabIndex = 0;
            // 
            // ProvinceListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ProvinceList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProvinceListDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Kies locatie";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ProvinceList;
    }
}