using WILL.WT.PINFORCE.Process;
using System;
using TS.FW;
using TS.FW.Dac.Alarm;

namespace WILL.WT.PINFORCE.Managers
{
    public class AlarmManager : IAlarmBase<eAlarm>
    {
        public override void AlarmPostAciton(AlarmData<eAlarm> alarm)
        {
			try
			{
				//AP.IO.BUZZER_01 = true;

				switch (alarm.Level)
				{
					case AlarmLevel.CycleStop:
						{

						}
						break;
					case AlarmLevel.Heavy:
						{
							//AP.Device.StopEx();
                            IProcessTimer.PauseAll();
                        }
						break;
					case AlarmLevel.Critical:
						{
							AP.Proc.Reset();
                            AP.ProcessStop();
                        }
						break;
				}
			}
			catch (Exception ex)
			{
				Logger.Write(this, ex);
			}
        }

        public override void AlarmClearAction()
        {
			try
			{
                if (this.IsAlarmNotLight) return;

				IProcessTimer.ResumeAll();
            }
			catch (Exception ex)
			{
				Logger.Write(this, ex);
			}
        }
    }
}
