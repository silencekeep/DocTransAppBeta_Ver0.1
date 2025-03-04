using HtmlAgilityPack;
using RtfPipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlDocument = HtmlAgilityPack.HtmlDocument;

/*
 * LocalRtf.cs
 * 包含RichText格式的样式分段方法和RichText格式的判断方法
 * 依赖第三方包：RtfPipe, HtmlAgilityPack
 */
namespace DocTransAppBeta1
{
    /// <summary>
    /// 解析用于复杂样式配置的Rtf文本/嵌入图像的样式段
    /// </summary>
    public class RtfSegment
    {
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsStrikeout { get; set; }
        public bool IsImage { get; set; }
        public Color BackgroundColor { get; set; }
        public Color Foreground { get; set; }
        private string? __content;
        public string? Content
        {
            get
            {
                return __content;//IsImage ? __content : HEX2BASE64(__content);
            }
            set
            {
                __content = value;
            }
        }
        //private string HEX2BASE64(string? hex)
        //{
        //    if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
        //        throw new ArgumentException("hex string length is odd or null.");

        //    byte[] byteArray = new byte[hex.Length / 2];
        //    for (int i = 0; i < hex.Length; i += 2)
        //    {
        //        byteArray[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        //    }
        //    return Convert.ToBase64String(byteArray);
        //}
    }

    /// <summary>
    /// 应用于本地的Rtf实用方法类
    /// </summary>
    public class LocalRtf
    {
        /// <summary>
        /// 对Rtf文本执行样式分段，返回的是样式组集合
        /// </summary>
        /// <param name="rtf">Rtf文本</param>
        /// <returns>包含RtfSegment集合的枚举器</returns>
        public static IEnumerable<RtfSegment> RtfStyleParse(string rtf) => RtfStyleParseIncomplete02(rtf);
        /// <summary>
        /// 判断文本是否是Rtf字符串
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static bool IsRtf(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            if (!text.TrimStart().StartsWith("{\\rtf", StringComparison.OrdinalIgnoreCase))
                return false;

            bool containsControlWord = text.IndexOf('\\') > -1;
            if (!containsControlWord)
                return false;

            return true;
        }
        /// <summary>
        /// 递归的对Rtf文本执行样式分段，返回的是样式组集合
        /// </summary>
        /// <param name="rtf">Rtf文本</param>
        /// <returns></returns>
        [Obsolete]
        private static List<RtfSegment> RtfStyleParseIncomplete01(string rtf)
        {
            List<RtfSegment> localList = new List<RtfSegment>();
            string htmlText = RtfPipe.Rtf.ToHtml(rtf);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlText);

            foreach (var node in doc.DocumentNode.DescendantsAndSelf())
            {
                if (node.NodeType == HtmlNodeType.Element || node.NodeType == HtmlNodeType.Text)
                {
                    var segment = new RtfSegment();
                    if (node.Attributes["style"] != null)
                    {
                        var styles = node.Attributes["style"].Value.Split(';');
                        foreach (var style in styles)
                        {
                            if (style.Contains("font-weight: bold"))
                                segment.IsBold = true;
                            else if (style.Contains("text-decoration: underline"))
                                segment.IsUnderline = true;
                            else if (style.Contains("font-style: italic"))
                                segment.IsItalic = true;
                            else if (style.Contains("color: "))
                            {
                                var colorCode = style.Split(':')[1].Trim();
                                segment.Foreground = ColorTranslator.FromHtml(colorCode);
                            }
                            else if (style.Contains("text-decoration: line-through"))
                                segment.IsStrikeout = true;
                        }
                    }
                    if (node.Name == "b" || node.Name == "strong")
                        segment.IsBold = true;
                    if (node.Name == "i" || node.Name == "em")
                        segment.IsItalic = true;
                    if (node.Name == "u")
                        segment.IsUnderline = true;
                    if (node.Name == "strike")
                        segment.IsStrikeout = true;
                    if (node.NodeType == HtmlNodeType.Text)
                    {
                        segment.Content = node.InnerText;
                        localList.Add(segment);
                    }
                    else if (node.Name == "img")
                    {
                        segment.IsImage = true;
                        segment.Content = node.GetAttributeValue("src", "");
                        localList.Add(segment);
                    }
                    else if (node.HasChildNodes)
                    {
                        foreach (var childSegment in RtfStyleParseIncomplete01(node.InnerHtml))
                        {
                            childSegment.IsBold |= segment.IsBold;
                            childSegment.IsItalic |= segment.IsItalic;
                            childSegment.IsUnderline |= segment.IsUnderline;
                            childSegment.IsStrikeout |= segment.IsStrikeout;
                            childSegment.Foreground = segment.Foreground;
                            localList.Add(segment);
                        }
                    }
                }
            }
            return localList;
        }
        /// <summary>
        /// 对Rtf文本执行样式分段，返回的是样式组集合
        /// </summary>
        /// <param name="rtf">Rtf文本</param>
        /// <returns>包含RtfSegment集合的枚举器</returns>
        private static IEnumerable<RtfSegment> RtfStyleParseIncomplete02(string rtf)
        {
            string htmlText = RtfPipe.Rtf.ToHtml(rtf);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlText);

            return ExtractSegments(doc.DocumentNode, new RtfSegment())
                .Where(seg => !string.IsNullOrEmpty(seg.Content));
        }
        /// <summary>
        /// 对HTML进行递归解析
        /// </summary>
        /// <param name="node">HTML节点</param>
        /// <param name="inheritedStyles">父级样式</param>
        /// <returns>包含RtfSegment集合的枚举器</returns>
        private static IEnumerable<RtfSegment> ExtractSegments(HtmlNode node, RtfSegment inheritedStyles)
        {
            var currentStyles = new RtfSegment
            {
                IsBold = inheritedStyles.IsBold,
                IsItalic = inheritedStyles.IsItalic,
                IsUnderline = inheritedStyles.IsUnderline,
                IsStrikeout = inheritedStyles.IsStrikeout,
                BackgroundColor = inheritedStyles.BackgroundColor,
                Foreground = inheritedStyles.Foreground
            };
            if (node.Attributes["style"] != null)
            {
                var styles = node.Attributes["style"].Value.Split(';');
                foreach (var style in styles)
                {
                    var parts = style.Trim().Split(':');
                    if (parts.Length == 2)
                    {
                        var property = parts[0].Trim().ToLower();
                        var value = parts[1].Trim();

                        switch (property)
                        {
                            case "font-weight":
                                if (value.Equals("bold", StringComparison.OrdinalIgnoreCase))
                                    currentStyles.IsBold = true;
                                break;
                            case "text-decoration":
                                if (value.Equals("underline", StringComparison.OrdinalIgnoreCase))
                                    currentStyles.IsUnderline = true;
                                else if (value.Equals("line-through", StringComparison.OrdinalIgnoreCase))
                                    currentStyles.IsStrikeout = true;
                                break;
                            case "font-style":
                                if (value.Equals("italic", StringComparison.OrdinalIgnoreCase))
                                    currentStyles.IsItalic = true;
                                break;
                            case "color":
                                currentStyles.Foreground = ColorTranslator.FromHtml(value);
                                break;
                            case "background-color":
                                currentStyles.BackgroundColor = ColorTranslator.FromHtml(value);
                                break;
                        }
                    }
                }
            }
            if (node.Name == "b" || node.Name == "strong")
                currentStyles.IsBold = true;
            if (node.Name == "i" || node.Name == "em")
                currentStyles.IsItalic = true;
            if (node.Name == "u")
                currentStyles.IsUnderline = true;
            if (node.Name == "strike")
                currentStyles.IsStrikeout = true;

            if (node.NodeType == HtmlNodeType.Text)
            {
                currentStyles.Content = node.InnerText.Trim();
                yield return currentStyles;
            }
            else if (node.Name == "img")
            {
                currentStyles.IsImage = true;
                currentStyles.Content = node.GetAttributeValue("src", "");
                yield return currentStyles;
            }
            else if (node.HasChildNodes)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    foreach (var segment in ExtractSegments(childNode, currentStyles))
                    {
                        yield return segment;
                    }
                }
            }
        }
        /// <summary>
        /// 初版Rtf解析器
        /// </summary>
        /// <param name="rtf"></param>
        /// <returns>包含RtfSegment集合的列表</returns>
        [Obsolete]
        private static List<RtfSegment> RtfStyleParseIncomplete00(string rtf)
        {
            List<RtfSegment> localList = new List<RtfSegment>();
            string htmlText = RtfPipe.Rtf.ToHtml(rtf);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlText);

            foreach (var node in doc.DocumentNode.DescendantsAndSelf())
            {
                if (node.NodeType == HtmlNodeType.Element || node.NodeType == HtmlNodeType.Text)
                {
                    var segment = new RtfSegment();

                    if (node.Attributes["style"] != null)
                    {
                        var styles = node.Attributes["style"].Value.Split(';');
                        foreach (var style in styles)
                        {
                            if (style.Contains("font-weight: bold"))
                                segment.IsBold = true;
                            else if (style.Contains("text-decoration: underline"))
                                segment.IsUnderline = true;
                            else if (style.Contains("font-style: italic"))
                                segment.IsItalic = true;
                            else if (style.Contains("color: "))
                            {
                                var colorCode = style.Split(':')[1].Trim();
                                segment.Foreground = ColorTranslator.FromHtml(colorCode);
                            }
                            else if (style.Contains("text-decoration: line-through"))
                                segment.IsStrikeout = true;
                        }
                    }
                    if (node.Name == "b" || node.Name == "strong")
                        segment.IsBold = true;
                    if (node.Name == "i" || node.Name == "em")
                        segment.IsItalic = true;
                    if (node.Name == "u")
                        segment.IsUnderline = true;
                    if (node.Name == "strike")
                        segment.IsStrikeout = true;
                    if (node.NodeType == HtmlNodeType.Text)
                    {
                        segment.Content = node.InnerText.Trim();
                        localList.Add(segment);
                        //yield return segment;
                    }
                    // Handle image nodes
                    else if (node.Name == "img")
                    {
                        segment.IsImage = true;
                        segment.Content = node.GetAttributeValue("src", "");
                        localList.Add(segment);
                        //yield return segment;
                    }
                    // Handle other elements by recursively processing their children
                    else if (node.HasChildNodes)
                    {
                        foreach (var childSegment in ParseChildren(node))
                        {
                            // Inherit parent styles
                            childSegment.IsBold |= segment.IsBold;
                            childSegment.IsItalic |= segment.IsItalic;
                            childSegment.IsUnderline |= segment.IsUnderline;
                            childSegment.IsStrikeout |= segment.IsStrikeout;
                            childSegment.Foreground = segment.Foreground;
                            localList.Add(segment);
                            //yield return segment;
                        }
                    }
                }
            }
            return localList.Where(seg => !string.IsNullOrEmpty(seg.Content)).ToList();
        }

        private static IEnumerable<RtfSegment> ParseChildren(HtmlNode parentNode)
        {
            var tempDoc = new HtmlDocument();
            tempDoc.LoadHtml("<div>" + parentNode.InnerHtml + "</div>");

            foreach (var childNode in tempDoc.DocumentNode.DescendantsAndSelf())
            {
                if (childNode.NodeType == HtmlNodeType.Element || childNode.NodeType == HtmlNodeType.Text)
                {
                    var segment = new RtfSegment();
                    if (childNode.Attributes["style"] != null)
                    {
                        var styles = childNode.Attributes["style"].Value.Split(';');
                        foreach (var style in styles)
                        {
                            if (style.Contains("font-weight: bold"))
                                segment.IsBold = true;
                            else if (style.Contains("text-decoration: underline"))
                                segment.IsUnderline = true;
                            else if (style.Contains("font-style: italic"))
                                segment.IsItalic = true;
                            else if (style.Contains("color: "))
                            {
                                var colorCode = style.Split(':')[1].Trim();
                                segment.Foreground = ColorTranslator.FromHtml(colorCode);
                            }
                            else if (style.Contains("text-decoration: line-through"))
                                segment.IsStrikeout = true;
                        }
                    }
                    if (childNode.Name == "b" || childNode.Name == "strong")
                        segment.IsBold = true;
                    if (childNode.Name == "i" || childNode.Name == "em")
                        segment.IsItalic = true;
                    if (childNode.Name == "u")
                        segment.IsUnderline = true;
                    if (childNode.Name == "strike")
                        segment.IsStrikeout = true;
                    if (childNode.NodeType == HtmlNodeType.Text)
                    {
                        segment.Content = childNode.InnerText.Trim();
                        yield return segment;
                    }
                    else if (childNode.Name == "img")
                    {
                        segment.IsImage = true;
                        segment.Content = childNode.GetAttributeValue("src", "");
                        yield return segment;
                    }
                }
            }
        }
    }
}
