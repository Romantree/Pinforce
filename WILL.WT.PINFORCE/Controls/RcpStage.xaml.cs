using WILL.WT.PINFORCE.Models.Recipe;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TS.FW;
using TS.FW.Wpf.v2;
using TS.FW.Wpf.v2.Controls;

namespace WILL.WT.PINFORCE.Controls
{
    /// <summary>
    /// RcpStage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RcpStage : UserControl
    {
        private static ControlProperty<RcpStage> Property = new ControlProperty<RcpStage>();

        public static readonly DependencyProperty CommandProperty = Property.ToCommand();

        public static readonly DependencyProperty IsMotionProperty = Property.ToProperty(false);
        public static readonly DependencyProperty RecipeProperty = Property.ToProperty<MainRecipeModel>();

        public ICommand Command { get => (ICommand)this.GetValue(CommandProperty); set => this.SetValue(CommandProperty, value); }

        public bool IsMotion { get => (bool)this.GetValue(IsMotionProperty); set => this.SetValue(IsMotionProperty, value); }

        public MainRecipeModel Recipe { get => (MainRecipeModel)this.GetValue(RecipeProperty); set => this.SetValue(RecipeProperty, value); }

        public RcpStage()
        {
            InitializeComponent();            
        }

        private void EventBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Command == null) return;

                var cnt = sender as EventBtn;
                if (cnt == null) return;

                this.Command.Execute(cnt.Tag);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
