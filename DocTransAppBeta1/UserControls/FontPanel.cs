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

namespace DocTransAppBeta1.UserControls
{
    public partial class FontPanel : UserControl
    {
        public static readonly string __default_fontfamily = "宋体";
        public static readonly int __default_fontsize = 10;
        public Font FontLocal { get; set; }
        public FontPanel()
        {
            InitializeComponent();
            FontLocal = new Font(__default_fontfamily, __default_fontsize, FontStyle.Regular);
            label_FontName.Text = __default_fontfamily;
            label_FontSize.Text = __default_fontsize.ToString();
            try
            {
                if (Parent != null) ((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
            }
            catch { }
        }

        private void button_setFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                FontLocal = new Font(fd.Font.Name, fd.Font.Size, FontStyle.Regular);
                label_FontName.Text = fd.Font.Name;
                label_FontSize.Text = fd.Font.Size.ToString();
                try
                {
                    if (Parent != null) ((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
                }
                catch { }
            }
        }

        private void button_resetFont_Click(object sender, EventArgs e)
        {
            FontLocal = new Font(__default_fontfamily, __default_fontsize, FontStyle.Regular);
            label_FontName.Text = __default_fontfamily;
            label_FontSize.Text = __default_fontsize.ToString();
            try
            {
                if (Parent != null) ((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
            }
            catch { }
        }

        private void button_setFont_Click_1(object sender, EventArgs e)
        {
            button_setFont_Click(sender, e);
        }
    }
}
