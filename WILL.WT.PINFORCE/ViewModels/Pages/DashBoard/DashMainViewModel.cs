using System.Windows;
using System.Windows.Media;
using WILL.WT.PINFORCE.Views.Pages.DashBoard;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Models.Recipe;
using System;
using TS.FW;
using WILL.WT.PINFORCE.Views.Win;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Windows.Media.Imaging;
using WILL.WT.PINFORCE.Managers;
using System.Linq;
using System.Drawing.Drawing2D;
using WILL.WT.PINFORCE.Models.Setup;
using WILL.WT.PINFORCE.Process.Work.Main;
using TS.FW.Device;

namespace WILL.WT.PINFORCE.ViewModels.Pages.DashBoard
{
    public class DashMainViewModel : IDashViewModel
    {
        private readonly FrameworkElement view = new DashMainView();

        public override int No => 0;

        public override string Name => "Main";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_clipboard", "Icons"] as Visual;

        // 현재 Binding 되어있는 Recipe Data; ex)  Text="{Binding Start, StringFormat={}{0:f0} um}"
        public MainRecipeModel Rcp { get => this.GetValue<MainRecipeModel>(); set => this.SetValue(value); }

        public bool IsBusy { get => this.GetValue<bool>(); set => this.SetValue(value); }

        // 필드를 속성으로 변경
        private int _testRptCnt = 0;
        public int TestRptCnt
        {
            get => _testRptCnt;
            set
            {
                if (_testRptCnt != value)
                {
                    _testRptCnt = value;
                    OnPropertyChanged(nameof(TestRptCnt));  // INotifyPropertyChanged 호출
                }
            }
        }

        // 필드를 속성으로 변경
        private double _progress = 0;
        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value * 100; // Percent
                    OnPropertyChanged(nameof(Progress));  // INotifyPropertyChanged 호출
                }
            }
        }

        public LoadcellModel LoadcellModel_1 { get; set; } = new LoadcellModel(true);
        public LoadcellModel LoadcellModel_2 { get; set; } = new LoadcellModel(false);

        public override void Show()
        {
            try
            {
                base.Show();

                if (this.Rcp != null) // Load된 Recipe가 있으면
                {
                    this.Rcp = AP.Rcp.Reload<MainRecipeModel>(this.Rcp);
                }
                else // Load된 Recipe가 없으면
                {
                    this.Rcp = AP.Rcp.GetRecipe(RecipeType.MAIN, 0) as MainRecipeModel;
                }
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

                TestRptCnt = AP.Proc.Main.TestRptCnt;
                Progress = AP.Proc.Main.Progress;

                // 작동중이면 Button Disable
                this.IsBusy = !AP.Proc.IsBusy;

                this.LoadcellModel_1.Update();
                this.LoadcellModel_2.Update();
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
                    case "Start":
                        {
                            //if(this.UC.RcpSelected ==null)
                            if (this.Rcp == null)
                            {
                                AP.System.InterlockMsgEvent("선택 된 레시피가 없습니다.");
                                return;
                            }

                            if (AP.Device[eAxis.AXIS_Z].ActPosition != 0)
                            {
                                AP.System.InterlockMsgEvent("좌표가 0으로 초기화되지 않았습니다.");
                                return;
                            }

                            if ((!AP.IsSim) && ((!LoadcellModel_1.IsOpen) || (!LoadcellModel_2.IsOpen)))
                            {
                                AP.System.InterlockMsgEvent("Loadcell이 연결되지 않았습니다.");
                                return;
                            }

                            if (LoadcellModel_1.Data != 0 || LoadcellModel_2.Data != 0)
                            {
                                AP.System.InterlockMsgEvent("Loadcell의 무게가 초기화되지 않았습니다.");
                                return;
                            }

                            AP.Proc.Start(this.Rcp);
                        }
                        break;
                    case "Stop":
                        {
                            AP.ProcessStop();
                        }
                        break;
                    case "PinStop":
                        {

                        }
                        break;
                    case "Initial":
                        {
                            AP.Proc.Init.Start();
                            // _axis.HomeAsync(out HomeAsyncResult result);
                            AP.Device[eAxis.AXIS_Z].HomeAsync(out HomeAsyncResult result);
                        }
                        break;
                    case "Rcp":
                        {
                            var view = new RcpSelectViewModel(Managers.RecipeType.MAIN);
                            if (view.Show() == false) return;

                            this.Rcp = view.RcpSelected as MainRecipeModel;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}