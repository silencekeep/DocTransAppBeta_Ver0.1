namespace DocTransAppBeta1.UserControls.ConfigPanels
{
    partial class CaptionConfigPanel
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            checkBox1 = new CheckBox();
            label1 = new Label();
            comboBox1 = new ComboBox();
            fontPanel1 = new FontPanel();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(checkBox1);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(comboBox1);
            flowLayoutPanel1.Controls.Add(fontPanel1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(238, 688);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(3, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(144, 28);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "是否作为图片";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 34);
            label1.Name = "label1";
            label1.Size = new Size(150, 24);
            label1.TabIndex = 1;
            label1.Text = "Caption对齐类型";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "上", "下", "左", "右" });
            comboBox1.Location = new Point(3, 61);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(221, 32);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // fontPanel1
            // 
            fontPanel1.Dock = DockStyle.Bottom;
            fontPanel1.FontLocal = new Font(FontPanel.__default_fontfamily, FontPanel.__default_fontsize);
            fontPanel1.Location = new Point(3, 99);
            fontPanel1.Name = "fontPanel1";
            fontPanel1.Size = new Size(221, 130);
            fontPanel1.TabIndex = 3;
            // 
            // CaptionConfigPanel
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flowLayoutPanel1);
            Name = "CaptionConfigPanel";
            Size = new Size(238, 688);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox checkBox1;
        private Label label1;
        private ComboBox comboBox1;
        private FontPanel fontPanel1;
    }
}
