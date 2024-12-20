using System;
using TS.FW.Device.Ajin.Lib;
using TS.FW.Device.Ajin;
using TS.FW.Device;
using TS.FW.Wpf.v2.Core;
using TS.FW;

namespace WILL.WT.PINFORCE.Models.Axis
{
    public class AxisLimitModel : IModel
    {
        public bool Enable { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool StopMode { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Type { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public double Plus { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Minus { get => this.GetValue<double>(); set => this.SetValue(value); }

        public void Update(IAxis axis)
        {
            try
            {
                var item = axis as AjinAxis;
                if (item == null) return;

                this.Enable = item.Setting.SoftwareLimit.Enable;
                this.StopMode = item.Setting.SoftwareLimit.StopMode == AXT_MOTION_STOPMODE.SLOWDOWN_STOP;
                this.Type = item.Setting.SoftwareLimit.Selection == AXT_MOTION_SELECTION.ACTUAL;
                this.Plus = item.Setting.SoftwareLimit.PositivePos * item.Setting.SCALE;
                this.Minus = item.Setting.SoftwareLimit.NegativePos * item.Setting.SCALE;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void SetLimit(IAxis axis)
        {
            try
            {
                var item = axis as AjinAxis;
                if (item == null) return;

                var enable = this.Enable;
                var stopMode = this.StopMode ? AXT_MOTION_STOPMODE.SLOWDOWN_STOP : AXT_MOTION_STOPMODE.EMERGENCY_STOP;
                var type = this.Type ? AXT_MOTION_SELECTION.ACTUAL : AXT_MOTION_SELECTION.COMMAND;
                var plus = this.Plus;
                var minus = this.Minus;

                var res = item.SetSoftwareLimit(enable, stopMode, type, plus, minus);
                if (res == true)
                {
                    AP.Device.Save();
                    return;
                }

                Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);

                AP.System.InterlockMsgEvent($"{(eAxis)axis.No} Limit Setting Fail.");
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                this.Update(axis);
            }
        }
    }
}
