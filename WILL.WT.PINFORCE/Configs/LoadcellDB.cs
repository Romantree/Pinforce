using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Dac.Cfg;

namespace WILL.WT.PINFORCE.Configs
{
    public class LoadcellDB : IConfigDb
    {
        public readonly LoadCellParam_1 Loadcell_1 = new LoadCellParam_1();
        public readonly LoadCellParam_2 Loadcell_2 = new LoadCellParam_2();
    }

    // Loadcell 객체 생성
    public class LoadCellParam_1 : ILoadcellData { }
    public class LoadCellParam_2 : ILoadcellData { }

    // Loadcell Parameter 설정 인터페이스
    public abstract class ILoadcellData : IConfigDb
    {
        /*
        [COM Port 속성]
        COM = String:PortName
        Baud Rate = int:Number
        Parity = enum:None, Odd, Even, Mark, Space
        DataBits = int:Number
        StopBits = enum:None, One, Two, OnePointFive
        Hand Shake = enum:None, XOnXOff, RequestToSend, RequestToSendXOnXOff
         */
        public string PortName { get => this.GetValue(); set => this.SetValue(value); }
        public int Baudrate { get => this.GetValueInt(); set => this.SetValue(value); }
        public int Parity { get => this.GetValueInt(); set => this.SetValue(value); }
        public int DataBits { get => this.GetValueInt(); set => this.SetValue(value); }
        public int StopBits { get => this.GetValueInt(); set => this.SetValue(value); }
        public int HandShake { get => this.GetValueInt(); set => this.SetValue(value); }
    }
}
