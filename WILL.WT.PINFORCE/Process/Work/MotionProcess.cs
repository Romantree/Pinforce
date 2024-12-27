using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.FW;
using TS.FW.Dac.Alarm;
using WILL.WT.PINFORCE.Configs;
using WILL.WT.PINFORCE.Models.Recipe;

namespace WILL.WT.PINFORCE.Process.Work
{
    public class MotionProcess : IUnitProcess<MotionStep>
    {
        public MotionProcess() : base(true) { }

        private readonly object _locker = new object();


        private double _errorTimeOut;

        private DateTime _tackTime = DateTime.Now;

        public override string Name => "MotionProcess";

        public WorkItem Item { get; private set; }  

        public MainRecipeModel Rcp => Item.Rcp;
        public WorkParamDB Work => DB.WorkParam;

        public double TackTime => (DateTime.Now - _tackTime).TotalSeconds;


        public StepResult START()
        {
            this.SetMsg("프로세스 시작");

            //this._errorTimeOut = DB.errorTimeout.CommErrorTimeout <= 0 ? 60 : DB.errorTimeout.CommErrorTimeout;

            return StepResult.Next;
        }


        public void SetWorkItem(WorkItem item)
        {
            try
            {
                lock (this._locker)
                {
                    this.Item = item;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
        public void ClearWorkItem() => this.SetWorkItem(null);

        protected override void Finish()
        {
            try
            {
                this.ClearWorkItem();
                base.Finish();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
        protected override void DoWorkCallback()
        {
            try
            {
                lock (this._locker)
                {
                    if (this.Item == null) return;

                    base.DoWorkCallback();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
        public StepResult AXIS_Z_MOVE_START() => StepResult.Next;

        public StepResult AXIS_Z_MOVE_READY_ENTER()
        {
            var z = Work.InitZ;

            if (Device[eAxis.AXIS_Z].ActPosition >= z.ReadyPosition) return StepResult.Jump;

            this.MotSet(eAxis.AXIS_Z,z.ReadyPosition,z.ReadySpeed);

            return this.MotEnter(eAxis.AXIS_Z);
        }
        public StepResult IMP_MOT_STAGE_READY_POLLING() => this.MotPoliing(Item, _errorTimeOut, eAxis.AXIS_Z);


        public StepResult AXIS_Z_MOVE_ENTER()
        {

            var data = DB.WorkParam.LimitZ;

            var ReadyPosition = DB.WorkParam.InitZ.ReadyPosition;

            var CurrentPosition = data.LimitPosition < ReadyPosition ? ReadyPosition : data.LimitPosition;
            var speed = data.LimitSpeed;

            var verticalMove = CurrentPosition - ReadyPosition;

            this.MotSet(eAxis.AXIS_Z, CurrentPosition, speed);

            var result = this.MotEnter(eAxis.AXIS_Z);


            return result;
        }
        public StepResult AXIS_Z_MOVE_POLLING()
        {
            var result = this.MotPoliing(Item, _errorTimeOut, eAxis.AXIS_Z);

            return result;
        }

        public StepResult END()
        {
            this._lastMsg = string.Empty;

            //IO.BUZZER = true;

            this.SetMsg("================= End Process =================");

            return StepResult.Finish;
        }


        //public StepResult DE_MOT_STAGE_READY_ENTER()
        //{
        //    var x = Work.InitZ;

        //    if (Device[eAxis.AXIS_Z].ActPosition <= x.Position) return StepResult.Jump;

        //    this._lastMsg = "레디 포지션 시작";

        //    this.MotSet(eAxis.AXIS_Z,x.Position,x.Speed);

        //    return this.MotEnter(eAxis.AXIS_Z);
        //}
        //public StepResult DE_MOT_STAGE_READY_POLLING() => this.MotPoliing(Item,_errorTimeOut,eAxis.AXIS_Z);

    }


}
