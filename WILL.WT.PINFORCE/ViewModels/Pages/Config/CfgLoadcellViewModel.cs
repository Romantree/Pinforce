using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFOCUS.Views.Pages.Config;

namespace WILL.WT.PINFOCUS.ViewModels.Pages.Config
{
    // Config (Main)Page -> Loadcell (Sub) Page 정의
    public class CfgLoadcellViewModel : IConfigViewModel
    {
        private readonly FrameworkElement view = new CfgLoadcellView();

        public override int No => 2;

        public override string Name => "Loadcell";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_scale_unbalanced", "Icons"] as Visual;
    }
}
