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
using System.Runtime.CompilerServices;

namespace DocTransAppBeta1.UserControls.ConfigPanels
{
    public partial class CaptionConfigPanel : UserControl, ConfigPanelNotifier, ConfigPanel
    {
        public bool IsRawImage { get; set; }
        public Font FontLocal { get; set; }
        public CaptionPosition CaptionPosition { get; set; }
        public CaptionConfigPanel()
        {
            InitializeComponent();
            FontLocal = fontPanel1.FontLocal;
            comboBox1.SelectedIndex = 1;
        }

        public void NotifyParentForSelfFlush()
        {
            //throw new NotImplementedException();
            bool isLogo = checkBox1.Checked;
            Font f = this.fontPanel1.FontLocal;
            IsRawImage = isLogo;
            FontLocal = f;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    CaptionPosition = CaptionPosition.Above;
                    break;
                case 1:
                    CaptionPosition = CaptionPosition.Below;
                    break;
                case 2:
                    CaptionPosition = CaptionPosition.Left;
                    break;
                case 3:
                    CaptionPosition = CaptionPosition.Right;
                    break;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    CaptionPosition = CaptionPosition.Above;
                    break;
                case 1:
                    CaptionPosition = CaptionPosition.Below;
                    break;
                case 2:
                    CaptionPosition = CaptionPosition.Left;
                    break;
                case 3:
                    CaptionPosition = CaptionPosition.Right;
                    break;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            NotifyParentForSelfFlush();
        }
    }
}
