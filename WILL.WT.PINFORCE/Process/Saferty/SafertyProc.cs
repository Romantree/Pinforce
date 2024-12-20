using System;
using System.Collections.Generic;
using TS.FW;

namespace WILL.WT.PINFORCE.Process.Saferty
{
    public class SafertyProc : IUnitProcess<SafertyStep>
    {
        private int _blank = 0;

        public SafertyProc() : base(false) { }

        public StepResult CHECK()
        {
            MOT_SERVO();
            MOT_ALRAM();
            TOWER();
            Loadcell();

            return StepResult.Pending;
        }

        private void MOT_SERVO()
        {
            try
            {
                foreach (var axis in EnumHelper.Axis)
                {
                    var mot = AP.Device[axis];
                    if (mot.IsServoOn == true) continue;

                    var alarm = (eAlarm)Enum.Parse(typeof(eAlarm), $"MOTION_SERVO_OFF_{axis}");
                    this.AlarmPost(alarm);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void MOT_ALRAM()
        {
            try
            {
                foreach (var axis in EnumHelper.Axis)
                {
                    var mot = AP.Device[axis];
                    if (mot.IsAlarm == false) continue;

                    var alarm = (eAlarm)Enum.Parse(typeof(eAlarm), $"MOTION_ALARM_ON_{axis}");
                    this.AlarmPost(alarm);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void TOWER()
        {
            try
            {
                if (AP.Alarm.IsAlarm)
                {
                    IO.TOWER_LAMP_RED = true;
                    IO.TOWER_LAMP_YELLOW = false;
                    IO.TOWER_LAMP_GREEN = false;
                }
                else if (AP.Proc.IsBusy)
                {
                    IO.TOWER_LAMP_RED = false;
                    IO.TOWER_LAMP_YELLOW = false;
                    IO.TOWER_LAMP_GREEN = true;
                }
                else if (AP.Proc.IsInit == false)
                {
                    IO.TOWER_LAMP_RED = false;
                    _blank++; if (_blank >= 10) { IO.TOWER_LAMP_YELLOW = !IO.TOWER_LAMP_YELLOW; _blank = 0; }
                    IO.TOWER_LAMP_GREEN = false;
                }
                else
                {
                    IO.TOWER_LAMP_RED = false;
                    IO.TOWER_LAMP_YELLOW = true;
                    IO.TOWER_LAMP_GREEN = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Loadcell()
        {
            try
            {
                if (AP.Proc.Main.IsBusy == false) return;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }

    public enum SafertyStep
    {
        CHECK,
    }

    public static class DelayTimeHelper
    {
        private static Dictionary<string, DateTime?> _time = new Dictionary<string, DateTime?>();

        public static void TimeCheck(this string key, double time, Func<bool> checker, Action action)
        {
            if(time <= 0 || checker() == false)
            {
                key.SetTime(null);
            }
            else if(key.GetTime().HasValue == false)
            {
                key.SetTime(DateTime.Now);
            }
            else if((DateTime.Now - key.GetTime().Value).TotalSeconds >= time)
            {
                action();
            }
        }

        private static DateTime? GetTime(this string key)
        {
            if (_time.ContainsKey(key) == false) _time.Add(key, null);

            return _time[key];
        }

        private static void SetTime(this string key, DateTime? value)
        {
            if (_time.ContainsKey(key) == false) _time.Add(key, null);

            _time[key] = value;
        }
    }
}
