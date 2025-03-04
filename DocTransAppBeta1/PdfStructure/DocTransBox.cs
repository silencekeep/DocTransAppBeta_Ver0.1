using DocTransAppBeta1.DocModules;
using DocTransAppBeta1.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/*
 * 就是表示后分析文档结构的类的集合，有一些XML序列化标签。
 */
namespace DocTransAppBeta1.PdfStructure
{
    /// <summary>
    /// 水平对齐方式枚举
    /// </summary>
    public enum BoxAlignMode
    {
        Default, Left, Center, Right
    }
    /// <summary>
    /// 标题位置枚举
    /// </summary>
    public enum CaptionPosition
    {
        Above, Below, Right, Left
    }
    /// <summary>
    /// Abandon类型枚举
    /// </summary>
    public enum AbandonType
    {
        Other, Header, Footer
    }
    [XmlInclude(typeof(DocTransTitleBox))]
    [XmlInclude(typeof(DocTransPlainTextBox))]
    [XmlInclude(typeof(DocTransAbandonBox))]
    [XmlInclude(typeof(DocTransCaptionBox))]
    [XmlInclude(typeof(DocTransFigureBox))]
    [XmlInclude(typeof(DocTransFormulaBox))]
    [XmlInclude(typeof(DocTransTableBox))]
    [XmlInclude(typeof(DocTransFigureCaptionBox))]
    [XmlInclude(typeof(DocTransFormulaCaptionBox))]
    [XmlInclude(typeof(DocTransTableCaptionBox))]
    [XmlInclude(typeof(DocTransTableFootnoteBox))]
    public class DocTransBox
    {
        public DocTransBox() { }
        
        public DocTransBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
        {
            IsFlowLayoutHead = isFlowHead;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            SourceImageHash = srcImageHash;
            BoxType = label;
            __fixed = isFixed;
            //__nextBox = nextBox;
        }
        [XmlElement("IsFlowLayoutHead", Order = 0)]
        public bool IsFlowLayoutHead { get; set; }
        [XmlAttribute("X")]
        public int X { get; set; }
        [XmlAttribute("Y")]
        public int Y { get; set; }
        [XmlAttribute("Width")]
        public int Width { get; set; }
        [XmlAttribute("Height")]
        public int Height { get; set; }
        [XmlElement("SourceImageHash", Order = 1)]
        public string? SourceImageHash { get; set; }
        [XmlElement("BoxType", Order = 2)]
        public DocLayoutLabel BoxType { get; set; }
        [XmlIgnore]

        private bool __fixed;

        private DocTransBox? __nextBox;
        [XmlAttribute("IsFixed")]
        public bool IsFixed
        {
            get => __fixed;
            set
            {
                __fixed = value;
                if (value) __nextBox = null;
            }
        }
        [XmlIgnore]
        public DocTransBox? NextBox
        {
            get => __fixed ? null : __nextBox;
            set => __nextBox = __fixed ? null : value;
        }
    }
    public class DocTransTitleBox : DocTransBox
    {
        public DocTransTitleBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
        : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {

        }
        public DocTransTitleBox() : base()
        {
        }
        [XmlIgnore]
        private bool __isLogo;
        [XmlElement("IsLogo", Order = 3)]
        public bool IsLogo
        {
            get => __isLogo;
            set
            {
                __isLogo = value;
                if (value)
                {
                    InnerAlignMode = BoxAlignMode.Default;
                    __text = null;
                    __translatedText = null;
                }
            }
        }
        [XmlElement("FontName", Order = 4)]
        public string? FontName { get; set; }
        [XmlElement("FontSize", Order = 5)]
        public int FontSize { get; set; }
        [XmlIgnore]
        private string? __text;
        [XmlIgnore]
        private string? __translatedText;
        [XmlElement("Text", Order = 6)]
        public string? Text
        {
            get => IsLogo ? null : __text;
            set => __text = IsLogo ? null : value;
        }
        [XmlElement("TranslatedText", Order = 7)]
        public string? TranslatedText
        {
            get => IsLogo ? null : __translatedText;
            set => __translatedText = IsLogo ? null : value;
        }
        [XmlIgnore]
        private BoxAlignMode __innerAlign;
        [XmlIgnore]
        private BoxAlignMode __outerAlign;
        [XmlElement("InnerAlignMode", Order = 8)]
        public BoxAlignMode InnerAlignMode
        {
            get => __isLogo ? BoxAlignMode.Default : __innerAlign;
            set => __innerAlign = __isLogo ? BoxAlignMode.Default : value;
        }
        [XmlElement("OuterAlignMode", Order = 9)]
        public BoxAlignMode OuterAlignMode
        {
            get => IsFixed ? BoxAlignMode.Default : __outerAlign;
            set => __outerAlign = IsFixed ? BoxAlignMode.Default : value;
        }
    }
    public class DocTransAbandonBox : DocTransTitleBox
    {
        public DocTransAbandonBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
            : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {
        }
        public DocTransAbandonBox()
        {

        }
        [XmlElement("AbandonType", Order = 10)]
        public AbandonType Type { get; set; }
    }
    public class DocTransPlainTextBox : DocTransBox
    {
        public DocTransPlainTextBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
        : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {

        }
        public DocTransPlainTextBox() : base()
        {
        }
        [XmlIgnore]
        private bool __isRawImage;
        [XmlElement("IsRawImage", Order = 3)]
        public bool IsRawImage
        {
            get => __isRawImage;
            set
            {
                __isRawImage = value;
                if (value)
                {
                    __text = null;
                    __translatedText = null;
                }
            }
        }
        [XmlIgnore]
        private bool __autoIndent;
        [XmlElement("AutoIndentation", Order = 4)]
        public bool AutoIndentation
        {
            get => __autoIndent;
            set
            {
                __autoIndent = value;
                if (value) __innerAlign = BoxAlignMode.Default;
            }
        }
        [XmlElement("FontName", Order = 5)]
        public string? FontName { get; set; }
        [XmlElement("FontSize", Order = 6)]
        public int FontSize { get; set; }
        [XmlIgnore]
        private string? __text;
        [XmlIgnore]
        private string? __translatedText;
        [XmlElement("Text", Order = 7)]
        public string? Text
        {
            get => __isRawImage ? null : __text;
            set => __text = __isRawImage ? null : value;
        }
        [XmlElement("TranslatedText", Order = 8)]
        public string? TranslatedText
        {
            get => __isRawImage ? null : __translatedText;
            set => __translatedText = __isRawImage ? null : value;
        }
        [XmlIgnore]
        private BoxAlignMode __innerAlign;
        [XmlIgnore]
        private BoxAlignMode __outerAlign;
        [XmlElement("InnerAlignMode", Order = 9)]
        public BoxAlignMode InnerAlignMode
        {
            get => __autoIndent ? BoxAlignMode.Default : __outerAlign;
            set => __outerAlign = __autoIndent ? BoxAlignMode.Default : value;
        }
        [XmlElement("OuterAlignMode", Order = 10)]
        public BoxAlignMode OuterAlignMode
        {
            get => IsFixed ? BoxAlignMode.Default : __outerAlign;
            set => __outerAlign = IsFixed ? BoxAlignMode.Default : value;
        }
    }
    [XmlInclude(typeof(DocTransFigureCaptionBox))]
    [XmlInclude(typeof(DocTransFormulaCaptionBox))]
    [XmlInclude(typeof(DocTransTableCaptionBox))]
    [XmlInclude(typeof(DocTransTableFootnoteBox))]
    public class DocTransCaptionBox : DocTransBox
    {
        public DocTransCaptionBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
        : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {

        }
        public DocTransCaptionBox() : base()
        {
        }
        [XmlIgnore]
        private bool __isRawImage;
        [XmlElement("IsRawImage", Order = 3)]
        public bool IsRawImage
        {
            get => __isRawImage;
            set
            {
                __isRawImage = value;
                if (value)
                {
                    __text = null;
                    __translatedText = null;
                }
            }
        }
        [XmlElement("CaptionPosition", Order = 4)]
        public CaptionPosition Position { get; set; }
        [XmlElement("FontName", Order = 5)]
        public string? FontName { get; set; }
        [XmlElement("FontSize", Order = 6)]
        public int FontSize { get; set; }
        [XmlIgnore]
        private string? __text;
        [XmlIgnore]
        private string? __translatedText;
        [XmlElement("Text", Order = 7)]
        public string? Text
        {
            get => __isRawImage ? null : __text;
            set => __text = __isRawImage ? null : value;
        }
        [XmlElement("TranslatedText", Order = 8)]
        public string? TranslatedText
        {
            get => __isRawImage ? null : __translatedText;
            set => __translatedText = __isRawImage ? null : value;
        }
    }
    public class DocTransFigureCaptionBox : DocTransCaptionBox
    {
        public DocTransFigureCaptionBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed) : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {
        }
        public DocTransFigureCaptionBox()
        {
        }
    }
    public class DocTransFormulaCaptionBox : DocTransCaptionBox
    {
        public DocTransFormulaCaptionBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed) : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {
        }
        public DocTransFormulaCaptionBox()
        {
        }
    }
    public class DocTransTableCaptionBox : DocTransCaptionBox
    {
        public DocTransTableCaptionBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed) : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {
        }
        public DocTransTableCaptionBox()
        {
        }
    }
    public class DocTransTableFootnoteBox : DocTransCaptionBox
    {
        public DocTransTableFootnoteBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed) : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {
        }
        public DocTransTableFootnoteBox()
        {
        }
    }
    public class DocTransFigureBox : DocTransBox
    {
        public DocTransFigureBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
        : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {

        }
        public DocTransFigureBox() : base()
        {
        }
        [XmlIgnore]
        private BoxAlignMode __align;
        [XmlElement("AlignMode", Order = 3)]
        public BoxAlignMode AlignMode
        {
            get => IsFixed ? BoxAlignMode.Default : __align;
            set => __align = IsFixed ? BoxAlignMode.Default : value;
        }
        [XmlElement("Caption", Order = 4)]
        public DocTransFigureCaptionBox? Caption { get; set; }
        [XmlIgnore]
        public DocImLabelBox? ImCaption { get; set; }
    }

    public class DocTransFormulaBox : DocTransBox
    {
        public DocTransFormulaBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
        : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {

        }
        public DocTransFormulaBox() : base()
        {
        }
        [XmlIgnore]
        private bool __isRawImage;
        [XmlElement("IsRawImage", Order = 3)]
        public bool IsRawImage
        {
            get => __isRawImage;
            set
            {
                __isRawImage = value;
                if (value)
                {
                    LaTeXExpression = null;
                }
            }
        }
        [XmlIgnore]
        private BoxAlignMode __align;
        [XmlElement("AlignMode", Order = 4)]
        public BoxAlignMode AlignMode
        {
            get => IsFixed ? BoxAlignMode.Default : __align;
            set => __align = IsFixed ? BoxAlignMode.Default : value;
        }
        [XmlIgnore]
        private string? __LaTeXExpression { get; set; }
        [XmlElement("LaTeXExpression", Order = 5)]
        public string? LaTeXExpression
        {
            get => __isRawImage ? null : __LaTeXExpression;
            set => __LaTeXExpression = __isRawImage ? null : value;
        }
        [XmlElement("Caption", Order = 6)]
        public DocTransFormulaCaptionBox? Caption { get; set; }
        [XmlIgnore]
        public DocImLabelBox? ImCaption { get; set; }
    }
    public class DocTransTableBox : DocTransBox
    {
        public DocTransTableBox(int x, int y, bool isFlowHead, int width, int height, string srcImageHash, DocLayoutLabel label, bool isFixed)
        : base(x, y, isFlowHead, width, height, srcImageHash, label, isFixed)
        {

        }
        public DocTransTableBox() : base()
        {
        }
        [XmlIgnore]
        private bool __isRawImage;
        [XmlElement("IsRawImage", Order = 3)]
        public bool IsRawImage
        {
            get => __isRawImage;
            set
            {
                __isRawImage = value;
                if (value)
                {
                    __text = null;
                    __translatedText = null;
                }
            }
        }
        [XmlIgnore]
        private BoxAlignMode __align;
        [XmlElement("AlignMode", Order = 4)]
        public BoxAlignMode AlignMode
        {
            get => IsFixed ? BoxAlignMode.Default : __align;
            set => __align = IsFixed ? BoxAlignMode.Default : value;
        }
        [XmlElement("FontName", Order = 5)]
        public string? FontName { get; set; }
        [XmlElement("FontSize", Order = 6)]
        public int FontSize { get; set; }
        [XmlIgnore]

        private string? __text;
        [XmlIgnore]
        private string? __translatedText;
        [XmlElement("Text", Order = 7)]
        public string? Text
        {
            get => __isRawImage ? null : __text;
            set => __text = __isRawImage ? null : value;
        }
        [XmlElement("TranslatedText", Order = 8)]
        public string? TranslatedText
        {
            get => __isRawImage ? null : __translatedText;
            set => __translatedText = __isRawImage ? null : value;
        }
        [XmlElement("Caption", Order = 9)]
        public DocTransTableCaptionBox? Caption { get; set; }
        [XmlElement("Footnode", Order = 10)]
        public DocTransTableFootnoteBox? Footnote { get; set; }

        [XmlIgnore]
        public DocImLabelBox? ImCaption { get; set; }
        [XmlIgnore]
        public DocImLabelBox? ImFootnote { get; set; }
    }
}
