using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocTransAppBeta1.DocModules;
using System.Runtime.InteropServices.JavaScript;
using static QuestPDF.Helpers.Colors;
using DocTransAppBeta1.PdfStructure;
using DocTransAppBeta1.UserControls.ConfigPanels;
using System.Security.Cryptography;
using System.Configuration;

namespace DocTransAppBeta1.UserControls
{
    public partial class DocImLabelBox : UserControl
    {
        private static readonly OCRServiceHost ocr_svc = new OCRServiceHost();
        private static readonly StructServiceHost struct_svc = new StructServiceHost();
        private DocTransBox my_box;
        private DocLayoutItem layout_item;
        private ConfigPanel configPanel;
        private readonly float display_scale;
        public DocImLabelBox? NextLabelBox
        {
            get; set;
        }
        public DocImLabelBox? LabelBoxCaption
        {
            get; set;
        }
        public DocImLabelBox? LabelBoxFootnote
        {
            get; set;
        }
        internal ConfigPanel inquireReturnConfigPanel()
        {
            return configPanel;
        }
        public DocTransBox? GetDocTransBox()
        {
            int x = LayoutItem.Box.Location.X * 72 / 300;
            int y = LayoutItem.Box.Location.Y * 72 / 300;
            int w = LayoutItem.Box.Width * 72 / 300;
            int h = LayoutItem.Box.Height * 72 / 300;
            var container = GetContainer();
            if (container == null) return null;
            Image this_Image = container.CropImageFromPlayer(LayoutItem.Box);
            Image save_Image = container.CropImageFromPlayer(LayoutItem.Box, 0);
            string MD5 = Utility.HashCompute.CalculateImageMD5(save_Image);
            if (!Directory.Exists(Path.Combine(MainForm.ProgramPathString, $"images")))
            {
                Directory.CreateDirectory(Path.Combine(MainForm.ProgramPathString, $"images"));
            }
            save_Image.Save(Path.Combine(MainForm.ProgramPathString, $"images\\{MD5}.png"), System.Drawing.Imaging.ImageFormat.Png);

            //my_box 修改
            switch (LayoutItem.Label)
            {
                case DocLayoutLabel.title:
                    var my_box_t = new DocTransTitleBox(x, y, false, w, h, MD5, DocLayoutLabel.title, GetContainer().IsFixedPage);
                    my_box_t.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t.NextBox = null;
                    my_box_t.IsLogo = ((TitleConfigPanel)configPanel).IsLogo;
                    StringBuilder sb = new StringBuilder();
                    if (!my_box_t.IsLogo)
                    {
                        foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                        {
                            sb.Append(s);
                            sb.Append(" ");
                        }
                    }
                    my_box_t.Text = sb.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t.FontName = ((TitleConfigPanel)configPanel).FontLocal.Name;
                    my_box_t.FontSize = (int)((TitleConfigPanel)configPanel).FontLocal.Size;
                    my_box_t.OuterAlignMode = ((TitleConfigPanel)configPanel).OuterAlignMode;
                    my_box_t.InnerAlignMode = ((TitleConfigPanel)configPanel).InnerAlignMode;
                    my_box = my_box_t;
                    //从绑定的Panel取得配置数据
                    break;
                case DocLayoutLabel.plain_text:
                    var my_box_t2 = new DocTransPlainTextBox(x, y, false, w, h, MD5, DocLayoutLabel.plain_text, GetContainer().IsFixedPage);
                    my_box_t2.NextBox = null;
                    my_box_t2.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t2.AutoIndentation = ((PlainTextConfigPanel)configPanel).AutoIndent;
                    StringBuilder sb2 = new StringBuilder();
                    //if (my_box_t2.AutoIndentation) 
                    foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                    {
                        sb2.Append(s);
                        sb2.Append(" ");
                    }
                    my_box_t2.Text = sb2.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t2.FontName = ((PlainTextConfigPanel)configPanel).FontLocal.Name;
                    my_box_t2.FontSize = (int)((PlainTextConfigPanel)configPanel).FontLocal.Size;
                    my_box_t2.OuterAlignMode = ((PlainTextConfigPanel)configPanel).OuterAlignMode;
                    my_box_t2.InnerAlignMode = ((PlainTextConfigPanel)configPanel).InnerAlignMode;
                    //my_box = new DocTransPlainTextBox(x, y, false, w, h, MD5, DocLayoutLabel.plain_text, true);
                    my_box = my_box_t2;
                    break;
                case DocLayoutLabel.figure:
                    var my_box_t3 = new DocTransFigureBox(x, y, false, w, h, MD5, DocLayoutLabel.figure, GetContainer().IsFixedPage);
                    my_box_t3.NextBox = null;
                    my_box_t3.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t3.ImCaption = LabelBoxCaption;
                    //my_box_t3.IsFlowLayoutHead = ((ImageConfigPanel)configPanel).;
                    //StringBuilder sb3 = new StringBuilder();
                    //if (my_box_t3.AutoIndentation) foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                    //{
                    //    sb3.AppendLine(s);
                    //}
                    //my_box_t3.Text = sb3.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t3.AlignMode = ((ImageConfigPanel)configPanel).AlignMode;
                    //my_box_t3.FontSize = (int)((ImageConfigPanel)configPanel).FontLocal.Size;
                    //my_box_t3.OuterAlignMode = ((ImageConfigPanel)configPanel).OuterAlignMode;
                    //my_box_t3.InnerAlignMode = ((PlainTextImageConfigPanelConfigPanel)configPanel).InnerAlignMode;
                    my_box = my_box_t3;
                    break;
                case DocLayoutLabel.figure_caption:
                    var my_box_t6 = new DocTransFigureCaptionBox(x, y, false, w, h, MD5, DocLayoutLabel.figure_caption, GetContainer().IsFixedPage);
                    my_box_t6.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t6.NextBox = null;
                    my_box_t6.IsRawImage = ((CaptionConfigPanel)configPanel).IsRawImage;
                    StringBuilder sb6 = new StringBuilder();
                    if (!my_box_t6.IsRawImage)
                    {
                        foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                        {
                            sb6.AppendLine(s);
                        }
                    }
                    my_box_t6.Text = sb6.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t6.FontName = ((CaptionConfigPanel)configPanel).FontLocal.Name;
                    my_box_t6.FontSize = (int)((CaptionConfigPanel)configPanel).FontLocal.Size;
                    my_box_t6.Position = ((CaptionConfigPanel)configPanel).CaptionPosition;
                    //my_box_t6.InnerAlignMode = ((CaptionConfigPanel)configPanel).InnerAlignMode;
                    my_box = my_box_t6;
                    break;
                case DocLayoutLabel.table:
                    var my_box_t4 = new DocTransTableBox(x, y, false, w, h, MD5, DocLayoutLabel.table, GetContainer().IsFixedPage);
                    my_box_t4.NextBox = null;
                    my_box_t4.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t4.IsRawImage = ((TableConfigPanel)configPanel).IsRawImage;
                    my_box_t4.Text = struct_svc.ImageParseTable(this_Image);
                    my_box_t4.FontName = ((TableConfigPanel)configPanel).FontLocal.Name;
                    my_box_t4.FontSize = (int)((TableConfigPanel)configPanel).FontLocal.Size;
                    my_box_t4.AlignMode = ((TableConfigPanel)configPanel).AlignMode;
                    my_box_t4.ImCaption = LabelBoxCaption;
                    my_box_t4.ImFootnote = LabelBoxFootnote;
                    my_box = my_box_t4;
                    break;
                case DocLayoutLabel.table_caption:
                    var my_box_t7 = new DocTransTableCaptionBox(x, y, false, w, h, MD5, DocLayoutLabel.table_caption, GetContainer().IsFixedPage);
                    my_box_t7.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t7.NextBox = null;
                    my_box_t7.IsRawImage = ((CaptionConfigPanel)configPanel).IsRawImage;
                    StringBuilder sb7 = new StringBuilder();
                    if (!my_box_t7.IsRawImage)
                    {
                        foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                        {
                            sb7.AppendLine(s);
                        }
                    }
                    my_box_t7.Text = sb7.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t7.FontName = ((CaptionConfigPanel)configPanel).FontLocal.Name;
                    my_box_t7.FontSize = (int)((CaptionConfigPanel)configPanel).FontLocal.Size;
                    my_box_t7.Position = ((CaptionConfigPanel)configPanel).CaptionPosition;
                    my_box = my_box_t7;
                    break;
                case DocLayoutLabel.table_footnote:
                    var my_box_t8 = new DocTransTableFootnoteBox(x, y, false, w, h, MD5, DocLayoutLabel.table_footnote, GetContainer().IsFixedPage);
                    my_box_t8.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t8.NextBox = null;
                    my_box_t8.IsRawImage = ((CaptionConfigPanel)configPanel).IsRawImage;
                    StringBuilder sb8 = new StringBuilder();
                    if (!my_box_t8.IsRawImage)
                    {
                        foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                        {
                            sb8.AppendLine(s);
                        }
                    }
                    my_box_t8.Text = sb8.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t8.FontName = ((CaptionConfigPanel)configPanel).FontLocal.Name;
                    my_box_t8.FontSize = (int)((CaptionConfigPanel)configPanel).FontLocal.Size;
                    my_box_t8.Position = ((CaptionConfigPanel)configPanel).CaptionPosition;
                    my_box = my_box_t8;
                    break;
                case DocLayoutLabel.isolate_formula:
                    var my_box_t5 = new DocTransFormulaBox(x, y, false, w, h, MD5, DocLayoutLabel.isolate_formula, GetContainer().IsFixedPage);
                    my_box_t5.NextBox = null;
                    my_box_t5.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t5.IsRawImage = ((FormulaConfigPanel)configPanel).IsRawImage;
                    my_box_t5.LaTeXExpression = struct_svc.ImageParseTable(this_Image);
                    my_box_t5.ImCaption = LabelBoxCaption;
                    //my_box_t5.FontName = ((FormulaConfigPanel)configPanel).FontLocal.Name;
                    //my_box_t5.FontSize = (int)((FormulaConfigPanel)configPanel).FontLocal.Size;
                    my_box_t5.AlignMode = ((FormulaConfigPanel)configPanel).AlignMode;
                    my_box = my_box_t5;
                    break;
                case DocLayoutLabel.formula_caption:
                    var my_box_t9 = new DocTransFormulaCaptionBox(x, y, false, w, h, MD5, DocLayoutLabel.formula_caption, GetContainer().IsFixedPage);
                    my_box_t9.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t9.NextBox = null;
                    my_box_t9.IsRawImage = ((CaptionConfigPanel)configPanel).IsRawImage;
                    StringBuilder sb77 = new StringBuilder();
                    if (!my_box_t9.IsRawImage)
                    {
                        foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                        {
                            sb77.AppendLine(s);
                        }
                    }
                    my_box_t9.Text = sb77.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t9.FontName = ((CaptionConfigPanel)configPanel).FontLocal.Name;
                    my_box_t9.FontSize = (int)((CaptionConfigPanel)configPanel).FontLocal.Size;
                    my_box_t9.Position = ((CaptionConfigPanel)configPanel).CaptionPosition;
                    my_box = my_box_t9;
                    break;
                case DocLayoutLabel.abandon:
                    var my_box_t0 = new DocTransAbandonBox(x, y, false, w, h, MD5, DocLayoutLabel.abandon, true);// GetContainer().IsFixedPage);
                    my_box_t0.IsFlowLayoutHead = IsFlowLayoutHead;
                    my_box_t0.NextBox = null;
                    my_box_t0.IsLogo = ((AbandonConfigPanel)configPanel).IsLogo;
                    StringBuilder sb777 = new StringBuilder();
                    if (!my_box_t0.IsLogo)
                    {
                        foreach (var s in ocr_svc.ImageRecognizeWithCRLF(this_Image))
                        {
                            sb777.AppendLine(s);
                        }
                    }
                    my_box_t0.Text = sb777.ToString();
                    //my_box_t.IsFixed = MainForm处理取得
                    my_box_t0.FontName = ((AbandonConfigPanel)configPanel).FontLocal.Name;
                    my_box_t0.FontSize = (int)((AbandonConfigPanel)configPanel).FontLocal.Size;
                    my_box_t0.Type = ((AbandonConfigPanel)configPanel).AbandonType;
                    my_box_t0.OuterAlignMode = ((AbandonConfigPanel)configPanel).OuterAlignMode;
                    my_box_t0.InnerAlignMode = ((AbandonConfigPanel)configPanel).InnerAlignMode;
                    my_box = my_box_t0;
                    break;
            }
            return my_box;
        }
        public DocLayoutItem LayoutItem
        {
            get { return layout_item; }
            set
            {
                //修改Loc和Sz
                this.Location = new Point((int)Math.Floor(value.Box.Location.X * display_scale), (int)Math.Floor(value.Box.Location.Y * display_scale));
                this.Size = new Size((int)Math.Ceiling(value.Box.Width * display_scale), (int)Math.Ceiling(value.Box.Height * display_scale));
                layout_item = value;
                this.Invalidate();
                //初始化configPanel
                switch (value.Label)
                {
                    case DocLayoutLabel.title:
                        configPanel = new TitleConfigPanel();
                        break;
                    case DocLayoutLabel.abandon:
                        configPanel = new AbandonConfigPanel();
                        break;
                    case DocLayoutLabel.plain_text:
                        configPanel = new PlainTextConfigPanel();
                        break;
                    case DocLayoutLabel.figure:
                        configPanel = new ImageConfigPanel();
                        break;
                    case DocLayoutLabel.table:
                        configPanel = new TableConfigPanel();
                        break;

                    case DocLayoutLabel.isolate_formula:
                        configPanel = new FormulaConfigPanel();
                        break;
                    case DocLayoutLabel.figure_caption:
                    case DocLayoutLabel.table_caption:
                    case DocLayoutLabel.table_footnote:
                    case DocLayoutLabel.formula_caption:
                    default:
                        configPanel = new CaptionConfigPanel();
                        break;
                }
                ((UserControl)configPanel).Dock = DockStyle.Fill;
            }
        }
        private DocImPlayer? GetContainer()
        {
            return this.Parent == null ? null : (DocImPlayer)this.Parent;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ratio">比例为父控件宽度/图片宽度</param>
        public DocImLabelBox(float ratio)
        {
            display_scale = ratio;//图片控件间交流都是默认300dpi，定死了
            InitializeComponent();
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();

            this.IsFlowLayoutHead = false;
        }

        private void DocImLabelBox_Load(object sender, EventArgs e)
        {

        }

        private void DocImLabelBox_Paint(object sender, PaintEventArgs e)
        {
            Color color = DocLayoutAnalyzer.labelNumToColors[layout_item.LabelNumber];
            //base.OnPaint(e);
            using (Pen pen = new Pen(color, 3))
            {
                SolidBrush sbrush = new SolidBrush(color);
                e.Graphics.DrawString(layout_item.LabelName, new Font("Arial", 12, FontStyle.Regular), sbrush, 0, 0);
                RectangleF drawRect = this.ClientRectangle;
                drawRect.Height -= 2;
                drawRect.Width -= 2;
                drawRect.X++;
                drawRect.Y++;
                e.Graphics.DrawRectangle(pen, drawRect);
                //e.Graphics.DrawRectangle(new Pen(Color.Transparent), this.ClientRectangle);
            }
        }

        //绘制维护的变量
        private bool __is_Down = false;
        private int __position = 0;
        private Point __last_mouse_loc = new Point();
        private Point __last_ctrl_loc;
        private Size __last_ctrl_size;
        private void DocImLabelBox_MouseDown(object sender, MouseEventArgs e)
        {
            __is_Down = true;
            __last_mouse_loc = e.Location;
            __last_ctrl_loc = this.Location;
            __last_ctrl_size = this.Size;
        }

        private void DocImLabelBox_MouseMove(object sender, MouseEventArgs e)
        {
            int dist_threshold = 6;
            if (__is_Down)
            {
                var pos_delta_x = e.Location.X - __last_mouse_loc.X;
                var pos_delta_y = e.Location.Y - __last_mouse_loc.Y;

                if (e.Button == MouseButtons.Left && __position == 0)
                {
                    Point pp = this.Location;
                    Size ss = this.Size;
                    //平移
                    pp.X += pos_delta_x;
                    pp.Y += pos_delta_y;
                    this.Location = pp;
                    var rectF = layout_item.BoxFloat;
                    rectF.X = (float)Math.Floor(pp.X / display_scale);
                    rectF.Y = (float)Math.Floor(pp.Y / display_scale);
                    layout_item.BoxFloat = rectF;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    Point pp = __last_ctrl_loc;
                    Size ss = __last_ctrl_size;
                    //找八角
                    switch (__position)
                    {
                        case 3:
                            ss.Width += pos_delta_x;
                            break;
                        case 4:
                            ss.Width += pos_delta_x;
                            ss.Height += pos_delta_y;
                            break;
                        case 5:
                            ss.Height += pos_delta_y;
                            break;
                        default:
                            break;
                    }
                    //this.Location = pp;
                    if (ss.Width < dist_threshold * 2)
                    {
                        ss.Width = dist_threshold * 2;
                    }
                    if (ss.Height < dist_threshold * 2)
                    {
                        ss.Height = dist_threshold * 2;
                    }
                    this.Size = ss;
                    var rectF = layout_item.BoxFloat;
                    //rectF.X = (float)Math.Floor(pp.X / display_scale);
                    //rectF.Y = (float)Math.Floor(pp.Y / display_scale);
                    rectF.Width = (float)Math.Floor(ss.Width / display_scale);
                    rectF.Height = (float)Math.Floor(ss.Height / display_scale);
                    layout_item.BoxFloat = rectF;
                }
            }
            //设置position 顺时针1到8，0为清除
            /* 8 1 2
             * 7   3
             * 6 5 4
             */
            else
            {
                Point p = this.Location;
                Size s = this.Size;
                int left_dist = e.Location.X - 0;
                int right_dist = 0 + s.Width - e.Location.X;
                int up_dist = e.Location.Y - 0;
                int down_dist = 0 + s.Height - e.Location.Y;

                if (left_dist < 0 || right_dist < 0 || up_dist < 0 || down_dist < 0)
                {
                    __position = 0;
                }
                else if (right_dist < dist_threshold && down_dist < dist_threshold)
                {
                    __position = 4;
                }
                else if (right_dist < dist_threshold && up_dist > dist_threshold && down_dist > dist_threshold)
                {
                    __position = 3;
                }
                else if (down_dist < dist_threshold && left_dist > dist_threshold && right_dist > dist_threshold)
                {
                    __position = 5;
                }
                else __position = 0;
                switch (__position)
                {
                    case 2:
                    case 6:
                        this.Cursor = Cursors.SizeNESW;
                        break;
                    case 4:
                    case 8:
                        this.Cursor = Cursors.SizeNWSE;
                        break;
                    case 1:
                    case 5:
                        this.Cursor = Cursors.SizeNS;
                        break;
                    case 3:
                    case 7:
                        this.Cursor = Cursors.SizeWE;
                        break;
                    default:
                        this.Cursor = Cursors.SizeAll;
                        break;
                }
            }
        }

        private void DocImLabelBox_MouseUp(object sender, MouseEventArgs e)
        {
            __is_Down = false;
            __position = 0;
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(this, e.Location);
            }
        }

        private void DocImLabelBox_MouseLeave(object sender, EventArgs e)
        {
            __is_Down = false;
            __position = 0;
        }

        private void ToolStripMenuItem_selectTargetLabelBox_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                GetContainer().SelectedLabelBox = this;
            }
        }

        private void ToolStripMenuItem_setAsSelectedLabelBoxNext_Click(object sender, EventArgs e)
        {
            try
            {
                GetContainer().SelectedLabelBox.NextLabelBox = this;
            }
            catch { MessageBox.Show("还未选中任何块。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
            BetaVersionDebugPrinter.WriteLine("选中块的后继设置好了", "_setAsSelectedLabelBoxNext");
        }

        private void ToolStripMenuItem_deleteThisLabelBox_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                if (MessageBox.Show("删吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    GetContainer().NotifyDeleteLabelBox(this);
            }
            BetaVersionDebugPrinter.WriteLine("离开删除块逻辑", "_deleteThisLabelBox");
        }

        private void ToolStripMenuItem_markAsHead_Click(object sender, EventArgs e)
        {
            this.IsFlowLayoutHead = true;
        }

        private void ToolStripMenuItem_markAsPageBegin_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                if (MessageBox.Show("确定设吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    //GetContainer().NotifySetHeadLabelBox(this);
                    GetContainer().HeadLabelBox = this;
            }
            BetaVersionDebugPrinter.WriteLine("离开标记头逻辑", "_markAsHead");
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            //Obsolete
        }

        private void ToolStripMenuItem_unmarkAsHead_Click(object sender, EventArgs e)
        {
            this.IsFlowLayoutHead = false;
        }

        private void ToolStripMenuItem_setAsTableFootnote_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                if (GetContainer().SelectedLabelBox.LayoutItem.Label != DocLayoutLabel.table)
                {
                    if (MessageBox.Show("确定footnode吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        GetContainer().SelectedLabelBox.LabelBoxFootnote = this;
                }
                else MessageBox.Show("当前选中不是Table.");
            }
            BetaVersionDebugPrinter.WriteLine("离开Footnote逻辑", "_markAsHead");
        }

        private void ToolStripMenuItem_setAsSelectedLabelBoxCap_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                if (GetContainer().SelectedLabelBox.LayoutItem.Label != DocLayoutLabel.table)
                {
                    if (MessageBox.Show("确定footnode吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        GetContainer().SelectedLabelBox.LabelBoxCaption = this;
                }
                else MessageBox.Show("当前选中不是Table.");
            }
            BetaVersionDebugPrinter.WriteLine("离开Caption逻辑", "_markAsHead");
        }

        private void 添加到列表中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetContainer().FAdd(this);
        }

        private bool IsFlowLayoutHead { get; set; }
        //private Font FontLocal { get; set; }
        //private BoxAlignMode InnerAlignMode { get; set; }
        //private BoxAlignMode OuterAlignMode { get; set; }
        public override string ToString()
        {
            return LayoutItem.LabelName;//base.ToString();
        }
        public string __g
        {
            get
            {
                return LayoutItem.LabelName;
            }
        }
    }
}
