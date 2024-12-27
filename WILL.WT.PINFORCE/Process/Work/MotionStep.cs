using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WILL.WT.PINFORCE.Process.Work
{
    public enum MotionStep
    {
        START,

        DE_START,

        MOT_STAGE_READY_ENTER,
        MOT_STAGE_READY_POLLING,

        MOT_STAGE_DOWN_ENDER,
        DE_MOT_STAGE_DOWN_POLLING,

        DE_END,

        END,
          
    }
}
