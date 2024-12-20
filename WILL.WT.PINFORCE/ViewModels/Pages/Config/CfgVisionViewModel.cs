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
    // Config (Main)Page -> Vision (Sub) Page 정의
    public class CfgVisionViewModel : IConfigViewModel
    {
        private readonly FrameworkElement view = new CfgVisionView();

        public override int No => 3;

        public override string Name => "Vision";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_camera", "Icons"] as Visual;
    }
}
