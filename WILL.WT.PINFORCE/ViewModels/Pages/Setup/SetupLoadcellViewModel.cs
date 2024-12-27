using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Models.Setup;
using WILL.WT.PINFORCE.Views.Pages.Setup;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Setup
{
    // Setup (Main)Page -> Loadcell (Sub) Page 정의
    public class SetupLoadcellViewModel : ISetupViewModel
    {
        private readonly FrameworkElement view = new SetupLoadcellView();

        public override int No => 2;

        public override string Name => "Loadcell";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_scale_unbalanced", "Icons"] as Visual;

        // Binding Data => DataModel

        public LoadcellModel LoadcellModel_1 { get; set; } = new LoadcellModel(true);
        public LoadcellModel LoadcellModel_2 { get; set; } = new LoadcellModel(false);

        public override void Init()
        {
            try
            {
                base.Init();

                this.LoadcellModel_1.Init();
                this.LoadcellModel_2.Init();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Show()
        {
            try
            {
                base.Show();
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
                base.Update();

                this.LoadcellModel_1.Update();
                this.LoadcellModel_2.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
