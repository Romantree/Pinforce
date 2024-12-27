using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WILL.WT.PINFORCE.Models.Recipe;

namespace WILL.WT.PINFORCE.Process.Work.AutoContect
{
    public class AutoContectProc : IUnitProcess<AutoContectStep>
    {
        private int _count = 0;

        private AutoContectRcpModel AutoRcp => AP.Rcp.AutoContect;

        public AutoContectProc() : base(true) { }

        public StepResult START()
        {
            _count = 0;

            return StepResult.Next;
        }

        public StepResult MOT_AUtO_CONTECT_READY_MOVE_ENTER()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUtO_CONTECT_READY_MOVE_POLLING()
        {
            return StepResult.Pending;
        }

        public StepResult COUNT_SETTTING()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_01_MOVE_ENTER()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_01_MOVE_POLLING()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_01_DELAY()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_02_MOVE_ENTER()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_02_MOVE_POLLING()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_02_DELAY()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_03_MOVE_ENTER()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_03_MOVE_POLLING()
        {
            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_03_DELAY()
        {
            return StepResult.Next;
        }

        public StepResult CONUT_CHECK()
        {
            return StepResult.Next;
        }

        public StepResult LAST_RELEASE_CHECK()
        {
            return StepResult.Next;
        }

        public StepResult MOT_LAST_RELEASE_MOVE_ENTER()
        {
            return StepResult.Next;
        }

        public StepResult MOT_LAST_RELEASE_MOVE_POLLING()
        {
            return StepResult.Next;
        }

        public StepResult END()
        {
            return StepResult.Finish;
        }

    }
}
