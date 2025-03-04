using DocTransAppBeta1.PdfStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/*
 * DocTransObjSerializer.cs
 * 包含对初步文档对象的序列化和反序列化操作
 * 依赖第三方包：无
 */
namespace DocTransAppBeta1.MarkupParser
{
    /// <summary>
    /// 结构化的文档对象序列化与反序列化器
    /// </summary>
    public class DocTransObjSerializer
    {
        private XmlSerializer xswl;
        private XmlSerializer xswl2;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DocTransObjSerializer()
        {
            xswl = new XmlSerializer(typeof(DocTransBox),
                [
                    typeof(DocTransTitleBox),
                    typeof(DocTransPlainTextBox),
                    typeof(DocTransAbandonBox),
                    typeof(DocTransCaptionBox),
                    typeof(DocTransFigureBox),
                    typeof(DocTransFormulaBox),
                    typeof(DocTransTableBox),
                    typeof(DocTransFigureCaptionBox),
                    typeof(DocTransFormulaCaptionBox),
                    typeof(DocTransTableCaptionBox),
                    typeof(DocTransTableFootnoteBox),
                ]
            );
            xswl2 = new XmlSerializer(typeof(DocTransPage));
        }
        /// <summary>
        /// 序列化DocTransBox及其派生类
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public string? BoxSerialize(DocTransBox box)
        {
            using (TextWriter tw = new StringWriter())
            {
                xswl.Serialize(tw, box);
                return tw.ToString();
            }
        }
        /// <summary>
        /// 序列化DocTransPage
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public string? PageSerialize(DocTransPage box)
        {
            using (TextWriter tw = new StringWriter())
            {
                xswl2.Serialize(tw, box);
                return tw.ToString();
            }
        }
        /// <summary>
        /// 反序列化DocTransBox及其派生类
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public DocTransBox? BoxDeserialize(string xml)
        {
            using (TextReader sr = new StringReader(xml))
            {
                return (DocTransBox?)xswl.Deserialize(sr);
            }
        }
        /// <summary>
        /// 反序列化DocTransPage
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public DocTransPage? PageDeserialize(string xml)
        {
            using (TextReader sr = new StringReader(xml))
            {
                return (DocTransPage?)xswl2.Deserialize(sr);
            }
        }
    }
}
