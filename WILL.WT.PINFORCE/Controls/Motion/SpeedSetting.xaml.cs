using System;
using System.Collections.Generic;
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
using TS.FW;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Models.Setup;

namespace WILL.WT.PINFORCE.Controls.Motion
{
    /// <summary>
    /// SpeedSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SpeedSetting : Window
    {
        public SpeedSetting()
        {
            InitializeComponent();
        }
    }

    public class SpeedSettingViewModel : IViewModel
    {
        private readonly SpeedSetting _view = new SpeedSetting();

        // Binding Data => DataModel (SpeedModel.cs)
        public SpeedModel speedModel { get; set; } = new SpeedModel();

        public SpeedSettingViewModel()
        {
            this._view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this._view.DataContext = this;
        }

        public bool? Show()
        {
            this.Refresh();

            return this._view.ShowDialog();
        }

        private void Refresh(bool isSave = false)
        {
            try
            {
                // AP.cs -> DB Class 내에 작성한 DB.cs 작성
                // DB.cs 구조는 Model과 동일하게
                if (isSave)
                {
                    // DB 저장
                    DB.DataCopyEx(this.speedModel, DB.Motion.speed);
                    // Debug.WriteLine(this.speedModel.JogLowSpeed);
                }

                // DB 불러오기
                DB.DataCopy(this.speedModel, DB.Motion.speed);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "SAVE":
                        {
                            this.Refresh(true);
                            AP.System.InterlockMsgEvent("User 데이터 저장 되었습니다.");

                            this._view.DialogResult = true;
                            this._view.Close();
                        }
                        break;
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
    }
}
