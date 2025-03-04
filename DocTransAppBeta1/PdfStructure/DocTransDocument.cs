using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTransAppBeta1.PdfStructure
{
    /// <summary>
    /// 自动页码格式枚举
    /// </summary>
    public enum DocTransPageNumberMode
    {
        FixedRightDown,FixedRightUp,FixedLeftUp,FixedLeftDown,
        CrossRightUp,CrossRightDown,CrossLeftUp,CrossLeftDown
    }
    /// <summary>
    /// 流式布局模式
    /// </summary>
    public enum DocFlowLayoutMode
    {
        ForceSingle,MixSingleAndDouble,ForceDouble,ForceTriple
    }
    /// <summary>
    /// 标记->应由解析器内置FlowLayoutFlag以及页首+流式布局首节点三条件满足 ~a&b&c=T成立
    /// 结束了的特征，FlowLayout到
    /// NextBox = null & （下一个页首为流式布局头 || 下一页不是流式布局页 || 无后继Box）
    /// 即视为结束，固定布局的（不在链表中的）Ignore。
    /// </summary>
    public class DocTransDocument
    {
        //如果False，将忽略一切相关选项
        public bool IsFlowLayoutDocument { get; set; }
        //自动列中比例
        public int MaxDocPagesCount { get; set; }
        public List<DocTransPage>? Pages { get; set; }
        public bool AutoPageNumber { get; set; }
        public DocTransDocument()
        {

        }
    }
}
