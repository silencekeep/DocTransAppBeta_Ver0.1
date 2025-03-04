using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocTransAppBeta1.PdfStructure
{
    /// <summary>
    /// 生成页的对象
    /// </summary>
    public class DocTransPage
    {
        //格式问题解析时报异常，FixedPage必须全Fix
        public DocTransPage()
        {
            Contents = [];
            Abandons = [];
        }
        [XmlAttribute("IsFixedPage")]
        public bool IsFixedPage { get; set; }
        [XmlElement("IsContentsPage", Order = 0)]
        public bool IsContentsPage { get; set; }
        [XmlIgnore]
        public DocTransBox? HeadBox { get; set; }
        [XmlElement("Artery", Order = 1)]
        public List<DocTransBox>? Artery { get; set; }

        public void RefreshArtery()
        {
            if (!IsFixedPage)
            {
                List<DocTransBox> ret = new List<DocTransBox>();
                DocTransBox curr = HeadBox;
                while (curr != null)
                {
                    ret.Add(curr);
                    curr = curr.NextBox;
                }
                Artery = ret;
            }
        }
        //[XmlIgnore]
        //private List<DocTransBox>? __artery;
        //[XmlElement("Artery", Order = 1)]
        //public List<DocTransBox>? Artery { 
        //    get
        //    {
        //        if (__artery != null) return __artery;
        //        List<DocTransBox> ret = new List<DocTransBox>();
        //        DocTransBox curr = HeadBox;
        //        while (curr != null)
        //        {
        //            ret.Add(curr);
        //            curr = curr.NextBox;
        //        }
        //        return ret;
        //    }
        //    set => __artery = value;
        //}
        [XmlElement("Contents", Order = 2)]
        public List<DocTransBox>? Contents { get; set; }
        [XmlElement("Abandons", Order = 3)]
        public List<DocTransBox>? Abandons { get; set; }
        [XmlElement("Captions", Order = 4)]
        public List<DocTransBox>? Captions { get; set; }
        //检查关系完整性  将链表节点构成的图视为有向连通图，如何判断单向连通分支个数？我给你两个方法，一个
        public bool CheckBasicCondition() => (Contents != null && IsFixedPage) || (HeadBox != null && !IsFixedPage);
        public bool CheckBoxesRelationCompleteness()
        {
            //改用纯Artery方法后，这些方法的使用逻辑就可以更简单了，也不用担心单链表成环等sb情况
            int nullCounter = 0;
            HashSet<DocTransBox> visited = new HashSet<DocTransBox>();
            if (Contents == null || HeadBox == null) return false;
            if (!Contents.Contains(HeadBox)) return false;
            int cap_counter = 0;
            foreach (var item in Contents)
            {
                if (item is DocTransFormulaBox t1)
                    if (t1.Caption != null) cap_counter++;
                if (item is DocTransFigureBox t2)
                    if (t2.Caption != null) cap_counter++;
                if (item is DocTransTableBox t3)
                {
                    if (t3.Caption != null) cap_counter++;
                    if (t3.Footnote != null) cap_counter++;
                }
                if (visited.Contains(item))
                {
                    return false;//Loop
                }
                visited.Add(item);
                if (item.NextBox == null)
                {
                    nullCounter++;
                }
            }

            return nullCounter == 1 && cap_counter == (Captions == null ? 0 : Captions.Count);
        }
        public bool CheckBoxesRelationHaveLoop()
        {
            int nullCounter = 0;
            HashSet<DocTransBox> visited = new HashSet<DocTransBox>();

            foreach (var item in Contents)
            {
                if (visited.Contains(item))
                {
                    return true;
                }
                visited.Add(item);
                if (item.NextBox == null)
                {
                    nullCounter++;
                }
            }
            return nullCounter == 0;
        }
        public IEnumerable<DocTransAbandonBox> GetSpecificAbandonsEnumerable(AbandonType type)
        {
            foreach (var item in Abandons)
            {
                if (item is DocTransAbandonBox box)
                {
                    if (box.Type == type)
                        yield return box;
                }
            }
        }
        public IEnumerable<DocTransCaptionBox> GetSpecificCaptionsEnumerable()
        {
            foreach (var item in Captions)
            {
                if (item is DocTransCaptionBox box)
                {
                    //if (box.Type == type)
                    yield return box;
                }
            }
        }
    }
}
