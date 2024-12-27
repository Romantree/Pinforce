using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Dac.Cfg;

namespace WILL.WT.PINFORCE.Configs
{
    public class VisionDB : IConfigDb
    {
        public readonly VisionData visionData = new VisionData();
    }

    public class VisionData : IVisionData { }

    public abstract class IVisionData : IConfigDb
    {
        public int WindowSize { get => this.GetValueInt(); set => this.SetValue(value); }
        public int FontSize { get => this.GetValueInt(); set => this.SetValue(value); }
        public bool CaptureNotice { get => this.GetValueBool(); set => this.SetValue(value); }
        public bool AxisZ { get => this.GetValueBool(); set => this.SetValue(value); }
        public bool StepOD { get => this.GetValueBool(); set => this.SetValue(value); }
        public bool Loadcell_1 { get => this.GetValueBool(); set => this.SetValue(value); }
        public bool Loadcell_2 { get => this.GetValueBool(); set => this.SetValue(value); }
    }
}
