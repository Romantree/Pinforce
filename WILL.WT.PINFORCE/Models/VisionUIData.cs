using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW;
using TS.FW.Wpf.Core;
using WILL.WT.PINFORCE.Models.Axis;

namespace WILL.WT.PINFORCE.Models
{
    public class VisionUIData : ModelBase
    {
        public AxisModel AxisZ { get; set; } = new AxisModel(eAxis.AXIS_Z);
        public double OD {get; set;} = new double();
        public double Weight1 { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double Weight2 { get => this.GetValue<double>(); set => this.SetValue(value); }

        public VisionUIData() { }

        public void Update()
        {
            try
            {
                this.AxisZ.Update();
                // this.OD = AP.Net.StageLeftLD.Data;
                // this.Weight1 = AP.Net.StageLeftLD.Data;
                // this.Weight2 = AP.Net.StageRightLD.Data;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
