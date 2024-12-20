using System.Threading;

namespace WILL.WT.PINFORCE.Process.Init
{
    public class InitProc : IUnitProcess<InitProcStep>
    {
        public InitProc() : base(false) { }

        public bool IsInit { get; set; } = false;

        public StepResult START()
        {
            SetMsg("Start");

            this.IsInit = false;
            return StepResult.Next;
        }

        public StepResult END()
        {
            SetMsg("End");

            this.IsInit = true;

            return StepResult.Finish;
        }
    }
}
