namespace DocTransAppBeta1.UserControls
{
    partial class FontPanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            button_setFont = new Button();
            button_resetFont = new Button();
            label_FontSize = new Label();
            label_FontName = new Label();
            label1 = new Label();
            label2 = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(button_setFont, 1, 2);
            tableLayoutPanel1.Controls.Add(button_resetFont, 0, 2);
            tableLayoutPanel1.Controls.Add(label_FontSize, 1, 1);
            tableLayoutPanel1.Controls.Add(label_FontName, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(239, 242);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // button_setFont
            // 
            button_setFont.Dock = DockStyle.Fill;
            button_setFont.Location = new Point(122, 163);
            button_setFont.Name = "button_setFont";
            button_setFont.Size = new Size(114, 76);
            button_setFont.TabIndex = 9;
            button_setFont.Text = "设置字体";
            button_setFont.UseVisualStyleBackColor = true;
            button_setFont.Click += button_setFont_Click_1;
            // 
            // button_resetFont
            // 
            button_resetFont.Dock = DockStyle.Fill;
            button_resetFont.Location = new Point(3, 163);
            button_resetFont.Name = "button_resetFont";
            button_resetFont.Size = new Size(113, 76);
            button_resetFont.TabIndex = 8;
            button_resetFont.Text = "重置字体";
            button_resetFont.UseVisualStyleBackColor = true;
            button_resetFont.Click += button_resetFont_Click;
            // 
            // label_FontSize
            // 
            label_FontSize.AutoSize = true;
            label_FontSize.Location = new Point(122, 80);
            label_FontSize.Name = "label_FontSize";
            label_FontSize.Size = new Size(32, 24);
            label_FontSize.TabIndex = 4;
            label_FontSize.Text = "12";
            // 
            // label_FontName
            // 
            label_FontName.AutoSize = true;
            label_FontName.Location = new Point(122, 0);
            label_FontName.Name = "label_FontName";
            label_FontName.Size = new Size(42, 24);
            label_FontName.TabIndex = 3;
            label_FontName.Text = "null";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 24);
            label1.TabIndex = 0;
            label1.Text = "字体名称：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 80);
            label2.Name = "label2";
            label2.Size = new Size(64, 24);
            label2.TabIndex = 1;
            label2.Text = "大小：";
            // 
            // FontPanel
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "FontPanel";
            Size = new Size(239, 242);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label_FontSize;
        private Label label_FontName;
        private Label label1;
        private Label label2;
        private Button button_setFont;
        private Button button_resetFont;
    }
}
