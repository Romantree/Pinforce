using System;
using System.Collections.Generic;
using System.Diagnostics;
using TS.FW;
using WILL.WT.PINFORCE.Models.Setup;

namespace WILL.WT.PINFORCE.Process.Saferty
{
    public class SafertyProc : IUnitProcess<SafertyStep>
    {
        private int _blank = 0;

        public SafertyProc() : base(false) { }

        public LoadcellModel LoadcellModel_1 { get; set; }
        public LoadcellModel LoadcellModel_2 { get; set; }

        public StepResult CHECK()
        {
            MOT_SERVO();
            MOT_ALRAM();
            TOWER();
            LOADCELL();

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

        private void LOADCELL()
        {
            try
            {
                if (LoadcellModel_1 == null)
                    LoadcellModel_1 = new LoadcellModel(true);
                if (LoadcellModel_2 == null)
                    LoadcellModel_2 = new LoadcellModel(false);

                if (LoadcellModel_1 != null)
                {
                    LoadcellModel_1.Update();
                    if (LoadcellModel_1.Data > DB.WorkParam.limit.LimitForce)
                        this.AlarmPost(eAlarm.LOADCELL_01_OVERWEIGHT);
                }
                if (LoadcellModel_2 != null)
                {
                    LoadcellModel_2.Update();
                    if (LoadcellModel_2.Data > DB.WorkParam.limit.LimitForce)
                        this.AlarmPost(eAlarm.LOADCELL_02_OVERWEIGHT);
                }
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
