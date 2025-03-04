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
    public partial class ImageConfigPanel : UserControl, ConfigPanelNotifier, ConfigPanel
    {
        public BoxAlignMode AlignMode { get; set; }
        public ImageConfigPanel()
        {
            InitializeComponent();
        }

        public void NotifyParentForSelfFlush()
        {
            //throw new NotImplementedException();
            AlignMode = alignSelectorPanel1.AlignMode;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NotifyParentForSelfFlush();

        }
    }
}
