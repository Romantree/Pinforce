using System;
using TS.FW.Device.Ajin;
using TS.FW.Device.Simulation;
using TS.FW.Device;
using TS.FW;
using TS.FW.Diagnostics;
using TS.FW.Device.Ajin.Lib;

namespace WILL.WT.PINFORCE.Managers
{
    public partial class DeviceManager
    {
        public IAxis this[eAxis axis] => this.Axis[axis];

        public IDevice Ins { get; private set; }

        public IAxisModule Axis => this.Ins as IAxisModule;

        public IDInOut IO => (this.Ins as IDInOutModule).IO;

        public IAnalog Analog => (this.Ins as IAnalogModule).Al;

        public DeviceManager()
        {
            if (AP.IsSim)
            {
                this.Ins = new SimulationDevice();
            }
            else
            {
                this.Ins = new AjinDevice();
            }
        }

        public Response Open()
        {
            try
            {
                var res = this.Ins.Open();

                if (res == true && this.Ins is AjinDevice)
                {
                    var load = (this.Ins as AjinDevice).LoadMotionFile(AP.MOT_FILE);
                    if (load == false) return load;

                    
                }

                return res;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public void Save()
        {
            try
            {
                if (this.Ins is AjinDevice)
                {
                    (this.Ins as AjinDevice).SaveMotionFile(AP.MOT_FILE);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }


    }
}
