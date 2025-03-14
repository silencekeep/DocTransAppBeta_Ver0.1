﻿namespace DocTransAppBeta1.UserControls.ConfigPanels
{
    partial class PlainTextConfigPanel
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
            alignSelectorPanel_inner = new AlignSelectorPanel();
            label2 = new Label();
            alignSelectorPanel_outer = new AlignSelectorPanel();
            fontPanel1 = new FontPanel();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(checkBox1);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(alignSelectorPanel_inner);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(alignSelectorPanel_outer);
            flowLayoutPanel1.Controls.Add(fontPanel1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(239, 680);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(3, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(144, 28);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "段落自动缩进";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 34);
            label1.Name = "label1";
            label1.Size = new Size(154, 24);
            label1.TabIndex = 2;
            label1.Text = "文本内对齐选项：";
            // 
            // alignSelectorPanel_inner
            // 
            alignSelectorPanel_inner.AlignMode = PdfStructure.BoxAlignMode.Default;
            alignSelectorPanel_inner.Dock = DockStyle.Top;
            alignSelectorPanel_inner.Location = new Point(3, 61);
            alignSelectorPanel_inner.Name = "alignSelectorPanel_inner";
            alignSelectorPanel_inner.Size = new Size(225, 89);
            alignSelectorPanel_inner.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 153);
            label2.Name = "label2";
            label2.Size = new Size(136, 24);
            label2.TabIndex = 3;
            label2.Text = "整体对齐选项：";
            // 
            // alignSelectorPanel_outer
            // 
            alignSelectorPanel_outer.AlignMode = PdfStructure.BoxAlignMode.Default;
            alignSelectorPanel_outer.Dock = DockStyle.Top;
            alignSelectorPanel_outer.Location = new Point(3, 180);
            alignSelectorPanel_outer.Name = "alignSelectorPanel_outer";
            alignSelectorPanel_outer.Size = new Size(225, 89);
            alignSelectorPanel_outer.TabIndex = 5;
            // 
            // fontPanel1
            // 
            fontPanel1.Dock = DockStyle.Bottom;
            fontPanel1.FontLocal = new Font(FontPanel.__default_fontfamily, FontPanel.__default_fontsize);
            fontPanel1.Location = new Point(3, 275);
            fontPanel1.Name = "fontPanel1";
            fontPanel1.Size = new Size(215, 119);
            fontPanel1.TabIndex = 7;
            // 
            // PlainTextConfigPanel
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flowLayoutPanel1);
            Name = "PlainTextConfigPanel";
            Size = new Size(239, 680);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox checkBox1;
        private Label label1;
        private AlignSelectorPanel alignSelectorPanel_inner;
        private Label label2;
        private AlignSelectorPanel alignSelectorPanel_outer;
        private FontPanel fontPanel1;
    }
}
