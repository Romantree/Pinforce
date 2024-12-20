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
    // Config (Main)Page -> I/O (Sub) Page 정의
    public class CfgIOViewModel : IConfigViewModel
    {
        private readonly FrameworkElement view = new CfgIOView();

        public override int No => 1;

        public override string Name => "I/O";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_source_fork", "Icons"] as Visual;
    }
}
