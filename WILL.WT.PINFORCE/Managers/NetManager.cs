using System;
using System.Linq;
using WILL.WT.PINFORCE.Networks;
using TS.FW;

namespace WILL.WT.PINFORCE.Managers
{
    public class NetManager
    {
        public readonly NetLoadcell Loadcell01 = new NetLoadcell();
        public readonly NetLoadcell Loadcell02 = new NetLoadcell();

        public INetSerialPort this[NetworkUnit unit]
        {
            get
            {
                switch (unit)
                {
                    case NetworkUnit.Loadcell01: return Loadcell01;
                    case NetworkUnit.Loadcell02: return Loadcell02;
                }

                return null;
            }
        }

        public void Start()
        {
            if (AP.IsSim) return;

            var flag = false;

            foreach (NetworkUnit unit in Enum.GetValues(typeof(NetworkUnit)))
            {
                if (this.Start(unit) == false) flag = true;
            }

            if (flag) AP.System.InterlockMsgEvent("Network 통신 연결에 실패하였습니다.");
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

        public Response Start(NetworkUnit unit)
        {
            try
            {
                var net = this[unit];
                var port = DB.Network[unit];

                if (string.IsNullOrWhiteSpace(port)) throw new Exception($"Port가 null 입니다. [{unit}]");
                if (EnumHelper.SerialPort.Any(t => t == port) == false) throw new Exception($"Port가 없습니다. [{unit}]");

                net.Start(port);
                net.Init();

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }
    }

    public enum NetworkUnit
    {
        Loadcell01,
        Loadcell02,
    }
}
