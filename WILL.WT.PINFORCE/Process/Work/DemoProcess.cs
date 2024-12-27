using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TS.FW;
using WILL.WT.PINFORCE.Controls.Chart;
using WILL.WT.PINFORCE.Controls.Vision;
using WILL.WT.PINFORCE.Managers;
using WILL.WT.PINFORCE.Models.Recipe;
using static Infragistics.Shared.DynamicResourceString;

namespace WILL.WT.PINFORCE.Process.Work
{
    // Vision, Graph 등의 Demo를 위한 Process
    // DemoStep에 Step단계를 정의한다.
    public class DemoProcess : IWorkProcess<DemoStep>
    {
        //public StepResult START()
        //{
        //    // ChartDisplay 초기화
        //    ChartDisplayViewModel.Instance.ChartData.ClearValue();

        //    return StepResult.Next;
        //}
        //public StepResult VISION()
        //{
        //    // Vision Start
        //    VisionDisplayViewModel.Instance.StartCamera();

        //    Debug.WriteLine("Process Step : Vision");
        //    return StepResult.Next;
        //}
        //public StepResult VERTICAL_MOVE()
        //{
        //    Debug.WriteLine("Process Step : VerticalMove");

        //    var verticalMove = DB.WorkParam.InitZ;
        //    MotSet(eAxis.AXIS_Z, verticalMove.ReadyPosition, verticalMove.ReadySpeed);
        //    return StepResult.Next;
        //}
        //public StepResult GRAPH()
        //{
        //    Debug.WriteLine("Process Step : Graph");
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        returnNumber();
        //        ChartDisplayViewModel.Instance.ChartData.AddValue(i, num1, num2);
        //        Debug.WriteLine($"{i}, {num1}, {num2}");
        //        Thread.Sleep(1000);
        //    }
        //    return StepResult.Next;
        //}
        //int num1 = 0, num2 = 0;
        //private void returnNumber()
        //{
        //    num1 = num1 % 10 + 2;
        //    num2 = num1 + 1;
        //    // Debug.WriteLine($"{num1}, {num2}");
        //}
        //public StepResult END()
        //{
        //    VisionDisplayViewModel.Instance.StopCamera();
        //    Debug.WriteLine("Process End");
        //    return StepResult.Finish;
        //}

        private double _readPos;
        private double _readSpd;
        private double _procPos;
        private double _procSpd;

        private double _count = 0;

        public StepResult START()
        {
            SetMsg("START");

            // DB에서 Pos/Spd 설정
            // _readPos = DB.WorkParam.InitZ.ReadyPosition;
            // _readSpd = DB.WorkParam.InitZ.ReadySpeed;
            // 
            // _procPos = DB.WorkParam.LimitZ.LimitPosition;
            // _procSpd = DB.WorkParam.LimitZ.LimitSpeed;

            // 동작 실행 횟수 초기화
            _count = 0;

            // ChartDisplay 초기화
            ChartDisplayViewModel.Instance.ChartData.ClearValue();
            // Vision Start
            VisionDisplayViewModel.Instance.StartCamera();

            return StepResult.Next;
        }

        public StepResult VISON_START_ENTER()
        {
            
            Debug.WriteLine("Process Start");
            Debug.WriteLine(Rcp.Name);

            return StepResult.Next;
        }

        public StepResult COUNT_SETTTING()
        {
            _count++;
            this.SetMsg(@"Process 동작 {_count}회");

            Debug.WriteLine("Process Step : Vision");
            return StepResult.Next;
        }

        public StepResult MOT_READY_POS_MOVE_ENTER()
        {
            SetMsg("Ready Position Move");

            this.MotSet(eAxis.AXIS_Z, _readPos, _readSpd);
            return this.MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_READY_POS_MOVE_POLLING()
        {
            var timeout = 10D;
            return this.MotPoliing(WorkItem, timeout, eAxis.AXIS_Z);
        }

        public StepResult MOT_PROCESS_POS_MOVE_ENTER()
        {
            SetMsg("Process Position Move");

            this.MotSet(eAxis.AXIS_Z, _procPos, _procSpd);
            return this.MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_PROCESS_POS_MOVE_POLLING()
        {
            var timeout = 10D;
            return this.MotPoliing(WorkItem, timeout, eAxis.AXIS_Z);
        }

        public StepResult CHECK_COUNT()
        {
            if(_count < Rcp.RepeatCount)
            {
                this.Step.Change(DemoStep.COUNT_SETTTING);
                return StepResult.Change;
            }

            return StepResult.Next;
        }

        public StepResult MOT_HOME_POS_MOVE_ENTER()
        {
            SetMsg("Ready Position Move");

            this.MotSet(eAxis.AXIS_Z, _readPos, _readSpd);
            return this.MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_HOME_POS_MOVE_POLLING()
        {
            var timeout = 10D;
            return this.MotPoliing(WorkItem, timeout, eAxis.AXIS_Z);
        }

        public StepResult END()
        {
            SetMsg("End");

            return StepResult.Finish;
        }

    }
}
