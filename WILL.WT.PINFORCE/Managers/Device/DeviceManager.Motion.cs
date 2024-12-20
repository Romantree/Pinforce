using System;
using System.Linq;
using TS.FW;
using TS.FW.Device;
using TS.FW.Device.Ajin;

namespace WILL.WT.PINFORCE.Managers
{
    public partial class DeviceManager
    {
        private HomeAsyncResult AXIS_Z;

        private HomeAsyncResult ToHomeResult(eAxis axis)
        {
            switch (axis)
            {
                case eAxis.AXIS_Z: return AXIS_Z;
            }

            return null;
        }

        public bool IsServoOnAll => this.Axis.All(t => t.IsServoOn == true);

        public bool IsAlarmAll => this.Axis.Any(t => t.IsAlarm == true);

        public bool IsBusyAll => this.Axis.Any(t => t.IsBusy == true);

        public void InitScale()
        {
            if (AP.IsSim) return;

            this.ToAjin(eAxis.AXIS_Z).Setting.SCALE = 0.001;
        }

        public void Home(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return;

            foreach (var item in axis)
            {
                switch (item)
                {
                    case eAxis.AXIS_Z: this[item].HomeAsync(out AXIS_Z); break;
                }
            }
        }

        public AjinAxis ToAjin(eAxis axis) => this[axis] as AjinAxis;

        public void ServoOn()
        {
            this.ServoOn(EnumHelper.Axis.ToArray());
        }

        public void ServoOn(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsServoOn == true) continue;

                    item.IsServoOn = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public bool IsServoOn(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsServoOn == false) return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return false;
            }
        }

        public void ResetAlarm()
        {
            this.ResetAlarm(EnumHelper.Axis.ToArray());
        }

        public void ResetAlarm(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsAlarm == false) continue;

                    item.ResetAlarm();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public bool IsAlarm(params eAxis[] list)
        {
            try
            {
                foreach (var axis in list)
                {
                    var item = this[axis];
                    if (item.IsAlarm == true) return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return true;
            }
        }

        public void Stop()
        {
            this.Stop(EnumHelper.Axis.ToArray());
        }

        public void Stop(params eAxis[] list)
        {
            try
            {
                foreach (var item in list)
                {
                    this[item].Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void HomeStop(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return;

            foreach (var item in axis)
            {
                var home = this.ToHomeResult(item);
                home.IsStop = true;
            }
        }

        public bool HomeSuccess(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return false;

            foreach (var item in axis)
            {
                var home = this.ToHomeResult(item);

                if (home.Complete == false || home.Success == false) return false;
            }

            return true;
        }

        public bool HomeFail(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return false;

            foreach (var item in axis)
            {
                var home = this.ToHomeResult(item);

                if (home.Complete == true && home.Success == false) return true;
            }

            return false;
        }

        public string HomeComment(eAxis axis) => this.ToHomeResult(axis).Comment;

        public bool Gantry(eAxis axis, bool gantry)
        {
            if (AP.IsSim) return gantry;

            return this.ToAjin(axis).Gantry == gantry;
        }

        public bool IsBusy(params eAxis[] axis)
        {
            if (axis == null || axis.Length <= 0) return false;

            foreach (var type in axis)
            {
                if (this[type].IsBusy == true) return true;
            }

            return false;
        }

        public bool CheckPos(eAxis axis, double pos, double gap = 0.01)
        {
            var item = this[axis];
            if (item.IsBusy == true) return false;

            var curPos = AP.IsSim ? item.ActPosition : item.ComPosition;

            return item.IsBusy == false && TS.FW.Helper.ProcessHelper.CheckPosition(curPos, pos, gap);
        }

        public Response GantryEnable(eAxis master, eAxis slave)
        {
            try
            {
                if (AP.IsSim) return new Response();

                var masterAxis = this.ToAjin(master);

                if (masterAxis.Gantry == true) return new Response();

                var res = masterAxis.GantryEnable(this.ToAjin(slave));
                if (res == false) throw new Exception(res.Comment);

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }

        public Response GantryDisable(eAxis master, eAxis slave)
        {
            try
            {
                if (AP.IsSim) return new Response();

                var masterAxis = this.ToAjin(master);

                if (masterAxis.Gantry == false) return new Response();

                var res = masterAxis.GantryDisable(this.ToAjin(slave));
                if (res == false) throw new Exception(res.Comment);

                return new Response();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
                return ex;
            }
        }
    }
}
