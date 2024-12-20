using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFOCUS.Views.Pages.Setup;

namespace WILL.WT.PINFOCUS.ViewModels.Pages.Setup
{
    // Setup (Main)Page -> Vision (Sub) Page 정의
    public class SetupVisionViewModel : ISetupViewModel
    {
        private readonly FrameworkElement view = new SetupVisionView();

        public override int No => 3;

        public override string Name => "Vision";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_camera", "Icons"] as Visual;
    }
}
