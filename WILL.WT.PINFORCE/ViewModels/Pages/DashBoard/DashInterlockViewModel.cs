using WILL.WT.PINFOCUS.Views.Pages.DashBoard;
using System;
using System.Windows.Media;
using System.Windows;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFOCUS.Models;
using TS.FW;

namespace WILL.WT.PINFOCUS.ViewModels.Pages.DashBoard
{
    public class DashInterlockViewModel : IDashViewModel
    {
        private readonly FrameworkElement view = new DashInterlockView();

        public override int No => 1;

        public override string Name => "Interlock";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_calendar_tomorrow", "Icons"] as Visual;

        public EqpControl Eqp { get; set; } = new EqpControl();

        public InterlockState Interlock { get; set; } = new InterlockState();

        public override void Init()
        {
            try
            {
                this.Eqp.Init();
                this.Interlock.Init();

                base.Init();
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
                this.Eqp.Update();
                this.Interlock.Update();

                base.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnCommand(object parameter)
        {
            try
            {
                switch (parameter as string)
                {

                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
