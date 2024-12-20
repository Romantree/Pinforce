using System;
using System.Windows;
using WILL.WT.PINFORCE.ViewModels;
using TS.FW;

namespace WILL.WT.PINFORCE.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DataContext = new MainViewModel();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Image_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                if (AP.IsSim == false) return;

                if (e.LeftButton == System.Windows.Input.MouseButtonState.Released) return;

                this.DragMove();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
