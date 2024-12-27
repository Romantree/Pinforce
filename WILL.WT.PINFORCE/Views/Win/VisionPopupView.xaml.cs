using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Views.Win
{
    /// <summary>
    /// VisionPopupView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VisionPopupView : Window
    {
        public VisionPopupView()
        {
            InitializeComponent();

            // 디자인 모드이면 런타임 빌드를 하지 않는다. => 생성자에 할당
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;
        }
    }

    public class VisionPopupViewModel : IViewModel
    {
        private readonly VisionPopupView _view = new VisionPopupView();

        private DispatcherTimer _visionTimer;
        private BitmapSource _snapImageSource;

        public NormalCommand OnPopupCmd { get; }
        public VisionPopupViewModel()
        {
            this._view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this._view.DataContext = this;

            _visionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            _visionTimer.Tick += UpdateFrame;
            _visionTimer.Start();
        }
        
        public BitmapSource SnapImageSource
        {
            get => _snapImageSource;
            set
            {
                _snapImageSource = value;
                OnPropertyChanged();
            }
        }

        public bool? Show()
        {
            return this._view.ShowDialog();
        }

        protected override void OnCommand(object commandParameter)
        {
            Debug.WriteLine("Clicked!");
            try
            {
                switch (commandParameter as string)
                {
                    case "CANCEL":
                        {
                            this._view.DialogResult = false;
                            this._view.Close();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        // DispatcherTimer에 의해 계속 실행되는 메서드
        private void UpdateFrame(object sender, EventArgs e)
        {
            //SnapImageSource = AP.Cam.CaptureFrame();
            if (SnapImageSource == null) { Debug.WriteLine("Failed to capture frame"); }
            else
            {
                // Debug.WriteLine("Frame captured successfully");
                // Debug.WriteLine($"Frame Size: {SnapImageSource.PixelWidth}x{SnapImageSource.PixelHeight}");
            }
        }
    }
}
