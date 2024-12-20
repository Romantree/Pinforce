using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using TS.FW.Dac.Alarm;
using WILL.WT.PINFORCE.Views.Pages.Alarm;
using TS.FW.Wpf.v2.Core;
using TS.FW.Wpf.v2.Helpers;
using TS.FW;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Alarm
{
    delegate void InsertAlarmHandler(AlarmModel item);
    delegate void DeleteAlarmAlarmHandler(eAlarm item);

    public class AlarmMainViewModel : IAlarmViewModel
    {
        private readonly FrameworkElement view = new AlarmMainView();

        private InsertAlarmHandler OnInsertAlarmHandler;
        private DeleteAlarmAlarmHandler OnDeleteAlarmAlarmHandler;

        public override int No => 0;

        public override string Name => "List";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_alert", "Icons"] as Visual;

        public ObservableCollection<AlarmModel> AlarmList { get; set; } = new ObservableCollection<AlarmModel>();

        public override void Init()
        {
            try
            {
                base.Init();

                this.OnInsertAlarmHandler = this.InsertAlarm;
                this.OnDeleteAlarmAlarmHandler = this.DeleteAlarm;

                AP.Alarm.OnAlarmPostEvent += Alarm_OnAlarmPostEvent;
                AP.Alarm.OnAlarmClearEvent += Alarm_OnAlarmClearEvent;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Show()
        {
            try
            {
                base.Show();

                this.UpdateAlarm();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Alarm_OnAlarmPostEvent(object sender, AlarmData<eAlarm> e)
        {
            try
            {
                if (this.view.IsVisible == false) return;

                IViewModel.Dispatcher.Invoke(this.OnInsertAlarmHandler, (AlarmModel)e);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Alarm_OnAlarmClearEvent(object sender, eAlarm e)
        {
            try
            {
                if (this.view.IsVisible == false) return;

                IViewModel.Dispatcher.Invoke(this.OnDeleteAlarmAlarmHandler, e);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnCommand(object parameter)
        {
            try
            {
                switch (parameter as string)
                {
                    case "Retry":
                        {
                            this.AlarmRecovery();
                        }
                        break;
                    case "Stop":
                        {
                            if (AP.Alarm.IsAlarmNotLight) AP.ProcessStop();

                            this.AlarmRecovery();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void AlarmRecovery()
        {
            foreach (var item in this.AlarmList.ToList())
            {
                AP.Alarm.AlarmRecovery(item.Alarm);
            }

            this.AlarmList.Clear();
        }

        private void UpdateAlarm()
        {
            lock (this.AlarmList)
            {
                this.AlarmList.Clear();

                var list = AP.Alarm.GetPostAlarm();

                foreach (var item in list)
                {
                    if (item == null) continue;

                    this.InsertAlarm(item);
                }
            }
        }

        private void InsertAlarm(AlarmModel item)
        {
            try
            {
                lock (this.AlarmList)
                {
                    var temp = this.AlarmList.FirstOrDefault(t => t.Alarm == item.Alarm);
                    if (temp != null) return;

                    this.AlarmList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void DeleteAlarm(eAlarm item)
        {
            try
            {
                lock (this.AlarmList)
                {
                    var temp = this.AlarmList.FirstOrDefault(t => t.Alarm == item);
                    if (temp == null) return;

                    this.AlarmList.Remove(temp);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }

    public class AlarmModel
    {
        public string Time { get; set; }

        public int ID { get; set; }

        public eAlarm Alarm { get; set; }

        public AlarmLevel Level { get; set; }

        public static implicit operator AlarmModel(AlarmData<eAlarm> item)
        {
            if (item == null) return null;

            return new AlarmModel()
            {
                Time = item.Time.ToString("HH:mm:ss"),
                ID = (int)item.Alarm,
                Alarm = item.Alarm,
                Level = item.Level,
            };
        }
    }
}
