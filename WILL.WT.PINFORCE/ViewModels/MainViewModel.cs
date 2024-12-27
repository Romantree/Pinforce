using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using TS.FW.Diagnostics;
using WILL.WT.PINFORCE.Models;
using TS.FW.Wpf.v2.Controls.InPut;
using TS.FW.Wpf.v2.Controls.Win;
using TS.FW.Wpf.v2.Core;
using TS.FW.Wpf.v2.Helpers;
using TS.FW.Wpf.v2.Subscribe;
using TS.FW;
using WILL.WT.PINFORCE.Models.Setup;
using System.Drawing;
using System.Windows.Media;

namespace WILL.WT.PINFORCE.ViewModels
{
    public partial class MainViewModel : IViewModel
    {
        private readonly BackgroundTimer trUpdate = new BackgroundTimer(ApartmentState.MTA);

        public MainViewModel()
        {
            IViewModel.SourceLevels = System.Diagnostics.SourceLevels.Critical;

            if (IViewModel.IsDesignMode) return;

            KeyPad.Scale = 1;
            KeyboardPad.Scale = 1;

            Logger.RootPath = AP.LOG_FILE;
            Logger.LogLevel = AP.LOG_LEVEL;
            Logger.FileDeleteDay = 30;

            this.trUpdate.SleepTimeMsc = 100;
            this.trUpdate.DoWork += trUpdate_DoWork;

            AP.System.OnLoginModeChangedEvent += System_OnLoginModeChangedEvent;
            AP.System.OnInterlockMsgEvent += System_OnInterlockMsgEvent;
            AP.System.OnInterlockCheckEvent += System_OnInterlockCheckEvent;

            this.ProgramStart();
        }

        public bool Alarm { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Buzzer { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public LoginMode Login { get => this.GetValue<LoginMode>(); set => this.SetValue(value); }

        public bool IsEnable { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public TowerLamp TowerLamp { get; set; } = new TowerLamp();

        public NetworkState NetworkState { get; set; } = new NetworkState();

        public ObservableCollection<IMainPageViewModel> MenuList { get; set; } = new ObservableCollection<IMainPageViewModel>();

        public IMainPageViewModel SelectedMenu { get => this.GetValue<IMainPageViewModel>(); set => this.SetValue(value); }

        public LoadcellModel LoadcellModel_1 { get; set; }
        public LoadcellModel LoadcellModel_2 { get; set; }

        private void trUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                this.Alarm = AP.Alarm.IsAlarm;

                this.TowerLamp.Update();

                if (LoadcellModel_1 == null)
                    LoadcellModel_1 = new LoadcellModel(true);
                if (LoadcellModel_2 == null)
                    LoadcellModel_2 = new LoadcellModel(false);

                if (LoadcellModel_1 != null)
                {
                    LoadcellModel_1.Update();
                }
                if (LoadcellModel_2 != null)
                {
                    LoadcellModel_2.Update();
                }

                // 작동중이면 Button Disable
                // this.IsEnable = !AP.Proc.IsBusy;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                this.SelectedMenu?.Update();
            }
        }

        private void System_OnLoginModeChangedEvent(object sender, LoginMode e)
        {
            try
            {
                this.Login = e;

                if (Login == LoginMode.Lock)
                {
                    this.IsEnable = false;

                    var menu = this.MenuList.First();
                    this.SetMenu(menu);
                }
                else
                {
                    this.IsEnable = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void System_OnInterlockMsgEvent(object sender, string e)
        {
            try
            {
                Logger.Write(sender, e, Logger.LogEventLevel.Error);

                MsgBox.Show(e, MsgBoxType.CLOSE);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private bool? System_OnInterlockCheckEvent(string arg)
        {
            try
            {
                Logger.Write(this, arg, Logger.LogEventLevel.Error);

                return MsgBox.Show(arg, MsgBoxType.YES_NO);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);

                return false;
            }
        }

        protected override void OnCommand(object parameter)
        {
            try
            {
                switch (parameter as string)
                {
                    case "EXIT":
                        {
                            Logger.Write(this, $"프로그램 종료({ProgramHelper.Ins.Version}) =====================");

                            BackgroundTimer.AllStop();
                            this.MainView.Close();
                        }
                        break;
                    case "Alarm":
                        {
                            var menu = this.MenuList.FirstOrDefault(t => t.Name == "Alarm");
                            this.SetMenu(menu);
                        }
                        break;
                    case "Buzzer":
                        {

                        }
                        break;
                    case "Login":
                        {
                            var mode = LoginPad.ShowLogin(DB.User.Operator.Password, DB.User.Engineer.Password, DB.User.Manager.Password);
                            if (mode.HasValue == false) return;

                            AP.System.LoginModeChangedEvent(mode.Value);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SetMenu(IMainPageViewModel menu)
        {
            if (this.SelectedMenu == menu) return;

            if (this.SelectedMenu != null) this.SelectedMenu.Hide();

            this.SelectedMenu = menu;
            this.SelectedMenu.Show();
        }
    }

    public partial class MainViewModel
    {
        private void ProgramStart()
        {
            this.MainView.Visibility = System.Windows.Visibility.Hidden;

            Logger.Write(this, $"프로그램 시작({ProgramHelper.Ins.Version}) =====================");

            StartControl.SetData("데이터 베이스 초기화", InitDatabase);
            StartControl.SetData("Device 초기화", InitDevice);
            StartControl.SetData("In/Out 초기화", InitIO);
            StartControl.SetData("Motion 초기화", InitMotion);
            StartControl.SetData("Network 초기화", AP.Net.Start);
            StartControl.SetData("Memory 수집 시작", ProgramHelper.Ins.Start);
            StartControl.SetData("프로그램 메뉴 생성", InitMenu);
            //StartControl.SetData("Vision 초기화", AP.Cam.InitCamera);
            StartControl.SetData("프로그램 실행", ProcessStart);

            StartControl.Start(ProgramCmp);
        }

        private void InitDatabase()
        {
            DB.LoadDatabase();

            AP.Alarm.LoadDataBase();
            AP.Rcp.InitPath();
            AP.Rcp.LoadDatabase();
        }

        private void InitDevice()
        {
            var open = AP.Device.Open();
            if (open == false) throw new Exception(open.Comment);
        }

        private void InitIO()
        {
            AP.IO.LoadDataBase();
            AP.IO.Start();
        }

        private void InitMotion()
        {
            var iniAxis = AP.Device.Axis.InitAxis(typeof(eAxis));
            if (iniAxis == false) throw new Exception(iniAxis.Comment);

            AP.Device.InitScale();
        }

        private void InitMenu() => IViewModel.Dispatcher.Invoke(MenuCreate);

        private void ProcessStart()
        {
            AP.System.LoginModeChangedEvent(AP.IsSim ? LoginMode.Programmer : LoginMode.Lock);

            AP.Proc.Saferty.Start();

            this.trUpdate.Start();
        }

        private void MenuCreate()
        {
            foreach (var item in IPageViewModel.ToPageViewList<IMainPageViewModel>())
            {
                item.Init();
                this.MenuList.Add(item);
            }
        }

        private void ProgramCmp()
        {
            this.SelectedMenu = MenuList.FirstOrDefault();
            if (this.SelectedMenu != null) this.SelectedMenu.Show();

            // 시작 권한을 설정하고 싶으면 아래처럼
            // AP.System.LoginModeChangedEvent(LoginMode.Operator);
            AP.System.ModeChangedEvent(false);

            this.MainView.Visibility = System.Windows.Visibility.Visible;
        }

    }
}
