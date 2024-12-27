using System.Windows;
using System.Windows.Media;
using WILL.WT.PINFORCE.ViewModels.Pages;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Views.Pages.Config;
using WILL.WT.PINFORCE.Models.Config;
using System;
using TS.FW;
using WILL.WT.PINFORCE.Models.Recipe;
using WILL.WT.PINFORCE.Views.Win;
using System.Diagnostics;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Config
{
    // Config (Main)Page -> Work (Sub) Page 정의
    public class CfgWorkViewModel : IConfigViewModel
    {
        // ViewModel 정의
        private readonly FrameworkElement view = new CfgWorkView();
        public override int No => 0;
        public override string Name => "Work";
        public override FrameworkElement View => view;
        public override Visual Icon => ResourceHelper.Ins["appbar_server", "Icons"] as Visual;

        // Binding Data => DataModel
        public MotionDataModel MotionDataModel { get; set; } = new MotionDataModel();

        public TimeoutDataModel TimeoutDataModel { get; set; } = new TimeoutDataModel();

        public DelayDataModel DelayDataModel { get; set; } = new DelayDataModel();

        public LimitDataModel LimitDataModel { get; set; } = new LimitDataModel();

        public LoadcellDataModel LoadcellDataModel { get; set; } = new LoadcellDataModel();
        public LogDataModel LogDataModel { get; set; } = new LogDataModel();

        public AutoContactRcpModel AutoContact { get => this.GetValue<AutoContactRcpModel>(); set => this.SetValue(value); }

        public override void Show()
        {
            try
            {
                base.Show();

                this.Refresh();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
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
                    DB.DataCopyEx(this.MotionDataModel, DB.WorkParam.motion);
                    DB.DataCopyEx(this.TimeoutDataModel, DB.WorkParam.errorTimeout);
                    DB.DataCopyEx(this.DelayDataModel, DB.WorkParam.delay);
                    DB.DataCopyEx(this.LimitDataModel, DB.WorkParam.limit);
                    DB.DataCopyEx(this.LoadcellDataModel, DB.WorkParam.loadcellRange);
                    DB.DataCopyEx(this.LogDataModel, DB.WorkParam.logData);

                    AP.Rcp.Save(AutoContact);
                }

                // DB 불러오기
                DB.DataCopy(this.MotionDataModel, DB.WorkParam.motion);
                DB.DataCopy(this.TimeoutDataModel, DB.WorkParam.errorTimeout);
                DB.DataCopy(this.DelayDataModel, DB.WorkParam.delay);
                DB.DataCopy(this.LimitDataModel, DB.WorkParam.limit);
                DB.DataCopy(this.LoadcellDataModel, DB.WorkParam.loadcellRange);
                DB.DataCopy(this.LogDataModel, DB.WorkParam.logData);

                AutoContact = AP.Rcp.AutoContact;
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
                    case "Save":
                        {
                            if (AP.System.InterlockCheckEvent("설정 저장 하시겠습니까?") == false) return;
                            this.Refresh(true);
                            AP.System.InterlockMsgEvent("User 데이터 저장 되었습니다.");
                        }
                        break;
                    case "Refresh":
                        {
                            AP.System.InterlockCheckEvent("데이터를 Refresh 하시겠습니까?");
                            this.Refresh();
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
