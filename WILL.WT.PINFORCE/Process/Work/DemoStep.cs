using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WILL.WT.PINFORCE.Process.Work
{
    // DemoProcess 작업내역 정의
    public enum DemoStep
    {
        START,

        VISON_START_ENTER,

        COUNT_SETTTING,

        MOT_READY_POS_MOVE_ENTER,
        MOT_READY_POS_MOVE_POLLING,

        MOT_PROCESS_POS_MOVE_ENTER,
        MOT_PROCESS_POS_MOVE_POLLING,

        CHECK_COUNT,

        MOT_HOME_POS_MOVE_ENTER,
        MOT_HOME_POS_MOVE_POLLING,

        END,
    }
}
