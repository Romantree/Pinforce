using WILL.WT.PINFORCE.Managers;
using System;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models.InOut
{
    public class OutControlModel : IModel
    {
        private string  _key = string.Empty;
        private IOData _data = null;

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string OnOff { get => this.GetValue<string>(); set => this.SetValue(value); }

        public NormalCommand OnBitChangeCmd => new NormalCommand(BitChangeCmd);

        public bool LoadData(string key)
        {
            try
            {
                if (AP.IO.outList.ContainsKey(key) == false) return false;

                this._key = key;
                this._data = AP.IO.outList[key];

                this.Name = _data.Name;
                this.OnOff = _data.OnOff ? "ON" : "OFF";

                return true;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return false;
            }
        }

        public void Update()
        {
            try
            {
                if (_data == null) return;

                this.OnOff = _data.OnOff ? "ON" : "OFF";
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void BitChangeCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "ON":
                        {
                            AP.IO.WriteY(true, _key);
                        }
                        break;
                    case "OFF":
                        {
                            AP.IO.WriteY(false, _key);
                        }
                        break;
                    case "ONOFF":
                        {
                            AP.IO.WriteY(!_data.OnOff, _key);
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
