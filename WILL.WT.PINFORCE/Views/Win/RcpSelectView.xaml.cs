using WILL.WT.PINFORCE.Managers;
using WILL.WT.PINFORCE.Models;
using System.Collections.ObjectModel;
using System;
using System.Windows;
using System.Windows.Input;
using TS.FW.Wpf.v2.Core;
using TS.FW;

namespace WILL.WT.PINFORCE.Views.Win
{
    /// <summary>
    /// RcpSelectView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RcpSelectView : Window
    {
        public RcpSelectView()
        {
            InitializeComponent();
        }

        private void TextBlock3D_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released) return;

            this.DragMove();
        }
    }

    public class RcpSelectViewModel : IViewModel
    {
        private readonly RcpSelectView _view = new RcpSelectView();

        public RecipeType Title { get => this.GetValue<RecipeType>(); set => this.SetValue(value); }

        public ObservableCollection<IRecipeModel> RcpList { get; set; } = new ObservableCollection<IRecipeModel>();

        public IRecipeModel RcpSelected { get => this.GetValue<IRecipeModel>(); set => this.SetValue(value); }

        public NormalCommand OnRcpSelectedCmd => new NormalCommand(RcpSelectedCmd);

        public RcpSelectViewModel(RecipeType title)
        {
            this.Title = title;

            this._view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this._view.DataContext = this;
        }

        public bool? Show()
        {
            this.LoadRecipe(this.Title);

            return this._view.ShowDialog();
        }

        private void LoadRecipe(RecipeType unit)
        {
            try
            {
                this.RcpList.Clear();

                foreach (var item in AP.Rcp.ToRecipeList(unit))
                {
                    this.RcpList.Add(item);
                }

                this.RcpSelected = null;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void RcpSelectedCmd(object param)
        {
            try
            {
                var item = param as IRecipeModel;
                if (item == null) return;

                this.RcpSelected = item;
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
                    case "OK":
                        {
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
