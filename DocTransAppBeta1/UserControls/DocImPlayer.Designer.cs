namespace DocTransAppBeta1.UserControls
{
    partial class DocImPlayer
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
            ToolStripMenuItem_addNewBox = new ToolStripMenuItem();
            ToolStripMenuItem_clearAllBox = new ToolStripMenuItem();
            ToolStripMenuItem_clearAllBoxRelations = new ToolStripMenuItem();
            设置此页为目录页ToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new Size(24, 24);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { ToolStripMenuItem_addNewBox, ToolStripMenuItem_clearAllBox, ToolStripMenuItem_clearAllBoxRelations, 设置此页为目录页ToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(243, 157);
            // 
            // ToolStripMenuItem_addNewBox
            // 
            ToolStripMenuItem_addNewBox.Name = "ToolStripMenuItem_addNewBox";
            ToolStripMenuItem_addNewBox.Size = new Size(242, 30);
            ToolStripMenuItem_addNewBox.Text = "添加新标记框";
            ToolStripMenuItem_addNewBox.Click += ToolStripMenuItem_addNewBox_Click;
            // 
            // ToolStripMenuItem_clearAllBox
            // 
            ToolStripMenuItem_clearAllBox.Name = "ToolStripMenuItem_clearAllBox";
            ToolStripMenuItem_clearAllBox.Size = new Size(242, 30);
            ToolStripMenuItem_clearAllBox.Text = "清除所有标记框";
            ToolStripMenuItem_clearAllBox.Click += ToolStripMenuItem_clearAllBox_Click;
            // 
            // ToolStripMenuItem_clearAllBoxRelations
            // 
            ToolStripMenuItem_clearAllBoxRelations.Name = "ToolStripMenuItem_clearAllBoxRelations";
            ToolStripMenuItem_clearAllBoxRelations.Size = new Size(242, 30);
            ToolStripMenuItem_clearAllBoxRelations.Text = "清除标记框所有关系";
            ToolStripMenuItem_clearAllBoxRelations.Click += ToolStripMenuItem_clearAllBoxRelations_Click;
            // 
            // 设置此页为目录页ToolStripMenuItem
            // 
            设置此页为目录页ToolStripMenuItem.Name = "设置此页为目录页ToolStripMenuItem";
            设置此页为目录页ToolStripMenuItem.Size = new Size(242, 30);
            设置此页为目录页ToolStripMenuItem.Text = "设置此页为目录页";
            // 
            // DocImPlayer
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Name = "DocImPlayer";
            Size = new Size(595, 842);
            Load += DocImPlayer_Load;
            Paint += DocImPlayer_Paint;
            MouseUp += DocImPlayer_MouseUp;
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem ToolStripMenuItem_addNewBox;
        private ToolStripMenuItem ToolStripMenuItem_clearAllBox;
        private ToolStripMenuItem ToolStripMenuItem_clearAllBoxRelations;
        private ToolStripMenuItem 设置此页为目录页ToolStripMenuItem;
    }
}
