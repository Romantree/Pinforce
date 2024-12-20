using WILL.WT.PINFORCE.Managers;

namespace WILL.WT.PINFORCE.Process
{
    public abstract partial class IProcessTimer
    {
        protected SystemManager Sys => AP.System;

        protected DeviceManager Mot => AP.Device;

        protected InOutManager IO => AP.IO;

        protected NetManager Net => AP.Net;

        protected AlarmManager Alarm => AP.Alarm;

        protected ProcManager Proc => AP.Proc;
    }
}
