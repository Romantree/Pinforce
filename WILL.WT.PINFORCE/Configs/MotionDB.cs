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
    public class MotionDB : IConfigDb
    {
        public readonly Speed speed = new Speed();
    }

    // 여기에는 아래의 인터페이스를 사용한 설정 파라미터를 정의한다.
    // Jog Speed Setting Page의 Motion 속도 설정
    public class Speed : ISpeedData { }
    // Config Page의 Timeout 설정

    // 여기에는 설정 파라미터 생성을 위한 인터페이스를 정의한다.
    // Jog Speed Setting Page의 Motion 속도 설정 인터페이스
    public abstract class ISpeedData : IConfigDb
    {
        // 단위 : 0 ~ 1
        public double JogLowSpeed { get => this.GetValueDouble(); set => this.SetValue(value); }
        public double JogMidSpeed { get => this.GetValueDouble(); set => this.SetValue(value); }
        public double JogHighSpeed { get => this.GetValueDouble(); set => this.SetValue(value); }
        public double HomeSpeed { get => this.GetValueDouble(); set => this.SetValue(value); }
        public double InitSpeed { get => this.GetValueDouble(); set => this.SetValue(value); }

        // 속도 기준값
        public int RefSpeed { get => this.GetValueInt(); set => this.SetValue(value); }
    }
}
