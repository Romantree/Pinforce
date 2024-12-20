using System;
using WILL.WT.PINFORCE.Configs;
using TS.FW.Wpf.v2.Controls.InPut;

namespace WILL.WT.PINFORCE.Managers
{
    public class SystemManager
    {
        public event EventHandler<LoginMode> OnLoginModeChangedEvent;
        public void LoginModeChangedEvent(LoginMode mode) => OnLoginModeChangedEvent?.Invoke(this, mode);

        public event EventHandler<string> OnInterlockMsgEvent;
        public void InterlockMsgEvent(string format, params object[] arg) => OnInterlockMsgEvent?.Invoke(this, string.Format(format, arg));

        public event Func<string, bool?> OnInterlockCheckEvent;
        public bool? InterlockCheckEvent(string format, params object[] arg) => OnInterlockCheckEvent?.Invoke(string.Format(format, arg));

        public event EventHandler<string> OnProcessMsgEvent;
        public void ProcessMsgEvent(object sender, string format, params object[] arg) => OnProcessMsgEvent?.Invoke(sender, string.Format(format, arg));

        public event EventHandler<bool> OnModeChangedEvent;
        public void ModeChangedEvent(bool isAutoMode) => this.OnModeChangedEvent?.Invoke(this, isAutoMode);
    }
}
