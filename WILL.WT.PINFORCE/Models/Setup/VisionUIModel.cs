using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Wpf.Core;

namespace WILL.WT.PINFORCE.Models.Setup
{
    public class VisionUIModel : DataModelBase
    {
        public int WindowSize { get => this.GetValue<int>(); set => this.SetValue(value); }
        public int FontSize { get => this.GetValue<int>(); set => this.SetValue(value); }
        public bool CaptureNotice { get => this.GetValue<bool>(); set => this.SetValue(value); }
        public bool AxisZ { get => this.GetValue<bool>(); set => this.SetValue(value); }
        public bool StepOD { get => this.GetValue<bool>(); set => this.SetValue(value); }
        public bool Loadcell_1 { get => this.GetValue<bool>(); set => this.SetValue(value); }
        public bool Loadcell_2 { get => this.GetValue<bool>(); set => this.SetValue(value); }
    }
}
