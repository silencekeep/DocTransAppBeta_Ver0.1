using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocTransAppBeta1.UserControls.ConfigPanels;
using DocTransAppBeta1.PdfStructure;

namespace DocTransAppBeta1.UserControls
{
    public partial class TitleConfigPanel : UserControl, ConfigPanelNotifier, ConfigPanel
    {
        public bool IsLogo { get; set; }
        public Font FontLocal { get; set; }
        public BoxAlignMode InnerAlignMode { get; set; }
        public BoxAlignMode OuterAlignMode { get; set; }
        public TitleConfigPanel()
        {
            InitializeComponent();
            FontLocal = fontPanel1.FontLocal;
        }
        //interface impl
        public void NotifyParentForSelfFlush()
        {
            //throw new NotImplementedException();
            //获取所有的值并保存到Box
            bool isLogo = checkBox1.Checked;
            Font f = this.fontPanel1.FontLocal;
            BoxAlignMode inner = alignSelectorPanel_inner.AlignMode;
            BoxAlignMode outer = alignSelectorPanel_outer.AlignMode;
            IsLogo = isLogo;
            FontLocal = f;
            InnerAlignMode = inner;
            OuterAlignMode = outer;
            //((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            NotifyParentForSelfFlush();
        }

        private void fontPanel1_Load(object sender, EventArgs e)
        {

        }
    }
}
