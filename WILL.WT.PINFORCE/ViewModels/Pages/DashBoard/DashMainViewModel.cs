using System.Windows;
using System.Windows.Media;
using WILL.WT.PINFORCE.Views.Pages.DashBoard;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Models.Recipe;
using System;
using TS.FW;
using WILL.WT.PINFORCE.Views.Win;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Windows.Media.Imaging;
using WILL.WT.PINFORCE.Managers;
using System.Linq;
using System.Drawing.Drawing2D;

namespace WILL.WT.PINFORCE.ViewModels.Pages.DashBoard
{
    public class DashMainViewModel : IDashViewModel
    {
        private readonly FrameworkElement view = new DashMainView();

        public override int No => 0;

        public override string Name => "Main";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_clipboard", "Icons"] as Visual;

        // 현재 Binding 되어있는 Recipe Data; ex)  Text="{Binding Start, StringFormat={}{0:f0} um}"
        public MainRecipeModel Rcp { get => this.GetValue<MainRecipeModel>(); set => this.SetValue(value); }

        public override void Show()
        {
            try
            {
                base.Show();

                if (this.Rcp != null) // Load된 Recipe가 있으면
                {
                    this.Rcp = AP.Rcp.Reload<MainRecipeModel>(this.Rcp);
                }
                else // Load된 Recipe가 없으면
                {
                    this.Rcp = AP.Rcp.GetRecipe(RecipeType.MAIN, 0) as MainRecipeModel;
                }
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
                    case "Start":
                        {
                            //if(this.UC.RcpSelected ==null)
                            if (this.Rcp == null)
                            {
                                AP.System.InterlockMsgEvent("선택 된 레시피가 없습니다.");
                                return;
                            }
                           
                            AP.Proc.Start(this.Rcp);
                        }
                        break;
                    case "Stop":
                        {
                            AP.ProcessStop();
                        }
                        break;
                    case "PinStop":
                        {

                        }
                        break;
                    case "Initial":
                        {
                            AP.System.InterlockCheckEvent("시스템 초기화를 진행하시겠습니까?");
                            AP.Proc.Init.Start();
                            
                        
                        }
                        break;
                    case "Rcp":
                        {
                            var view = new RcpSelectViewModel(Managers.RecipeType.MAIN);
                            if (view.Show() == false) return;

                            this.Rcp = view.RcpSelected as MainRecipeModel;
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