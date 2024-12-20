using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models
{
    public abstract class IConfigModel<T> : IModel
    {
        public T Unit { get; private set; }

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public IConfigModel(T unit)
        {
            this.Unit = unit;
            this.Name = $"{unit}".Replace("_", " ");
        }

        public override string ToString() => this.Name;
    }
}