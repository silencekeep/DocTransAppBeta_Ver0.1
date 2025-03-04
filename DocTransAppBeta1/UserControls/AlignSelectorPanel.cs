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
using DocTransAppBeta1.UserControls.ConfigPanels;

namespace DocTransAppBeta1.UserControls
{
    public partial class AlignSelectorPanel : UserControl
    {
        private BoxAlignMode _AlignMode;
        public BoxAlignMode AlignMode {
            get
            {
                return _AlignMode;
            }
            set 
            {
                _AlignMode = value;
                switch (value)
                {
                    case BoxAlignMode.Default:
                        radioButton_alignDefault.Checked = true;
                        break;
                    case BoxAlignMode.Left:
                        radioButton_alignLeft.Checked = true;
                        break;
                    case BoxAlignMode.Center:
                        radioButton_alignCenter.Checked = true;
                        break;
                    case BoxAlignMode.Right:
                        radioButton_alignRight.Checked = true;
                        break;
                }
            }
        }
        public AlignSelectorPanel()
        {
            InitializeComponent();
            _AlignMode = BoxAlignMode.Default;
        }

        private void radioButton_alignDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_alignDefault.Checked)
            {
                _AlignMode = BoxAlignMode.Default;
                ((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
            }
        }

        private void radioButton_alignLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_alignLeft.Checked)
            {
                _AlignMode = BoxAlignMode.Left;
                ((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
            }
        }

        private void radioButton_alignCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_alignCenter.Checked)
            {
                _AlignMode = BoxAlignMode.Center;
                ((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
            }
        }

        private void radioButton_alignRight_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_alignRight.Checked)
            {
                _AlignMode = BoxAlignMode.Right;
                ((ConfigPanelNotifier)(Parent.Parent)).NotifyParentForSelfFlush();
            }
        }
    }
}
