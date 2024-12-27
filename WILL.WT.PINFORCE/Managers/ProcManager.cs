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
        public bool IsBusy => Init.IsBusy || Main.IsBusy;

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

            // WorkProcess를 실행한다.
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
}
