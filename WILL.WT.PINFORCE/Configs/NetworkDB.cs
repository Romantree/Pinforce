using WILL.WT.PINFORCE.Managers;
using TS.FW.Dac.Cfg;

namespace WILL.WT.PINFORCE.Configs
{
    public class NetworkDB : IConfigDb
    {
        public string this[NetworkUnit unit] { get => this.GetValue(unit.ToString()); set => this.SetValue(value, unit.ToString()); }
    }
}
