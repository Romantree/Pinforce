using System;
using WILL.WT.PINFORCE.Managers;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models.InOut
{
    public class InOutModel : IModel
    {
        public readonly IOData data;
        public readonly string Key;

        public bool IsAType { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string Address { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool OnOff { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public InOutModel(string key, IOData data) : this(data)
        {
            this.Key = key;
        }

        public InOutModel(IOData data)
        {
            this.data = data;
            this.IsAType = data.IsAType;
            this.Address = data.Address;
            this.Name = data.Name;
            this.OnOff = data.OnOff;
        }

        public void Update()
        {
            try
            {
                this.IsAType = this.data.IsAType;
                this.OnOff = this.data.OnOff;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public string ToCSV() => $"{this.Address},{this.Name}";

        public static implicit operator InOutModel(IOData data) => new InOutModel(data);
    }
}
