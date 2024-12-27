using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW;

namespace WILL.WT.PINFORCE.Networks
{
    public class NetLoadcell : INetSerialPort
    {
        public NetLoadcell()
        {
            // SerialPort 속성
            this.baudRate = 9600;
            this.dataBit = 8;
            this.stopBits = StopBits.One;
            this.parity = Parity.None;
            this.readBuffer = 50;
            this.IsRecvEvent = true;
        }

        public double Data { get; set; }

        protected override void DataReceived(SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Eof) return;
            if (this._client.BytesToRead <= 0) return;

            try
            {
                var buffer = new byte[this._client.BytesToRead];
                var len = this._client.Read(buffer, 0, buffer.Length);

                if (buffer.Length != len) return;

                // byte[]
                // this.Data = buffer;
                // string
                // this.Data = Convert.ToDouble(Encoding.UTF8.GetString(buffer));
                this.Data = ConvertValue(buffer);
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

        // Buffer를 Double로 변환
        // Loadcell Zero -> DOUT 이용?
        private double ConvertValue(byte[] buffer)
        {
            double value = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                if ((buffer[i] == 'S' && buffer[i + 1] == 'T') || (buffer[i] == 'U' && buffer[i + 1] == 'S'))
                {
                    if (i + 17 < buffer.Length)
                    {
                        if (buffer[i + 16] == 0x0D && buffer[i + 17] == 0x0A)
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < 8; j++)
                            {
                                char c = Convert.ToChar(buffer[i + 6 + j]);
                                sb.Append(c.ToString());
                            }

                            string strVal = sb.ToString();
                            value = Convert.ToDouble(strVal);
                        }
                    }
                }
            }
            return value;
        }
    }
}
