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
using DocTransAppBeta1.PdfStructure;
using DocTransAppBeta1.UserControls.ConfigPanels;

namespace DocTransAppBeta1.UserControls
{
    public partial class DocImPlayer : UserControl
    {
        /// <summary>
        /// 图片对象
        /// </summary>
        private Image __Image;
        //private DocLayoutCollection? __Collection;
        /// <summary>
        /// set写入图片则重绘
        /// </summary>
        public Image Image
        {
            get { return __Image; }
            set
            {
                __Image = value;
                this.Invalidate(); // 强制重绘
            }
        }
        /// <summary>
        /// 本来是打算用无哨兵的链表但是发现操作太墨迹，弃用。
        /// </summary>
        public DocImLabelBox HeadLabelBox { get; set; }
        /// <summary>
        /// 选中的框控件，控件应与解析对象绑定。
        /// </summary>
        private DocImLabelBox? __selectedLabelBox;
        /// <summary>
        /// 用于确定独一无二的那个ListBox控件来显示自由布局的正文顺序
        /// 这个列表进行修改时，如果可以增加需求，建议在每个框的某个角落显示数字代表主干内容的顺序。
        /// 此时需要构建事件，来对其刷新Items列表进行处理。
        /// </summary>
        internal ListBox listBox;
        /// <summary>
        /// 返回选中值，set用于选中项修改后同时修改主界面对应的listbox和对应LabelBox的控制面板
        /// 其中，控制面板实现了ControlPanel这一接口，用于规定父子控件之间互通信息的交互方法
        /// </summary>

        public DocImLabelBox? SelectedLabelBox
        {
            get
            {
                return __selectedLabelBox;
            }
            set
            {
                __selectedLabelBox = value;
                if (value == null) return;
                //接口也可以是空实现要求的接口用来方便类型传递和强制转换
                ConfigPanel panel = value.inquireReturnConfigPanel();
                GetContainer().LoadPanel(panel);
                GetContainer().NotifiedByDocImPlayerForEventOccurred();
            }
        }
        
        //数据同步用，标记本页是不是固定页
        public bool IsFixedPage { get; set; }
        //是否是目录页，同理
        public bool IsContentsPage { get; set; }
        //从listbox1中导出或者使用链表节点导出（打算弃用）
        public DocTransPage CurrentPage
        {
            get
            {
                //处理页
                DocTransPage page = new DocTransPage();
                //初始化值，可以重建DocTransPage构造函数，传入非集合参数，其余的自动new();
                page.IsContentsPage = IsContentsPage;
                page.Abandons = new();
                var box = SelectedLabelBox;
                page.IsFixedPage = IsFixedPage;
                page.Contents = new();
                page.Captions = new();
                Dictionary<DocImLabelBox, DocTransBox> keyValuePairs = new Dictionary<DocImLabelBox, DocTransBox>();
                //构建哈希表并解析Box结构，为什么构建哈希表？因为如果ImLabelBox缓存对象的话逻辑会更复杂
                //GetDocTransBox不支持这一功能，因此先构建存入哈希表。
                //建议修改一下逻辑，这逻辑啰嗦又复杂且难以维护。
                foreach (var i in this.Controls)
                {
                    //遍历确认类型
                    if (i is DocImLabelBox boxs)
                    {
                        //启动解析后的耗时步骤，内部会做OCR操作，建议将OCR操作移植出来，挪动到外部的
                        //Utility方法中。
                        keyValuePairs[boxs] = boxs.GetDocTransBox();
                        //var trans_box = boxs.GetDocTransBox();
                        //if(trans_box is DocTransAbandonBox abandon)
                        //if (boxs.LayoutItem.Label == DocLayoutLabel.abandon)
                        //{
                        //    page.Abandons.Add(boxs.GetDocTransBox());
                        //}
                    }
                }
                foreach (var kvp in keyValuePairs)
                {
                    //构建三种列表，DocTransPage各种参数具体什么意思，详情见其定义
                    if (kvp.Value is DocTransAbandonBox boxw)
                    {
                        page.Abandons.Add(boxw);
                    }
                    else if (
                        kvp.Key.LayoutItem.Label == DocLayoutLabel.figure_caption ||
                        kvp.Key.LayoutItem.Label == DocLayoutLabel.formula_caption ||
                        kvp.Key.LayoutItem.Label == DocLayoutLabel.table_caption ||
                        kvp.Key.LayoutItem.Label == DocLayoutLabel.table_footnote
                        )
                    {
                        page.Captions.Add(kvp.Value);
                    }
                    //C#所有Object类型默认都支持GetType()方法并支持多态。
                    //可以利用GetType方法和switch语句代替判断子类类型
                    else if (kvp.Key.LayoutItem.Label != DocLayoutLabel.unknown)
                    {
                        if (kvp.Value is DocTransFigureBox boxa && boxa.ImCaption != null)
                        {
                            boxa.Caption = (DocTransFigureCaptionBox?)keyValuePairs[boxa.ImCaption];
                        }
                        else if (kvp.Value is DocTransFormulaBox boxaa && boxaa.ImCaption != null)
                        {
                            boxaa.Caption = (DocTransFormulaCaptionBox?)keyValuePairs[boxaa.ImCaption];
                        }
                        else if (kvp.Value is DocTransTableBox boxaaa && boxaaa.ImCaption != null)
                        {
                            boxaaa.Caption = (DocTransTableCaptionBox?)keyValuePairs[boxaaa.ImCaption];
                        }
                        else if (kvp.Value is DocTransTableBox boxaaaa && boxaaaa.ImFootnote != null)
                        {
                            boxaaaa.Footnote = (DocTransTableFootnoteBox?)keyValuePairs[boxaaaa.ImFootnote];
                        }
                        page.Contents.Add(kvp.Value);
                    }
                }
                //if (HeadLabelBox == null) return null;
                //这里的逻辑是对页面正文进行构建，固定页一个DTPage就是某页的所有元素进行存入就完事了
                //不是固定页（自动页）则要排出一个主干列表（不能有Abandon），存入Artery列表中
                if (!page.IsFixedPage)
                {
                    //老链表法，一坨，建议弃用
                    if (HeadLabelBox != null)
                    {
                        page.HeadBox = keyValuePairs[HeadLabelBox];
                        var current = HeadLabelBox;
                        while (current != null && !page.CheckBoxesRelationHaveLoop())
                        {
                            var cont = keyValuePairs[current];
                            cont.IsFixed = false;
                            //page.Contents.AddLast(cont);
                            if (current.NextLabelBox == null)
                            {
                                cont.NextBox = null;
                            }
                            else
                            {
                                var cs = keyValuePairs[current.NextLabelBox];
                                cont.NextBox = cs;
                            }
                            current = current.NextLabelBox;
                        }
                    }
                    //新的方法——直接从ListBox控件的Items列表中获得主干标记顺序，按链表存入
                    else
                    {
                        //ListBox获取控件顺序，对应正文顺序
                        var listimbox = FetchListBox();//GetContainer().FetchListBox();
                        //if(listimbox)
                        if(listBox.Items.Count > 0)
                        {
                            //控件->正文哈希表
                            page.HeadBox = keyValuePairs[listimbox.FirstOrDefault()];
                            for (int i = 0; i < listimbox.Count - 1; i++)
                            {
                                //直接遍历存入Artery当然更好，奈何代码是屎山，不敢乱改逻辑
                                keyValuePairs[listimbox[i]].NextBox = keyValuePairs[listimbox[i + 1]];
                            }
                        }
                    }
                    //调用该方法实现从链表直接转为Artery列表。是否可以对结构进行修改？
                    //以使得HeadBox这一链表头和ImLabelBox以及DTBox中链表相关元素弃用呢？
                    //可以弃用，因为序列化反序列化后，链表项即失效。
                    page.RefreshArtery();
                }
                //else
                //page.CheckBoxesRelationCompleteLoop
                if (page.IsFixedPage)
                {
                    BetaVersionDebugPrinter.WriteLine("固定页处理完成", "Player");
                }
                else
                {
                    bool loop = page.CheckBoxesRelationHaveLoop();
                    bool comp = page.CheckBoxesRelationCompleteness();
                    BetaVersionDebugPrinter.WriteLine($"{(!page.IsContentsPage ? "活动" : "目录")}页处理完成：完整性={comp},有环={loop},半完备={page.CheckBasicCondition()}", "Player");
                } 
                //返回
                return page;
            }
        }
        /// <summary>
        /// 从ListBox拿ImLabelBox们代表的正文顺序
        /// </summary>
        /// <returns></returns>
        public List<DocImLabelBox> FetchListBox()
        {
            List<DocImLabelBox> lbx = new();
            foreach (var i in listBox.Items)
            {
                if (i is DocImLabelBox)
                    lbx.Add(i as DocImLabelBox);
            }
            return lbx;
        }
        //添加正文对象到ListBox
        public void FAdd(DocImLabelBox i)
        {
            //GetContainer().FetchAdd(i);
            listBox.Items.Add(i);
        }
        //从这里获取和写入->每页文档分析的结果，写入后会初始化框的布局并将参数与框绑定
        //一般采用什么方法？懒加载法，ImLabelBox控件修改后，数据同步到ImLabelBox的本地变量
        //然后执行什么？不执行。等待外部通知工作完成，调用ImPlayer内方法，用来一把取走所有ImLabelBox对应的
        //DocTransPage数据。
        public DocLayoutCollection? Collection
        {
            get
            {
                //数据绑定？这做的貌似没有读取，都是在这里写入初始化的。
                DocLayoutCollection collection = new DocLayoutCollection();
                foreach (var ctrl in this.Controls)
                {
                    if (ctrl is DocImLabelBox box)
                    {
                        collection.Add(box.LayoutItem);
                    }
                }
                return collection;
                //return __Collection; 
            }
            set
            {
                //写入空，则隐藏所有框但不删除。其实是用于什么？用于PDF显示器下面的预览模式。
                //隐藏可以单封装一个方法共外部调用。因为不一定什么时候会需要清空这一数据，
                //目前传入null没能力清空。功能被隐藏给占了。
                //右键查找所有引用后你会发现很多赋值collection为null的地方其实就是隐藏所有已经绘制的框的意思。
                var collection = value;
                //__Image = value;
                if (collection == null)
                {
                    if(this.Controls.Count > 0)
                    {
                        foreach(var i in this.Controls)
                        {
                            if(i is UserControl u)
                            {
                                u.Visible = false;
                            }
                        }
                    }
                    //刷新，重新Paint
                    this.Invalidate();
                    return;
                }
                //每次设置都重新添加一边框，恢复隐藏后，也要重新全写一遍，其实挺浪费内存，建议改一下？
                this.Controls.Clear();
                foreach (var item in collection)
                {
                    DocImLabelBox labelBox = new(1.0f * this.Width / __Image.Width);
                    labelBox.LayoutItem = item;
                    this.Controls.Add(labelBox);
                }
                this.Invalidate(); //强制重绘
            }
        }
        //构造函数，设置支持透明双缓冲等，与WPF实际情况不符合。仅作参考
        public DocImPlayer()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);
            this.UpdateStyles();
            IsFixedPage = true;
            IsContentsPage = false;
            
            
            listBox = new ListBox()
            {
                Dock = DockStyle.Fill,
                //这个变量专门用来控制ListBox上条目的显示。结合WPF实际情况。
                DisplayMember = "__g",
            };
        }
        //Player页选中LabelBox
        internal void NotifySelectedLabelBox(DocImLabelBox labelBox)
        {
            //GetContainer().NotifySelectLabelBox();
            SelectedLabelBox = labelBox;
            //!PROC -> selectedLabelBox
            DocLayoutItem layoutItem = SelectedLabelBox.LayoutItem;
        }
        private void DocImPlayer_Load(object sender, EventArgs e)
        {

        }

        private void DocImPlayer_Paint(object sender, PaintEventArgs e)
        {
            if (__Image != null)
            {
                // 计算缩放比例
                float scaleX = (float)this.ClientSize.Width / __Image.Width;
                //float scaleY = (float)this.ClientSize.Height / __Image.Height;
                float scale = scaleX;//Math.Min(scaleX, scaleY);

                // 计算绘制位置
                int drawWidth = (int)(__Image.Width * scale);
                int drawHeight = (int)(__Image.Height * scale);
                //int drawX = (this.ClientSize.Width - drawWidth) / 2;
                //int drawY = (this.ClientSize.Height - drawHeight) / 2;

                //210 毫米 x 297 毫米（或大约 8.27 英寸 x 11.69 英寸）。
                //A4宽度(像素) = (210 毫米 / 25.4 毫米/英寸) * 96 DPI ≈ 794 像素
                //PageSize.A4：这是 iText 提供的一个常量，表示 A4 纸张的尺寸（595 点 x 842 点，基于默认的 72 DPI）

                // 绘制图像
                e.Graphics.DrawImage(__Image, new Rectangle(0, 0, drawWidth, drawHeight));
            }
        }
        //用于子标记框通知父容器删除自身
        internal void NotifyDeleteLabelBox(DocImLabelBox labelBox)
        {
            this.Controls.Remove(labelBox);
            foreach (var kvp in GetContainer().PageCache)
            {
                if (kvp.Key.Equals(this))
                {
                    kvp.Value.Remove(labelBox.LayoutItem);
                }
            }

        }
        //找爸爸函数
        private MainForm? GetContainer()
        {
            return this.Parent == null ? null : (MainForm)this.Parent.Parent;
        }
        //废弃函数，无视
        //internal DocImLabelBox NotifyForSelectedLabelBox() => GetContainer().NotifyForSelectedLabelBox();
        //internal void NotifySelectedLabelBox(DocImLabelBox labelBox) => GetContainer().NotifySelectedLabelBox(labelBox);
        //internal void NotifySetHeadLabelBox(DocImLabelBox labelBox) => GetContainer().NotifySelectHeadLabelBox(labelBox);
        
        //从目前显示的PDF渲染页图片中截取部分，边缘30是因为傻逼OCR有时候不认贴边文字。
        internal Image CropImageFromPlayer(Rectangle cropped, int margin = 30)
        {
            return Utility.ImageAndFontAnalysis.CropImageByRectangle(Image, cropped, margin);
        }
        //添加个新的ImLabelBox控件到ImPlayer
        private void ToolStripMenuItem_addNewBox_Click(object sender, EventArgs e)
        {
            DocImLabelBox labelBox = new(1.0f * this.Width / __Image.Width);
            labelBox.LayoutItem = new DocLayoutItem(DocLayoutLabel.unknown, 0.0f, new RectangleF(0, 0, 500.0f, 500.0f));
            this.Controls.Add(labelBox);
            this.Invalidate();
        }
        //删除所有控件，用来弥补Collection的不足，而且目前考虑到没有外部清空的需求，
        //如果有那就完了，建议改一下那个Collection的绑定和建立新方法来支持需求。
        private void ToolStripMenuItem_clearAllBox_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("确定", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                this.Controls.Clear();
        }
        //删除所有关系，其实就是链表控制，建议完全弃用，菜单还能更精简
        private void ToolStripMenuItem_clearAllBoxRelations_Click(object sender, EventArgs e)
        {
            foreach (var control in this.Controls)
            {
                if (control is DocImLabelBox box)
                {
                    box.NextLabelBox = null;
                    box.LabelBoxCaption = null;
                    box.LabelBoxFootnote = null;
                }
            }
        }
        //右键松开，菜单光标处显现。
        private void DocImPlayer_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(this, e.Location);
            }
        }
    }
}
