using System;
using System.IO.Ports;
using System.Text;
using TS.FW;

namespace WILL.WT.PINFORCE.Networks
{
    public abstract class INetSerialPort
    {
        private readonly object _locker = new object();

        protected SerialPort _client;

        protected string portName;
        protected int baudRate = 9600;
        protected int dataBit = 8;
        protected Parity parity = Parity.None;
        protected StopBits stopBits = StopBits.None;
        protected int readTimeOut = 1000;
        protected int readBuffer = 1024;
        protected Encoding encoding = Encoding.ASCII;
        protected bool IsRecvEvent = false;

        public bool IsOpen => this._client != null ? this._client.IsOpen : false;

        public void Start(string portName)
        {
            this.portName = portName;
            this.Start();
        }

        protected void Start()
        {
            if (this.IsOpen == true) this.Stop();

            this._client = new SerialPort()
            {
                PortName = this.portName,
                BaudRate = this.baudRate,
                Parity = this.parity,
                StopBits = this.stopBits,
                ReadTimeout = this.readTimeOut,
                ReadBufferSize = this.readBuffer,
                Encoding = this.encoding,
            };

            this._client.ErrorReceived += _client_ErrorReceived;
            this._client.PinChanged += _client_PinChanged;

            if (this.IsRecvEvent) this._client.DataReceived += _client_DataReceived;

            this._client.Open();
        }

        public void Stop()
        {
            if (this.IsOpen == false) return;

            this._client.Close();
            this._client.Dispose();

            this._client = null;
        }

        public virtual void Init() { }

        protected void Send(string cmd)
        {
            var buffer = this.encoding.GetBytes(cmd);

            this.Send(buffer);
        }

        protected void Send(byte[] buffer)
        {
            lock (this._locker)
            {
                this._client.Write(buffer, 0, buffer.Length);
            }
        }

        private void _client_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            try
            {
                Logger.Write(this, $"PinChanged : {this.portName} {e.EventType}", Logger.LogEventLevel.Error);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void _client_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            try
            {
                Logger.Write(this, $"ErrorRecv : {this.portName} {e.EventType}", Logger.LogEventLevel.Error);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void _client_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (this._locker)
                {
                    this.DataReceived(e);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected virtual void DataReceived(SerialDataReceivedEventArgs e) { }
    }
}
