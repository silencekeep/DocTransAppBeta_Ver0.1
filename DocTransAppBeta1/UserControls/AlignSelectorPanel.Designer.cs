namespace DocTransAppBeta1.UserControls
{
    partial class AlignSelectorPanel
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
            radioButton_alignDefault = new RadioButton();
            radioButton_alignLeft = new RadioButton();
            radioButton_alignCenter = new RadioButton();
            radioButton_alignRight = new RadioButton();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(radioButton_alignDefault);
            flowLayoutPanel1.Controls.Add(radioButton_alignLeft);
            flowLayoutPanel1.Controls.Add(radioButton_alignCenter);
            flowLayoutPanel1.Controls.Add(radioButton_alignRight);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(150, 150);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // radioButton_alignDefault
            // 
            radioButton_alignDefault.AutoSize = true;
            radioButton_alignDefault.Checked = true;
            radioButton_alignDefault.Location = new Point(3, 3);
            radioButton_alignDefault.Name = "radioButton_alignDefault";
            radioButton_alignDefault.Size = new Size(71, 28);
            radioButton_alignDefault.TabIndex = 0;
            radioButton_alignDefault.TabStop = true;
            radioButton_alignDefault.Text = "默认";
            radioButton_alignDefault.UseVisualStyleBackColor = true;
            radioButton_alignDefault.CheckedChanged += radioButton_alignDefault_CheckedChanged;
            // 
            // radioButton_alignLeft
            // 
            radioButton_alignLeft.AutoSize = true;
            radioButton_alignLeft.Location = new Point(3, 37);
            radioButton_alignLeft.Name = "radioButton_alignLeft";
            radioButton_alignLeft.Size = new Size(89, 28);
            radioButton_alignLeft.TabIndex = 1;
            radioButton_alignLeft.Text = "左对齐";
            radioButton_alignLeft.UseVisualStyleBackColor = true;
            radioButton_alignLeft.CheckedChanged += radioButton_alignLeft_CheckedChanged;
            // 
            // radioButton_alignCenter
            // 
            radioButton_alignCenter.AutoSize = true;
            radioButton_alignCenter.Location = new Point(3, 71);
            radioButton_alignCenter.Name = "radioButton_alignCenter";
            radioButton_alignCenter.Size = new Size(107, 28);
            radioButton_alignCenter.TabIndex = 2;
            radioButton_alignCenter.Text = "中间对齐";
            radioButton_alignCenter.UseVisualStyleBackColor = true;
            radioButton_alignCenter.CheckedChanged += radioButton_alignCenter_CheckedChanged;
            // 
            // radioButton_alignRight
            // 
            radioButton_alignRight.AutoSize = true;
            radioButton_alignRight.Location = new Point(3, 105);
            radioButton_alignRight.Name = "radioButton_alignRight";
            radioButton_alignRight.Size = new Size(89, 28);
            radioButton_alignRight.TabIndex = 3;
            radioButton_alignRight.Text = "右对齐";
            radioButton_alignRight.UseVisualStyleBackColor = true;
            radioButton_alignRight.CheckedChanged += radioButton_alignRight_CheckedChanged;
            // 
            // AlignSelectorPanel
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flowLayoutPanel1);
            Name = "AlignSelectorPanel";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private RadioButton radioButton_alignDefault;
        private RadioButton radioButton_alignLeft;
        private RadioButton radioButton_alignCenter;
        private RadioButton radioButton_alignRight;
    }
}
