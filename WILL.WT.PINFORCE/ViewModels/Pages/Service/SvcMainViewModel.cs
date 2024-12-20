using System.Windows;
using System.Windows.Media;
using WILL.WT.PINFORCE.ViewModels.Pages;
using TS.FW.Wpf.v2.Helpers;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Service
{
    public class SvcMainViewModel : ISvcViewModel
    {
        private readonly FrameworkElement view = new FrameworkElement();

        public override int No => 0;

        public override string Name => "Main";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_clipboard", "Icons"] as Visual;
    }
}
