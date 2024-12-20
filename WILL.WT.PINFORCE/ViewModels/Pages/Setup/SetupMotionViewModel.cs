using WILL.WT.PINFORCE.Views.Pages.Setup;
using System;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Models.Axis;
using TS.FW;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Setup
{
    public class SetupMotionViewModel : ISetupViewModel
    {
        private readonly FrameworkElement view = new SetupMotionView();

        public override int No => 0;

        public override string Name => "Motion";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_server", "Icons"] as Visual;

        public MotionModel Stage { get; set; } = new MotionModel();

        public override void Init()
        {
            try
            {
                base.Init();

                Stage.Axis.Add(new AxisModel(eAxis.AXIS_Z));

                Stage.SelectAxisCmd(Stage.Axis.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Update()
        {
            try
            {
                Stage.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
