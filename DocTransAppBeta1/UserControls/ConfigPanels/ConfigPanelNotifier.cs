using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTransAppBeta1.UserControls.ConfigPanels
{
    internal interface ConfigPanelNotifier
    {
        public void NotifyParentForSelfFlush();
    }
    internal interface ConfigPanel
    {

    }
}
