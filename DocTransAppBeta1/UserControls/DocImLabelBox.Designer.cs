namespace DocTransAppBeta1.UserControls
{
    partial class DocImLabelBox
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
            components = new System.ComponentModel.Container();
            contextMenuStrip = new ContextMenuStrip(components);
            ToolStripMenuItem_selectTargetLabelBox = new ToolStripMenuItem();
            ToolStripMenuItem_markAsHead = new ToolStripMenuItem();
            ToolStripMenuItem_unmarkAsHead = new ToolStripMenuItem();
            ToolStripMenuItem_markAsPageBegin = new ToolStripMenuItem();
            ToolStripMenuItem_setAsSelectedLabelBoxNext = new ToolStripMenuItem();
            ToolStripMenuItem_setAsSelectedLabelBoxCap = new ToolStripMenuItem();
            ToolStripMenuItem_setAsTableFootnote = new ToolStripMenuItem();
            ToolStripMenuItem_deleteThisLabelBox = new ToolStripMenuItem();
            添加到列表中ToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new Size(24, 24);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { ToolStripMenuItem_selectTargetLabelBox, ToolStripMenuItem_markAsHead, ToolStripMenuItem_unmarkAsHead, ToolStripMenuItem_markAsPageBegin, ToolStripMenuItem_setAsSelectedLabelBoxNext, ToolStripMenuItem_setAsSelectedLabelBoxCap, ToolStripMenuItem_setAsTableFootnote, ToolStripMenuItem_deleteThisLabelBox, 添加到列表中ToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(279, 307);
            contextMenuStrip.Opening += contextMenuStrip_Opening;
            // 
            // ToolStripMenuItem_selectTargetLabelBox
            // 
            ToolStripMenuItem_selectTargetLabelBox.Name = "ToolStripMenuItem_selectTargetLabelBox";
            ToolStripMenuItem_selectTargetLabelBox.Size = new Size(278, 30);
            ToolStripMenuItem_selectTargetLabelBox.Text = "选中该块";
            ToolStripMenuItem_selectTargetLabelBox.Click += ToolStripMenuItem_selectTargetLabelBox_Click;
            // 
            // ToolStripMenuItem_markAsHead
            // 
            ToolStripMenuItem_markAsHead.Name = "ToolStripMenuItem_markAsHead";
            ToolStripMenuItem_markAsHead.Size = new Size(278, 30);
            ToolStripMenuItem_markAsHead.Text = "标记为流式布局头";
            ToolStripMenuItem_markAsHead.Click += ToolStripMenuItem_markAsHead_Click;
            // 
            // ToolStripMenuItem_unmarkAsHead
            // 
            ToolStripMenuItem_unmarkAsHead.Name = "ToolStripMenuItem_unmarkAsHead";
            ToolStripMenuItem_unmarkAsHead.Size = new Size(278, 30);
            ToolStripMenuItem_unmarkAsHead.Text = "取消流式布局头标记";
            ToolStripMenuItem_unmarkAsHead.Click += ToolStripMenuItem_unmarkAsHead_Click;
            // 
            // ToolStripMenuItem_markAsPageBegin
            // 
            ToolStripMenuItem_markAsPageBegin.Name = "ToolStripMenuItem_markAsPageBegin";
            ToolStripMenuItem_markAsPageBegin.Size = new Size(278, 30);
            ToolStripMenuItem_markAsPageBegin.Text = "标记为该页正文头";
            ToolStripMenuItem_markAsPageBegin.Click += ToolStripMenuItem_markAsPageBegin_Click;
            // 
            // ToolStripMenuItem_setAsSelectedLabelBoxNext
            // 
            ToolStripMenuItem_setAsSelectedLabelBoxNext.Name = "ToolStripMenuItem_setAsSelectedLabelBoxNext";
            ToolStripMenuItem_setAsSelectedLabelBoxNext.Size = new Size(278, 30);
            ToolStripMenuItem_setAsSelectedLabelBoxNext.Text = "设置该块为选中块的后继";
            ToolStripMenuItem_setAsSelectedLabelBoxNext.Click += ToolStripMenuItem_setAsSelectedLabelBoxNext_Click;
            // 
            // ToolStripMenuItem_setAsSelectedLabelBoxCap
            // 
            ToolStripMenuItem_setAsSelectedLabelBoxCap.Name = "ToolStripMenuItem_setAsSelectedLabelBoxCap";
            ToolStripMenuItem_setAsSelectedLabelBoxCap.Size = new Size(278, 30);
            ToolStripMenuItem_setAsSelectedLabelBoxCap.Text = "设置该块为选中块的批注";
            ToolStripMenuItem_setAsSelectedLabelBoxCap.Click += ToolStripMenuItem_setAsSelectedLabelBoxCap_Click;
            // 
            // ToolStripMenuItem_setAsTableFootnote
            // 
            ToolStripMenuItem_setAsTableFootnote.Name = "ToolStripMenuItem_setAsTableFootnote";
            ToolStripMenuItem_setAsTableFootnote.Size = new Size(278, 30);
            ToolStripMenuItem_setAsTableFootnote.Text = "标记为选中Table的脚注";
            ToolStripMenuItem_setAsTableFootnote.Click += ToolStripMenuItem_setAsTableFootnote_Click;
            // 
            // ToolStripMenuItem_deleteThisLabelBox
            // 
            ToolStripMenuItem_deleteThisLabelBox.Name = "ToolStripMenuItem_deleteThisLabelBox";
            ToolStripMenuItem_deleteThisLabelBox.Size = new Size(278, 30);
            ToolStripMenuItem_deleteThisLabelBox.Text = "删除该块";
            ToolStripMenuItem_deleteThisLabelBox.Click += ToolStripMenuItem_deleteThisLabelBox_Click;
            // 
            // 添加到列表中ToolStripMenuItem
            // 
            添加到列表中ToolStripMenuItem.Name = "添加到列表中ToolStripMenuItem";
            添加到列表中ToolStripMenuItem.Size = new Size(278, 30);
            添加到列表中ToolStripMenuItem.Text = "添加到列表中";
            添加到列表中ToolStripMenuItem.Click += 添加到列表中ToolStripMenuItem_Click;
            // 
            // DocImLabelBox
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Name = "DocImLabelBox";
            Load += DocImLabelBox_Load;
            Paint += DocImLabelBox_Paint;
            MouseDown += DocImLabelBox_MouseDown;
            MouseLeave += DocImLabelBox_MouseLeave;
            MouseMove += DocImLabelBox_MouseMove;
            MouseUp += DocImLabelBox_MouseUp;
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem ToolStripMenuItem_selectTargetLabelBox;
        private ToolStripMenuItem 设置该Box为选中Box的前驱ToolStripMenuItem;
        private ToolStripMenuItem ToolStripMenuItem_setAsSelectedLabelBoxNext;

        public ToolStripMenuItem ToolStripMenuItem_setAsSelectedPageHeadLabelBox { get; private set; }

        private ToolStripMenuItem ToolStripMenuItem_setAsSelectedLabelBoxCaption;
        private ToolStripMenuItem ToolStripMenuItem_deleteThisLabelBox;
        private ToolStripMenuItem 页眉ToolStripMenuItem;
        private ToolStripMenuItem 页脚ToolStripMenuItem;
        private ToolStripMenuItem 固定Abandon水印批注ToolStripMenuItem;
        private ToolStripMenuItem ToolStripMenuItem_markAsHead;
        private ToolStripMenuItem ToolStripMenuItem_markAsPageBegin;
        private ToolStripMenuItem ToolStripMenuItem_setAsSelectedLabelBoxCap;
        private ToolStripMenuItem ToolStripMenuItem_setAsTableFootnote;
        private ToolStripMenuItem ToolStripMenuItem_unmarkAsHead;
        private ToolStripMenuItem 添加到列表中ToolStripMenuItem;
    }
}
