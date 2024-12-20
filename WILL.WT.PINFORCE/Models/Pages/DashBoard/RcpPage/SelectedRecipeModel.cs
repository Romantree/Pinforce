using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS.FW.Wpf.Core;

namespace WILL.WT.PINFOCUS.Configs
{
    public class SelectedRecipeModel : DataModelBase
    {
        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public SelectedRecipeModel(string name) => this.Name = name;
    }
}
