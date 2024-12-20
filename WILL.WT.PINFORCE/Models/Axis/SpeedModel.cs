using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Wpf.Core;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models.Setup
{
    public class SpeedModel : DataModelBase
    {
        // public string Name { get; private set; }

        // 단위 : 0 ~ 1
        public double JogLowSpeed { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double JogMidSpeed { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double JogHighSpeed { get => this.GetValue<double>(); set => this.SetValue(value); }

        public double HomeSpeed { get => this.GetValue<double>(); set => this.SetValue(value); }
        public double InitSpeed { get => this.GetValue<double>(); set => this.SetValue(value); }

        // 속도 기준값
        public int RefSpeed { get => this.GetValue<int>(); set => this.SetValue(value); }

        // 생성자
        // public MotionDataModel(string name) => this.Name = name; // ViewModel에서 입력한 "NAME"
    }
}
