using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Models.Axis;
using WILL.WT.PINFORCE.Models.Setup;

namespace WILL.WT.PINFORCE.Models
{
    public class VisionUIData : IModel
    {
        public AxisModel AxisZ { get; set; } = new AxisModel(eAxis.AXIS_Z);
        public LoadcellModel LoadcellDataModel_1 { get; set; } = new LoadcellModel(true);
        public LoadcellModel LoadcellDataModel_2 { get; set; } = new LoadcellModel(false);

        public VisionUIData() { }

        public void Update()
        {
            try
            {
                this.AxisZ.Update();
                this.LoadcellDataModel_1.Update();
                this.LoadcellDataModel_2.Update();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
