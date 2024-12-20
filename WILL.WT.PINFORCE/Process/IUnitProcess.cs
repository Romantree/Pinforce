using System;
using TS.FW;
using TS.FW.Dac.Alarm;

namespace WILL.WT.PINFORCE.Process
{
    public abstract class IUnitProcess<T> : IProcessTimer where T : struct, IConvertible
    {
        public ProcStep<T> Step { get; protected set; } = new ProcStep<T>();

        protected IUnitProcess(bool isManagement) : base(isManagement) { }

        public override void Start()
        {
            if (this.IsBusy) return;

            this.Step.Init();
            base.Start();
        }

        protected override void RecoveryCallback(AlarmData<eAlarm> alarm)
        {
            {
                try
                {
                    if (Step.Contains("POLLING")) Step.Prev();

                    base.Resume();
                }
                catch (Exception ex)
                {
                    Logger.Write(this, ex);
                }
            }
        }

        protected virtual StepResult ExcuteProcess() => this.Step.ExcuteProcess(this, null);

        protected virtual void Finish() { }

        protected override void DoWorkCallback()
        {
            try
            {
                var res = this.ExcuteProcess();

                switch (res)
                {
                    case StepResult.Next:
                        {
                            this.Step.Next();
                        }
                        break;
                    case StepResult.Alarm:
                        {
                            this.SetMsg($"Pause : {this.Step.CurrentStep}");
                            this.Pause();
                        }
                        break;
                    case StepResult.Not_Found_Step:
                    case StepResult.Error:
                    case StepResult.Stop:
                        {
                            this.SetMsg($"{res} : {this.Step.CurrentStep}");
                            this.Stop();
                        }
                        break;
                    case StepResult.Finish:
                        {
                            this.Finish();
                            this.Stop();
                        }
                        break;
                    case StepResult.Jump:
                        {
                            this.Step.Next(2);
                        }
                        break;
                }

                if (AP.IsSim) System.Threading.Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}