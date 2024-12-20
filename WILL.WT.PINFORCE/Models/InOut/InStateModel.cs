using WILL.WT.PINFORCE.Managers;
using System;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models.InOut
{
    public class InStateModel : IModel
    {
        private IOData _data = null;

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string OnOff { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool LoadData(string name)
        {
            try
            {
                if (AP.IO.inList.ContainsKey(name) == false) return false;

                this._data = AP.IO.inList[name];

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
    }
}
