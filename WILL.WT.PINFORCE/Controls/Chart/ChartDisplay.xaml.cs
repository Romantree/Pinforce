using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WILL.WT.PINFORCE.Managers;

namespace WILL.WT.PINFORCE.Controls.Chart
{
    /// <summary>
    /// ChartDisplay.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChartDisplay : UserControl
    {
        public ChartDisplay()
        {
            InitializeComponent();

            // 디자인 모드이면 런타임 빌드를 하지 않는다. => 생성자에 할당
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            this.DataContext = ChartDisplayViewModel.Instance;
        }
    }

    public class ChartDisplayViewModel : INotifyPropertyChanged
    {
        // 싱글톤 인스턴스
        private static ChartDisplayViewModel _instance;

        // Thread-safe 접근을 위한 Lock 객체
        private static readonly object _lock = new object();

        // 싱글톤 인스턴스 반환
        public static ChartDisplayViewModel Instance
        {
            get
            {
                // Thread-safe하게 인스턴스를 생성
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ChartDisplayViewModel();
                        }
                    }
                }
                return _instance;
            }
        }

        // ChartManager의 DataList
        private ChartDataList _chartData;

        public ChartDataList ChartData
        {
            get => _chartData;
            set
            {
                _chartData = value;
                OnPropertyChanged(nameof(ChartData));
            }
        }

        public ChartDisplayViewModel()
        {
            // ChartDataList 초기화
            ChartData = new ChartDataList();

            // 샘플 데이터 추가 (테스트용)
            ChartData.AddValue(1, 10, 15);
            ChartData.AddValue(2, 20, 25);
            ChartData.AddValue(3, 30, 35);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
