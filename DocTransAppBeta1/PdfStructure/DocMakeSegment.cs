using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTransAppBeta1.PdfStructure
{
    public class DocMakeSegment
    {
        public bool IsFixed { get; set; }
        public bool IsContentSegment { get; set; }
        public List<DocTransBox> SequentialBoxList { get; set; }
        //public 
    }
}
