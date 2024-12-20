using WILL.WT.PINFORCE.Models.InOut;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using TS.FW;

namespace WILL.WT.PINFORCE.Views.Pages.Setup
{
    /// <summary>
    /// SetupInOutView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SetupInOutView : UserControl
    {
        public SetupInOutView()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (AP.IsSim == false) return;

                var model = (sender as Ellipse).DataContext as InOutModel;
                if (model == null || string.IsNullOrEmpty(model.Name)) return;

                AP.IO.WriteX(!model.OnOff, model.Key);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
