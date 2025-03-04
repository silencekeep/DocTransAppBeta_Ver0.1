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
    public partial class AbandonConfigPanel : UserControl, ConfigPanelNotifier, ConfigPanel
    {
        public AbandonType AbandonType { get; set; }
        public bool IsLogo { get; set; }
        public Font FontLocal { get; set; }
        public BoxAlignMode InnerAlignMode { get; set; }
        public BoxAlignMode OuterAlignMode { get; set; }
        public AbandonConfigPanel()
        {
            InitializeComponent();
            FontLocal = fontPanel1.FontLocal;
            comboBox1.SelectedIndex = 0;
        }

        public void NotifyParentForSelfFlush()
        {
            //throw new NotImplementedException();
            bool isLogo = checkBox1.Checked;
            Font f = this.fontPanel1.FontLocal;
            BoxAlignMode inner = alignSelectorPanel_inner.AlignMode;
            BoxAlignMode outer = alignSelectorPanel_inner.AlignMode;
            IsLogo = isLogo;
            FontLocal = f;
            InnerAlignMode = inner;
            OuterAlignMode = outer;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    AbandonType = AbandonType.Other;
                    break;
                case 1:
                    AbandonType = AbandonType.Header;
                    break;
                case 2:
                    AbandonType = AbandonType.Footer;
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    AbandonType = AbandonType.Other;
                    break;
                case 1:
                    AbandonType = AbandonType.Header;
                    break;
                case 2:
                    AbandonType = AbandonType.Footer;
                    break;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            NotifyParentForSelfFlush();
        }
    }
}
