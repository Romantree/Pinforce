using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW.Wpf.Core;

namespace WILL.WT.PINFORCE.Models.Setup
{
    public class LoadcellDataModel : DataModelBase
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
        public string PortName { get => this.GetValue<string>(); set => this.SetValue(value); }
        public int Baudrate { get => this.GetValue<int>(); set => this.SetValue(value); }
        public int Parity { get => this.GetValue<int>(); set => this.SetValue(value); }
        public int DataBits { get => this.GetValue<int>(); set => this.SetValue(value); }
        public int StopBits { get => this.GetValue<int>(); set => this.SetValue(value); }
        public int HandShake { get => this.GetValue<int>(); set => this.SetValue(value); }
    }
}
