using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW;
using TS.FW.Wpf.Core;
using WILL.WT.PINFORCE.Managers;
using WILL.WT.PINFORCE.Models.Network;
using WILL.WT.PINFORCE.Networks;

namespace WILL.WT.PINFORCE.Models.Setup
{
    public class LoadcellModel : INetSerialPortModel<NetLoadcell>
    {
        public LoadcellModel(bool isFirst) : base(isFirst ? NetworkUnit.Loadcell01 : NetworkUnit.Loadcell02)
        {
            this.Name = isFirst ? "LOADCELL #1" : "LOADCELL #2";
        }

        public double Data { get => this.GetValue<double>(); set => this.SetValue(value); }

        public override void Update()
        {
            try
            {
                base.Update();

                this.Data = _Client.Data;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
