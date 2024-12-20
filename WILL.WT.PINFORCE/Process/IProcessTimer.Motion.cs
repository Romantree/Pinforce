using System;
using System.Collections.Generic;
using TS.FW.Device;

namespace WILL.WT.PINFORCE.Process
{
    public abstract partial class IProcessTimer
    {
        protected const double MOT_TIMEOUT = 1D;
        protected static readonly Dictionary<eAxis, MotMove> _motList = new Dictionary<eAxis, MotMove>();

        protected MotMove this[eAxis key]
        {
            get
            {
                if (_motList.ContainsKey(key) == false) _motList[key] = new MotMove();

                return _motList[key];
            }
        }

        protected StepResult ServoEnter()
        {
            if (Mot.IsServoOnAll) return StepResult.Jump;

            this.SetMsg($"Motion Servo On Enter");

            Mot.ServoOn();

            TimeStart();

            return StepResult.Next;
        }

        protected StepResult ServoPolling()
        {
            if (Mot.IsServoOnAll)
            {
                TimeStop();

                this.SetMsg($"Motion Servo On");

                return StepResult.Next;
            }
            else if (Timeout(MOT_TIMEOUT))
            {
                TimeStop();

                this.SetMsg($"Motion Servo On Timeout");

                return AlarmPost(eAlarm.MOTION_SERVO_ON_TIMEOUT);
            }

            return StepResult.Pending;
        }

        protected StepResult MotAlarmResetEnter()
        {
            if (Mot.IsAlarmAll == false) return StepResult.Jump;

            this.SetMsg($"Motion Alarm Reset Enter");

            Mot.ResetAlarm();

            TimeStart();

            return StepResult.Next;
        }

        protected StepResult MotAlarmResetPolling()
        {
            if (Mot.IsAlarmAll == false)
            {
                TimeStop();

                this.SetMsg($"Motion Alarm Reset");

                return StepResult.Next;
            }
            else if (Timeout(MOT_TIMEOUT))
            {
                TimeStop();

                this.SetMsg($"Motion Alarm Reset Polling");

                return AlarmPost(eAlarm.MOTION_ALARM_RESET_TIMEOUT);
            }

            return StepResult.Pending;
        }

        protected StepResult GentryEnableEnter(eAxis master, eAxis slave)
        {
            if (Mot.Gantry(master, true)) return StepResult.Jump;

            SetMsg($"Motion Gantry [{master}:{slave}] Enter");

            Mot.GantryEnable(master, slave);

            TimeStart();

            return StepResult.Next;
        }

        protected StepResult GentryEnablePolling(eAxis master, eAxis slave)
        {
            if (Mot.Gantry(master, true))
            {
                TimeStop();

                SetMsg($"Motion Gantry [{master}:{slave}]");

                return StepResult.Next;
            }
            else if (Timeout(MOT_TIMEOUT))
            {
                TimeStop();

                SetMsg($"Motion Gantry [{master}:{slave}] Timeout");

                return AlarmPost(eAlarm.MOTION_GANTRY_TIMEOUT);
            }

            return StepResult.Pending;
        }

        protected StepResult MotLimitEnter(params eAxis[] list)
        {
            this.SetMsg("Motion Limit Setting");

            if (AP.IsSim) return StepResult.Next;

            foreach (var item in list)
            {
                Mot.ToAjin(item).SetSoftwareLimit();
            }

            return StepResult.Next;
        }

        protected void MotSet(eAxis axis, double pos, double speed)
        {
            this[axis].Position = Math.Round(pos, 3);
            this[axis].Speed = speed;
            this[axis].Acc = speed * 4;
        }

        protected StepResult MotEnter(params eAxis[] list)
        {
            foreach (var axis in list)
            {
                var data = this[axis];
                var mot = Mot[axis];

                var curPos = ToCurrentPos(mot);

                if (Mot.CheckPos(axis, data.Position))
                {
                    data.Bypass = true;
                    data.IsComplete = true;
                    this.SetMsg("{0} Bypass : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);
                }
                else
                {
                    mot.Stop();

                    data.Bypass = false;
                    this.SetMsg("{0} Move : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);
                }
            }

            foreach (var axis in list)
            {
                var data = this[axis];
                if (data.Bypass) continue;

                var mot = Mot[axis];
                mot.MoveABS(data.Position, data.Speed, data.Acc, data.Acc);

                data.BusyTime = null;
                data.IsComplete = false;
            }

            TimeStart();

            return StepResult.Next;
        }

        protected StepResult MotPoliing(Work.WorkItem item, double time, params eAxis[] list)
        {
            foreach (var axis in list)
            {
                var data = this[axis];
                var mot = Mot[axis];

                if (data.IsComplete || data.Bypass) continue;

                if (Mot.CheckPos(axis, data.Position))
                {
                    var curPos = ToCurrentPos(mot);
                    this.SetMsg("{0} : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);

                    data.IsComplete = true;
                }
                else if (Timeout(time))
                {
                    mot.Stop();

                    var curPos = ToCurrentPos(mot);

                    var res = MotTimeout(axis, curPos, data);
                    if (res != StepResult.Next) return res;
                }
                else if (mot.IsBusy == false)
                {
                    if (data.BusyTime == null)
                    {
                        data.BusyTime = DateTime.Now;
                    }
                    else if ((DateTime.Now - data.BusyTime.Value).TotalSeconds >= 1)
                    {
                        var curPos = ToCurrentPos(mot);
                        this.SetMsg("{0} Retry : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);

                        mot.MoveABS(data.Position, data.Speed, data.Acc, data.Acc);

                        data.BusyTime = null;
                    }
                }
            }

            if (IsComplete(list))
            {
                TimeStop();

                return StepResult.Next;
            }

            return StepResult.Pending;
        }

        private bool IsComplete(params eAxis[] list)
        {
            foreach (var axis in list)
            {
                if (this[axis].IsComplete == false) return false;
            }

            return true;
        }

        private double ToCurrentPos(IAxis axis) => AP.IsSim ? axis.ActPosition : axis.ComPosition;

        private StepResult MotTimeout(eAxis axis, double curPos, MotMove data)
        {
            this.SetMsg("{0} : Cur:{1:f3}mm Move:{2:f3}mm", axis, curPos, data.Position);

            var res = AlarmPost(ToAlarm(axis));
            if (res != StepResult.Next) return res;

            data.IsComplete = true;

            return StepResult.Next;
        }

        private eAlarm ToAlarm(eAxis axis)
        {
            var name = $"MOTION_MOVE_TIMEOUT_{axis}";

            return (eAlarm)Enum.Parse(typeof(eAlarm), name);
        }
    }

    public class MotMove
    {
        public double Position { get; set; }

        public double Speed { get; set; }

        public double Acc { get; set; }

        public DateTime? BusyTime { get; set; } = null;

        public bool Bypass { get; set; } = false;

        public bool IsComplete { get; set; } = false;
    }
}