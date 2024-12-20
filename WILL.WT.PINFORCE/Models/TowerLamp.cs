using System;
using TS.FW.Wpf.v2.Core;
using TS.FW;

namespace WILL.WT.PINFORCE.Models
{
    public class TowerLamp : IModel
    {
        public bool Red { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Yellow { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Green { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public void Update()
        {
            try
            {
                this.Red = AP.IO.TOWER_LAMP_RED;
                this.Yellow = AP.IO.TOWER_LAMP_YELLOW;
                this.Green = AP.IO.TOWER_LAMP_GREEN;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
