using DocTransAppBeta1.DocModules;
using DocTransAppBeta1.LLMTranslationMod;
using DocTransAppBeta1.MarkupParser;
using DocTransAppBeta1.PdfStructure;
using DocTransAppBeta1.UserControls;
using DocTransAppBeta1.UserControls.ConfigPanels;
using PdfiumViewer;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data.Common;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace DocTransAppBeta1
{
    /// <summary>
    /// 很多逻辑都堆到一起的“主窗体”
    /// </summary>
    public partial class MainForm : Form
    {
        //项目目录
        public static string? ProgramPathString { get; set; }
        public static List<DocTransPage> ProcPages { get; set; }
        //本地变量组
        PdfiumViewer.PdfDocument? document;
        Image? current_page = null;
        DocLayoutCollection? current_page_layout_collection = null;
        DocImLabelBox? headLabelBox_forFlowLayout = null;

        //构造函数
        public MainForm()
        {
            InitializeComponent();
            //BetaVersionDebugPrinter.WriteLine("", "Form1.Form1()");
            //PdfViewer pv = new PdfViewer();
            //pv.Size = new System.Drawing.Size(595, 842);
            //this.Controls.Add(pv);
        }
        //静态构造函数
        static MainForm() { ProcPages = new(); }
        //打开PDF按钮
        private void button_openRawPDF_Click(object sender, EventArgs e)
        {
            //打开
            if (openFileDialog_rawPDF.ShowDialog() == DialogResult.OK)
            {
                //打开OK了，绑定TextChange事件读取PDF，需要做异常处理，如果是非法PDF文件怎么办？
                textBox_rawPDFPath.Text = openFileDialog_rawPDF.FileName;
                ProgramPathString = Path.GetDirectoryName(openFileDialog_rawPDF.FileName);
            }
            //打开之前清除目录下的解析结果，最好是可以保存解析结果，
            //以提升很多页文档（50页+）的解析效率（意思就是磁盘缓存，可以选择性实现）
            //保存为XML，如果目录下有缓存文件（cache），则读取，没缓存的页继续做版面解析。
            try
            {
                Directory.Delete(Path.Combine(ProgramPathString, "images"), true);
            }
            catch { }
            try
            {
                Directory.Delete(Path.Combine(ProgramPathString, "pages"), true);
            }
            catch { }
        }
        //加速识别的异步方法
        private void __ocr(int index, Image img)
        {
            //DocImPlayer player = new DocImPlayer();
            //player.Dock = DockStyle.Fill;
            //float ratio = 0.0f;
            //int offset = 0;
            //var input_tensor = DocLayoutAnalyzer.ConvertImageToTensor(img, out ratio, out offset);
            //var inference_result = analyzer.Inference(input_tensor);
            //DocLayoutAnalyzer.OutputFittingWithRatio(ref inference_result, ratio, offset);
            //var collection = DocLayoutAnalyzer.DocLayoutInferenceResultTransform(inference_result);
            ////current_page_layout_collection = collection;
            //player.Image = img;
            //player.Collection = collection;
            //player.Collection = null;

            //PageCache.Add(KeyValuePair.Create<DocImPlayer, DocLayoutCollection>(player, collection));
            //BetaVersionDebugPrinter.WriteLine($"第{index + 1}页处理完。", "Loader");
        }
        //打开PDF的逻辑
        private void textBox_rawPDFPath_TextChanged(object sender, EventArgs e)
        {
            using (DocLayoutAnalyzer analyzer = new DocLayoutAnalyzer())
            {
                PageCache.Clear();
                //此处处理文件打开逻辑
                document = PdfDocument.Load(textBox_rawPDFPath.Text);
                int pageCount = document.PageCount;
                //int dpi = 300;//2550*3300 dpi300
                current_page = document.Render(0, 300, 300, PdfRenderFlags.CorrectFromDpi);
                //docImPlayer.Image = current_page;
                docImPlayer = new DocImPlayer();
                docImPlayer.Dock = DockStyle.Fill;
                docImPlayer.listBox = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    DisplayMember = "__g",
                };

                //推理逻辑，存在图像在输入推理器DocLayoutAnalyzer时的像素偏移和比例变化
                //使用引用传递和获取多参数。
                float ratiox = 0.0f;
                int offsetx = 0;
                var input_tensorx = DocLayoutAnalyzer.ConvertImageToTensor(current_page, out ratiox, out offsetx);
                var inference_resultx = analyzer.Inference(input_tensorx);
                DocLayoutAnalyzer.OutputFittingWithRatio(ref inference_resultx, ratiox, offsetx);
                var collectionx = DocLayoutAnalyzer.DocLayoutInferenceResultTransform(inference_resultx);
                current_page_layout_collection = collectionx;
                docImPlayer.Image = current_page;

                docImPlayer.listBox = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    DisplayMember = "__g",
                };
                //建议重写PDF加载策略。插入缓存的应用，缓存存入目录下cache文件夹。
                //分离Render（渲染），Inference（版面解析的推理），cache（缓存）和同步到UI
                //分步进行。
                docImPlayer.Collection = collectionx;
                docImPlayer.Collection = null;
                PageCache.Add(KeyValuePair.Create<DocImPlayer, DocLayoutCollection>(docImPlayer, collectionx));
                BetaVersionDebugPrinter.WriteLine($"第{1}页处理完。", "Loader");

                //使用异步等待方法加速识别
                //ThreadPool.SetMaxThreads(3, 3);
                //List<Task> ocrtaskList = new List<Task>();
                for (int i = 1; i < pageCount; i++)
                {
                    Image img = document.Render(i, 300, 300, PdfRenderFlags.CorrectFromDpi);
                    //ocrtaskList.Add(Task.Run(
                    //    () => __ocr(i, img)
                    //));
                    DocImPlayer player = new DocImPlayer();
                    player.Dock = DockStyle.Fill;
                    float ratio = 0.0f;
                    int offset = 0;
                    //Image转输入张量
                    var input_tensor = DocLayoutAnalyzer.ConvertImageToTensor(img, out ratio, out offset);
                    //推理
                    var inference_result = analyzer.Inference(input_tensor);
                    //消除输出张量中的偏移影响
                    DocLayoutAnalyzer.OutputFittingWithRatio(ref inference_result, ratio, offset);
                    //获得可读的推理结果集合
                    var collection = DocLayoutAnalyzer.DocLayoutInferenceResultTransform(inference_result);
                    //current_page_layout_collection = collection;
                    player.Image = img;
                    player.Collection = collection;
                    player.Collection = null;
                    //把页的Player和处理完的Collection塞进页缓存
                    PageCache.Add(KeyValuePair.Create<DocImPlayer, DocLayoutCollection>(player, collection));
                    BetaVersionDebugPrinter.WriteLine($"第{i + 1}页处理完。", "Loader");
                }
                //Task.WaitAll(ocrtaskList.ToArray());

                //初始化一些UI控件
                label_maxPageCount.Text = "/" + pageCount;
                numericUpDown_pageSelector.Value = 1;
                numericUpDown_pageSelector.Minimum = 1;
                numericUpDown_pageSelector.Maximum = pageCount;
                numericUpDown_pageSelector.Enabled = true;
                flowLayoutPanel_displayMode.Enabled = true;
                radioButton_lookingMode.Checked = true;

                //current_page = document.Render((int)(numericUpDown_pageSelector.Value - 1), 300, 300, PdfRenderFlags.CorrectFromDpi);
                //float ratio = 0.0f;
                //int offset = 0;
                //var input_tensor = DocLayoutAnalyzer.ConvertImageToTensor(current_page, out ratio, out offset);
                //var inference_result = analyzer.Inference(input_tensor);
                //DocLayoutAnalyzer.OutputFittingWithRatio(ref inference_result, ratio, offset);
                //var collection = DocLayoutAnalyzer.DocLayoutInferenceResultTransform(inference_result);
                //current_page_layout_collection = collection;
            }
            //强制回收垃圾
            System.GC.Collect();
        }
        //下面的修改模式为查看模式，没框（隐藏）
        private void radioButton_lookingMode_CheckedChanged(object sender, EventArgs e)
        {
            if (current_page != null && radioButton_lookingMode.Checked)
            {
                docImPlayer.Collection = null;
                docImPlayer.Image = current_page;
                labelBoxConfigPanel.Controls.Clear();
                comboBox1.SelectedIndex = 10;
            }

        }
        //预览模式，绘制Collection到Image
        private void radioButton_viewingMode_CheckedChanged(object sender, EventArgs e)
        {
            if (current_page != null && current_page_layout_collection != null && radioButton_viewingMode.Checked)
            {
                var image = DocLayoutAnalyzer.DrawBoundingBoxesOnImage(current_page, current_page_layout_collection);
                GC.Collect();
                docImPlayer.Collection = null;
                docImPlayer.Image = image;
                labelBoxConfigPanel.Controls.Clear();
                comboBox1.SelectedIndex = 10;
            }
        }
        //编辑模式，最重要的，调整版面
        private void radioButton_editingMode_CheckedChanged(object sender, EventArgs e)
        {
            if (current_page != null && current_page_layout_collection != null && radioButton_editingMode.Checked)
            {
                //var image = DocLayoutAnalyzer.DrawBoundingBoxesOnImage(current_page, current_page_layout_collection);
                GC.Collect();
                docImPlayer.Collection = current_page_layout_collection;
                docImPlayer.Image = current_page;
            }
        }
        //弃用
        [Obsolete]
        public List<DocImLabelBox> FetchListBox()
        {
            List<DocImLabelBox> lbx = new();
            foreach (var i in listBox1.Items)
            {
                if (i is DocImLabelBox)
                    lbx.Add(i as DocImLabelBox);
            }
            return lbx;
        }
        //没啥用
        private void MainForm_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "__g";
            //listBox1.Items.Add("sb");
            //IsFixedPage = true;
            //analyzer = new DocLayoutAnalyzer();
            //PaddleOCR.PaddleOCR.Initialize();
            //PaddleOCR.PaddleOCR.Recognize();
        }
        //页缓存
        public List<KeyValuePair<DocImPlayer, DocLayoutCollection>> PageCache = new List<KeyValuePair<DocImPlayer, DocLayoutCollection>>();
        //切换页，从页缓存PageCache读取处理好的结果
        private void numericUpDown_pageSelector_ValueChanged(object sender, EventArgs e)
        {
            if (document != null)
            {
                //listBox1.Items.Clear();
                //current_page = document.Render((int)(numericUpDown_pageSelector.Value - 1), 300, 300, PdfRenderFlags.CorrectFromDpi);
                //float ratio = 0.0f;
                //int offset = 0;
                //var input_tensor = DocLayoutAnalyzer.ConvertImageToTensor(current_page, out ratio, out offset);
                //var inference_result = analyzer.Inference(input_tensor);
                //DocLayoutAnalyzer.OutputFittingWithRatio(ref inference_result, ratio, offset);
                //var collection = DocLayoutAnalyzer.DocLayoutInferenceResultTransform(inference_result);

                //就是切换UI中对应的控件
                panel1.Controls.Clear();
                panel2.Controls.Clear();
                var pg = PageCache[(int)(numericUpDown_pageSelector.Value - 1)];
                current_page_layout_collection = pg.Value;
                current_page = document.Render((int)(numericUpDown_pageSelector.Value - 1), 300, 300, PdfRenderFlags.CorrectFromDpi);

                docImPlayer = pg.Key;
                listBox1 = pg.Key.listBox;
                docImPlayer.Image = current_page;
                panel1.Controls.Add(docImPlayer);
                panel2.Controls.Add(docImPlayer.listBox);
            }
            //切换单选框显示模式的续集
            if (current_page != null && current_page_layout_collection != null && radioButton_editingMode.Checked)
            {
                //var image = DocLayoutAnalyzer.DrawBoundingBoxesOnImage(current_page, current_page_layout_collection);
                GC.Collect();
                docImPlayer.Collection = current_page_layout_collection;
                docImPlayer.Image = current_page;
            }

            if (current_page != null && current_page_layout_collection != null && radioButton_viewingMode.Checked)
            {
                var image = DocLayoutAnalyzer.DrawBoundingBoxesOnImage(current_page, current_page_layout_collection);
                GC.Collect();
                docImPlayer.Collection = null;
                docImPlayer.Image = image;
            }

            if (current_page != null && radioButton_lookingMode.Checked)
            {
                docImPlayer.Collection = null;
                docImPlayer.Image = current_page;
            }
            flowLayoutPanel1.Enabled = true;
            //radioButton_setPageAsFixed.Checked = true;
            comboBox1.SelectedIndex = 10;
            labelBoxConfigPanel.Controls.Clear();
            if (docImPlayer.IsFixedPage)
                radioButton_setPageAsFixed.Checked = true;
            else radioButton_setPageAsFlowLayouted.Checked = true;
        }
        //通知主窗体选中框，（可跨页保留）
        //internal void NotifySelectedLabelBox(DocImLabelBox labelBox)
        //{
        //    //GetContainer().NotifySelectLabelBox();
        //    selectedLabelBox = labelBox;
        //    //!PROC -> selectedLabelBox
        //    DocLayoutItem layoutItem = selectedLabelBox.LayoutItem;
        //}
        [Obsolete]
        internal void FetchClearList()
        {
            listBox1.ClearSelected();
        }
        [Obsolete]
        internal void NotifySelectHeadLabelBox(DocImLabelBox labelBox)
        {
            //throw new NotImplementedException();

            headLabelBox_forFlowLayout = labelBox;
        }

        //internal DocImLabelBox NotifyForSelectedLabelBox()
        //{
        //    //throw new NotImplementedException();
        //    return selectedLabelBox;
        //}
        //看链表
        private void button_msgBoxShowLinkNode_Click(object sender, EventArgs e)
        {
            //如果舍弃链表的话，这个也一点β用没有
            StringBuilder sb = new StringBuilder();
            DocImLabelBox? curr = docImPlayer.HeadLabelBox;
            while (curr != null)
            {
                var la = curr.LayoutItem;
                sb.AppendLine($"{la.LabelName,-20}\t[{la.Confidence.ToString("F4"),-6}]\t({la.Box.X,-4},{la.Box.Y,4})\t{{{la.Box.Width,-4},{la.Box.Height,4}}}");
                curr = curr.NextLabelBox;
            }
            MessageBox.Show(sb.ToString());
        }
        [Obsolete]
        private void button_fixedPageParse_Click(object sender, EventArgs e)
        {
            //PdfiumViewer.PdfDocument? document;
            //Image? current_page = null;
            //DocLayoutAnalyzer analyzer = new DocLayoutAnalyzer();
            //DocLayoutCollection? current_page_layout_collection = null;
            //DocImLabelBox? headLabelBox_forFlowLayout = null;
            numericUpDown_pageSelector.Value = 1;
        }
        //把目前显示的页保存下来
        private void button_saveImageFrame_Click(object sender, EventArgs e)
        {
            saveFileDialog.Title = "保存标记页";
            if (saveFileDialog.ShowDialog() == DialogResult.OK && current_page != null && current_page_layout_collection != null)
                DocLayoutAnalyzer.
                    DrawBoundingBoxesOnImage(current_page, current_page_layout_collection)
                    .Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        //public bool IsFixedPage { get; set; }
        //设置为固定页
        private void radioButton_setPageAsFixed_CheckedChanged(object sender, EventArgs e)
        {
            docImPlayer.IsFixedPage = true;
        }
        //设置为自动页
        private void radioButton_setPageAsFlowLayouted_CheckedChanged(object sender, EventArgs e)
        {
            docImPlayer.IsFixedPage = false;
        }
        //加载对应panel到
        internal void LoadPanel(ConfigPanel panel)
        {
            //throw new NotImplementedException();
            labelBoxConfigPanel.Controls.Clear();
            labelBoxConfigPanel.Controls.Add((UserControl)panel);
        }
        //通知选中了某Box，更新UI
        internal void NotifiedByDocImPlayerForEventOccurred()
        {
            var label = docImPlayer.SelectedLabelBox;
            comboBox1.SelectedIndex = label.LayoutItem.LabelNumber;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (docImPlayer.SelectedLabelBox == null || comboBox1.SelectedIndex == 10) return;
            docImPlayer.SelectedLabelBox.LayoutItem.Label = (DocLayoutLabel)comboBox1.SelectedIndex;
            docImPlayer.Invalidate();
        }
        /// <summary>
        /// 很重要的函数——页面解析
        /// 功能不复杂，就是啰嗦，要针对各种类型写差不多一样的方法，
        /// 目前没找到降低行数的方法sorry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_previewThisNext_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string s = docImPlayer.SelectedLabelBox.NextLabelBox.LayoutItem.LabelName;
            //    MessageBox.Show(s);
            //}
            //catch
            //{
            //    BetaVersionDebugPrinter.WriteLine("发生错误，可能是没Next或者Select", "_markAsHead");
            //}
            DocTransDocument document = new DocTransDocument();
            List<DocTransPage> pages = new List<DocTransPage>();
            for (int i = 0; i < PageCache.Count; i++)
            {
                var kvp = PageCache[i];
                var uts = kvp.Key;
                if (uts.HeadLabelBox == null && !uts.IsFixedPage)
                {
                    //MessageBox.Show($"警告{i + 1}页没有页头。");
                    //return;
                }
                var page = uts.CurrentPage;

                pages.Add(page);
                BetaVersionDebugPrinter.WriteLine($"导出第{i}页了", "parser");
            }
            BetaVersionDebugPrinter.WriteLine($"导出结构体完成", "parser");
            if (false)
                //启动翻译（显而易见的逻辑）
                foreach (var pg in pages)
                {
                    if (pg.IsFixedPage)
                    {
                        foreach (var box in pg.Contents)
                        {
                            if (box is DocTransTitleBox b1)
                            {
                                if (!b1.IsLogo)
                                {
                                    string u = "";
                                    OllamaApis.DoTextSegmentSync(b1.Text, out u);
                                    b1.Text = u;
                                    string v = "";
                                    OllamaApis.TranslateTextSync(u, out v);
                                    b1.TranslatedText = v;
                                }
                            }
                            else if (box is DocTransPlainTextBox b2)
                            {
                                string u = "";
                                string k = (b2.AutoIndentation ? "\t" : "") + b2.Text;
                                OllamaApis.DoTextSegmentSync(k, out u);
                                b2.Text = u;
                                string v = "";
                                OllamaApis.TranslateTextSync(u, out v);
                                b2.TranslatedText = v;
                            }
                            else if (box is DocTransAbandonBox b3)
                            {
                                if (!b3.IsLogo)
                                {
                                    string u = "";
                                    OllamaApis.DoTextSegmentSync(b3.Text, out u);
                                    b3.Text = u;
                                    string v = "";
                                    OllamaApis.TranslateTextSync(u, out v);
                                    b3.TranslatedText = v;
                                }
                            }
                            //else if (box is DocTransFigureBox b4)
                            //{
                            //}
                            else if (box is DocTransTableBox b5)
                            {
                                if (!b5.IsRawImage)
                                {
                                    string u = "";
                                    OllamaApis.TranslateHTMLSync(b5.Text, out u);
                                    b5.TranslatedText = u;
                                }
                            }
                            //else if (box is DocTransFormulaBox b6)
                            //{
                            //}
                            else if (box is DocTransCaptionBox b7)
                            {
                                if (!b7.IsRawImage)
                                {
                                    string u = "";
                                    OllamaApis.DoTextSegmentSync(b7.Text, out u);
                                    b7.Text = u;
                                    string v = "";
                                    OllamaApis.TranslateTextSync(u, out v);
                                    b7.TranslatedText = v;
                                }
                            }
                        }
                        foreach (var box in pg.Abandons)
                        {
                            if (box is DocTransAbandonBox b1)
                            {
                                if (!b1.IsLogo)
                                {
                                    string u = "";
                                    OllamaApis.DoTextSegmentSync(b1.Text, out u);
                                    b1.Text = u;
                                    string v = "";
                                    OllamaApis.TranslateTextSync(u, out v);
                                    b1.TranslatedText = v;
                                }
                            }
                        }
                        foreach (var box in pg.Captions)
                        {
                            if (box is DocTransCaptionBox b1)
                            {
                                if (!b1.IsRawImage)
                                {
                                    string u = "";
                                    OllamaApis.DoTextSegmentSync(b1.Text, out u);
                                    b1.Text = u;
                                    string v = "";
                                    OllamaApis.TranslateTextSync(u, out v);
                                    b1.TranslatedText = v;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var box in pg.Artery)
                        {
                            if (box is DocTransTitleBox b1)
                            {
                                if (!b1.IsLogo)
                                {
                                    string u = "";
                                    OllamaApis.DoTextSegmentSync(b1.Text, out u);
                                    b1.Text = u;
                                    string v = "";
                                    OllamaApis.TranslateTextSync(u, out v);
                                    b1.TranslatedText = v;
                                }
                            }
                            else if (box is DocTransPlainTextBox b2)
                            {
                                string u = "";
                                string k = (b2.AutoIndentation ? "\t" : "") + b2.Text;
                                OllamaApis.DoTextSegmentSync(k, out u);
                                b2.Text = u;
                                string v = "";
                                OllamaApis.TranslateTextSync(u, out v);
                                b2.TranslatedText = v;
                            }
                            else if (box is DocTransAbandonBox b3)
                            {
                                if (!b3.IsLogo)
                                {
                                    string u = "";
                                    OllamaApis.DoTextSegmentSync(b3.Text, out u);
                                    b3.Text = u;
                                    string v = "";
                                    OllamaApis.TranslateTextSync(u, out v);
                                    b3.TranslatedText = v;
                                }
                            }
                            //else if (box is DocTransFigureBox b4)
                            //{
                            //}
                            else if (box is DocTransTableBox b5)
                            {
                                if (!b5.IsRawImage)
                                {
                                    string u = "";
                                    OllamaApis.TranslateHTMLSync(b5.Text, out u);
                                    b5.TranslatedText = u;
                                }
                                if (b5.Caption != null)
                                {
                                    if (!b5.Caption.IsRawImage)
                                    {
                                        string u = "";
                                        OllamaApis.DoTextSegmentSync(b5.Caption.Text, out u);
                                        b5.Caption.Text = u;
                                        string v = "";
                                        OllamaApis.TranslateTextSync(u, out v);
                                        b5.Caption.TranslatedText = v;
                                    }
                                }
                            }
                            else if (box is DocTransFormulaBox b6)
                            {
                                if (b6.Caption != null)
                                {
                                    if (!b6.Caption.IsRawImage)
                                    {
                                        string u = "";
                                        OllamaApis.DoTextSegmentSync(b6.Caption.Text, out u);
                                        b6.Caption.Text = u;
                                        string v = "";
                                        OllamaApis.TranslateTextSync(u, out v);
                                        b6.Caption.TranslatedText = v;
                                    }
                                }
                            }
                            else if (box is DocTransFigureBox b7)
                            {
                                if (b7.Caption != null)
                                {
                                    if (!b7.Caption.IsRawImage)
                                    {
                                        string u = "";
                                        OllamaApis.DoTextSegmentSync(b7.Caption.Text, out u);
                                        b7.Caption.Text = u;
                                        string v = "";
                                        OllamaApis.TranslateTextSync(u, out v);
                                        b7.Caption.TranslatedText = v;
                                    }
                                }
                            }

                        }
                    }

                    BetaVersionDebugPrinter.WriteLine("完成一页了", "parser");
                    //pg
                }
            MainForm.ProcPages = pages;
            //对所有引用执行翻译（如果可翻译）

        }

        private void button_AutoRelation_Click(object sender, EventArgs e)
        {
            //listbox选中项删除
            try
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
            catch
            {
                BetaVersionDebugPrinter.WriteLine("删除失败了", "DellistBox1");
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            docImPlayer.IsContentsPage = checkBox1.Checked;
        }
        /// <summary>
        /// 这里很重要，整个一函数是页面解析器的支持
        /// QuestPDF库的文档写的很清楚很清楚，C#
        /// 这么多PDF构件库里面最好用功能最全的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_generateNewPDF_Click(object sender, EventArgs e)
        {
            if (MainForm.ProcPages.Count == 0)
            {
                MessageBox.Show("没有解析对象列表。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        int margin_pix = 50;
                        page.Size(PageSizes.A4);
                        page.Margin(margin_pix, Unit.Point);//595 842 Point 72DPI

                        page.DefaultTextStyle(x =>
                        {
                            x.FontFamily(FontPanel.__default_fontfamily);
                            x.FontSize(FontPanel.__default_fontsize);
                            return x;
                        });
                        //page.PageColor(Colors.White);
                        //page.DefaultTextStyle(x => x.FontSize(20));
                        //page.Header.
                        ////page.Header().Text("QuestPDF测试").SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
                        //page.Content().PaddingVertical(1, Unit.Centimetre).Column(x => {
                        //    x.Spacing(20);
                        //    x.Item().Text("这是页眉内容");
                        //    //x.Item().Image("path/to/image.jpg", ImageScaling.Resize);
                        //});
                        //page.Footer().AlignCenter().Text(x => x.Span("页码 "));
                        // 使用Canvas进行绝对定位
                        // 自动布局的文本内容
                        //page.Content().Box();
                        /*page.Content().Row(row =>
                        {
                            //row.ConstantItem().TranslateX(374 - 50).TranslateY(39 - 50).Width(186).Height(76)
                                //.Image("C:\\Users\\56279\\Desktop\\Title_Intel.png");
                        });*/
                        page.Content().Column(column =>
                        {
                            //column.RelativeItem()

                            column.Spacing(10); // 设置段落之间的间距10
                            for (int i = 0; i < MainForm.ProcPages.Count; i++)
                            {
                                var pg = MainForm.ProcPages[i];
                                //break;
                                int col_spacing_pix = 10;
                                float plainTextHeight = 1.0f;
                                column.Spacing(pg.IsFixedPage ? 0 : col_spacing_pix);
                                if (pg.IsFixedPage)
                                {
                                    //if(false)
                                    foreach (var a in pg.Contents)
                                    {
                                        var kt = column.Item().Unconstrained()
                                                   .TranslateX(a.X - margin_pix)
                                                   .TranslateY(a.Y - margin_pix)
                                                   .Width(a.Width);
                                        if (a is DocTransTitleBox b)
                                        {
                                            if (b.IsLogo)
                                            {
                                                //.Background(QuestPDF.Infrastructure.Color.FromARGB(255,255,0,255))
                                                //InnerAlignMode
                                                kt.Height(b.Height).Image(
                                                    Path.Combine(MainForm.ProgramPathString,
                                                    $"images\\{b.SourceImageHash}.png")
                                                    );
                                                //break;
                                            }
                                            else
                                            {
                                                switch (b.InnerAlignMode)
                                                {
                                                    case BoxAlignMode.Default:
                                                        kt = kt.AlignCenter();
                                                        break;
                                                    case BoxAlignMode.Left:
                                                        kt = kt.AlignLeft();
                                                        break;
                                                    case BoxAlignMode.Center:
                                                        kt = kt.AlignCenter();
                                                        break;
                                                    case BoxAlignMode.Right:
                                                        kt = kt.AlignRight();
                                                        break;
                                                }
                                                kt = kt.MaxHeight(a.Height);
                                                ////int max_line_count = (int)Math.Ceiling(1.0 * b.Height / (1.0 * b.FontSize * 4 * 72 / 300 + col_spacing_pix));
                                                string txt = string.IsNullOrEmpty(b.TranslatedText) ? b.Text ?? "" : b.TranslatedText.Trim();
                                                ////kt = kt.MaxHeight(b.Height);
                                                kt.Text(txt.Trim()).Justify().FontFamily(b.FontName).FontSize(b.FontSize);//.LineHeight(1.0f);//.ClampLines(max_line_count, "<TRUNCATED>");
                                                //kt.Text(txt =>
                                                //{

                                                //});
                                            }
                                        }
                                        
                                        if (a is DocTransPlainTextBox c)
                                        {
                                            switch (c.InnerAlignMode)
                                            {
                                                case BoxAlignMode.Default:
                                                    kt = kt.AlignLeft();
                                                    break;
                                                case BoxAlignMode.Left:
                                                    kt = kt.AlignLeft();
                                                    break;
                                                case BoxAlignMode.Center:
                                                    kt = kt.AlignCenter();
                                                    break;
                                                case BoxAlignMode.Right:
                                                    kt = kt.AlignRight();
                                                    break;
                                            }
                                            //int max_line_count = (int)Math.Ceiling(1.0 * c.Height / (1.0 * c.FontSize * 4 * 72 / 300 + col_spacing_pix));
                                            string txt = (c.AutoIndentation ? "    " : "") + (string.IsNullOrEmpty(c.TranslatedText) ? c.Text : c.TranslatedText.Trim());
                                            kt = kt.MaxHeight(c.Height);
                                            if (LocalRtf.IsRtf(txt))
                                            {
                                                var list = LocalRtf.RtfStyleParse(txt);
                                                kt.Text(txt =>
                                                {
                                                    foreach (var i in list)
                                                    {

                                                        if (i.IsImage)
                                                        {
                                                            txt.Element().Image(Convert.FromBase64String(i.Content));
                                                        }
                                                        else
                                                        {
                                                            var txt2 = txt.Span(i.Content)
                                                            .FontColor(QuestPDF.Infrastructure.Color.FromARGB(255, i.ForegroundColor.R, i.ForegroundColor.G, i.ForegroundColor.B))
                                                            .FontFamily(c.FontName).FontSize(c.FontSize).LineHeight(plainTextHeight);
                                                            if (i.IsBold) txt2 = txt2.Bold();
                                                            if (i.IsItalic) txt2 = txt2.Italic();
                                                            if (i.IsStrikeout) txt2 = txt2.Strikethrough();
                                                            if (i.IsUnderline) txt2 = txt2.Underline();

                                                            //txt2 = txt2.BackgroundColor(QuestPDF.Infrastructure.Color.FromARGB(i.BackgroundColor.A, i.BackgroundColor.R, i.BackgroundColor.G, i.BackgroundColor.B));
                                                        }
                                                    }
                                                });
                                            }
                                            else kt.Text(txt.TrimEnd()).Justify().FontFamily(c.FontName).FontSize(c.FontSize).LineHeight(plainTextHeight);//.ClampLines(max_line_count, "<TRUNCATED>");
                                        }
                                        if (a is DocTransTableBox d)
                                        {

                                        }
                                        if (a is DocTransFigureBox e)
                                        {

                                        }
                                        if (a is DocTransFormulaBox f)
                                        {

                                        }
                                        //.Image("C:\\Users\\56279\\Desktop\\Title_Intel.png", ImageScaling.FitArea);
                                    }
                                    foreach (var a in pg.Captions)
                                    {

                                    }
                                    foreach (var a in pg.Abandons)
                                    {

                                    }
                                    column.Item().PageBreak();
                                }
                                else if (pg.IsContentsPage)
                                {
                                    //处置目录页的逻辑
                                }
                                else
                                {

                                }
                            }

                            //column.Item()
                            //column.Item().Inlined(inline)
                            /*column.Item().MultiColumn(multiColumn =>
                            {
                                // control number of columns, default is 2
                                multiColumn.Columns(2);
                                // 超级重要非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！
                                multiColumn.BalanceHeight(true);
                                // control space between columns, default is 0
                                multiColumn.Spacing(25);

                                // set container primary content
                                multiColumn
                                    .Content()
                                    .Column(column =>
                                    {
                                        column.Spacing(10);

                                        foreach (var sectionId in Enumerable.Range(0, 7))
                                        {
                                            foreach (var textId in Enumerable.Range(0, 3))
                                                column.Item().Text(Placeholders.Paragraph()).FontFamily("Arial").Justify();

                                            //foreach (var blockId in Enumerable.Range(0, 3))
                                                //column.Item().Width(50 + blockId * 10).Height(25).Background(Placeholders.BackgroundColor());
                                        }
                                    });
                            });
                            column.Item().PageBreak();*/

                            //Unconstrained 非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！非常重要！！！
                            //column.Item().Unconstrained().TranslateX(374 - 50).TranslateY(39 - 50).Width(186).Height(76)
                            //    .Image("C:\\Users\\56279\\Desktop\\Title_Intel.png");

                            //column.Item().Unconstrained().TranslateX(374 - 50).TranslateY(39 + 76 - 50).Width(186).Height(76)
                            //    .Image("C:\\Users\\56279\\Desktop\\Title_Intel.png");

                            //column.Item().Text("This is an automatically laid out text.\r\nCRLF")
                            //    .FontSize(12);

                            //column.
                            //Will Padding Height of ImageDefined .Height(76)
                            //column.Item().TranslateX(374 - 50).TranslateY(-100).Width(186).Height(76)
                            //.Image("C:\\Users\\56279\\Desktop\\Title_Intel.png");

                            //column.Item().TranslateX(374 - 50).TranslateY(119 - 50).Width(186).Height(76)
                            //.Image("C:\\Users\\56279\\Desktop\\Title_Intel.png", ImageScaling.FitArea);

                            //column.Item().Text("You can add as many texts as you want here.")
                            //    .FontSize(12);

                            //for (int i = 0; i < 100; i++)
                            //    column.Item().Text("The layout will adjust based on the content.")
                            //    .FontSize(12);
                        });

                    });
                    //container.Column(col => xxxxx
                    //{
                    //    // 单列区域
                    //    col.Item().Text("单列标题").FontSize(16);
                    //    col.Item().Text(Placeholders.LoremIpsum());

                    //    // 双列嵌套
                    //    col.Item().Row(row =>
                    //    {
                    //        row.RelativeItem().Column(subCol =>
                    //        {
                    //            subCol.Item().Text("左栏内容");
                    //            subCol.Item().Image(Placeholders.Image(200, 100));
                    //        });

                    //        row.RelativeItem().Column(subCol =>
                    //        {
                    //            subCol.Item().Text("右栏内容");
                    //            subCol.Item().Image(Placeholders.Image(200, 100));
                    //        });
                    //    });

                    //    // 继续单列内容
                    //    col.Item().Text("后续内容").FontColor(Colors.Red);
                    //});
                }).GeneratePdfAndShow(); //("C:\\Users\\56279\\Desktop\\Output2.pdf");////
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //下面都是富文本修改器的窗体调用代码
        private void button1_Click(object sender, EventArgs e)
        {
            //listbox清空
            listBox1.Items.Clear();
        }

        internal void FetchAdd(DocImLabelBox i)
        {
            //throw new NotImplementedException();
            string s = i.ToString();
            listBox1.Items.Add(i);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MainForm.ProcPages == null)
            {
                MessageBox.Show("还没处理过吧？");
                return;
            }
            PagesEditor pe = new PagesEditor(MainForm.ProcPages);
            pe.ShowDialog();
        }
        //保存XML
        private void button3_Click(object sender, EventArgs e)
        {
            //Xml解析~
            DocTransObjSerializer serial = new DocTransObjSerializer();
            string xml2 = "";
            var pages = MainForm.ProcPages;
            //Directory.Delete(Path.Combine(MainForm.ProgramPathString, $"pages"), true);
            for (int i = 1; i <= pages.Count; i++)
            {
                string xml = serial.PageSerialize(pages[i - 1]);
                xml2 = string.IsNullOrEmpty(xml2) ? xml : xml2;
                try
                {
                    File.WriteAllText(Path.Combine(MainForm.ProgramPathString, $"pages\\page{i}.xml"), xml);
                }
                catch
                {
                    Directory.CreateDirectory(Path.Combine(MainForm.ProgramPathString, $"pages"));
                    File.WriteAllText(Path.Combine(MainForm.ProgramPathString, $"pages\\page{i}.xml"), xml);
                }
            }
            richTextBox1.Clear();
            richTextBox1.Text = xml2;
        }
    }
}
