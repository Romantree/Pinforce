using WILL.WT.PINFORCE.Managers;

namespace WILL.WT.PINFORCE.Process
{
    public abstract partial class IProcessTimer
    {
        protected static DeviceManager Device => AP.Device;

        private StepResult SetCylinder(CyUnit unit, CyState state)
        {
            if (IO.GetCY(unit, state)) return StepResult.Jump;

            this.SetMsg($"{unit} {state} Enter");

            IO.SetCY(unit, state, true);

            TimeStart();

            return StepResult.Next;
        }

        protected StepResult GetCylinder(CyUnit unit, CyState state, double time, eAlarm alarm)
        {
            if (IO.GetCY(unit, state))
            {
                TimeStop();

                this.SetMsg($"{unit} {state}");

                return StepResult.Next;
            }
            else if (Timeout(time))
            {
                TimeStop();

                this.SetMsg($"{unit} {state} Timeout");

                return AlarmPost(alarm);
            }

            return StepResult.Pending;
        }
    }
}
