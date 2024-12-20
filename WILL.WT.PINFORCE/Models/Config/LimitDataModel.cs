using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Wpf.Core;

namespace WILL.WT.PINFORCE.Models.Config
{
    public class LimitDataModel : DataModelBase
    {
        public double LimitSpeed { get => this.GetValue<double>(); set { this.SetValue(value); } }
        public double LimitForce { get => this.GetValue<double>(); set { this.SetValue(value); } }
    }
}
