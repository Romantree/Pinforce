using System;
using System.Windows;
using System.Windows.Media;
using WILL.WT.PINFORCE.Views.Pages.Utility;
using TS.FW;
using TS.FW.Wpf.v2.Core;
using TS.FW.Wpf.v2.Helpers;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Utility
{
    public class UtUserViewModel : IUtilViewModel
    {
        private readonly FrameworkElement view = new UtUserView();

        public override int No => 1;

        public override string Name => "User";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_user_tie", "Icons"] as Visual;

        public UserModel Operator { get; set; } = new UserModel();

        public UserModel Engineer { get; set; } = new UserModel();

        public UserModel Manager { get; set; } = new UserModel();

        public override void Show()
        {
            try
            {
                DB.DataCopy(Operator, DB.User.Operator);
                DB.DataCopy(Engineer, DB.User.Engineer);
                DB.DataCopy(Manager, DB.User.Manager);

                base.Show();
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
                    case "SAVE":
                        {
                            if (AP.System.InterlockCheckEvent("User 데이터 저장 하시겠습니까?") == false) return;

                            DB.DataCopyEx(Operator, DB.User.Operator);
                            DB.DataCopyEx(Engineer, DB.User.Engineer);
                            DB.DataCopyEx(Manager, DB.User.Manager);

                            AP.System.InterlockMsgEvent("User 데이터 저장 되었습니다.");
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

    public class UserModel : IModel
    {
        public int Password { get => this.GetValue<int>(); set => this.SetValue(value); }

        public bool Recipe { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Service { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Config { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Utilty { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Setup { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Alarm { get => this.GetValue<bool>(); set => this.SetValue(value); }
    }
}
