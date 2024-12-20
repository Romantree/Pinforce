using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using TS.FW;
using TS.FW.Device;
using TS.FW.Device.Dto.DInOut;
using TS.FW.Diagnostics;
using TS.FW.Serialization;

namespace WILL.WT.PINFORCE.Managers
{
    public partial class InOutManager
    {
        

        private const int BIT_COUNT = 32;
        private const int TIMER_MSC = 10;

        private const eIOType IN = eIOType.IN;
        private const eIOType OUT = eIOType.OUT;

        private readonly BackgroundTimer _trUpdate = new BackgroundTimer(ApartmentState.MTA);

        private IDInOut IO => AP.Device.IO;

        public Dictionary<string, IOData> inList = new Dictionary<string, IOData>();
        public Dictionary<string, IOData> outList = new Dictionary<string, IOData>();

        public InOutManager()
        {
            this._trUpdate.SleepTimeMsc = TIMER_MSC;
            this._trUpdate.DoWork += _trUpdate_DoWork;
        }

        public void Abort()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void BitChanged(KeyValuePair<string, IOData> data)
        {
            try
            {
                if (data.Value.OnOff == true)
                {
                    this.BitChangedOn(data.Key);
                }
                else
                {
                    this.BitChangedOff(data.Key);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void LoadDataBase()
        {
            this.InitProperty();
            this.InitFile();
            this.CreateFile();
        }

        public void Start()
        {
            this.UpdateOutList();
            this._trUpdate.Start();
        }

        private void _trUpdate_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                this.UpdateInList();
                this.UpdateOutList();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void InitProperty()
        {
            var list = this.ToList();

            foreach (var item in list.Where(t => t.Value.Type == eIOType.IN))
            {
                this.inList.Add(item.Key, item.Value);
            }

            foreach (var item in list.Where(t => t.Value.Type == eIOType.OUT))
            {
                this.outList.Add(item.Key, item.Value);
            }
        }

        private void InitFile()
        {
            if (File.Exists(AP.INOUT_FILE))
            {
                var res = Serialization.JsonDeserializerFile<List<IoSettingData>>(AP.INOUT_FILE);
                if (res == false) return;

                foreach (var item in res.Result.Where(t => t.Type == eIOType.IN))
                {
                    if (this.inList.ContainsKey(item.Key) == false) continue;

                    this.inList[item.Key].SignalType = item.SignalType;
                }

                foreach (var item in res.Result.Where(t => t.Type == eIOType.OUT))
                {
                    if (this.outList.ContainsKey(item.Key) == false) continue;

                    this.outList[item.Key].SignalType = item.SignalType;
                }
            }
        }

        public void CreateFile()
        {
            var list = new List<IoSettingData>();

            foreach (var item in this.inList)
            {
                list.Add(new IoSettingData()
                {
                    Key = item.Key,
                    Address = item.Value.Address,
                    Name = item.Value.Name,
                    Type = item.Value.Type,
                    SignalType = item.Value.SignalType,
                });
            }

            foreach (var item in this.outList)
            {
                list.Add(new IoSettingData()
                {
                    Key = item.Key,
                    Address = item.Value.Address,
                    Name = item.Value.Name,
                    Type = item.Value.Type,
                    SignalType = item.Value.SignalType,
                });
            }

            var res = Serialization.JsonSerializerFile(list, AP.INOUT_FILE);
            if (res == false) throw new Exception(res.Comment);
        }

        private void UpdateOutList()
        {
            if (this.IO == null) return;

            foreach (var module in this.outList.GroupBy(t => t.Value.ModuleNo))
            {
                var data = this.ReadOut32Bit(module.Key);
                if (data == null) continue;

                this.UpdateList(data, module);
            }
        }

        private void UpdateInList()
        {
            if (this.IO == null) return;

            foreach (var module in this.inList.GroupBy(t => t.Value.ModuleNo))
            {
                var data = this.ReadIn32Bit(module.Key);
                if (data == null) continue;

                this.UpdateList(data, module);
            }
        }

        private void UpdateList(DInOutDWord data, IGrouping<int, KeyValuePair<string, IOData>> list)
        {
            foreach (var item in list)
            {
                var bit = data[item.Value.ModuleNo, item.Value.BitNo];
                if (bit == null) continue;

                var onoff = bit.Signal == eSignal.ON;
                if (onoff == item.Value.OnOff_Org) continue;

                item.Value.OnOff_Org = onoff;

                if (item.Value.Type == eIOType.IN)
                {
                    this.InOutHistory(item.Value, item.Value.OnOff);
                    this.BitChanged(item);
                }
            }
        }

        private DInOutDWord ReadIn32Bit(int moduleNo)
        {
            var res = this.IO.ReadDWordIn(moduleNo);
            if (res == false)
            {
                Logger.Write(this, res.Comment, Logger.LogEventLevel.Warning);
                return null;
            }

            return res.Result;
        }

        private DInOutDWord ReadOut32Bit(int moduleNo)
        {
            var res = this.IO.ReadDWordOut(moduleNo);
            if (res == false)
            {
                Logger.Write(this, res.Comment, Logger.LogEventLevel.Warning);
                return null;
            }

            return res.Result;
        }

        public bool ReadX([CallerMemberName] string key = null)
        {
            var item = this.GetInData(key);
            if (item == null || item.BitNo == -1) throw new Exception(string.Format("[IN] = '{0}'", key));

            return item.OnOff;
        }

        public bool ReadY([CallerMemberName] string key = null)
        {
            var item = this.GetOutData(key);
            if (item == null || item.BitNo == -1) throw new Exception(string.Format("[OUT] = '{0}'", key));

            return item.OnOff;
        }

        public void WriteX(bool onOff, [CallerMemberName] string key = null)
        {
            var item = this.GetInData(key);
            if (item == null || item.BitNo == -1) throw new Exception(string.Format("[IN] = '{0}'", key));

            if (item.OnOff == onOff) return;

            var value = item.IsAType ? onOff : !onOff;
            this.Write(item.ModuleNo, item.BitNo, value);
        }

        public void WriteY(bool onOff, [CallerMemberName] string key = null)
        {
            var item = this.GetOutData(key);
            if (item == null || item.BitNo == -1) throw new Exception(string.Format("[OUT] = '{0}'", key));

            if (item.OnOff == onOff) return;

            this.InOutHistory(item, onOff);

            var value = item.IsAType ? onOff : !onOff;
            this.Write(item.ModuleNo, item.BitNo, value);
        }

        public void Write(int moduleNo, int bitNo, bool onOff)
        {
            this.IO.WriteBit(moduleNo, bitNo, onOff ? eSignal.ON : eSignal.OFF);
        }

        public IOData GetIoData(string key)
        {
            if (this.inList.ContainsKey(key) == true) return this.inList[key];
            else if (this.outList.ContainsKey(key) == true) return this.outList[key];
            else return null;
        }

        private IOData GetInData(string key)
        {
            if (this.inList.ContainsKey(key) == false) return null;

            return this.inList[key];
        }

        private IOData GetOutData(string key)
        {
            if (this.outList.ContainsKey(key) == false) return null;

            return this.outList[key];
        }

        public List<IOData> CreateIOList(eIOType type, int start, int mCount)
        {
            var list = new List<IOData>();

            for (int module = 0; module < mCount; module++)
            {
                for (int bit = 0; bit < BIT_COUNT; bit++)
                {
                    var item = new IOData()
                    {
                        ModuleNo = start + module,
                        BitNo = bit,
                        Type = type,
                        SignalType = eSignalType.A,
                        Name = string.Empty,
                        Address = string.Format("{0}{1}"
                        , type == eIOType.IN ? "X" : "Y"
                        , (bit + (module * BIT_COUNT)).ToString("X").PadLeft(2, '0')),
                    };

                    list.Add(item);
                }
            }
            return list;
        }

        private Dictionary<string, IOData> ToList()
        {
            var list = new Dictionary<string, IOData>();

            foreach (var info in this.GetType().GetProperties())
            {
                var at = info.GetCustomAttribute<IOSettingAttribute>();
                if (at == null) continue;

                var no = (int)(at.BitNo + (at.ModuleNo * BIT_COUNT) - (at.Type == eIOType.IN ? 0 : OA));
                var add = string.Format("{0}{1}"
                    , at.Type == eIOType.IN ? "X" : "Y"
                    , no.ToString("X").PadLeft(2, '0'));

                var data = new IOData()
                {
                    ModuleNo = at.ModuleNo,
                    BitNo = at.BitNo,
                    Name = at.Name,
                    Type = at.Type,
                    SignalType = at.SignalType,
                    Address = add,
                };

                list.Add(info.Name, data);
            }

            return list;
        }

        private void InOutHistory(IOData item, bool onOff)
        {
            //Logger.CustomWrite("InOutHistory", this, $"{item} - {(onOff ? "ON" : "OFF")}");
        }
    }

    public class IOData
    {
        public string Address { get; set; }

        public int ModuleNo { get; set; }

        public int BitNo { get; set; }

        public string Name { get; set; }

        public eIOType Type { get; set; }

        public eSignalType SignalType { get; set; }

        public bool OnOff_Org { get; set; }

        public bool OnOff => this.IsAType ? this.OnOff_Org : !this.OnOff_Org;

        public bool IsAType => this.SignalType == eSignalType.A;

        public override string ToString() => $"{this.Address} : {this.Name}";

        public override int GetHashCode() => $"{this.ModuleNo} {this.BitNo}".GetHashCode();

        public override bool Equals(object obj)
        {
            if (object.Equals(obj, null)) return false;

            return this.GetHashCode() == obj.GetHashCode();
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class IOSettingAttribute : Attribute
    {
        public int ModuleNo { get; private set; }

        public int BitNo { get; private set; }

        public string Name { get; private set; }

        public eIOType Type { get; private set; }

        public eSignalType SignalType { get; private set; }

        public IOSettingAttribute(eIOType type, int bNo, string name, eSignalType signalType = eSignalType.A)
        {
            this.Type = type;
            this.ModuleNo = (int)Math.Truncate(bNo / 32D);
            this.BitNo = bNo % 32;
            this.Name = name;
            this.SignalType = signalType;
        }
    }

    public class IoSettingData
    {
        public string Key { get; set; }

        public eIOType Type { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public eSignalType SignalType { get; set; }

        public override string ToString() => $"{this.Address} : {this.Name}";
    }
}
