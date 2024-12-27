using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Wpf.Core;

namespace WILL.WT.PINFORCE.Models.Config
{
    public class MotionDataModel : DataModelBase
    {
        // public string Name { get; private set; }

        // 단위 : mm/s
        public double WorkMoveSpeed { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double AutoContactAlarmDepth { get => this.GetValue<double>(); set => SetValue(value); }

        // 생성자
        // public MotionDataModel(string name) => this.Name = name; // ViewModel에서 입력한 "NAME"
    }

    public class TimeoutDataModel : DataModelBase
    {
        // 단위 : sec
        public double MotionErrorTimeout { get => this.GetValue<double>(); set => SetValue(value); }
        public double CommErrorTimeout { get => this.GetValue<double>(); set => SetValue(value); }
    }

    public class DelayDataModel : DataModelBase
    {
        // 단위 : sec
        public double MotionDelay { get => this.GetValue<double>(); set { this.SetValue(value); } }

        public double SamplingStartDelay { get => this.GetValue<double>(); set { this.SetValue(value); } }
    }

    public class LimitDataModel : DataModelBase
    {
        // 단위 : mm/s
        public double LimitSpeed { get => this.GetValue<double>(); set { this.SetValue(value); } }
        // 단위 : g
        public double LimitForce { get => this.GetValue<double>(); set { this.SetValue(value); } }
    }

    public class LoadcellDataModel : DataModelBase
    {
        // 단위 : %
        public double Min { get => this.GetValue<double>(); set { this.SetValue(value); } }
        public double Max { get => this.GetValue<double>(); set { this.SetValue(value); } }
    }

    public class LogDataModel : DataModelBase
    {
        // 단위 : %
        public double Interval { get => this.GetValue<double>(); set { this.SetValue(value); } }
    }
}
