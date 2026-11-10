namespace UploadToolGui
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ItemList = new System.Windows.Forms.ListBox();
            this.TabPicture = new System.Windows.Forms.TabPage();
            this.PicturePanel = new System.Windows.Forms.TableLayoutPanel();
            this.PictureSize = new System.Windows.Forms.ComboBox();
            this.PictureName = new System.Windows.Forms.TextBox();
            this.OpenPath = new System.Windows.Forms.Button();
            this.PictureInnerPanel = new System.Windows.Forms.Panel();
            this.PlaceHolder = new System.Windows.Forms.Label();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.MainLayout = new System.Windows.Forms.SplitContainer();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabMain = new System.Windows.Forms.TabPage();
            this.MainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lbName = new System.Windows.Forms.Label();
            this.NameInput = new System.Windows.Forms.TextBox();
            this.lbRarity = new System.Windows.Forms.Label();
            this.RaritySelector = new System.Windows.Forms.ComboBox();
            this.lbPoints = new System.Windows.Forms.Label();
            this.PointsSelector = new System.Windows.Forms.NumericUpDown();
            this.lbTextPage = new System.Windows.Forms.Label();
            this.PageSelector = new System.Windows.Forms.ComboBox();
            this.FormattingTools = new System.Windows.Forms.FlowLayoutPanel();
            this.ApplyBold = new System.Windows.Forms.Button();
            this.ApplyItalic = new System.Windows.Forms.Button();
            this.ApplyStrikeOut = new System.Windows.Forms.Button();
            this.ApplyHeader = new System.Windows.Forms.Button();
            this.AddLink = new System.Windows.Forms.Button();
            this.lbList = new System.Windows.Forms.Label();
            this.ApplyUnsortedList = new System.Windows.Forms.Button();
            this.ApplyNumList = new System.Windows.Forms.Button();
            this.lbPaste = new System.Windows.Forms.Label();
            this.PasteWord = new System.Windows.Forms.Button();
            this.PasteHtml = new System.Windows.Forms.Button();
            this.TextBox = new System.Windows.Forms.RichTextBox();
            this.TabQuestion = new System.Windows.Forms.TabPage();
            this.QuestionPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lbQuestion = new System.Windows.Forms.Label();
            this.QuestionInput = new System.Windows.Forms.TextBox();
            this.lbQuestionPoints = new System.Windows.Forms.Label();
            this.QuestionPointsSelector = new System.Windows.Forms.NumericUpDown();
            this.lbChoices = new System.Windows.Forms.Label();
            this.lbIsAnswer = new System.Windows.Forms.Label();
            this.Choice0 = new System.Windows.Forms.TextBox();
            this.Choice1 = new System.Windows.Forms.TextBox();
            this.Choice2 = new System.Windows.Forms.TextBox();
            this.Choice3 = new System.Windows.Forms.TextBox();
            this.Choice4 = new System.Windows.Forms.TextBox();
            this.Answer0 = new System.Windows.Forms.CheckBox();
            this.Answer1 = new System.Windows.Forms.CheckBox();
            this.Answer2 = new System.Windows.Forms.CheckBox();
            this.Answer3 = new System.Windows.Forms.CheckBox();
            this.Answer4 = new System.Windows.Forms.CheckBox();
            this.TabAvailability = new System.Windows.Forms.TabPage();
            this.AvailibiltyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lbAvList0 = new System.Windows.Forms.Label();
            this.ToggleAvList0 = new System.Windows.Forms.Button();
            this.AvList0 = new System.Windows.Forms.CheckedListBox();
            this.lbAvList1 = new System.Windows.Forms.Label();
            this.ToggleAvList1 = new System.Windows.Forms.Button();
            this.AvList1 = new System.Windows.Forms.CheckedListBox();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.MenuMode = new System.Windows.Forms.ToolStripComboBox();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.TabPicture.SuspendLayout();
            this.PicturePanel.SuspendLayout();
            this.PictureInnerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainLayout)).BeginInit();
            this.MainLayout.Panel1.SuspendLayout();
            this.MainLayout.Panel2.SuspendLayout();
            this.MainLayout.SuspendLayout();
            this.Tabs.SuspendLayout();
            this.TabMain.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PointsSelector)).BeginInit();
            this.FormattingTools.SuspendLayout();
            this.TabQuestion.SuspendLayout();
            this.QuestionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QuestionPointsSelector)).BeginInit();
            this.TabAvailability.SuspendLayout();
            this.AvailibiltyPanel.SuspendLayout();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemList
            // 
            this.ItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemList.FormattingEnabled = true;
            this.ItemList.ItemHeight = 20;
            this.ItemList.Location = new System.Drawing.Point(0, 0);
            this.ItemList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ItemList.Name = "ItemList";
            this.ItemList.Size = new System.Drawing.Size(199, 436);
            this.ItemList.TabIndex = 0;
            this.ItemList.SelectedIndexChanged += new System.EventHandler(this.ItemList_SelectedIndexChanged);
            // 
            // TabPicture
            // 
            this.TabPicture.Controls.Add(this.PicturePanel);
            this.TabPicture.Location = new System.Drawing.Point(4, 29);
            this.TabPicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabPicture.Name = "TabPicture";
            this.TabPicture.Size = new System.Drawing.Size(581, 405);
            this.TabPicture.TabIndex = 3;
            this.TabPicture.Text = "Plaatje";
            // 
            // PicturePanel
            // 
            this.PicturePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.PicturePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PicturePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.PicturePanel.Controls.Add(this.PictureSize, 0, 0);
            this.PicturePanel.Controls.Add(this.PictureName, 1, 0);
            this.PicturePanel.Controls.Add(this.OpenPath, 2, 0);
            this.PicturePanel.Controls.Add(this.PictureInnerPanel, 0, 1);
            this.PicturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PicturePanel.Location = new System.Drawing.Point(0, 0);
            this.PicturePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PicturePanel.Name = "PicturePanel";
            this.PicturePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PicturePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PicturePanel.Size = new System.Drawing.Size(581, 405);
            this.PicturePanel.TabIndex = 0;
            // 
            // PictureSize
            // 
            this.PictureSize.FormattingEnabled = true;
            this.PictureSize.Location = new System.Drawing.Point(3, 2);
            this.PictureSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PictureSize.Name = "PictureSize";
            this.PictureSize.Size = new System.Drawing.Size(103, 28);
            this.PictureSize.TabIndex = 2;
            this.PictureSize.Text = "Klein";
            // 
            // PictureName
            // 
            this.PictureName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureName.Location = new System.Drawing.Point(117, 2);
            this.PictureName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PictureName.Name = "PictureName";
            this.PictureName.Size = new System.Drawing.Size(360, 27);
            this.PictureName.TabIndex = 3;
            // 
            // OpenPath
            // 
            this.OpenPath.Location = new System.Drawing.Point(483, 2);
            this.OpenPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OpenPath.Name = "OpenPath";
            this.OpenPath.Size = new System.Drawing.Size(94, 30);
            this.OpenPath.TabIndex = 4;
            this.OpenPath.Text = "Open";
            this.OpenPath.UseVisualStyleBackColor = true;
            // 
            // PictureInnerPanel
            // 
            this.PictureInnerPanel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PicturePanel.SetColumnSpan(this.PictureInnerPanel, 3);
            this.PictureInnerPanel.Controls.Add(this.PlaceHolder);
            this.PictureInnerPanel.Controls.Add(this.PictureBox);
            this.PictureInnerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureInnerPanel.Location = new System.Drawing.Point(3, 36);
            this.PictureInnerPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PictureInnerPanel.Name = "PictureInnerPanel";
            this.PictureInnerPanel.Size = new System.Drawing.Size(575, 367);
            this.PictureInnerPanel.TabIndex = 5;
            // 
            // PlaceHolder
            // 
            this.PlaceHolder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PlaceHolder.AutoSize = true;
            this.PlaceHolder.BackColor = System.Drawing.Color.White;
            this.PlaceHolder.Location = new System.Drawing.Point(232, 166);
            this.PlaceHolder.Name = "PlaceHolder";
            this.PlaceHolder.Size = new System.Drawing.Size(96, 20);
            this.PlaceHolder.TabIndex = 0;
            this.PlaceHolder.Text = "Bezig laden...";
            // 
            // PictureBox
            // 
            this.PictureBox.Location = new System.Drawing.Point(175, 66);
            this.PictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(250, 250);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PictureBox.TabIndex = 1;
            this.PictureBox.TabStop = false;
            // 
            // MainLayout
            // 
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 32);
            this.MainLayout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MainLayout.Name = "MainLayout";
            // 
            // MainLayout.Panel1
            // 
            this.MainLayout.Panel1.Controls.Add(this.ItemList);
            // 
            // MainLayout.Panel2
            // 
            this.MainLayout.Panel2.Controls.Add(this.Tabs);
            this.MainLayout.Panel2MinSize = 500;
            this.MainLayout.Size = new System.Drawing.Size(793, 436);
            this.MainLayout.SplitterDistance = 199;
            this.MainLayout.SplitterWidth = 5;
            this.MainLayout.TabIndex = 0;
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.TabMain);
            this.Tabs.Controls.Add(this.TabQuestion);
            this.Tabs.Controls.Add(this.TabAvailability);
            this.Tabs.Controls.Add(this.TabPicture);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 2;
            this.Tabs.Size = new System.Drawing.Size(589, 436);
            this.Tabs.TabIndex = 1;
            // 
            // TabMain
            // 
            this.TabMain.Controls.Add(this.MainPanel);
            this.TabMain.Location = new System.Drawing.Point(4, 29);
            this.TabMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabMain.Name = "TabMain";
            this.TabMain.Size = new System.Drawing.Size(581, 403);
            this.TabMain.TabIndex = 0;
            this.TabMain.Text = "Tekst";
            // 
            // MainPanel
            // 
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.MainPanel.Controls.Add(this.lbName, 0, 0);
            this.MainPanel.Controls.Add(this.NameInput, 1, 0);
            this.MainPanel.Controls.Add(this.lbRarity, 0, 1);
            this.MainPanel.Controls.Add(this.RaritySelector, 1, 1);
            this.MainPanel.Controls.Add(this.lbPoints, 0, 2);
            this.MainPanel.Controls.Add(this.PointsSelector, 1, 2);
            this.MainPanel.Controls.Add(this.lbTextPage, 0, 4);
            this.MainPanel.Controls.Add(this.PageSelector, 1, 4);
            this.MainPanel.Controls.Add(this.FormattingTools, 0, 5);
            this.MainPanel.Controls.Add(this.TextBox, 0, 6);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.RowCount = 7;
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainPanel.Size = new System.Drawing.Size(581, 403);
            this.MainPanel.TabIndex = 0;
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(3, 6);
            this.lbName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(104, 20);
            this.lbName.TabIndex = 99;
            this.lbName.Text = "Naam";
            // 
            // NameInput
            // 
            this.NameInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameInput.Location = new System.Drawing.Point(117, 2);
            this.NameInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NameInput.MaxLength = 256;
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(360, 27);
            this.NameInput.TabIndex = 2;
            // 
            // lbRarity
            // 
            this.lbRarity.Location = new System.Drawing.Point(3, 38);
            this.lbRarity.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbRarity.Name = "lbRarity";
            this.lbRarity.Size = new System.Drawing.Size(104, 20);
            this.lbRarity.TabIndex = 99;
            this.lbRarity.Text = "Zeldzaamheid";
            // 
            // RaritySelector
            // 
            this.RaritySelector.FormattingEnabled = true;
            this.RaritySelector.Location = new System.Drawing.Point(117, 34);
            this.RaritySelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RaritySelector.Name = "RaritySelector";
            this.RaritySelector.Size = new System.Drawing.Size(251, 28);
            this.RaritySelector.TabIndex = 3;
            // 
            // lbPoints
            // 
            this.lbPoints.Location = new System.Drawing.Point(3, 70);
            this.lbPoints.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbPoints.Name = "lbPoints";
            this.lbPoints.Size = new System.Drawing.Size(104, 20);
            this.lbPoints.TabIndex = 99;
            this.lbPoints.Text = "Punten";
            // 
            // PointsSelector
            // 
            this.PointsSelector.Location = new System.Drawing.Point(117, 66);
            this.PointsSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PointsSelector.Name = "PointsSelector";
            this.PointsSelector.Size = new System.Drawing.Size(249, 27);
            this.PointsSelector.TabIndex = 4;
            // 
            // lbTextPage
            // 
            this.lbTextPage.Location = new System.Drawing.Point(3, 102);
            this.lbTextPage.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbTextPage.Name = "lbTextPage";
            this.lbTextPage.Size = new System.Drawing.Size(104, 20);
            this.lbTextPage.TabIndex = 99;
            this.lbTextPage.Text = "Pagina";
            // 
            // PageSelector
            // 
            this.PageSelector.FormattingEnabled = true;
            this.PageSelector.Location = new System.Drawing.Point(117, 98);
            this.PageSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PageSelector.Name = "PageSelector";
            this.PageSelector.Size = new System.Drawing.Size(251, 28);
            this.PageSelector.TabIndex = 6;
            // 
            // FormattingTools
            // 
            this.MainPanel.SetColumnSpan(this.FormattingTools, 3);
            this.FormattingTools.Controls.Add(this.ApplyBold);
            this.FormattingTools.Controls.Add(this.ApplyItalic);
            this.FormattingTools.Controls.Add(this.ApplyStrikeOut);
            this.FormattingTools.Controls.Add(this.ApplyHeader);
            this.FormattingTools.Controls.Add(this.AddLink);
            this.FormattingTools.Controls.Add(this.lbList);
            this.FormattingTools.Controls.Add(this.ApplyUnsortedList);
            this.FormattingTools.Controls.Add(this.ApplyNumList);
            this.FormattingTools.Controls.Add(this.lbPaste);
            this.FormattingTools.Controls.Add(this.PasteWord);
            this.FormattingTools.Controls.Add(this.PasteHtml);
            this.FormattingTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormattingTools.Location = new System.Drawing.Point(0, 128);
            this.FormattingTools.Margin = new System.Windows.Forms.Padding(0);
            this.FormattingTools.Name = "FormattingTools";
            this.FormattingTools.Size = new System.Drawing.Size(581, 34);
            this.FormattingTools.TabIndex = 7;
            // 
            // ApplyBold
            // 
            this.ApplyBold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ApplyBold.Location = new System.Drawing.Point(3, 2);
            this.ApplyBold.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApplyBold.Name = "ApplyBold";
            this.ApplyBold.Size = new System.Drawing.Size(29, 30);
            this.ApplyBold.TabIndex = 8;
            this.ApplyBold.Text = "B";
            this.ApplyBold.UseVisualStyleBackColor = true;
            // 
            // ApplyItalic
            // 
            this.ApplyItalic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.ApplyItalic.Location = new System.Drawing.Point(38, 2);
            this.ApplyItalic.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApplyItalic.Name = "ApplyItalic";
            this.ApplyItalic.Size = new System.Drawing.Size(29, 30);
            this.ApplyItalic.TabIndex = 9;
            this.ApplyItalic.Text = "I";
            this.ApplyItalic.UseVisualStyleBackColor = true;
            // 
            // ApplyStrikeOut
            // 
            this.ApplyStrikeOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point);
            this.ApplyStrikeOut.Location = new System.Drawing.Point(73, 2);
            this.ApplyStrikeOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApplyStrikeOut.Name = "ApplyStrikeOut";
            this.ApplyStrikeOut.Size = new System.Drawing.Size(29, 30);
            this.ApplyStrikeOut.TabIndex = 10;
            this.ApplyStrikeOut.Text = "S";
            this.ApplyStrikeOut.UseVisualStyleBackColor = true;
            // 
            // ApplyHeader
            // 
            this.ApplyHeader.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.ApplyHeader.Location = new System.Drawing.Point(108, 2);
            this.ApplyHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApplyHeader.Name = "ApplyHeader";
            this.ApplyHeader.Size = new System.Drawing.Size(29, 30);
            this.ApplyHeader.TabIndex = 11;
            this.ApplyHeader.Text = "T";
            this.ApplyHeader.UseVisualStyleBackColor = true;
            // 
            // AddLink
            // 
            this.AddLink.Location = new System.Drawing.Point(143, 2);
            this.AddLink.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AddLink.Name = "AddLink";
            this.AddLink.Size = new System.Drawing.Size(56, 30);
            this.AddLink.TabIndex = 12;
            this.AddLink.Text = "Link";
            this.AddLink.UseVisualStyleBackColor = true;
            // 
            // lbList
            // 
            this.lbList.Location = new System.Drawing.Point(205, 6);
            this.lbList.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbList.Name = "lbList";
            this.lbList.Size = new System.Drawing.Size(39, 22);
            this.lbList.TabIndex = 99;
            this.lbList.Text = "Lijst:";
            // 
            // ApplyUnsortedList
            // 
            this.ApplyUnsortedList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ApplyUnsortedList.Location = new System.Drawing.Point(250, 2);
            this.ApplyUnsortedList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApplyUnsortedList.Name = "ApplyUnsortedList";
            this.ApplyUnsortedList.Size = new System.Drawing.Size(29, 30);
            this.ApplyUnsortedList.TabIndex = 13;
            this.ApplyUnsortedList.Text = "-";
            this.ApplyUnsortedList.UseVisualStyleBackColor = true;
            // 
            // ApplyNumList
            // 
            this.ApplyNumList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ApplyNumList.Location = new System.Drawing.Point(285, 2);
            this.ApplyNumList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ApplyNumList.Name = "ApplyNumList";
            this.ApplyNumList.Size = new System.Drawing.Size(29, 30);
            this.ApplyNumList.TabIndex = 14;
            this.ApplyNumList.Text = "1.";
            this.ApplyNumList.UseVisualStyleBackColor = true;
            // 
            // lbPaste
            // 
            this.lbPaste.Location = new System.Drawing.Point(320, 6);
            this.lbPaste.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbPaste.Name = "lbPaste";
            this.lbPaste.Size = new System.Drawing.Size(40, 22);
            this.lbPaste.TabIndex = 99;
            this.lbPaste.Text = "Plak:";
            // 
            // PasteWord
            // 
            this.PasteWord.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PasteWord.Location = new System.Drawing.Point(366, 2);
            this.PasteWord.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PasteWord.Name = "PasteWord";
            this.PasteWord.Size = new System.Drawing.Size(29, 30);
            this.PasteWord.TabIndex = 13;
            this.PasteWord.Text = "W";
            this.PasteWord.UseVisualStyleBackColor = true;
            // 
            // PasteHtml
            // 
            this.PasteHtml.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PasteHtml.Location = new System.Drawing.Point(401, 2);
            this.PasteHtml.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PasteHtml.Name = "PasteHtml";
            this.PasteHtml.Size = new System.Drawing.Size(29, 30);
            this.PasteHtml.TabIndex = 13;
            this.PasteHtml.Text = "H";
            this.PasteHtml.UseVisualStyleBackColor = true;
            // 
            // TextBox
            // 
            this.MainPanel.SetColumnSpan(this.TextBox, 3);
            this.TextBox.DetectUrls = false;
            this.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox.Location = new System.Drawing.Point(3, 164);
            this.TextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(575, 237);
            this.TextBox.TabIndex = 15;
            this.TextBox.Text = "";
            // 
            // TabQuestion
            // 
            this.TabQuestion.Controls.Add(this.QuestionPanel);
            this.TabQuestion.Location = new System.Drawing.Point(4, 29);
            this.TabQuestion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabQuestion.Name = "TabQuestion";
            this.TabQuestion.Size = new System.Drawing.Size(581, 405);
            this.TabQuestion.TabIndex = 1;
            this.TabQuestion.Text = "Vraag";
            // 
            // QuestionPanel
            // 
            this.QuestionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.QuestionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.QuestionPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.QuestionPanel.Controls.Add(this.lbQuestion, 0, 0);
            this.QuestionPanel.Controls.Add(this.QuestionInput, 1, 0);
            this.QuestionPanel.Controls.Add(this.lbQuestionPoints, 0, 1);
            this.QuestionPanel.Controls.Add(this.QuestionPointsSelector, 1, 1);
            this.QuestionPanel.Controls.Add(this.lbChoices, 0, 2);
            this.QuestionPanel.Controls.Add(this.lbIsAnswer, 2, 2);
            this.QuestionPanel.Controls.Add(this.Choice0, 0, 3);
            this.QuestionPanel.Controls.Add(this.Choice1, 0, 4);
            this.QuestionPanel.Controls.Add(this.Choice2, 0, 5);
            this.QuestionPanel.Controls.Add(this.Choice3, 0, 6);
            this.QuestionPanel.Controls.Add(this.Choice4, 0, 7);
            this.QuestionPanel.Controls.Add(this.Answer0, 2, 3);
            this.QuestionPanel.Controls.Add(this.Answer1, 2, 4);
            this.QuestionPanel.Controls.Add(this.Answer2, 2, 5);
            this.QuestionPanel.Controls.Add(this.Answer3, 2, 6);
            this.QuestionPanel.Controls.Add(this.Answer4, 2, 7);
            this.QuestionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestionPanel.Location = new System.Drawing.Point(0, 0);
            this.QuestionPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.QuestionPanel.Name = "QuestionPanel";
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionPanel.Size = new System.Drawing.Size(581, 405);
            this.QuestionPanel.TabIndex = 0;
            // 
            // lbQuestion
            // 
            this.lbQuestion.Location = new System.Drawing.Point(3, 6);
            this.lbQuestion.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbQuestion.Name = "lbQuestion";
            this.lbQuestion.Size = new System.Drawing.Size(104, 22);
            this.lbQuestion.TabIndex = 99;
            this.lbQuestion.Text = "Vraag";
            // 
            // QuestionInput
            // 
            this.QuestionInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestionInput.Location = new System.Drawing.Point(117, 2);
            this.QuestionInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.QuestionInput.MaxLength = 255;
            this.QuestionInput.Name = "QuestionInput";
            this.QuestionInput.Size = new System.Drawing.Size(360, 27);
            this.QuestionInput.TabIndex = 2;
            // 
            // lbQuestionPoints
            // 
            this.lbQuestionPoints.Location = new System.Drawing.Point(3, 40);
            this.lbQuestionPoints.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbQuestionPoints.Name = "lbQuestionPoints";
            this.lbQuestionPoints.Size = new System.Drawing.Size(104, 22);
            this.lbQuestionPoints.TabIndex = 99;
            this.lbQuestionPoints.Text = "Punten";
            // 
            // QuestionPointsSelector
            // 
            this.QuestionPointsSelector.Location = new System.Drawing.Point(117, 36);
            this.QuestionPointsSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.QuestionPointsSelector.Name = "QuestionPointsSelector";
            this.QuestionPointsSelector.Size = new System.Drawing.Size(249, 27);
            this.QuestionPointsSelector.TabIndex = 3;
            // 
            // lbChoices
            // 
            this.lbChoices.Location = new System.Drawing.Point(3, 74);
            this.lbChoices.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbChoices.Name = "lbChoices";
            this.lbChoices.Size = new System.Drawing.Size(104, 22);
            this.lbChoices.TabIndex = 99;
            this.lbChoices.Text = "Keuzes";
            // 
            // lbIsAnswer
            // 
            this.lbIsAnswer.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbIsAnswer.Location = new System.Drawing.Point(483, 74);
            this.lbIsAnswer.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.lbIsAnswer.Name = "lbIsAnswer";
            this.lbIsAnswer.Size = new System.Drawing.Size(94, 22);
            this.lbIsAnswer.TabIndex = 99;
            this.lbIsAnswer.Text = "Is correct";
            // 
            // Choice0
            // 
            this.QuestionPanel.SetColumnSpan(this.Choice0, 2);
            this.Choice0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Choice0.Location = new System.Drawing.Point(3, 104);
            this.Choice0.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Choice0.MaxLength = 255;
            this.Choice0.Name = "Choice0";
            this.Choice0.Size = new System.Drawing.Size(474, 27);
            this.Choice0.TabIndex = 4;
            // 
            // Choice1
            // 
            this.QuestionPanel.SetColumnSpan(this.Choice1, 2);
            this.Choice1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Choice1.Location = new System.Drawing.Point(3, 138);
            this.Choice1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Choice1.MaxLength = 255;
            this.Choice1.Name = "Choice1";
            this.Choice1.Size = new System.Drawing.Size(474, 27);
            this.Choice1.TabIndex = 6;
            // 
            // Choice2
            // 
            this.QuestionPanel.SetColumnSpan(this.Choice2, 2);
            this.Choice2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Choice2.Location = new System.Drawing.Point(3, 172);
            this.Choice2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Choice2.MaxLength = 255;
            this.Choice2.Name = "Choice2";
            this.Choice2.Size = new System.Drawing.Size(474, 27);
            this.Choice2.TabIndex = 8;
            // 
            // Choice3
            // 
            this.QuestionPanel.SetColumnSpan(this.Choice3, 2);
            this.Choice3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Choice3.Location = new System.Drawing.Point(3, 206);
            this.Choice3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Choice3.MaxLength = 255;
            this.Choice3.Name = "Choice3";
            this.Choice3.Size = new System.Drawing.Size(474, 27);
            this.Choice3.TabIndex = 10;
            // 
            // Choice4
            // 
            this.QuestionPanel.SetColumnSpan(this.Choice4, 2);
            this.Choice4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Choice4.Location = new System.Drawing.Point(3, 240);
            this.Choice4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Choice4.MaxLength = 255;
            this.Choice4.Name = "Choice4";
            this.Choice4.Size = new System.Drawing.Size(474, 27);
            this.Choice4.TabIndex = 12;
            // 
            // Answer0
            // 
            this.Answer0.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Answer0.Location = new System.Drawing.Point(521, 110);
            this.Answer0.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Answer0.Name = "Answer0";
            this.Answer0.Size = new System.Drawing.Size(19, 18);
            this.Answer0.TabIndex = 5;
            this.Answer0.UseVisualStyleBackColor = true;
            // 
            // Answer1
            // 
            this.Answer1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Answer1.Location = new System.Drawing.Point(521, 144);
            this.Answer1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Answer1.Name = "Answer1";
            this.Answer1.Size = new System.Drawing.Size(19, 18);
            this.Answer1.TabIndex = 7;
            this.Answer1.UseVisualStyleBackColor = true;
            // 
            // Answer2
            // 
            this.Answer2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Answer2.Location = new System.Drawing.Point(521, 178);
            this.Answer2.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Answer2.Name = "Answer2";
            this.Answer2.Size = new System.Drawing.Size(19, 18);
            this.Answer2.TabIndex = 9;
            this.Answer2.UseVisualStyleBackColor = true;
            // 
            // Answer3
            // 
            this.Answer3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Answer3.Location = new System.Drawing.Point(521, 212);
            this.Answer3.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Answer3.Name = "Answer3";
            this.Answer3.Size = new System.Drawing.Size(19, 18);
            this.Answer3.TabIndex = 11;
            this.Answer3.UseVisualStyleBackColor = true;
            // 
            // Answer4
            // 
            this.Answer4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Answer4.Location = new System.Drawing.Point(521, 246);
            this.Answer4.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Answer4.Name = "Answer4";
            this.Answer4.Size = new System.Drawing.Size(19, 18);
            this.Answer4.TabIndex = 13;
            this.Answer4.UseVisualStyleBackColor = true;
            // 
            // TabAvailability
            // 
            this.TabAvailability.Controls.Add(this.AvailibiltyPanel);
            this.TabAvailability.Location = new System.Drawing.Point(4, 29);
            this.TabAvailability.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TabAvailability.Name = "TabAvailability";
            this.TabAvailability.Size = new System.Drawing.Size(581, 405);
            this.TabAvailability.TabIndex = 2;
            this.TabAvailability.Text = "Beschikbaarheid";
            // 
            // AvailibiltyPanel
            // 
            this.AvailibiltyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.AvailibiltyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.AvailibiltyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.AvailibiltyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.AvailibiltyPanel.Controls.Add(this.lbAvList0, 0, 0);
            this.AvailibiltyPanel.Controls.Add(this.ToggleAvList0, 1, 0);
            this.AvailibiltyPanel.Controls.Add(this.AvList0, 0, 1);
            this.AvailibiltyPanel.Controls.Add(this.lbAvList1, 2, 0);
            this.AvailibiltyPanel.Controls.Add(this.ToggleAvList1, 3, 0);
            this.AvailibiltyPanel.Controls.Add(this.AvList1, 2, 1);
            this.AvailibiltyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvailibiltyPanel.Location = new System.Drawing.Point(0, 0);
            this.AvailibiltyPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AvailibiltyPanel.Name = "AvailibiltyPanel";
            this.AvailibiltyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.AvailibiltyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AvailibiltyPanel.Size = new System.Drawing.Size(581, 405);
            this.AvailibiltyPanel.TabIndex = 0;
            // 
            // lbAvList0
            // 
            this.lbAvList0.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbAvList0.Location = new System.Drawing.Point(27, 6);
            this.lbAvList0.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.lbAvList0.Name = "lbAvList0";
            this.lbAvList0.Size = new System.Drawing.Size(120, 20);
            this.lbAvList0.TabIndex = 99;
            this.lbAvList0.Text = "Biotopen";
            // 
            // ToggleAvList0
            // 
            this.ToggleAvList0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ToggleAvList0.Location = new System.Drawing.Point(198, 2);
            this.ToggleAvList0.Margin = new System.Windows.Forms.Padding(3, 2, 21, 2);
            this.ToggleAvList0.Name = "ToggleAvList0";
            this.ToggleAvList0.Size = new System.Drawing.Size(71, 30);
            this.ToggleAvList0.TabIndex = 2;
            this.ToggleAvList0.Text = "Wissel";
            this.ToggleAvList0.UseVisualStyleBackColor = true;
            // 
            // AvList0
            // 
            this.AvList0.CheckOnClick = true;
            this.AvailibiltyPanel.SetColumnSpan(this.AvList0, 2);
            this.AvList0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvList0.FormattingEnabled = true;
            this.AvList0.Location = new System.Drawing.Point(3, 36);
            this.AvList0.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AvList0.Name = "AvList0";
            this.AvList0.Size = new System.Drawing.Size(284, 367);
            this.AvList0.TabIndex = 3;
            // 
            // lbAvList1
            // 
            this.lbAvList1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbAvList1.Location = new System.Drawing.Point(317, 6);
            this.lbAvList1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.lbAvList1.Name = "lbAvList1";
            this.lbAvList1.Size = new System.Drawing.Size(120, 20);
            this.lbAvList1.TabIndex = 99;
            this.lbAvList1.Text = "Kalender";
            // 
            // ToggleAvList1
            // 
            this.ToggleAvList1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ToggleAvList1.Location = new System.Drawing.Point(489, 2);
            this.ToggleAvList1.Margin = new System.Windows.Forms.Padding(3, 2, 21, 2);
            this.ToggleAvList1.Name = "ToggleAvList1";
            this.ToggleAvList1.Size = new System.Drawing.Size(71, 30);
            this.ToggleAvList1.TabIndex = 4;
            this.ToggleAvList1.Text = "Wissel";
            this.ToggleAvList1.UseVisualStyleBackColor = true;
            // 
            // AvList1
            // 
            this.AvList1.CheckOnClick = true;
            this.AvailibiltyPanel.SetColumnSpan(this.AvList1, 2);
            this.AvList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AvList1.FormattingEnabled = true;
            this.AvList1.Location = new System.Drawing.Point(293, 36);
            this.AvList1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AvList1.Name = "AvList1";
            this.AvList1.Size = new System.Drawing.Size(285, 367);
            this.AvList1.TabIndex = 5;
            // 
            // Menu
            // 
            this.Menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuMode,
            this.MenuSave,
            this.MenuRemove,
            this.MenuFilter});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.Menu.Size = new System.Drawing.Size(793, 32);
            this.Menu.TabIndex = 1;
            this.Menu.Text = "menuStrip1";
            // 
            // MenuMode
            // 
            this.MenuMode.Name = "MenuMode";
            this.MenuMode.Size = new System.Drawing.Size(121, 28);
            this.MenuMode.SelectedIndexChanged += new System.EventHandler(this.MenuMode_SelectedIndexChanged);
            // 
            // MenuSave
            // 
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.Size = new System.Drawing.Size(65, 28);
            this.MenuSave.Text = "Sla op";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuRemove
            // 
            this.MenuRemove.Name = "MenuRemove";
            this.MenuRemove.Size = new System.Drawing.Size(85, 28);
            this.MenuRemove.Text = "Verwijder";
            this.MenuRemove.Click += new System.EventHandler(this.MenuRemove_Click);
            // 
            // MenuFilter
            // 
            this.MenuFilter.Name = "MenuFilter";
            this.MenuFilter.Size = new System.Drawing.Size(56, 28);
            this.MenuFilter.Text = "Filter";
            this.MenuFilter.Click += new System.EventHandler(this.MenuFilter_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 468);
            this.Controls.Add(this.MainLayout);
            this.Controls.Add(this.Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(808, 500);
            this.Name = "MainForm";
            this.Text = "De Speurzoekers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.TabPicture.ResumeLayout(false);
            this.PicturePanel.ResumeLayout(false);
            this.PicturePanel.PerformLayout();
            this.PictureInnerPanel.ResumeLayout(false);
            this.PictureInnerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.MainLayout.Panel1.ResumeLayout(false);
            this.MainLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainLayout)).EndInit();
            this.MainLayout.ResumeLayout(false);
            this.Tabs.ResumeLayout(false);
            this.TabMain.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PointsSelector)).EndInit();
            this.FormattingTools.ResumeLayout(false);
            this.TabQuestion.ResumeLayout(false);
            this.QuestionPanel.ResumeLayout(false);
            this.QuestionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.QuestionPointsSelector)).EndInit();
            this.TabAvailability.ResumeLayout(false);
            this.AvailibiltyPanel.ResumeLayout(false);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        internal System.Windows.Forms.ListBox ItemList;
        internal System.Windows.Forms.SplitContainer MainLayout;
        internal System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripComboBox MenuMode;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        private System.Windows.Forms.ToolStripMenuItem MenuRemove;
        private System.Windows.Forms.ToolStripMenuItem MenuFilter;

        // Tab main
        internal System.Windows.Forms.TabPage TabMain;
        internal System.Windows.Forms.TableLayoutPanel MainPanel;
        internal System.Windows.Forms.TextBox NameInput;
        internal System.Windows.Forms.ComboBox RaritySelector;
        internal System.Windows.Forms.NumericUpDown PointsSelector;
        internal System.Windows.Forms.ComboBox PageSelector;

        internal System.Windows.Forms.FlowLayoutPanel FormattingTools;
        internal System.Windows.Forms.Button ApplyBold;
        internal System.Windows.Forms.Button ApplyItalic;
        internal System.Windows.Forms.Button ApplyStrikeOut;
        internal System.Windows.Forms.Button ApplyHeader;
        internal System.Windows.Forms.Button AddLink;
        internal System.Windows.Forms.Label lbList;
        internal System.Windows.Forms.Button ApplyNumList;
        internal System.Windows.Forms.Button ApplyUnsortedList;
        internal System.Windows.Forms.Label lbPaste;
        internal System.Windows.Forms.Button PasteWord;
        internal System.Windows.Forms.Button PasteHtml;

        internal System.Windows.Forms.RichTextBox TextBox;
        // Tab question
        internal System.Windows.Forms.TabPage TabQuestion;
        internal System.Windows.Forms.TableLayoutPanel QuestionPanel;
        internal System.Windows.Forms.NumericUpDown QuestionPointsSelector;
        internal System.Windows.Forms.TextBox QuestionInput;

        internal System.Windows.Forms.TextBox Choice0;
        internal System.Windows.Forms.TextBox Choice1;
        internal System.Windows.Forms.TextBox Choice2;
        internal System.Windows.Forms.TextBox Choice3;
        internal System.Windows.Forms.TextBox Choice4;
        internal System.Windows.Forms.CheckBox Answer0;
        internal System.Windows.Forms.CheckBox Answer1;
        internal System.Windows.Forms.CheckBox Answer2;
        internal System.Windows.Forms.CheckBox Answer3;
        internal System.Windows.Forms.CheckBox Answer4;
        // Tab avaibility
        internal System.Windows.Forms.TabPage TabAvailability;
        internal System.Windows.Forms.TableLayoutPanel AvailibiltyPanel;
        internal System.Windows.Forms.Button ToggleAvList0;
        internal System.Windows.Forms.CheckedListBox AvList0;
        internal System.Windows.Forms.Button ToggleAvList1;
        internal System.Windows.Forms.CheckedListBox AvList1;
        // Tab picture
        internal System.Windows.Forms.TabPage TabPicture;
        internal System.Windows.Forms.TableLayoutPanel PicturePanel;
        internal System.Windows.Forms.ComboBox PictureSize;
        internal System.Windows.Forms.TextBox PictureName;
        internal System.Windows.Forms.Button OpenPath;
        internal System.Windows.Forms.Panel PictureInnerPanel;
        internal System.Windows.Forms.PictureBox PictureBox;
        // Labels
        internal System.Windows.Forms.Label lbName;
        internal System.Windows.Forms.Label lbTextPage;
        internal System.Windows.Forms.Label lbRarity;
        internal System.Windows.Forms.Label lbPoints;
        internal System.Windows.Forms.Label lbQuestion;
        internal System.Windows.Forms.Label lbChoices;
        internal System.Windows.Forms.Label lbQuestionPoints;
        internal System.Windows.Forms.Label lbIsAnswer;
        internal System.Windows.Forms.Label lbAvList0;
        internal System.Windows.Forms.Label lbAvList1;
        internal System.Windows.Forms.Label PlaceHolder;
    }
}

