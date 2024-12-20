using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW;

namespace WILL.WT.PINFORCE.Networks
{
    public  class NetLoadcell : INetSerialPort
    {
        public NetLoadcell()
        {
            this.baudRate = 9600;
            this.dataBit = 8;
            this.stopBits = StopBits.One;
            this.parity = Parity.None;
            this.readBuffer = 50;
            this.IsRecvEvent = true;
        }

        protected override void DataReceived(SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof) return;
            if (this._client.BytesToRead <= 0) return;

            try
            {
                var buffer = new byte[this._client.BytesToRead];
                var len = this._client.Read(buffer, 0, buffer.Length);

                if (buffer.Length != len) return;

                
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                this._client.DiscardInBuffer();
            }
        }
    }
}
