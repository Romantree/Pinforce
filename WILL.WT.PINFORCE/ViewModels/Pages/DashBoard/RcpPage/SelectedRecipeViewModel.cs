using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TS.FW;
using TS.FW.Wpf.Helper;

using WILL.WT.PINFOCUS.Configs;
using WILL.WT.PINFOCUS.Views.Pages.DashBoard.RcpPage;

namespace WILL.WT.PINFOCUS.ViewModels.Pages.DashBoard.RcpPage
{
    public class SelectedRecipeViewModel : SelectedRecipeModel
    {
        public SelectedRecipeViewModel(string name) : base(name) { }

        public SelectedRecipeModel StartOD { get; set; } = new SelectedRecipeModel("Start O/D");

        public SelectedRecipeModel um { get; set; } = new SelectedRecipeModel("㎛");
        public SelectedRecipeModel sec { get; set; } = new SelectedRecipeModel("sec");
    }
}
