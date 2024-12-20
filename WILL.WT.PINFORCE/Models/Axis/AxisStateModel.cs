using System;
using TS.FW.Device;
using TS.FW.Wpf.v2.Core;
using TS.FW;

namespace WILL.WT.PINFORCE.Models.Axis
{
    public class AxisStateModel : IModel
    {
        public bool IsServoOn { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsAlarm { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsBusy { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsHome { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsPlus { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsMinus { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public double ActPos { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double CmdPos { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double Speed { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double AbsPos { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double RelPos { get => this.GetValue<double>(); set => this.SetValue(value); }

        public AxisStateModel()
        {
            this.Speed = 1;
            this.AbsPos = 0;
            this.RelPos = 1;
        }

        public void Update(IAxis axis)
        {
            try
            {
                if (axis == null)
                {
                    this.IsServoOn = false;
                    this.IsAlarm = true;
                    this.IsBusy = false;
                    this.IsHome = false;
                    this.IsPlus = false;
                    this.IsMinus = false;
                    this.ActPos = 0;
                    this.CmdPos = 0;
                }
                else
                {
                    this.IsServoOn = axis.IsServoOn;
                    this.IsAlarm = axis.IsAlarm;
                    this.IsBusy = axis.IsBusy;
                    this.IsHome = axis.HomeSensor;
                    this.IsPlus = axis.LimitPlus;
                    this.IsMinus = axis.LimitMinus;
                    this.ActPos = axis.ActPosition;
                    this.CmdPos = axis.ComPosition;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
