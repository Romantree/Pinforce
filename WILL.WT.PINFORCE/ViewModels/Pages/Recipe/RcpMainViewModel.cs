using WILL.WT.PINFORCE.Managers;
using WILL.WT.PINFORCE.Views.Pages.Recipe;
using System;
using System.Windows;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Models.Recipe;
using System.Collections.Generic;
using WILL.WT.PINFORCE.Models.Axis;
using System.Linq;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Recipe
{
    public class RcpMainViewModel : IRcpViewModel
    {
        private readonly FrameworkElement view = new RcpMainView();

        public override int No => 0;

        public override string Name => "Main";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_clipboard", "Icons"] as Visual;

        public UcRecipeModel<MainRecipeModel> UC { get; set; } = new UcRecipeModel<MainRecipeModel>(RecipeType.MAIN);

        // Jog Motion 제어용
        public MotionModel Stage { get; set; } = new MotionModel();

        public override void Init()
        {
            try
            {
                base.Init();

                Stage.Axis.Add(new AxisModel(eAxis.AXIS_Z));

                Stage.SelectAxisCmd(Stage.Axis.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Show()
        {
            try
            {
                base.Show();
                UC.LoadRecipe();
                Stage.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
