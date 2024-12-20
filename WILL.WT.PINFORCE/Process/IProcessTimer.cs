using WILL.WT.PINFORCE.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using TS.FW;
using TS.FW.Dac.Alarm;
using TS.FW.Diagnostics;

namespace WILL.WT.PINFORCE.Process
{
    public abstract partial class IProcessTimer : BackgroundTimer
    {
        private const int SLEEP_TIME = 50;
        private static readonly List<IProcessTimer> _procList = new List<IProcessTimer>();

        private readonly Type _type;
        private readonly Stopwatch _watch = new Stopwatch();
        protected string _lastMsg = string.Empty;

        public virtual string Name => _type.Name;

        public double WatchTime => _watch.ElapsedMilliseconds;

        public IProcessTimer(bool isManagement) : base(ApartmentState.MTA)
        {
            if (isManagement && _procList.Contains(this) == false) _procList.Add(this);

            this._type = this.GetType();

            this.SleepTimeMsc = SLEEP_TIME;
            this.DoWork += IProcessTimer_DoWork;
        }

        protected abstract void DoWorkCallback();

        protected virtual void RecoveryCallback(AlarmData<eAlarm> alarm) => base.Resume();

        protected void SetMsg(string format, params object[] args)
        {
            var msg = string.Format(format, args);

            if (_lastMsg == msg) return;

            try
            {
                Logger.CustomWrite(this.Name, this, msg, Logger.LogEventLevel.Information);

                AP.System.ProcessMsgEvent(this.Name, msg);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                _lastMsg = msg;
            }
        }

        protected void TimeStart() => _watch.Restart();

        protected void TimeStop() => _watch.Stop();

        protected bool Timeout(double time) => Timeout(Convert.ToInt32(time * 1000));

        protected bool Timeout(int time)
        {
            if (time <= 0) return true;

            return WatchTime > time;
        }

        protected void Sleep(double time) => Sleep(Convert.ToInt32(time * 1000));

        protected void Sleep(int time)
        {
            if (time <= 0) return;

            this.SetMsg($"Delay Time {time} msec");

            Thread.Sleep(time);
        }

        protected StepResult AlarmPost(eAlarm eAlarm, object item = null)
        {
            try
            {
                var level = Alarm.AlarmPost(eAlarm, RecoveryCallback, item);
                if (level == AlarmLevel.Light || level == AlarmLevel.CycleStop) return StepResult.Next;
                if (level == AlarmLevel.Heavy) return StepResult.Alarm;

                return StepResult.Stop;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return StepResult.Error;
            }
        }

        private void IProcessTimer_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (this.IsPause) return;

                this.DoWorkCallback();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public static void Abort() => _procList.ForEach(t => t.Stop());

        public static void PauseAll() => _procList.ForEach(t => t.Pause());

        public static void ResumeAll() => _procList.ForEach(t => t.Resume());
    }
}
