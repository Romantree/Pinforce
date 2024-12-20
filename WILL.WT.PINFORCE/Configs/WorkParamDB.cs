using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Dac.Cfg;

namespace WILL.WT.PINFORCE.Configs
{
    // 여기에는 DB에 저장할 전체 설정값을 정의한다.
    // 전체 Parameter DB 정의
    public class WorkParamDB : IConfigDb
    {
        public readonly MoveSpeed moveSpeed = new MoveSpeed();
        public readonly ErrorTimeout errorTimeout = new ErrorTimeout();
        public readonly Delay delay = new Delay();
        public readonly Limit limit = new Limit();
        public readonly LoadcellRange loadcellRange = new LoadcellRange();
    }

    // 여기에는 아래의 인터페이스를 사용한 설정 파라미터를 정의한다.
    // Config Page의 Motion 속도 설정
    public class MoveSpeed : IMotionData { }
    // Config Page의 Timeout 설정
    public class ErrorTimeout : ITimeoutData { }
    // Config Page의 Delay 설정
    public class Delay : IDelayData { }

    public class Limit : ILimitData { }
    public class LoadcellRange : ILoadcellRangeData { }

    // 여기에는 설정 파라미터 생성을 위한 인터페이스를 정의한다.
    // Config Page의 Motion 속도 설정 인터페이스
    public abstract class IMotionData : IConfigDb
    {
        // 단위 : mm/s
        public int ReadyPosMoveSpeed { get => this.GetValueInt(); set => this.SetValue(value); }
    }

    public abstract class ITimeoutData : IConfigDb
    {
        // 단위 : sec
        public double MotionErrorTimeout { get => this.GetValueDouble(); set => SetValue(value); }
        public double CommErrorTimeout { get => this.GetValueDouble(); set => SetValue(value); }
    }

    public abstract class IDelayData : IConfigDb
    {
        // 단위 : sec
        public double MotionDelay { get => this.GetValueDouble(); set { this.SetValue(value); } }
        public double SamplingStartDelay { get => this.GetValueDouble(); set { this.SetValue(value); } }
    }

    public abstract class ILimitData : IConfigDb
    {
        public double LimitSpeed { get => this.GetValueDouble(); set => SetValue(value); }

        public double LimitForce { get => this.GetValueDouble(); set => SetValue(value); }
    }

    public abstract class ILoadcellRangeData : IConfigDb
    {
        public double Min { get => this.GetValueDouble(); set => SetValue(value); }
        public double Max { get => this.GetValueDouble(); set => SetValue(value); }
    }
}
