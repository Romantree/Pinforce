using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WILL.WT.PINFORCE.Models.Recipe;

namespace WILL.WT.PINFORCE.Process.Work.Main
{
    public class MainProc : IWorkProcess<MainStep>
    {
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        private int _autoConectCount = 0;
        private double _autoConectTimeout =0;

        private readonly List<LoadcellHistory> _loadcell = new List<LoadcellHistory>();

        private AutoContectRcpModel AutoRcp => AP.Rcp.AutoContect;

        public StepResult START()
        {
            this.SetMsg("Start");

            _autoConectCount = 0;
            _autoConectTimeout = AutoRcp.ProcTimeout;

            return StepResult.Next;
        }

        public StepResult AUTO_CONTECT_CHECK()
        {
            if(AutoRcp.IsAutoContect == false)
            {
                SetMsg("Auto Contect X");

                this.Step.Change(MainStep.MAIN_START);
                return StepResult.Change;
            }

            SetMsg("Auto Contect Start");

            return StepResult.Next;
        }

        public StepResult MOT_AUtO_CONTECT_READY_MOVE_ENTER()
        {
            var curPos = Mot[eAxis.AXIS_Z].ComPosition + AutoRcp.AutoAdjustmentDist;
            var spd = 10;

            MotSet(eAxis.AXIS_Z, curPos, spd);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUtO_CONTECT_READY_MOVE_POLLING()
        {
            var time = _autoConectTimeout;

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult AUTO_CONTECT_COUNT_SETTTING()
        {
            _autoConectCount++;

            SetMsg($"Auto Contect {_autoConectCount}회");

            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_01_MOVE_ENTER()
        {
            var data = AutoRcp.Data01;

            var curPos = Mot[eAxis.AXIS_Z].ComPosition + data.ContactStepDist;
            var spd = 10;

            MotSet(eAxis.AXIS_Z, curPos, spd);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTECT_01_MOVE_POLLING()
        {
            var time = _autoConectTimeout;

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTECT_01_DELAY()
        {
            var data = AutoRcp.Data01;
            if (data.MotionDelay <= 0) return StepResult.Next;

            Sleep(data.MotionDelay);

            // loadcell Check

            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_02_MOVE_ENTER()
        {
            var data = AutoRcp.Data02;

            var curPos = Mot[eAxis.AXIS_Z].ComPosition + data.ContactStepDist;
            var spd = 10;

            MotSet(eAxis.AXIS_Z, curPos, spd);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTECT_02_MOVE_POLLING()
        {
            var time = _autoConectTimeout;

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTECT_02_DELAY()
        {
            var data = AutoRcp.Data02;
            if (data.MotionDelay <= 0) return StepResult.Next;

            Sleep(data.MotionDelay);

            // loadcell Check

            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_03_MOVE_ENTER()
        {
            var data = AutoRcp.Data03;

            var curPos = Mot[eAxis.AXIS_Z].ComPosition + data.ContactStepDist;
            var spd = 10;

            MotSet(eAxis.AXIS_Z, curPos, spd);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTECT_03_MOVE_POLLING()
        {
            var time = _autoConectTimeout;

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTECT_03_DELAY()
        {
            var data = AutoRcp.Data03;
            if (data.MotionDelay <= 0) return StepResult.Next;

            Sleep(data.MotionDelay);

            // loadcell Check

            return StepResult.Next;
        }

        public StepResult AUTO_CONTECT_CONUT_CHECK()
        {
            if(_autoConectCount < AutoRcp.RepeatCount)
            {
                Step.Change(MainStep.AUTO_CONTECT_COUNT_SETTTING);
                return StepResult.Change;
            }

            return StepResult.Next;
        }

        public StepResult LAST_RELEASE_CHECK()
        {
            if (AutoRcp.IsLastReleaseMove == false)
            {
                SetMsg("Last Release Move X");

                Step.Change(MainStep.MAIN_START);
                return StepResult.Change;
            }

            SetMsg("Last Release Move Start");

            return StepResult.Next;
        }

        public StepResult MOT_LAST_RELEASE_MOVE_ENTER()
        {
            var curPos = Mot[eAxis.AXIS_Z].ComPosition + AutoRcp.LastReleaseMoveDist;
            var spd = 10;

            MotSet(eAxis.AXIS_Z, curPos, spd);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_LAST_RELEASE_MOVE_POLLING()
        {
            var time = _autoConectTimeout;

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MAIN_START()
        {
            SetMsg("Main Start");

            return StepResult.Next;
        }

        public StepResult MOT_COL_START_ENTER()
        {
            SetMsg("Loadcell 수집 시작");

            _loadcell.Clear();

            TimeStart();

            return StepResult.Next;
        }

        public StepResult MOT_COL_START_POLLING()
        {
            _loadcell.Add(_random.Next(0, 100)); // 데이터 수집

            if (Timeout(5.0D))
            {
                var list = _loadcell.GroupBy(t => t.Key).Select(t => t.First()).ToList();

                return StepResult.Next;
            }

            return StepResult.Pending;
        }

        public StepResult END()
        {
            this.SetMsg("End");

            return StepResult.Finish;
        }
    }

    public class LoadcellHistory
    {
        public DateTime Time { get; set; } = DateTime.Now;

        public string Key { get; set; }

        public double Data { get; set; }

        public LoadcellHistory(double data)
        {
            Key = Time.ToString("mm:ss");
            Data = data;
        }

        public override string ToString() => $"{Key} : {Data}";

        public static implicit operator LoadcellHistory(double data) => new LoadcellHistory(data);
    }
}
