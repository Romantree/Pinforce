using OpenCvSharp.ML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WILL.WT.PINFORCE.Models;

namespace WILL.WT.PINFORCE.Controls.Vision
{
    /// <summary>
    /// VisionDisplay.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VisionDisplay : UserControl
    {
        public VisionDisplay()
        {
            InitializeComponent();

            // 디자인 모드이면 런타임 빌드를 하지 않는다. => 생성자에 할당
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            this.DataContext = VisionDisplayViewModel.Instance;
        }
    }

    // VisionDisplay ViewModel
    public class VisionDisplayViewModel : INotifyPropertyChanged
    {
        // 싱글톤 인스턴스
        private static VisionDisplayViewModel _instance;

        // 싱글톤 인스턴스 반환
        public static VisionDisplayViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VisionDisplayViewModel();
                }
                return _instance;
            }
        }

        // Visible 프로퍼티
        private Visibility _visionVisibility = Visibility.Collapsed;
        private Visibility _uiVisibility = Visibility.Visible;
        public Visibility VisionVisibility
        {
            get => _visionVisibility;
            set
            {
                _visionVisibility = value;
                OnPropertyChanged(nameof(VisionVisibility));
            }
        }

        public Visibility UIVisibility
        {
            get => _uiVisibility;
            set
            {
                _uiVisibility = value;
                OnPropertyChanged(nameof(UIVisibility));
            }
        }

        // private CameraManager _camera;
        private DispatcherTimer _visionTimer;
        private DispatcherTimer _uiTimer;
        private BitmapSource _snapImageSource;

        public NormalCommand OnVisionCmd { get; }
        public VisionUIData UI { get; set; } = new VisionUIData();

        public VisionDisplayViewModel()
        {
            Debug.WriteLine($"{this.ToString()} + VisionDisplayViewModel 생성됨");
            // _camera = AP.Cam;
            _visionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            _visionTimer.Tick += UpdateFrame;

            /*
            if (_visionTimer.IsEnabled)
            {
                _visionTimer.Stop();
                SnapImageSource = null;
            }
            */

            if (AP.Device.Ins.IsOpen)
            {
                _uiTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(10)
                };
                _uiTimer.Tick += UpdateUI;
                _uiTimer.Start();
            }

            OnVisionCmd = new NormalCommand(VisionCmd);
        }

        ~VisionDisplayViewModel()
        {
            StopCamera();
            AP.Cam.ReleaseCamera();
            _uiTimer.Stop();
            Debug.WriteLine("Release CAM");
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

        private void VisionCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "LIVE":
                        {
                            StartCamera();
                            Debug.WriteLine("LIVE Clicked");
                        }
                        break;
                    case "STOP":
                        {
                            StopCamera();
                            Debug.WriteLine("STOP Clicked");
                        }
                        break;
                    case "SAVE":
                        {
                            Debug.WriteLine("SAVE Clicked");
                        }
                        break;
                    case "DATA":
                        {
                            // 안보이는 상태면 보여주고, 보이는 상태이면 숨긴다.
                            UIVisibility = (UIVisibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
                            Debug.WriteLine("DATA Clicked");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        // VisionDisplayViewModel.Instance.StartCamera(); 로 Camera 실행 가능
        public void StartCamera()
        {
            if (_visionTimer.IsEnabled == false)
            {
                VisionVisibility = Visibility.Visible;
                _visionTimer.Start();
                Debug.WriteLine("LIVE Start");
            }
        }

        // VisionDisplayViewModel.Instance.StopCamera();
        public void StopCamera()
        {
            VisionVisibility = Visibility.Collapsed;
            _visionTimer.Stop();
            Debug.WriteLine("LIVE Stop");
            // _camera.StopCamera();
            SnapImageSource = null;
            // Debug.WriteLine("Camera stopped");
        }

        // DispatcherTimer에 의해 계속 실행되는 메서드
        private void UpdateFrame(object sender, EventArgs e)
        {
            // Debug.WriteLine("Image Refreshed");
            SnapImageSource = AP.Cam.CaptureFrame();
            if (SnapImageSource == null) { Debug.WriteLine("Failed to capture frame"); }
            else
            {
                // Debug.WriteLine("Frame captured successfully");
                // Debug.WriteLine($"Frame Size: {SnapImageSource.PixelWidth}x{SnapImageSource.PixelHeight}");
            }
        }

        // DispatcherTimer에 의해 계속 실행되는 메서드
        private void UpdateUI(object sender, EventArgs e)
        {
            this.UI.Update();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
