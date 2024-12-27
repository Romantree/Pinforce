using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Models.Setup;
using WILL.WT.PINFORCE.Views.Pages.Setup;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Setup
{
    public class SetupVisionViewModel : ISetupViewModel
    {
        private readonly FrameworkElement view = new SetupVisionView();

        public override int No => 3;

        public override string Name => "Vision";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_camera", "Icons"] as Visual;

        // Binding Data => DataModel
        public VisionUIModel VisionUIDataModel { get; set; } = new VisionUIModel();

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
                    DB.DataCopyEx(this.VisionUIDataModel, DB.Vision.visionData);
                }

                // DB 불러오기
                DB.DataCopy(this.VisionUIDataModel, DB.Vision.visionData);
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
