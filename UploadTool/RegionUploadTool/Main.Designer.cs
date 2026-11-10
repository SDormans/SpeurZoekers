using System;
using System.Windows.Forms;

namespace RegionUploadTool
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.RegionList = new System.Windows.Forms.TreeView();
            this.MaxPositions = new System.Windows.Forms.NumericUpDown();
            this.MinPositions = new System.Windows.Forms.NumericUpDown();
            this.UrlLabel = new System.Windows.Forms.Label();
            this.BiotopeSelection = new System.Windows.Forms.ComboBox();
            this.lbMinMax = new System.Windows.Forms.Label();
            this.lbBiotope = new System.Windows.Forms.Label();
            this.NameInput = new System.Windows.Forms.TextBox();
            this.lbName = new System.Windows.Forms.Label();
            this.OpenAdvancedDialog = new System.Windows.Forms.Button();
            this.RemoveRegion = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.DetailsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BrowserPanel = new System.Windows.Forms.Panel();
            this.MainLayout = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.MaxPositions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinPositions)).BeginInit();
            this.DetailsPanel.SuspendLayout();
            this.BrowserPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainLayout)).BeginInit();
            this.MainLayout.Panel1.SuspendLayout();
            this.MainLayout.Panel2.SuspendLayout();
            this.MainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // RegionList
            // 
            this.RegionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RegionList.HideSelection = false;
            this.RegionList.ItemHeight = 20;
            this.RegionList.Location = new System.Drawing.Point(0, 0);
            this.RegionList.Name = "RegionList";
            this.RegionList.Size = new System.Drawing.Size(145, 436);
            this.RegionList.TabIndex = 0;
            this.RegionList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.RegionList_AfterSelect);
            // 
            // MaxPositions
            // 
            this.MaxPositions.Location = new System.Drawing.Point(243, 74);
            this.MaxPositions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaxPositions.Minimum = 0;
            this.MaxPositions.Maximum = int.MaxValue;
            this.MaxPositions.Name = "MaxPositions";
            this.MaxPositions.Size = new System.Drawing.Size(120, 27);
            this.MaxPositions.TabIndex = 4;
            this.MaxPositions.ValueChanged += new System.EventHandler(this.MaxPositions_ValueChanged);
            // 
            // MinPositions
            // 
            this.MinPositions.Location = new System.Drawing.Point(113, 74);
            this.MinPositions.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinPositions.Minimum = 0;
            this.MinPositions.Maximum = int.MaxValue;
            this.MinPositions.Name = "MinPositions";
            this.MinPositions.Size = new System.Drawing.Size(120, 27);
            this.MinPositions.TabIndex = 3;
            this.MinPositions.ValueChanged += new System.EventHandler(this.MinPositions_ValueChanged);
            // 
            // UrlLabel
            // 
            this.UrlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UrlLabel.AutoSize = true;
            this.UrlLabel.BackColor = System.Drawing.SystemColors.Control;
            this.UrlLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UrlLabel.Location = new System.Drawing.Point(0, 323);
            this.UrlLabel.Name = "UrlLabel";
            this.UrlLabel.Size = new System.Drawing.Size(28, 15);
            this.UrlLabel.TabIndex = 8;
            this.UrlLabel.TabStop = true;
            this.UrlLabel.Text = "URL";
            // 
            // BiotopeSelection
            // 
            this.DetailsPanel.SetColumnSpan(this.BiotopeSelection, 2);
            this.BiotopeSelection.FormattingEnabled = true;
            this.BiotopeSelection.Location = new System.Drawing.Point(113, 38);
            this.BiotopeSelection.Name = "BiotopeSelection";
            this.BiotopeSelection.Size = new System.Drawing.Size(250, 28);
            this.BiotopeSelection.TabIndex = 2;
            this.BiotopeSelection.SelectedIndexChanged += new System.EventHandler(this.BiotopeSelection_SelectedIndexChanged);
            // 
            // lbMinMax
            // 
            this.lbMinMax.Location = new System.Drawing.Point(3, 77);
            this.lbMinMax.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lbMinMax.Name = "lbMinMax";
            this.lbMinMax.Size = new System.Drawing.Size(104, 20);
            this.lbMinMax.TabIndex = 6;
            this.lbMinMax.Text = "Min/Max";
            // 
            // lbBiotope
            // 
            this.lbBiotope.Location = new System.Drawing.Point(3, 42);
            this.lbBiotope.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lbBiotope.Name = "lbBiotope";
            this.lbBiotope.Size = new System.Drawing.Size(104, 20);
            this.lbBiotope.TabIndex = 6;
            this.lbBiotope.Text = "Biotoop";
            // 
            // NameInput
            // 
            this.DetailsPanel.SetColumnSpan(this.NameInput, 2);
            this.NameInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameInput.Location = new System.Drawing.Point(113, 3);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(254, 27);
            this.NameInput.TabIndex = 1;
            this.NameInput.TextChanged += new System.EventHandler(this.NameInput_TextChanged);
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(3, 7);
            this.lbName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(104, 20);
            this.lbName.TabIndex = 3;
            this.lbName.Text = "Naam";
            // 
            // OpenAdvancedDialog
            // 
            this.OpenAdvancedDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenAdvancedDialog.Location = new System.Drawing.Point(403, 73);
            this.OpenAdvancedDialog.Name = "OpenAdvancedDialog";
            this.OpenAdvancedDialog.Size = new System.Drawing.Size(94, 29);
            this.OpenAdvancedDialog.TabIndex = 7;
            this.OpenAdvancedDialog.Text = "Meer...";
            this.OpenAdvancedDialog.UseVisualStyleBackColor = true;
            this.OpenAdvancedDialog.Click += new System.EventHandler(this.OpenAdvancedDialog_Click);
            // 
            // RemoveRegion
            // 
            this.RemoveRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveRegion.Location = new System.Drawing.Point(403, 38);
            this.RemoveRegion.Name = "RemoveRegion";
            this.RemoveRegion.Size = new System.Drawing.Size(94, 29);
            this.RemoveRegion.TabIndex = 6;
            this.RemoveRegion.Text = "Verwijder";
            this.RemoveRegion.UseVisualStyleBackColor = true;
            this.RemoveRegion.Click += new System.EventHandler(this.RemoveRegion_Click);
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Location = new System.Drawing.Point(403, 3);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(94, 29);
            this.Save.TabIndex = 5;
            this.Save.Text = "Sla op";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Browser
            // 
            this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Browser.Location = new System.Drawing.Point(0, 0);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(494, 339);
            this.Browser.TabIndex = 9;
            this.Browser.Navigating += Browser_Navigating;
            // 
            // DetailsPanel
            // 
            this.DetailsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.DetailsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.DetailsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DetailsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.DetailsPanel.Controls.Add(this.lbName, 0, 0);
            this.DetailsPanel.Controls.Add(this.NameInput, 1, 0);
            this.DetailsPanel.Controls.Add(this.lbBiotope, 0, 1);
            this.DetailsPanel.Controls.Add(this.BiotopeSelection, 1, 1);
            this.DetailsPanel.Controls.Add(this.lbMinMax, 0, 2);
            this.DetailsPanel.Controls.Add(this.MinPositions, 1, 2);
            this.DetailsPanel.Controls.Add(this.MaxPositions, 2, 2);
            this.DetailsPanel.Controls.Add(this.BrowserPanel, 0, 3);
            this.DetailsPanel.Controls.Add(this.Save, 3, 0);
            this.DetailsPanel.Controls.Add(this.RemoveRegion, 3, 1);
            this.DetailsPanel.Controls.Add(this.OpenAdvancedDialog, 3, 2);
            this.DetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailsPanel.Location = new System.Drawing.Point(0, 0);
            this.DetailsPanel.MinimumSize = new System.Drawing.Size(500, 450);
            this.DetailsPanel.Name = "DetailsPanel";
            this.DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.DetailsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DetailsPanel.Size = new System.Drawing.Size(500, 450);
            this.DetailsPanel.TabIndex = 2;
            // 
            // BrowserPanel
            // 
            this.DetailsPanel.SetColumnSpan(this.BrowserPanel, 4);
            this.BrowserPanel.Controls.Add(this.UrlLabel);
            this.BrowserPanel.Controls.Add(this.Browser);
            this.BrowserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowserPanel.Location = new System.Drawing.Point(3, 108);
            this.BrowserPanel.Name = "BrowserPanel";
            this.BrowserPanel.Size = new System.Drawing.Size(494, 339);
            this.BrowserPanel.TabIndex = 10;
            // 
            // MainLayout
            // 
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            // 
            // MainLayout.Panel1
            // 
            this.MainLayout.Panel1.Controls.Add(this.RegionList);
            // 
            // MainLayout.Panel2
            // 
            this.MainLayout.Panel2.Controls.Add(this.DetailsPanel);
            this.MainLayout.Panel2MinSize = 500;
            this.MainLayout.Size = new System.Drawing.Size(649, 436);
            this.MainLayout.SplitterDistance = 145;
            this.MainLayout.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 436);
            this.Controls.Add(this.MainLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(650, 450);
            this.Name = "Main";
            this.Text = "De Speurzoekers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MaxPositions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinPositions)).EndInit();
            this.DetailsPanel.ResumeLayout(false);
            this.DetailsPanel.PerformLayout();
            this.BrowserPanel.ResumeLayout(false);
            this.BrowserPanel.PerformLayout();
            this.MainLayout.Panel1.ResumeLayout(false);
            this.MainLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainLayout)).EndInit();
            this.MainLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer MainLayout;
        private System.Windows.Forms.TreeView RegionList;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.ComboBox BiotopeSelection;
        private System.Windows.Forms.Label lbBiotope;
        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.Button RemoveRegion;
        private System.Windows.Forms.Label UrlLabel;
        private System.Windows.Forms.Button OpenAdvancedDialog;
        private System.Windows.Forms.Label lbMinMax;
        private System.Windows.Forms.NumericUpDown MaxPositions;
        private System.Windows.Forms.NumericUpDown MinPositions;
        private System.Windows.Forms.TableLayoutPanel DetailsPanel;
        private System.Windows.Forms.WebBrowser Browser;
        private Panel BrowserPanel;
    }
}

