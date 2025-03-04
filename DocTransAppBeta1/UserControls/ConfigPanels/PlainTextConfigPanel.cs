using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocTransAppBeta1.PdfStructure;

namespace DocTransAppBeta1.UserControls.ConfigPanels
{
    public partial class PlainTextConfigPanel : UserControl, ConfigPanelNotifier, ConfigPanel
    {
        public bool AutoIndent { get; set; }
        public Font FontLocal { get; set; }
        public BoxAlignMode InnerAlignMode { get; set; }
        public BoxAlignMode OuterAlignMode { get; set; }
        public PlainTextConfigPanel()
        {
            InitializeComponent();
            FontLocal = fontPanel1.FontLocal;
        }

        public void NotifyParentForSelfFlush()
        {
            //throw new NotImplementedException();
            bool _AutoIndent = checkBox1.Checked;
            Font f = this.fontPanel1.FontLocal;
            BoxAlignMode inner = alignSelectorPanel_inner.AlignMode;
            BoxAlignMode outer = alignSelectorPanel_outer.AlignMode;
            AutoIndent = _AutoIndent;
            FontLocal = f;
            InnerAlignMode = inner;
            OuterAlignMode = outer;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            NotifyParentForSelfFlush();
        }
    }
}
