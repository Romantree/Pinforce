using WILL.WT.PINFORCE.Managers;
using WILL.WT.PINFORCE.Networks;
using System;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models.Network
{
    public abstract class INetSerialPortModel : IModel
    {
        protected readonly NetworkUnit _unit;

        public INetSerialPortModel(NetworkUnit unit)
        {
            _unit = unit;
            this.Port = this._Port;
        }

        protected string _Port { get => DB.Network[_unit]; set => DB.Network[_unit] = value; }

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string Port { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool IsOpen { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public virtual void Init() { } // Init 생성

        public virtual void Update() { }
    }

    public abstract class INetSerialPortModel<T> : INetSerialPortModel where T : INetSerialPort
    {
        protected T _Client => AP.Net[_unit] as T;

        public INetSerialPortModel(NetworkUnit unit) : base(unit) { }

        public NormalCommand OnConnectionCmd => new NormalCommand(ConnectionCmd);

        public override void Init() // Init 시도
        {
            try
            {
                this.IsOpen = _Client.IsOpen;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Update()
        {
            try
            {
                this.IsOpen = _Client.IsOpen;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void ConnectionCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "OPEN":
                        {
                            this._Port = this.Port;

                            _Client.Start(this.Port);
                            _Client.Init();
                        }
                        break;
                    case "CLOSE":
                        {
                            _Client.Stop();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
