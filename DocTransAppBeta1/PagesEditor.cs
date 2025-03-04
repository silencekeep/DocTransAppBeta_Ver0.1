using DocTransAppBeta1.PdfStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DocTransAppBeta1
{
    public partial class PagesEditor : Form
    {
        readonly List<DocTransPage> PagesReference;
        public PagesEditor(List<DocTransPage> lst)
        {
            PagesReference = lst;
            InitializeComponent();
        }

        private void PagesEditor_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < PagesReference.Count; i++)
            {
                var p = PagesReference[i];
                TreeNode tn = new TreeNode(p.IsFixedPage ? "固定页" : "自动页");
                tn.Tag = p;
                treeView1.Nodes.Add(tn);
                if (p.IsFixedPage)
                {
                    if (p.Contents.Count > 0)
                    {
                        TreeNode tnc1 = new TreeNode("正文内容");
                        tn.Nodes.Add(tnc1);
                        tnc1.Tag = p.Contents;
                        for (int j = 0; j < p.Contents.Count; j++)
                        {
                            var obj = p.Contents[j];
                            TreeNode tncc = new TreeNode(obj.BoxType.ToString());
                            tnc1.Nodes.Add(tncc);
                            tncc.Tag = obj;
                        }
                    }
                    if (p.Captions.Count > 0)
                    {
                        TreeNode tnc2 = new TreeNode("Caption内容");
                        tn.Nodes.Add(tnc2);
                        tnc2.Tag = p.Captions;
                        for (int j = 0; j < p.Contents.Count; j++)
                        {
                            var obj = p.Contents[j];
                            TreeNode tncc = new TreeNode(obj.BoxType.ToString());
                            tnc2.Nodes.Add(tncc);
                            tncc.Tag = obj;
                        }
                    }
                    if (p.Abandons.Count > 0)
                    {
                        TreeNode tnc3 = new TreeNode("页边内容");
                        tn.Nodes.Add(tnc3);
                        tnc3.Tag = p.Abandons;
                        for (int j = 0; j < p.Contents.Count; j++)
                        {
                            var obj = p.Contents[j];
                            TreeNode tncc = new TreeNode(obj.BoxType.ToString());
                            tnc3.Nodes.Add(tncc);
                            tncc.Tag = obj;
                        }
                    }
                }
                else
                {
                    if (p.Artery.Count > 0)
                    {
                        for (int j = 0; j < p.Artery.Count; j++)
                        {
                            var obj = p.Artery[j];
                            TreeNode tncc = new TreeNode(obj.BoxType.ToString());
                            tn.Nodes.Add(tncc);
                            tncc.Tag = obj;
                        }
                    }
                }
            }
        }
        object? refP = null;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var n = e.Node;
            var obj = n.Tag;
            if (obj is DocTransTitleBox)
            {
                var ks = obj as DocTransTitleBox;
                if (!ks.IsLogo)
                {
                    refP = ks;
                    if (!string.IsNullOrEmpty(ks.TranslatedText))
                    {
                        richTextBox1.Clear();
                        if (!LocalRtf.IsRtf(ks.TranslatedText))
                            richTextBox1.Text = ks.TranslatedText;
                        else richTextBox1.Rtf = ks.TranslatedText;
                    }
                    else
                    {
                        richTextBox1.Clear();
                        if (!LocalRtf.IsRtf(ks.Text))
                            richTextBox1.Text = ks.Text;
                        else richTextBox1.Rtf = ks.Text;
                    }
                }
            }
            if (obj is DocTransPlainTextBox)
            {
                var ks = obj as DocTransPlainTextBox;
                if (!ks.IsRawImage)
                {
                    refP = ks;
                    if (!string.IsNullOrEmpty(ks.TranslatedText))
                    {
                        richTextBox1.Clear();
                        if (!LocalRtf.IsRtf(ks.TranslatedText))
                            richTextBox1.Text = ks.TranslatedText;
                        else richTextBox1.Rtf = ks.TranslatedText;
                    }
                    else
                    {
                        richTextBox1.Clear();
                        if (!LocalRtf.IsRtf(ks.Text))
                            richTextBox1.Text = ks.Text;
                        else richTextBox1.Rtf = ks.Text;
                    }
                }
            }
            if (refP is DocTransPlainTextBox)
            {
                var z = refP as DocTransPlainTextBox;
                label3.Text = z.FontName;
                label4.Text = z.FontSize.ToString();
            }
            if (refP is DocTransTitleBox)
            {
                var z = refP as DocTransTitleBox;
                label3.Text = z.FontName;
                label4.Text = z.FontSize.ToString();
            }
            if (refP is DocTransCaptionBox)
            {
                var z = refP as DocTransCaptionBox;
                label3.Text = z.FontName;
                label4.Text = z.FontSize.ToString();
            }
            if (refP is DocTransTableBox)
            {
                var z = refP as DocTransTableBox;
                label3.Text = z.FontName;
                label4.Text = z.FontSize.ToString();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 检查是否有选中的文本
            if (richTextBox1.SelectedText.Length > 0)
            {
                // 获取当前选中文本的字体样式
                Font currentFont = richTextBox1.SelectionFont;
                FontStyle newFontStyle = currentFont.Style;

                newFontStyle |= FontStyle.Bold;

                // 应用新的字体样式
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 检查是否有选中的文本
            if (richTextBox1.SelectedText.Length > 0)
            {
                // 获取当前选中文本的字体样式
                Font currentFont = richTextBox1.SelectionFont;
                FontStyle newFontStyle = currentFont.Style;

                newFontStyle |= FontStyle.Italic;

                // 应用新的字体样式
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 检查是否有选中的文本
            if (richTextBox1.SelectedText.Length > 0)
            {
                // 获取当前选中文本的字体样式
                Font currentFont = richTextBox1.SelectionFont;
                FontStyle newFontStyle = currentFont.Style;

                newFontStyle &= ~(FontStyle.Bold & FontStyle.Italic & FontStyle.Underline & FontStyle.Strikeout);

                // 应用新的字体样式
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            var pk = LocalRtf.RtfStyleParse(richTextBox1.Rtf);
            foreach (var s in pk)
            {
                BetaVersionDebugPrinter.WriteLine($"{s.IsImage}\t{s.Content}", "SegmentationResult");
            }
            if (refP is DocTransPlainTextBox)
            {
                var z = refP as DocTransPlainTextBox;
                if (!string.IsNullOrEmpty(z.TranslatedText))
                {
                    //refP = z.TranslatedText;
                    //RtfPipe.Rtf
                    z.TranslatedText = richTextBox1.Rtf;//RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                else
                {
                    //refP = z.Text;
                    z.Text = richTextBox1.Rtf;//RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                //richTextBox1.Clear();
            }
            if (refP is DocTransTitleBox)
            {
                var z = refP as DocTransTitleBox;
                if (!string.IsNullOrEmpty(z.TranslatedText))
                {
                    //refP = z.TranslatedText;
                    z.TranslatedText = richTextBox1.Rtf;//RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                else
                {
                    //refP = z.Text;
                    z.Text = richTextBox1.Rtf; //RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                //richTextBox1.Clear();
            }
            if (refP is DocTransCaptionBox)
            {
                var z = refP as DocTransCaptionBox;
                if (!string.IsNullOrEmpty(z.TranslatedText))
                {
                    //refP = z.TranslatedText;
                    z.TranslatedText = richTextBox1.Rtf;//RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                else
                {
                    //refP = z.Text;
                    z.Text = richTextBox1.Rtf; //RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                //richTextBox1.Clear();
            }
            if (refP is DocTransTableBox)
            {
                var z = refP as DocTransTableBox;
                if (!string.IsNullOrEmpty(z.TranslatedText))
                {
                    //refP = z.TranslatedText;
                    z.TranslatedText = richTextBox1.Rtf;//RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                else
                {
                    //refP = z.Text;
                    z.Text = richTextBox1.Rtf; //RtfPipe.Rtf.ToHtml(richTextBox1.Rtf); //richTextBox1.Text;
                }
                //richTextBox1.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            richTextBox2.Text = RtfPipe.Rtf.ToHtml(richTextBox1.Rtf);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                label3.Text = fd.Font.Name;
                label4.Text = fd.Font.Size.ToString();
                if (refP is DocTransPlainTextBox)
                {
                    var z = refP as DocTransPlainTextBox;
                    z.FontName = fd.Font.Name;
                    z.FontSize = Convert.ToInt32(fd.Font.Size);
                }
                if (refP is DocTransTitleBox)
                {
                    var z = refP as DocTransTitleBox;
                    z.FontName = fd.Font.Name;
                    z.FontSize = Convert.ToInt32(fd.Font.Size);
                }
                if (refP is DocTransCaptionBox)
                {
                    var z = refP as DocTransCaptionBox;
                    z.FontName = fd.Font.Name;
                    z.FontSize = Convert.ToInt32(fd.Font.Size);
                }
                if (refP is DocTransTableBox)
                {
                    var z = refP as DocTransTableBox;
                    z.FontName = fd.Font.Name;
                    z.FontSize = Convert.ToInt32(fd.Font.Size);
                }
            }
        }
        private Color HexToColor(string hexColor)
        {
            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }
            byte r = Convert.ToByte(hexColor.Substring(0, 2), 16);
            byte g = Convert.ToByte(hexColor.Substring(2, 2), 16);
            byte b = Convert.ToByte(hexColor.Substring(4, 2), 16);
            return Color.FromArgb(r, g, b);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText.Length > 0)
            {
                // 获取当前选中文本的字体样式
                //Font currentFont = richTextBox1.SelectionFont;
                //FontStyle newFontStyle = currentFont.Style;

                //newFontStyle |= FontStyle.Italic;

                // 应用新的字体样式
                //richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, currentFont.Style);
                richTextBox1.SelectionColor = HexToColor(textBox1.Text);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText.Length > 0)
            {
                // 获取当前选中文本的字体样式
                Font currentFont = richTextBox1.SelectionFont;
                FontStyle newFontStyle = currentFont.Style;

                newFontStyle |= FontStyle.Underline;

                // 应用新的字体样式
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText.Length > 0)
            {
                // 获取当前选中文本的字体样式
                Font currentFont = richTextBox1.SelectionFont;
                FontStyle newFontStyle = currentFont.Style;

                newFontStyle |= FontStyle.Strikeout;

                // 应用新的字体样式
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText.Length > 0)
            {
                // 获取当前选中文本的字体样式
                //Font currentFont = richTextBox1.SelectionFont;
                //FontStyle newFontStyle = currentFont.Style;

                //newFontStyle |= FontStyle.Italic;

                // 应用新的字体样式
                //richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, currentFont.Style);
                richTextBox1.SelectionBackColor = HexToColor(textBox1.Text);
            }
        }
    }
}
