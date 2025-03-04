﻿using System;
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
    public partial class FormulaConfigPanel : UserControl, ConfigPanelNotifier, ConfigPanel
    {
        public bool IsRawImage { get; set; }
        public BoxAlignMode AlignMode { get; set; }
        public FormulaConfigPanel()
        {
            InitializeComponent();
        }

        public void NotifyParentForSelfFlush()
        {
            //throw new NotImplementedException();
            bool isLogo = checkBox1.Checked;
            //Font f = this.fontPanel1.FontLocal;
            BoxAlignMode inner = alignSelectorPanel_outer.AlignMode;
            IsRawImage = isLogo;
            //FontLocal = f;
            AlignMode = inner;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            NotifyParentForSelfFlush();
        }
    }
}
