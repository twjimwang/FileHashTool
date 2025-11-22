namespace FileHashTool
{
    partial class Form1
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
            splitContainer = new SplitContainer();
            fileListBox = new ListBox();
            selectFilesButton = new Button();
            removeFilesButton = new Button();
            instructionLabel = new Label();
            progressLabel = new Label();
            progressBar = new ProgressBar();
            copyAllButton = new Button();
            searchTextBox = new TextBox();
            outputTextBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(fileListBox);
            splitContainer.Panel1.Controls.Add(selectFilesButton);
            splitContainer.Panel1.Controls.Add(removeFilesButton);
            splitContainer.Panel1.Controls.Add(instructionLabel);
            splitContainer.Panel1.Controls.Add(searchTextBox);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(outputTextBox);
            splitContainer.Panel2.Controls.Add(progressBar);
            splitContainer.Panel2.Controls.Add(progressLabel);
            splitContainer.Panel2.Controls.Add(copyAllButton);
            splitContainer.Size = new Size(984, 561);
            splitContainer.SplitterDistance = 320;
            splitContainer.TabIndex = 0;
            // 
            // instructionLabel
            // 
            instructionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            instructionLabel.AutoSize = true;
            instructionLabel.Location = new Point(12, 118);
            instructionLabel.Name = "instructionLabel";
            instructionLabel.Size = new Size(241, 15);
            instructionLabel.TabIndex = 4;
            instructionLabel.Text = "拖曳檔案到下方清單或點擊加入／移除檔案";
            // 
            // fileListBox
            // 
            fileListBox.AllowDrop = true;
            fileListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fileListBox.FormattingEnabled = true;
            fileListBox.ItemHeight = 15;
            fileListBox.Location = new Point(12, 136);
            fileListBox.Name = "fileListBox";
            fileListBox.Size = new Size(293, 418);
            fileListBox.SelectionMode = SelectionMode.MultiExtended;
            fileListBox.TabIndex = 5;
            fileListBox.DragDrop += FileListBox_DragDrop;
            fileListBox.DragEnter += FileListBox_DragEnter;
            fileListBox.KeyDown += FileListBox_KeyDown;
            // 
            // selectFilesButton
            // 
            selectFilesButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            selectFilesButton.Location = new Point(12, 12);
            selectFilesButton.Name = "selectFilesButton";
            selectFilesButton.Size = new Size(293, 28);
            selectFilesButton.TabIndex = 1;
            selectFilesButton.Text = "選擇檔案...";
            selectFilesButton.UseVisualStyleBackColor = true;
            selectFilesButton.Click += SelectFilesButton_Click;
            // 
            // removeFilesButton
            // 
            removeFilesButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            removeFilesButton.Location = new Point(12, 46);
            removeFilesButton.Name = "removeFilesButton";
            removeFilesButton.Size = new Size(293, 28);
            removeFilesButton.TabIndex = 2;
            removeFilesButton.Text = "移除選取檔案";
            removeFilesButton.UseVisualStyleBackColor = true;
            removeFilesButton.Click += RemoveFilesButton_Click;
            // 
            // searchTextBox
            // 
            searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchTextBox.Location = new Point(12, 80);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "搜尋檔名...";
            searchTextBox.Size = new Size(293, 23);
            searchTextBox.TabIndex = 3;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            // 
            // outputTextBox
            // 
            outputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            outputTextBox.Location = new Point(12, 94);
            outputTextBox.Multiline = true;
            outputTextBox.Name = "outputTextBox";
            outputTextBox.ReadOnly = true;
            outputTextBox.ScrollBars = ScrollBars.Vertical;
            outputTextBox.Size = new Size(636, 455);
            outputTextBox.TabIndex = 0;
            // 
            // progressLabel
            // 
            progressLabel.AutoSize = true;
            progressLabel.Location = new Point(12, 14);
            progressLabel.Name = "progressLabel";
            progressLabel.Size = new Size(31, 15);
            progressLabel.TabIndex = 1;
            progressLabel.Text = "待命";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 40);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(636, 23);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 2;
            progressBar.Visible = false;
            // 
            // copyAllButton
            // 
            copyAllButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyAllButton.Location = new Point(543, 11);
            copyAllButton.Name = "copyAllButton";
            copyAllButton.Size = new Size(105, 23);
            copyAllButton.TabIndex = 3;
            copyAllButton.Text = "複製全部";
            copyAllButton.UseVisualStyleBackColor = true;
            copyAllButton.Click += CopyAllButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(splitContainer);
            MinimumSize = new Size(800, 480);
            Name = "Form1";
            Text = "檔案 MD5 計算工具";
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer;
        private Button selectFilesButton;
        private ListBox fileListBox;
        private TextBox outputTextBox;
        private Label instructionLabel;
        private Button removeFilesButton;
        private Label progressLabel;
        private ProgressBar progressBar;
        private Button copyAllButton;
        private TextBox searchTextBox;
    }
}
