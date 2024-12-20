using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Views.Pages.Setup;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Setup
{
    public class SetupVisionViewModel : ISetupViewModel
    {
        private readonly FrameworkElement view = new SetupVisionView();

        public override int No => 3;

        public override string Name => "Vision";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_camera", "Icons"] as Visual;

    }
}
