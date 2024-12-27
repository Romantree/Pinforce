using OpenCvSharp.ML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;
using TS.FW;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Models;
using WILL.WT.PINFORCE.Models.Recipe;
using WILL.WT.PINFORCE.Views.Win;

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

        public void CaptureArea()
        {

        }
    }

    // VisionDisplay ViewModel
    public class VisionDisplayViewModel : IViewModel
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

        // MainProc -> OD
        #region OD
        private double _od;
        public double OD
        {
            get => _od;
            set
            {
                if (_od != value)
                {
                    _od = value; // Percent
                    OnPropertyChanged(nameof(OD));  // INotifyPropertyChanged 호출
                }
            }
        }
        #endregion

        // DB -> WindowSize
        #region WindowSize
        private int _windowSize;
        public int WindowSize
        {
            get => _windowSize;
            set
            {
                if (_windowSize != value)
                {
                    _windowSize = value; // Percent
                    OnPropertyChanged(nameof(WindowSize));  // INotifyPropertyChanged 호출
                }
            }
        }
        #endregion
        // DB -> FontSize
        #region FontSize
        private int _fontSize;
        public int FontSize
        {
            get => _fontSize;
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value; // Percent
                    OnPropertyChanged(nameof(FontSize));  // INotifyPropertyChanged 호출
                }
            }
        }
        #endregion

        public VisionDisplayViewModel()
        {
            _visionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            _visionTimer.Tick += UpdateFrame;

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
            //AP.Cam.ReleaseCamera();
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
                            CaptureArea();
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
                    case "EXTD":
                        {
                            Debug.WriteLine("EXTD Clicked");

                            if (SnapImageSource != null)
                            {
                                var view = new VisionPopupViewModel();
                                view.Show();
                            }
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

                // Crevis Camera 일때는 StreamStart를, 아니면 주석처리
                // AP.Cam.StreamStart();

                _visionTimer.Start();
            }
        }

        // VisionDisplayViewModel.Instance.StopCamera();
        public void StopCamera()
        {
            VisionVisibility = Visibility.Collapsed;

            // Crevis Camera 일때는 StreamStart를, 아니면 주석처리
            // AP.Cam.StreamEnd();

            _visionTimer.Stop();
            // _camera.StopCamera();
            SnapImageSource = null;
        }

        private const string ROOT = @"..\Data\Screenshots";
        private void CaptureArea()
        {
            try
            {
                if (SnapImageSource != null)
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(SnapImageSource));

                    var fileName = "Screenshot";
                    var filePath = Path.Combine(Path.GetFullPath(ROOT), $"{DateTime.Now:yyyyMMdd_HHmmss}_{fileName}.png");

                    // 폴더가 존재하지 않으면 생성
                    if (Directory.Exists(ROOT) == false) Directory.CreateDirectory(ROOT);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }

                    if (DB.Vision.visionData.CaptureNotice)
                        AP.System.InterlockMsgEvent("이미지 저장 완료.");

                   // VisionDisplay _view = new VisionDisplay();
                   // 
                   // DataTemplate result = _view.Resources["xVisionArea"] as DataTemplate;
                   // Image img = FindChildElement<Image>(this, "xImage");
                   // if (result != null)
                   //     Debug.WriteLine("Founded");
                   // else
                   //     Debug.WriteLine("Not Found");

                    
                    // CaptureDataTemplateAsImage(result);
                }
            }
            catch (Exception ex)
            { 
                Logger.Write(this, ex);
            }
        }

        public T FindChildElement<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // 자식 요소의 Name이 같다면 해당 요소를 반환
                if (child is T && (child as FrameworkElement).Name == name)
                {
                    return (T)child;
                }

                // 자식 요소들을 다시 탐색
                var result = FindChildElement<T>(child, name);
                if (result != null) return result;
            }
            return null;
        }

        public void CaptureDataTemplateAsImage(DataTemplate dataTemplate)
        {
            // 1. DataTemplate을 포함하는 컨트롤 생성
            var contentControl = new ContentControl
            {
                ContentTemplate = dataTemplate,
                Content = dataTemplate.DataType // ViewModel의 데이터 바인딩이 필요함.
            };

            // 2. ContentControl을 렌더링하여 이미지를 캡처
            var renderBitmap = new RenderTargetBitmap(
                (int)contentControl.ActualWidth,
                (int)contentControl.ActualHeight,
                96, 96, PixelFormats.Pbgra32);

            contentControl.Measure(new System.Windows.Size(contentControl.ActualWidth, contentControl.ActualHeight));
            contentControl.Arrange(new System.Windows.Rect(0, 0, contentControl.ActualWidth, contentControl.ActualHeight));

            renderBitmap.Render(contentControl);

            // 3. RenderTargetBitmap을 이미지로 저장
            SaveBitmapToFile(renderBitmap);
        }

        public void SaveBitmapToFile(RenderTargetBitmap renderBitmap)
        {
            var fileName = "Screenshot";
            var filePath = Path.Combine(Path.GetFullPath(ROOT), $"{DateTime.Now:yyyyMMdd_HHmmss}_{fileName}.png");

            // 폴더가 존재하지 않으면 생성
            if (Directory.Exists(ROOT) == false) Directory.CreateDirectory(ROOT);

            // BitmapEncoder를 사용하여 이미지를 파일로 저장
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        // DispatcherTimer에 의해 계속 실행되는 메서드
        private void UpdateFrame(object sender, EventArgs e)
        {
            // Debug.WriteLine("Image Refreshed");
            //SnapImageSource = AP.Cam.CaptureFrame();
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
            this.OD = AP.Proc.Main.Step_OD;

            this.WindowSize = DB.Vision.visionData.WindowSize;
            this.FontSize = DB.Vision.visionData.FontSize;
        }
    }
}
