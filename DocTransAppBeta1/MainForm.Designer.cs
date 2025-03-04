namespace DocTransAppBeta1
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
            button_openRawPDF = new Button();
            label1 = new Label();
            textBox_rawPDFPath = new TextBox();
            flowLayoutPanel_displayMode = new FlowLayoutPanel();
            radioButton_lookingMode = new RadioButton();
            radioButton_viewingMode = new RadioButton();
            radioButton_editingMode = new RadioButton();
            label2 = new Label();
            numericUpDown_pageSelector = new NumericUpDown();
            label_maxPageCount = new Label();
            openFileDialog_rawPDF = new OpenFileDialog();
            labelBoxConfigPanel = new Panel();
            button_msgBoxShowLinkNode = new Button();
            groupBox = new GroupBox();
            richTextBox1 = new RichTextBox();
            label3 = new Label();
            textBox_xmlDocNamePreview = new TextBox();
            button_saveImageFrame = new Button();
            saveFileDialog = new SaveFileDialog();
            comboBox1 = new ComboBox();
            label4 = new Label();
            button_parseAllPageToXml = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            radioButton_setPageAsFixed = new RadioButton();
            radioButton_setPageAsFlowLayouted = new RadioButton();
            panel1 = new Panel();
            button_AutoRelation = new Button();
            checkBox1 = new CheckBox();
            button_generateNewPDF = new Button();
            treeView1 = new TreeView();
            listBox1 = new ListBox();
            button1 = new Button();
            button2 = new Button();
            panel2 = new Panel();
            button3 = new Button();
            flowLayoutPanel_displayMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_pageSelector).BeginInit();
            groupBox.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // button_openRawPDF
            // 
            button_openRawPDF.Location = new Point(12, 52);
            button_openRawPDF.Name = "button_openRawPDF";
            button_openRawPDF.Size = new Size(158, 34);
            button_openRawPDF.TabIndex = 0;
            button_openRawPDF.Text = "打开PDF文件";
            button_openRawPDF.UseVisualStyleBackColor = true;
            button_openRawPDF.Click += button_openRawPDF_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(136, 24);
            label1.TabIndex = 1;
            label1.Text = "当前文件路径：";
            // 
            // textBox_rawPDFPath
            // 
            textBox_rawPDFPath.Location = new Point(154, 6);
            textBox_rawPDFPath.Name = "textBox_rawPDFPath";
            textBox_rawPDFPath.ReadOnly = true;
            textBox_rawPDFPath.Size = new Size(621, 30);
            textBox_rawPDFPath.TabIndex = 2;
            textBox_rawPDFPath.TextChanged += textBox_rawPDFPath_TextChanged;
            // 
            // flowLayoutPanel_displayMode
            // 
            flowLayoutPanel_displayMode.Controls.Add(radioButton_lookingMode);
            flowLayoutPanel_displayMode.Controls.Add(radioButton_viewingMode);
            flowLayoutPanel_displayMode.Controls.Add(radioButton_editingMode);
            flowLayoutPanel_displayMode.Enabled = false;
            flowLayoutPanel_displayMode.Location = new Point(1044, 857);
            flowLayoutPanel_displayMode.Name = "flowLayoutPanel_displayMode";
            flowLayoutPanel_displayMode.Size = new Size(345, 39);
            flowLayoutPanel_displayMode.TabIndex = 0;
            // 
            // radioButton_lookingMode
            // 
            radioButton_lookingMode.AutoSize = true;
            radioButton_lookingMode.Location = new Point(3, 3);
            radioButton_lookingMode.Name = "radioButton_lookingMode";
            radioButton_lookingMode.Size = new Size(107, 28);
            radioButton_lookingMode.TabIndex = 0;
            radioButton_lookingMode.Text = "查看模式";
            radioButton_lookingMode.UseVisualStyleBackColor = true;
            radioButton_lookingMode.CheckedChanged += radioButton_lookingMode_CheckedChanged;
            // 
            // radioButton_viewingMode
            // 
            radioButton_viewingMode.AutoSize = true;
            radioButton_viewingMode.Location = new Point(116, 3);
            radioButton_viewingMode.Name = "radioButton_viewingMode";
            radioButton_viewingMode.Size = new Size(107, 28);
            radioButton_viewingMode.TabIndex = 1;
            radioButton_viewingMode.Text = "预览模式";
            radioButton_viewingMode.UseVisualStyleBackColor = true;
            radioButton_viewingMode.CheckedChanged += radioButton_viewingMode_CheckedChanged;
            // 
            // radioButton_editingMode
            // 
            radioButton_editingMode.AutoSize = true;
            radioButton_editingMode.Location = new Point(229, 3);
            radioButton_editingMode.Name = "radioButton_editingMode";
            radioButton_editingMode.Size = new Size(107, 28);
            radioButton_editingMode.TabIndex = 2;
            radioButton_editingMode.Text = "编辑模式";
            radioButton_editingMode.UseVisualStyleBackColor = true;
            radioButton_editingMode.CheckedChanged += radioButton_editingMode_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(794, 864);
            label2.Name = "label2";
            label2.Size = new Size(64, 24);
            label2.TabIndex = 4;
            label2.Text = "页码：";
            // 
            // numericUpDown_pageSelector
            // 
            numericUpDown_pageSelector.Enabled = false;
            numericUpDown_pageSelector.Location = new Point(854, 862);
            numericUpDown_pageSelector.Name = "numericUpDown_pageSelector";
            numericUpDown_pageSelector.Size = new Size(61, 30);
            numericUpDown_pageSelector.TabIndex = 5;
            numericUpDown_pageSelector.ValueChanged += numericUpDown_pageSelector_ValueChanged;
            // 
            // label_maxPageCount
            // 
            label_maxPageCount.AutoSize = true;
            label_maxPageCount.Location = new Point(921, 864);
            label_maxPageCount.Name = "label_maxPageCount";
            label_maxPageCount.Size = new Size(29, 24);
            label_maxPageCount.TabIndex = 6;
            label_maxPageCount.Text = "/0";
            // 
            // openFileDialog_rawPDF
            // 
            openFileDialog_rawPDF.DefaultExt = "pdf";
            openFileDialog_rawPDF.Filter = "可移植文件格式|*.pdf";
            openFileDialog_rawPDF.Title = "选择PDF文件";
            // 
            // labelBoxConfigPanel
            // 
            labelBoxConfigPanel.BackColor = Color.DimGray;
            labelBoxConfigPanel.Location = new Point(1413, 344);
            labelBoxConfigPanel.Name = "labelBoxConfigPanel";
            labelBoxConfigPanel.Size = new Size(300, 544);
            labelBoxConfigPanel.TabIndex = 8;
            // 
            // button_msgBoxShowLinkNode
            // 
            button_msgBoxShowLinkNode.Location = new Point(1413, 12);
            button_msgBoxShowLinkNode.Name = "button_msgBoxShowLinkNode";
            button_msgBoxShowLinkNode.Size = new Size(300, 34);
            button_msgBoxShowLinkNode.TabIndex = 9;
            button_msgBoxShowLinkNode.Text = "查看文档块链表";
            button_msgBoxShowLinkNode.UseVisualStyleBackColor = true;
            button_msgBoxShowLinkNode.Click += button_msgBoxShowLinkNode_Click;
            // 
            // groupBox
            // 
            groupBox.Controls.Add(richTextBox1);
            groupBox.Location = new Point(328, 94);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(447, 794);
            groupBox.TabIndex = 13;
            groupBox.TabStop = false;
            groupBox.Text = "XML 预览";
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(3, 26);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(441, 765);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(191, 57);
            label3.Name = "label3";
            label3.Size = new Size(175, 24);
            label3.TabIndex = 14;
            label3.Text = "该XML子文件名称：";
            // 
            // textBox_xmlDocNamePreview
            // 
            textBox_xmlDocNamePreview.Location = new Point(366, 54);
            textBox_xmlDocNamePreview.Name = "textBox_xmlDocNamePreview";
            textBox_xmlDocNamePreview.ReadOnly = true;
            textBox_xmlDocNamePreview.Size = new Size(406, 30);
            textBox_xmlDocNamePreview.TabIndex = 15;
            // 
            // button_saveImageFrame
            // 
            button_saveImageFrame.Location = new Point(1413, 52);
            button_saveImageFrame.Name = "button_saveImageFrame";
            button_saveImageFrame.Size = new Size(300, 34);
            button_saveImageFrame.TabIndex = 16;
            button_saveImageFrame.Text = "保存当前页标记图";
            button_saveImageFrame.UseVisualStyleBackColor = true;
            button_saveImageFrame.Click += button_saveImageFrame_Click;
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "PNG File|*.png";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "title", "plain_text", "abandon", "figure", "figure_caption", "table", "table_caption", "table_footnote", "isolate_formula", "formula_caption", "unknown" });
            comboBox1.Location = new Point(1500, 142);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(213, 32);
            comboBox1.TabIndex = 17;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1413, 142);
            label4.Name = "label4";
            label4.Size = new Size(81, 24);
            label4.TabIndex = 18;
            label4.Text = "Box类型:";
            // 
            // button_parseAllPageToXml
            // 
            button_parseAllPageToXml.Location = new Point(1413, 291);
            button_parseAllPageToXml.Name = "button_parseAllPageToXml";
            button_parseAllPageToXml.Size = new Size(300, 34);
            button_parseAllPageToXml.TabIndex = 19;
            button_parseAllPageToXml.Text = "全部解析";
            button_parseAllPageToXml.UseVisualStyleBackColor = true;
            button_parseAllPageToXml.Click += button_previewThisNext_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(radioButton_setPageAsFixed);
            flowLayoutPanel1.Controls.Add(radioButton_setPageAsFlowLayouted);
            flowLayoutPanel1.Enabled = false;
            flowLayoutPanel1.Location = new Point(1416, 98);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(297, 34);
            flowLayoutPanel1.TabIndex = 20;
            // 
            // radioButton_setPageAsFixed
            // 
            radioButton_setPageAsFixed.AutoSize = true;
            radioButton_setPageAsFixed.Location = new Point(3, 3);
            radioButton_setPageAsFixed.Name = "radioButton_setPageAsFixed";
            radioButton_setPageAsFixed.Size = new Size(125, 28);
            radioButton_setPageAsFixed.TabIndex = 0;
            radioButton_setPageAsFixed.Text = "固定页模式";
            radioButton_setPageAsFixed.UseVisualStyleBackColor = true;
            radioButton_setPageAsFixed.CheckedChanged += radioButton_setPageAsFixed_CheckedChanged;
            // 
            // radioButton_setPageAsFlowLayouted
            // 
            radioButton_setPageAsFlowLayouted.AutoSize = true;
            radioButton_setPageAsFlowLayouted.Location = new Point(134, 3);
            radioButton_setPageAsFlowLayouted.Name = "radioButton_setPageAsFlowLayouted";
            radioButton_setPageAsFlowLayouted.Size = new Size(125, 28);
            radioButton_setPageAsFlowLayouted.TabIndex = 1;
            radioButton_setPageAsFlowLayouted.Text = "自动页模式";
            radioButton_setPageAsFlowLayouted.UseVisualStyleBackColor = true;
            radioButton_setPageAsFlowLayouted.CheckedChanged += radioButton_setPageAsFlowLayouted_CheckedChanged;
            // 
            // panel1
            // 
            panel1.Location = new Point(794, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(595, 842);
            panel1.TabIndex = 21;
            // 
            // button_AutoRelation
            // 
            button_AutoRelation.Location = new Point(1413, 211);
            button_AutoRelation.Name = "button_AutoRelation";
            button_AutoRelation.Size = new Size(300, 34);
            button_AutoRelation.TabIndex = 22;
            button_AutoRelation.Text = "列表选中删除";
            button_AutoRelation.UseVisualStyleBackColor = true;
            button_AutoRelation.Click += button_AutoRelation_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(1414, 179);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(144, 28);
            checkBox1.TabIndex = 23;
            checkBox1.Text = "是否是目录页";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button_generateNewPDF
            // 
            button_generateNewPDF.Location = new Point(12, 120);
            button_generateNewPDF.Name = "button_generateNewPDF";
            button_generateNewPDF.Size = new Size(158, 65);
            button_generateNewPDF.TabIndex = 24;
            button_generateNewPDF.Text = "生成新PDF";
            button_generateNewPDF.UseVisualStyleBackColor = true;
            button_generateNewPDF.Click += button_generateNewPDF_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(12, 246);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(294, 639);
            treeView1.TabIndex = 25;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(0, 0);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(282, 876);
            listBox1.TabIndex = 26;
            // 
            // button1
            // 
            button1.Location = new Point(1413, 251);
            button1.Name = "button1";
            button1.Size = new Size(300, 34);
            button1.TabIndex = 27;
            button1.Text = "列表选中清空";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(191, 123);
            button2.Name = "button2";
            button2.Size = new Size(115, 65);
            button2.TabIndex = 28;
            button2.Text = "编辑后生成页";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(listBox1);
            panel2.Location = new Point(1729, 9);
            panel2.Name = "panel2";
            panel2.Size = new Size(282, 876);
            panel2.TabIndex = 29;
            // 
            // button3
            // 
            button3.Location = new Point(12, 191);
            button3.Name = "button3";
            button3.Size = new Size(294, 49);
            button3.TabIndex = 30;
            button3.Text = "保存所有XML！";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2048, 902);
            Controls.Add(button3);
            Controls.Add(panel2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(treeView1);
            Controls.Add(button_generateNewPDF);
            Controls.Add(checkBox1);
            Controls.Add(button_AutoRelation);
            Controls.Add(panel1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(button_parseAllPageToXml);
            Controls.Add(label4);
            Controls.Add(comboBox1);
            Controls.Add(button_saveImageFrame);
            Controls.Add(textBox_xmlDocNamePreview);
            Controls.Add(label3);
            Controls.Add(groupBox);
            Controls.Add(button_msgBoxShowLinkNode);
            Controls.Add(labelBoxConfigPanel);
            Controls.Add(label_maxPageCount);
            Controls.Add(numericUpDown_pageSelector);
            Controls.Add(label2);
            Controls.Add(flowLayoutPanel_displayMode);
            Controls.Add(textBox_rawPDFPath);
            Controls.Add(label1);
            Controls.Add(button_openRawPDF);
            Name = "MainForm";
            Text = "电子PDF自动/半自动化翻译/重排版工具betaVer1.0 - 版面解析模块";
            Load += MainForm_Load;
            flowLayoutPanel_displayMode.ResumeLayout(false);
            flowLayoutPanel_displayMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_pageSelector).EndInit();
            groupBox.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_openRawPDF;
        private Label label1;
        private TextBox textBox_rawPDFPath;
        private FlowLayoutPanel flowLayoutPanel_displayMode;
        private RadioButton radioButton_lookingMode;
        private RadioButton radioButton_viewingMode;
        private RadioButton radioButton_editingMode;
        private Label label2;
        private NumericUpDown numericUpDown_pageSelector;
        private Label label_maxPageCount;
        private OpenFileDialog openFileDialog_rawPDF;
        private UserControls.DocImPlayer docImPlayer;
        private Panel labelBoxConfigPanel;
        private Button button_msgBoxShowLinkNode;
        private GroupBox groupBox;
        private RichTextBox richTextBox1;
        private Label label3;
        private TextBox textBox_xmlDocNamePreview;
        private Button button_saveImageFrame;
        private SaveFileDialog saveFileDialog;
        private ComboBox comboBox1;
        private Label label4;
        private Button button_parseAllPageToXml;
        private FlowLayoutPanel flowLayoutPanel1;
        private RadioButton radioButton_setPageAsFixed;
        private RadioButton radioButton_setPageAsFlowLayouted;
        private Panel panel1;
        private Button button_AutoRelation;
        private CheckBox checkBox1;
        private Button button_generateNewPDF;
        private TreeView treeView1;
        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private Panel panel2;
        private Button button3;
    }
}
