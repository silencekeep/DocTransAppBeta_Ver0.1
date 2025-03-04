namespace DocTransAppBeta1.UserControls.ConfigPanels
{
    partial class TableConfigPanel
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
            label2 = new Label();
            alignSelectorPanel_outer = new AlignSelectorPanel();
            fontPanel1 = new FontPanel();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(checkBox1);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(alignSelectorPanel_outer);
            flowLayoutPanel1.Controls.Add(fontPanel1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(227, 687);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(3, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(151, 28);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "是否视为Logo";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 34);
            label2.Name = "label2";
            label2.Size = new Size(136, 24);
            label2.TabIndex = 3;
            label2.Text = "整体对齐选项：";
            // 
            // alignSelectorPanel_outer
            // 
            alignSelectorPanel_outer.AlignMode = PdfStructure.BoxAlignMode.Default;
            alignSelectorPanel_outer.Dock = DockStyle.Top;
            alignSelectorPanel_outer.Location = new Point(3, 61);
            alignSelectorPanel_outer.Name = "alignSelectorPanel_outer";
            alignSelectorPanel_outer.Size = new Size(225, 89);
            alignSelectorPanel_outer.TabIndex = 5;
            // 
            // fontPanel1
            // 
            fontPanel1.Dock = DockStyle.Bottom;
            fontPanel1.FontLocal = new Font(FontPanel.__default_fontfamily, FontPanel.__default_fontsize);
            fontPanel1.Location = new Point(3, 156);
            fontPanel1.Name = "fontPanel1";
            fontPanel1.Size = new Size(215, 119);
            fontPanel1.TabIndex = 7;
            // 
            // TableConfigPanel
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flowLayoutPanel1);
            Name = "TableConfigPanel";
            Size = new Size(227, 687);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox checkBox1;
        private Label label2;
        private AlignSelectorPanel alignSelectorPanel_outer;
        private FontPanel fontPanel1;
    }
}
