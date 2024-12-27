using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WILL.WT.PINFORCE.Controls.Chart;
using WILL.WT.PINFORCE.Controls.Vision;
using WILL.WT.PINFORCE.Managers;
using WILL.WT.PINFORCE.Models.Config;
using WILL.WT.PINFORCE.Models.Recipe;
using WILL.WT.PINFORCE.Models.Setup;

namespace WILL.WT.PINFORCE.Process.Work.Main
{
    public class MainProc : IWorkProcess<MainStep>
    {
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        private int _autoContactCount = 0;
        private double _autoContactTimeout = 0;

        private int _pinForceCount = 0;
        private double _pinContactOriginPos = 0;

        private double _workSpeed = 0; // 작업 속도

        // Loadcell 평균값 계산용
        private List<double> _loadcellSamplingArray_1 = new List<double>();
        private List<double> _loadcellSamplingArray_2 = new List<double>();
        private List<double> _loadcellContactArray_1 = new List<double>();
        private List<double> _loadcellContactArray_2 = new List<double>();
        bool collectData = false;

        // Loadcell
        public LoadcellModel LoadcellModel_1 { get; set; }
        public LoadcellModel LoadcellModel_2 { get; set; }

        // VisionUIData 에서 보여질 Data
        public double Step_OD = 0;
        // 이송할 Step 단계
        public List<double> Step_OD_List = new List<double>();
        // 이송할 Step 단계 Index
        public int Step_OD_Idx = 0;

        // DashMainView 에서 보여질 Data
        public int StepRptCnt = 0; // Step 반복 횟수
        public int TestRptCnt = 0; // 테스트 반복 횟수
        public int NowStep = 0; // 현재 작업 단계
        public int TotalStep = 0; // 전체 작업 단계 => 총 반복 횟수
        public double Progress = 0; // 총 진행률 (DashMainView 출력시 *100)

        private AutoContactRcpModel AutoRcp => AP.Rcp.AutoContact;

        public override void Stop()
        {
            base.Stop();
        }

        private void GetLoadcellData()
        {
            if (LoadcellModel_1 == null)
                LoadcellModel_1 = new LoadcellModel(true);
            if (LoadcellModel_2 == null)
                LoadcellModel_2 = new LoadcellModel(false);

            this.LoadcellModel_1.Update();
            this.LoadcellModel_2.Update();
        }

        private void ProgressUp()
        {
            // 현재 작업률 증가
            TestRptCnt = _pinForceCount - 1; // 반복을 완료한 횟수
            NowStep = (Step_OD_Idx + 1) + StepRptCnt * (TestRptCnt * Step_OD_List.Count() /* 반복 완료 횟수 * 1번 반복당 진행하는 Step의 갯수 */ );

            // 전체 작업 단계 => 1회당 PinForce의 Step 갯수 * PinForce Step의 총 반복 횟수 * PinForce의 총 반복 횟수
            // TotalStep = Step_OD_List.Count * /* WorkItem.Rcp.StepRepeatCount * */ WorkItem.Rcp.RepeatCount;
            Progress = (double)NowStep / (double)TotalStep;
        }

        // MainProc 동작 간 작동하는 Thread
        protected override StepResult ExcuteProcess()
        {
            // List<LoadcellHistory> tempLC_1 = new List<LoadcellHistory>();
            // List<LoadcellHistory> tempLC_2 = new List<LoadcellHistory>();

            switch (this.Step.CurrentStep)
            {
                // 아래의 Step 단계에서 RawData 수집 준비한다.
                // case MainStep.MOT_OD_MOVE_DELAY:
                    /*
                    {
                        TimeStart(); // Timer 갱신
                        // Data Clear
                        tempLC_1.Clear();
                        tempLC_2.Clear();

                        break;
                    }
                    */
                // 아래의 Step 단계동안 RawData를 수집한다.
                case MainStep.MOT_OD_SAMPLING_DELAY:
                case MainStep.MOT_OD_CONTACT_DELAY:
                case MainStep.ADDLOG:
                case MainStep.INC_OD:
                // case MainStep.CHECK_REPEAT_MODE:
                    /*
                    {
                        // Get New Loadcell Data
                        GetLoadcellData();

                        // Temporary Collect
                        tempLC_1.Add(LoadcellModel_1.Data);
                        tempLC_2.Add(LoadcellModel_2.Data);

                        if (Timeout(DB.WorkParam.logData.Interval))
                        {
                            // 수집한 Data를 설정한 시간 간격으로 Set하여 Average
                            double val_1 = tempLC_1.GroupBy(t => t.Key).Select(t => t.First().Data).Average();
                            double val_2 = tempLC_2.GroupBy(t => t.Key).Select(t => t.First().Data).Average();

                            // Chart, Excel Add
                            ChartDisplayViewModel.Instance.ChartData.AddValue(Step_OD, val_1, val_2);
                            ExcelManager.Instance.ExcelRawData.AddValue(Mot[eAxis.AXIS_Z].ComPosition, Step_OD, val_1, val_2);

                            tempLC_1.Clear();
                            tempLC_2.Clear();

                            TimeStart();

                        }    
                    }
                    */
                    {
                        // 임시로 Sleep
                        Sleep(DB.WorkParam.logData.Interval);

                        // Get New Loadcell Data
                        GetLoadcellData();

                        // Chart에 출력한다.
                        ChartDisplayViewModel.Instance.ChartData.AddValue(Step_OD, LoadcellModel_1.Data, LoadcellModel_2.Data);

                        // Excel 데이터 List 입력한다.
                        ExcelManager.Instance.ExcelRawData.AddValue(Mot[eAxis.AXIS_Z].ComPosition, Step_OD, LoadcellModel_1.Data, LoadcellModel_2.Data);
                    }
                    break;
            }
            return base.ExcuteProcess();
        }

        public StepResult START()
        {
            this.SetMsg("Process 시작");

            _autoContactCount = 0;
            _autoContactTimeout = AutoRcp.ProcTimeout;
            _workSpeed = 0;

            // 시작시 초기화
            Step_OD = 0;
            StepRptCnt = 0;
            NowStep = 0;
            Progress = 0;

            _pinContactOriginPos = 0;
            _pinForceCount = 0;

            Step_OD_List.Clear();
            Step_OD_Idx = 0;

            // Vision, Chart, Excel 초기화
            ChartDisplayViewModel.Instance.ChartData.ClearValue();
            ExcelManager.Instance.ExcelRawData.ClearValue();
            ExcelManager.Instance.ExcelData.ClearValue();
            VisionDisplayViewModel.Instance.StartCamera();

            // Step 단계 List 추가
            // 하강방향; Max까지 이송해야 하므로 <=(Max 포함)
            for (double step = WorkItem.Rcp.Start; step <= WorkItem.Rcp.Max; step += WorkItem.Rcp.Step)
                Step_OD_List.Add(step);

            // 상승방향; Max와 End가 달라야 End점 이송 추가
            if (WorkItem.Rcp.Max != WorkItem.Rcp.End)
                // Max - Step 부터 End점 까지
                for (double step = WorkItem.Rcp.Max - WorkItem.Rcp.Step; step >= WorkItem.Rcp.End; step -= WorkItem.Rcp.Step)
                    Step_OD_List.Add(step);

            // 전체 작업 단계 => 1회당 PinForce의 Step 갯수 * PinForce Step의 총 반복 횟수 * PinForce의 총 반복 횟수
            TotalStep = Step_OD_List.Count * /* WorkItem.Rcp.StepRepeatCount * */ WorkItem.Rcp.RepeatCount;

            return StepResult.Next;
        }

        public StepResult AUTO_CONTACT_CHECK()
        {
            // AutoContact 실행 여부
            if(AutoRcp.IsAutoContact == false)
            {
                SetMsg("Auto Contact Mode : Manual");

                this.Step.Change(MainStep.MAIN_START);
                return StepResult.Change;
            }

            SetMsg("Auto Contact Mode : Auto");
            SetMsg("Auto Contact 시작");

            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTECT_READY_MOVE_ENTER()
        {
            // 현재 위치 + 초기 시작점 조절 (AutoAdjustmentDist 는 위로 가는 방향)
            var curPos = Mot[eAxis.AXIS_Z].ComPosition + (AutoRcp.AutoAdjustmentDist / 1000 /* um */);

            // 작업 속도 결정 : 리밋 속도를 초과하는 입력을 발견하면 Limit Speed로, 아니면 설정한 속도로
            _workSpeed = (DB.WorkParam.motion.WorkMoveSpeed >= DB.WorkParam.limit.LimitSpeed) ? DB.WorkParam.limit.LimitSpeed : DB.WorkParam.motion.WorkMoveSpeed;

            // 초기 시작점 조절이 지정되지 않았으면
            if (AutoRcp.AutoAdjustmentDist == 0)
            {
                SetMsg("Auto Contact Adjustment 생략");
                // 바로 1st Contact으로
                Step.Change(MainStep.MOT_AUTO_CONTACT_01_MOVE_ENTER);
                return StepResult.Change;
            }

            SetMsg("Auto Contact Adjustment 시작");
            MotSet(eAxis.AXIS_Z, curPos, _workSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTECT_READY_MOVE_POLLING()
        {
            var time = _autoContactTimeout;

            // 초기 시작점 이송 도착 대기
            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult AUTO_CONTACT_COUNT_SETTTING()
        {
            // Auto Contact Count 증가
            _autoContactCount++;

            SetMsg($"Auto Contact : {_autoContactCount}회");

            return StepResult.Next;
        }

        public StepResult MOT_AUTO_CONTACT_01_MOVE_ENTER()
        {
            var data = AutoRcp.Data01;
            Step_OD = data.ContactStepDist;

            // 1번째 Contact
            var curPos = Mot[eAxis.AXIS_Z].ComPosition - (Step_OD / 1000 /* um */); // 현재 위치에서 - 방향으로 내려감

            SetMsg("1st Auto Contact 시작");
            MotSet(eAxis.AXIS_Z, curPos, _workSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTACT_01_MOVE_POLLING()
        {
            var time = _autoContactTimeout;

            // 폴링 중에 AutoContactAlarmDepth Check를 해야하는데, 이 부분 확인 필요

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTACT_01_DELAY()
        {
            var data = AutoRcp.Data01;
            // if (data.MotionDelay <= 0) return StepResult.Next;

            Sleep(data.MotionDelay);

            // Loadcell Check
            GetLoadcellData();
            // 설정한 Force보다 높게 측정되면
            if (LoadcellModel_1.Data >= data.OriginForce)
            {
                SetMsg("1st Auto Contact 종료");
                return StepResult.Next;
            }
            else
            {
                SetMsg("1st Auto Contact 실패, 재시도");
                // 한번 더 내려간다.
                Step.Change(MainStep.MOT_AUTO_CONTACT_01_MOVE_ENTER);
                return StepResult.Change;
            }
        }

        public StepResult MOT_AUTO_CONTACT_02_MOVE_ENTER()
        {
            var data = AutoRcp.Data02;
            Step_OD = data.ContactStepDist;

            // 2번째 Contact
            var curPos = Mot[eAxis.AXIS_Z].ComPosition + (Step_OD / 1000 /* um */); // 현재 위치에서 + 방향으로 올라감

            SetMsg("2nd Auto Contact 시작");
            MotSet(eAxis.AXIS_Z, curPos, _workSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTACT_02_MOVE_POLLING()
        {
            var time = _autoContactTimeout;

            // 폴링 중에 AutoContactAlarmDepth Check를 해야하는데, 이 부분 확인 필요

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTACT_02_DELAY()
        {
            var data = AutoRcp.Data02;
            // if (data.MotionDelay <= 0) return StepResult.Next;

            Sleep(data.MotionDelay);

            // Loadcell Check
            GetLoadcellData();
            // 설정한 Force보다 낮게 측정되면
            if (LoadcellModel_1.Data <= data.OriginForce)
            {
                SetMsg("2nd Auto Contact 종료");
                return StepResult.Next;
            }
            else
            {
                SetMsg("2nd Auto Contact 실패, 재시도");
                // 한번 더 올라간다.
                Step.Change(MainStep.MOT_AUTO_CONTACT_02_MOVE_ENTER);
                return StepResult.Change;
            }
        }

        public StepResult MOT_AUTO_CONTACT_03_MOVE_ENTER()
        {
            var data = AutoRcp.Data03;
            Step_OD = data.ContactStepDist;

            var curPos = Mot[eAxis.AXIS_Z].ComPosition - (Step_OD / 1000 /* um */); // 현재 위치에서 - 방향으로 내려감

            SetMsg("3rd Auto Contact 시작");
            MotSet(eAxis.AXIS_Z, curPos, _workSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTACT_03_MOVE_POLLING()
        {
            var time = _autoContactTimeout;

            // 폴링 중에 AutoContactAlarmDepth Check를 해야하는데, 이 부분 확인 필요

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_AUTO_CONTACT_03_DELAY()
        {
            var data = AutoRcp.Data03;
            // if (data.MotionDelay <= 0) return StepResult.Next;

            Sleep(data.MotionDelay);

            // Loadcell Check
            GetLoadcellData();
            // 설정한 Force보다 높게 측정되면
            if (LoadcellModel_1.Data >= data.OriginForce)
            {
                SetMsg("3rd Auto Contact 종료");
                return StepResult.Next;
            }
            else
            {
                SetMsg("3rd Auto Contact 실패, 재시도");
                // 한번 더 내려간다.
                Step.Change(MainStep.MOT_AUTO_CONTACT_03_MOVE_ENTER);
                return StepResult.Change;
            }
        }

        public StepResult AUTO_CONTACT_CONUT_CHECK()
        {
            // Auto Contact 반복 횟수보다 적으면
            if(_autoContactCount < AutoRcp.RepeatCount)
            {
                Step.Change(MainStep.AUTO_CONTACT_COUNT_SETTTING);
                return StepResult.Change;
            }

            return StepResult.Next;
        }

        public StepResult LAST_RELEASE_CHECK()
        {
            // 완료 후 Release 설정이 되지 않았으면
            if (AutoRcp.IsLastReleaseMove == false)
            {
                SetMsg("Last Release Move 생략");

                Step.Change(MainStep.MAIN_START);
                return StepResult.Change;
            }
            else if (AutoRcp.LastReleaseMoveDist == 0) // 완료 후 Release 설정은 됐는데 거리가 0일 경우
            {
                SetMsg("Last Release Move Dist. : 0, 생략");

                Step.Change(MainStep.MAIN_START);
                return StepResult.Change;
            }
            return StepResult.Next;
        }

        public StepResult MOT_LAST_RELEASE_MOVE_ENTER()
        {
            // 현재 위치 + Last Release 지점 조절 (LastReleaseMoveDist 는 위로 가는 방향)
            var curPos = Mot[eAxis.AXIS_Z].ComPosition + (AutoRcp.LastReleaseMoveDist / 1000 /* um */);

            SetMsg("Last Release Move 시작");
            MotSet(eAxis.AXIS_Z, curPos, _workSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_LAST_RELEASE_MOVE_POLLING()
        {
            var time = _autoContactTimeout;

            // Last Release 지점 이송 도착 대기
            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MAIN_START()
        {
            // 현재 Contact 지점 저장 = 현재 Z축의 위치
            _pinContactOriginPos = Mot[eAxis.AXIS_Z].ComPosition;

            if (Step_OD_List.Count > 0)
                Step_OD = Step_OD_List[0]; // START O/D
            else
            {
                // Step 단계 연산에 문제가 발생할 경우
            }

            // PinForce Count Set
            _pinForceCount++;

            // Step Count Set
            StepRptCnt++;

            // Log 출력
            // SetMsg($"{_pinForceCount} 회 PinForce 측정 시작");
            SetMsg($"{WorkItem.Rcp.RepeatCount} 회 PinForce 측정 시작");

            return StepResult.Next;
        }

        public StepResult MOT_OD_MOVE_ENTER()
        {
            // 작업 진행 현황 업데이트
            ProgressUp();

            var curPos = _pinContactOriginPos - (Step_OD / 1000 /* um */); // 현재 위치에서 - 방향으로 내려감
            // 작업 속도 결정 : 리밋 속도를 초과하는 입력을 발견하면 Limit Speed로, 아니면 설정한 속도로
            _workSpeed = (WorkItem.Rcp.WorkSpeed >= DB.WorkParam.limit.LimitSpeed) ? DB.WorkParam.limit.LimitSpeed : WorkItem.Rcp.WorkSpeed;

            SetMsg($"Round {_pinForceCount}, PinForce O/D : {Step_OD} um Move Start");
            MotSet(eAxis.AXIS_Z, curPos, _workSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_OD_MOVE_POLLING()
        {
            var time = DB.WorkParam.errorTimeout.MotionErrorTimeout;

            // 폴링 중에 AutoContactAlarmDepth Check를 해야하는데, 이 부분 확인 필요

            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_OD_MOVE_DELAY() // 모션 딜레이
        {
            SetMsg($"Motion Delay...");
            Sleep(DB.WorkParam.delay.MotionDelay);

            return StepResult.Next;
        }

        public StepResult MOT_OD_SAMPLING_START() // 샘플링 시작 딜레이
        {
            TimeStart(); // 아래의 Sampling Delay TimeStart
            SetMsg($"Sampling Delay...");

            // Excel Raw 데이터 수집은 여기서 시작한다.
            SetMsg($"Start Collecting Raw Data");

            return StepResult.Next;
        }

        public StepResult MOT_OD_SAMPLING_DELAY() // 샘플링 시작 딜레이
        {
            // New Loadcell Data Get
            GetLoadcellData();

            // Loadcell Data 수집
            _loadcellSamplingArray_1.Add(LoadcellModel_1.Data);
            _loadcellSamplingArray_2.Add(LoadcellModel_2.Data);

            // Sampling Delay를 포함하여 Loadcell Data 수집
            if (Timeout(DB.WorkParam.delay.SamplingStartDelay))
                return StepResult.Next;

            return StepResult.Pending;
        }

        public StepResult MOT_OD_CONTACT_START() // 샘플링 시작 딜레이
        {
            TimeStart(); // 아래의 Contact Delay TimeStart
            SetMsg($"Contact Delay...");
            return StepResult.Next;
        }

        public StepResult MOT_OD_CONTACT_DELAY() // 컨택트 시간 : 이 동안은 Probe가 누르고 있는다.
        {
            // New Loadcell Data Get
            GetLoadcellData();

            // Loadcell Data 수집
            _loadcellSamplingArray_1.Add(LoadcellModel_1.Data);
            _loadcellSamplingArray_2.Add(LoadcellModel_2.Data);
            _loadcellContactArray_1.Add(LoadcellModel_1.Data);
            _loadcellContactArray_2.Add(LoadcellModel_2.Data);

            if (Timeout(WorkItem.Rcp.ContactTime))
                return StepResult.Next;

            return StepResult.Pending;
        }

        public StepResult ADDLOG()
        {
            // 아래는 전체 값의 Range이다.
            /*
            // Contact Time 동안의 Loadcell Data
            // 현재까지 담긴 Loadcell Data의 최대, 최소값
            double MaxVal_1 = _loadcellContactArray_1.Max() * (DB.WorkParam.loadcellRange.Max / 100);
            double MinVal_1 = _loadcellContactArray_1.Min() * (DB.WorkParam.loadcellRange.Min / 100);
            double MaxVal_2 = _loadcellContactArray_2.Max() * (DB.WorkParam.loadcellRange.Max / 100);
            double MinVal_2 = _loadcellContactArray_2.Min() * (DB.WorkParam.loadcellRange.Min / 100);

            // Range 범위 안의 값을 평균 계산
            double Avg_1 = _loadcellContactArray_1.Where(val => val >= MinVal_1 && val <= MaxVal_1).Average();
            double Avg_2 = _loadcellContactArray_2.Where(val => val >= MinVal_2 && val <= MaxVal_2).Average();

            // Sampling Delay를 포함한 Loadcell Data
            MaxVal_1 = _loadcellSamplingArray_1.Max() * (DB.WorkParam.loadcellRange.Max / 100);
            MinVal_1 = _loadcellSamplingArray_1.Min() * (DB.WorkParam.loadcellRange.Min / 100);
            MaxVal_2 = _loadcellSamplingArray_2.Max() * (DB.WorkParam.loadcellRange.Max / 100);
            MinVal_2 = _loadcellSamplingArray_2.Min() * (DB.WorkParam.loadcellRange.Min / 100);

            double Tot_1 = _loadcellSamplingArray_1.Where(val => val >= MinVal_1 && val <= MaxVal_1).Average();
            double Tot_2 = _loadcellSamplingArray_2.Where(val => val >= MinVal_2 && val <= MaxVal_2).Average();
            */

            // 모든 값의 평균편차만을 Range로 한다. (Contact Array를 대상으로만 한다.)
            if (_loadcellContactArray_1.Count > 3) // 값이 3개 이상일 때만
                for (int i = 0; i < (_loadcellContactArray_1.Count - 1) * (DB.WorkParam.loadcellRange.Min / 100); i++)
                    _loadcellContactArray_1.RemoveAt(0); // 맨 앞의 값을 Min 비율만큼 제거한다.
                for (int i = (_loadcellContactArray_1.Count - 1); i > (_loadcellContactArray_1.Count - 1) * (DB.WorkParam.loadcellRange.Max / 100); i--)
                    _loadcellContactArray_1.RemoveAt(_loadcellContactArray_1.Count - 1); // 맨 뒤의 값을 Max 비율만큼 제거한다.

            if (_loadcellContactArray_2.Count > 3) // 값이 3개 이상일 때만
                for (int i = 0; i < (_loadcellContactArray_2.Count - 1) * (DB.WorkParam.loadcellRange.Min / 100); i++)
                    _loadcellContactArray_2.RemoveAt(0); // 맨 앞의 값을 Min 비율만큼 제거한다.
            for (int i = (_loadcellContactArray_2.Count - 1); i > (_loadcellContactArray_2.Count - 1) * (DB.WorkParam.loadcellRange.Max / 100); i--)
                _loadcellContactArray_2.RemoveAt(_loadcellContactArray_2.Count - 1); // 맨 뒤의 값을 Max 비율만큼 제거한다.

            // Range 범위 안의 값을 평균 계산
            double Avg_1 = _loadcellContactArray_1.Average();
            double Avg_2 = _loadcellContactArray_2.Average();
            double Tot_1 = _loadcellSamplingArray_1.Average();
            double Tot_2 = _loadcellSamplingArray_2.Average();

            // Excel 데이터 List 입력한다.
            ExcelManager.Instance.ExcelData.AddValue(Mot[eAxis.AXIS_Z].ComPosition, Step_OD,
                (double)Avg_1, (double)Avg_2, (double)Tot_1, (double)Tot_2,
                (double)_loadcellContactArray_1[_loadcellContactArray_1.Count - 1], (double)_loadcellContactArray_2[_loadcellContactArray_2.Count - 1]);

            // List 클리어
            _loadcellSamplingArray_1.Clear();
            _loadcellSamplingArray_2.Clear();
            _loadcellContactArray_1.Clear();
            _loadcellContactArray_2.Clear();

            return StepResult.Next;
        }

        public StepResult INC_STEP()
        {
            // 현재 Step 반복 횟수가 레시피의 Step 반복횟수를 충족하면
            if (StepRptCnt >= WorkItem.Rcp.StepRepeat)
            {
                StepRptCnt = 0; // Step 반복 횟수 초기화

                SetMsg($"OD Step Count Reset.");
                return StepResult.Next; // OD 증가하러...
            }
            else // Step 반복 횟수가 남았으면
            {
                StepRptCnt++;

                SetMsg($"OD Step Count Up : {StepRptCnt}");
                Step.Change(MainStep.CHECK_REPEAT_MODE); // Count 증가 생략
                return StepResult.Change; // 복귀이송 or 다음 Step 진행
            }
        }

        public StepResult INC_OD() // Excel Output 후 OD(Step) 증가
        {
            if (Step_OD_Idx >= Step_OD_List.Count) // 모든 Step 단계를 진행했으면
            {
                Step_OD_Idx = 0; // Step 단계 처음으로
                Step_OD = Step_OD_List[Step_OD_Idx]; // Next Step OD 입력

                SetMsg($"Step OD Index Reset.");
                return StepResult.Next;
            }
            else // 더 진행해야 할 Step 단계가 있으면
            {
                Step_OD = Step_OD_List[Step_OD_Idx]; // Next Step OD 입력
                Step_OD_Idx++; // Step OD Index 증가

                SetMsg($"Step OD Index Up : {Step_OD_Idx}");
                Step.Change(MainStep.CHECK_REPEAT_MODE); // Count 증가 생략
                return StepResult.Change; // 복귀이송 or 다음 Step 진행
            } 
        }

        public StepResult INC_CNT()
        {
            // 복귀이송 전 카운트를 증가시킨다.
            _pinForceCount++;

            return StepResult.Next;
        }

        public StepResult CHECK_REPEAT_MODE()
        {
            switch(WorkItem.Rcp.RepeatMethod)
            {
                // Sequencial Mode이면 복귀이송을 진행한다.
                case RepeatType.Sequential:
                    {
                        SetMsg($"Repeat Mode : Sequential");
                        /*
                        Step.Change(MainStep.MOT_RETURN_MOVE_ENTER);
                        return StepResult.Change;
                        */
                        return StepResult.Next;
                    }
                // Zero Mode이면 복귀이송을 진행하지 않고 계속 내려간다.
                case RepeatType.Zero:
                    {
                        SetMsg($"Repeat Mode : Zero");
                        Step.Change(MainStep.MOT_CHECK_REPEATCOUNT);
                        return StepResult.Change;
                    }
            }

            // 기본값은 원점 이송
            SetMsg($"Repeat Mode : Not Set! => Default = Sequential");
            Step.Change(MainStep.MOT_RETURN_MOVE_ENTER);
            return StepResult.Change;
        }

        public StepResult MOT_RETURN_MOVE_ENTER() // 시작점으로 되돌아간다.
        {
            // Excel Raw 데이터 수집은 여기서 종료한다.

            // 원점 복귀; ReleaseUse를 사용하면 LastReleaseMoveDist 가산, 사용하지 않으면 원점 그대로 사용
            var curPos = (WorkItem.Rcp.ReleaseUse) ? _pinContactOriginPos + (AutoRcp.LastReleaseMoveDist / 1000 /* um */) : _pinContactOriginPos;

            SetMsg($"시작점 복귀 이송");
            MotSet(eAxis.AXIS_Z, curPos, WorkItem.Rcp.WorkSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult MOT_RETURN_MOVE_POLLING()
        {
            var time = DB.WorkParam.errorTimeout.MotionErrorTimeout;

            // 폴링 중에 AutoContactAlarmDepth Check를 해야하는데, 이 부분 확인 필요

            SetMsg($"시작점 복귀 도착");
            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult MOT_RETURN_MOVE_DELAY() // 모션 딜레이
        {
            SetMsg($"Motion Delay...");
            Sleep(DB.WorkParam.delay.MotionDelay);
            // Sleep(DB.WorkParam.delay.SamplingStartDelay);

            // 복귀이송 다했으면 카운트 체크하러...
            SetMsg($"시작점 복귀 완료");
            return StepResult.Next;
        }

        public StepResult MOT_CHECK_REPEATCOUNT()
        {
            // 설정한 횟수를 모두 완료했으면
            if (_pinForceCount > WorkItem.Rcp.RepeatCount)
            {
                // 현재 위치가 End OD 위치인지 확인한다.
                if (Mot[eAxis.AXIS_Z].ComPosition >= -WorkItem.Rcp.End) // 종료점보다 프로브가 높게 위치하면
                {
                    SetMsg($"END O/D 확인, 반복 완료.");
                    return StepResult.Next; // 완료하러
                }
                else // 현재 Probe가 종료점보다 낮을 경우
                {
                    SetMsg($"END O/D 위치 초과, STEP 이송 계속.");
                    Step.Change(MainStep.MOT_OD_MOVE_ENTER);
                    return StepResult.Change;
                }
            }
            else
            {
                SetMsg($"반복 진행");
                Step.Change(MainStep.MOT_OD_MOVE_ENTER);
                return StepResult.Change;
            }
        }

        public StepResult CHECK_RELEASE_USE()
        {
            if (WorkItem.Rcp.ReleaseUse)
            {
                SetMsg($"완료 이송 실행");
                return StepResult.Next;
            }
            else
            {
                SetMsg($"완료 이송 생략");
                Step.Change(MainStep.STOP_CAMERA);
                return StepResult.Change;
            }
        }

        public StepResult RELEASE_USE_MOVE_ENTER()
        {
            // 현재 위치 + Release 지점 조절 (ReleaseDistance 는 위로 가는 방향)
            var curPos = Mot[eAxis.AXIS_Z].ComPosition + (WorkItem.Rcp.ReleaseDist / 1000 /* um */);

            SetMsg("Release Move 시작");
            MotSet(eAxis.AXIS_Z, curPos, _workSpeed);

            return MotEnter(eAxis.AXIS_Z);
        }

        public StepResult RELEASE_USE_MOVE_POLLING()
        {
            var time = WorkItem.Rcp.ReleaseTime;

            // Last Release 지점 이송 도착 대기
            return MotPoliing(WorkItem, time, eAxis.AXIS_Z);
        }

        public StepResult STOP_CAMERA()
        {
            VisionDisplayViewModel.Instance.StopCamera();
            SetMsg("Camera Stop");
            return StepResult.Next;
        }

        public StepResult SAVE_DATA()
        {
            ExcelManager.Instance.ExcelRawData.SaveExcel(); // 1차 Excel Save
            ExcelManager.Instance.ExcelData.SaveExcel(); // 2차 Excel Save
            SetMsg("Excel Save");
            return StepResult.Next;
        }

        public StepResult END()
        {
            SetMsg("Process 종료");

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
