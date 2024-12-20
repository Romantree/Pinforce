using WILL.WT.PINFORCE.Process;
using WILL.WT.PINFORCE.Process.Init;
using WILL.WT.PINFORCE.Process.Saferty;
using System;
using TS.FW;
using WILL.WT.PINFORCE.Models.Recipe;
using WILL.WT.PINFORCE.Process.Work;
using TS.FW.Diagnostics;
using WILL.WT.PINFORCE.Process.Work.Main;

namespace WILL.WT.PINFORCE.Managers
{
    public class ProcManager
    {
        public readonly SafertyProc Saferty = new SafertyProc();
        public readonly InitProc Init = new InitProc();
        public readonly MainProc Main = new MainProc();

        public  BackgroundTimer _bg = new BackgroundTimer();
        public bool IsAuto { get; set; } = false;

        public bool IsInit => Init.IsInit;

        // Process는 IUnitProcess를 상속받았다. IsBusy 등 있음
        public bool IsBusy => Init.IsBusy  || Main.IsBusy;

        public bool ProcessCheck(bool isReady = true)
        {
            if (AP.Alarm.IsAlarmNotLight == true)
            {
                AP.System.InterlockMsgEvent("알람 해제 후 재시도 해주세요.");
                return true;
            }

            if (this.IsInit == false)
            {
                AP.System.InterlockMsgEvent("프로세스 초기화 후 재시도 해주세요.");
                return true;
            }

            if (this.IsBusy == true)
            {
                AP.System.InterlockMsgEvent("프로세스 정지 후 재시도 해주세요.");
                return true;
            }

            return false;
        }

        public void Start(MainRecipeModel rcp)
        {
            if (ProcessCheck()) return;

            // DemoProcess를 실행한다.
            this.Main.SetWorkItem(new WorkItem(rcp));
            this.Main.Start();

        }

        public void Reset()
        {
            try
            {
                Init.IsInit = false;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
        public void StartAuto(MainRecipeModel rcp)
        {

            try
            {
                if (ProcessCheck()) return;

                // var ready = DB.WorkParam.InitZ;

        

                // this.motionProcess.SetWorkItem(new WorkItem(rcp));
                // this.motionProcess.Start();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
        public void Abort()
        {
            try
            {
                Init.Stop();

                IProcessTimer.Abort();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }



        //public void StartAxisZMove(MainRecipeModel rcp)
        //{
        //    try
        //    {
        //        int rcpcount = rcp.RepeatCount;

        //        if (rcpcount <= 0)
        //        {
        //            AP.System.InterlockCheckEvent("reapeatcount가 설정되지 않았거나 0 입니다. ");
        //            return;
        //            ;
        //        }
        //        var Info = DB.WorkParam.InitZ;
        //        var readyposition = Info.Position;
        //        var currentposition = Info.Position < readyposition ? readyposition : Info.Position;
        //        double movedistance = 10; //이동 거리 

        //        bool flag = true; //성공 혹은 실패 확인 플래그

        //        Logger.Write(this, $"{rcpcount} 횟수");

        //        for (int i = 0; i < rcpcount; i++)
        //        {
        //            Debug.WriteLine($"{i}번째 이동");


        //            if (currentposition > readyposition + movedistance)
        //            {
        //                AP.Device[eAxis.AXIS_Z].MoveABS(readyposition - movedistance);
        //            }
        //            else
        //            {
        //                AP.System.InterlockCheckEvent("위치를 벗어나 아래로 이동할 수 없습니다.");
        //                flag = false; //이동 실패.
        //                break;
        //            }

        //            if (currentposition < readyposition + movedistance)
        //            {
        //                AP.Device[eAxis.AXIS_Z].MoveABS(readyposition + movedistance);
        //            }
        //            else
        //            {
        //                AP.System.InterlockCheckEvent("위치를 벗어나 위로 이동할 수 없습니다.");
        //                flag = false; //이동 실패.
        //                break;

        //            }

        //        }
        //        if (flag)
        //        {
        //            AP.System.InterlockCheckEvent("z axis 이동 완료");

        //        }
        //        else
        //        {
        //            AP.System.InterlockCheckEvent("작동중 에러가 발생.");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Write(this, ex);
        //    }
        //}

 
    
}
