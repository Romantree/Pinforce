using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Dac.Cfg;

namespace WILL.WT.PINFORCE.Configs
{
    public class AutoContactParamDB : IConfigDb
    {

    }

    // Auto Contact Parameter 인터페이스
    public abstract class IAutoContactParam : IConfigDb
    {
        /*
            OriginForce = 설정값 이상 감지되면 Contact 지점으로 설정할 Loadcell 값
            ContactStepDist = ???
            
        */
        public int RepeatCount { get => this.GetValueInt(); set => this.SetValue(value); }

        // 1st ##################################################
        public double Target_OriginForce_1 { get => this.GetValueDouble(); set => this.SetValue(value); }
        public int Target_ContactStepDist_1 { get => this.GetValueInt(); set => this.SetValue(value); }
        public int MotionDelay_1 { get => this.GetValueInt(); set => SetValue(value); }

        // 2nd ##################################################
        public double Target_OriginForce_2 { get => this.GetValueDouble(); set => this.SetValue(value); }
        public int Target_ContactStepDist_2 { get => this.GetValueInt(); set => this.SetValue(value); }
        public int MotionDelay_2 { get => this.GetValueInt(); set => SetValue(value); }

        // 3rd ##################################################
        public double Target_OriginForce_3 { get => this.GetValueDouble(); set => this.SetValue(value); }
        public int Target_ContactStepDist_3 { get => this.GetValueInt(); set => this.SetValue(value); }
        public int MotionDelay_3 { get => this.GetValueInt(); set => SetValue(value); }

        public bool UseLastReleaseMove { get => this.GetValueBool(); set => this.SetValue(value); }
        public int LastReleaseMoveDist { get => this.GetValueInt(); set => SetValue(value); }
        public int AutoAdjustmentDist { get => this.GetValueInt(); set => SetValue(value); }
        public int TimeoutRef { get => this.GetValueInt(); set => SetValue(value); }
    }
}
