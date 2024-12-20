using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TS.FW.Dac.Alarm;
using WILL.WT.PINFORCE.Views.Pages.Alarm;
using TS.FW.Utils;
using TS.FW.Wpf.v2.Helpers;
using TS.FW;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Alarm
{
    public class AlarmHistoryViewModel : IAlarmViewModel
    {
        private const int MAX_COUNT = 17;

        private readonly FrameworkElement view = new AlarmHistoryView();
        private Dictionary<int, List<AlarmHistoryModel>> total = new Dictionary<int, List<AlarmHistoryModel>>();
        private int curPage = 1;
        private int maxPage = 1;

        public override int No => 1;

        public override string Name => "History";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_calendar_year", "Icons"] as Visual;

        public DateTime StartTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public DateTime EndTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public string Page { get => this.GetValue<string>(); set => this.SetValue(value); }

        public ObservableCollection<AlarmHistoryModel> AlarmList { get; set; } = new ObservableCollection<AlarmHistoryModel>();

        public override void Init()
        {
            try
            {
                base.Init();

                this.StartTime = DateTime.Now.Date;
                this.EndTime = DateTime.Now.Date;
                this.Page = "1 / 1";
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
                    case "Search":
                        {
                            UpdateAlarm();                            
                        }
                        break;
                    case "Prev":
                        {
                            UpdateAlarm(curPage - 1);
                        }
                        break;
                    case "Next":
                        {
                            UpdateAlarm(curPage + 1);
                        }
                        break;
                    case "First":
                        {
                            UpdateAlarm(1);
                        }
                        break;
                    case "Last":
                        {
                            UpdateAlarm(maxPage);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateAlarm()
        {
            if(this.StartTime > this.EndTime)
            {
                AP.System.InterlockMsgEvent("검색 시간 설정이 잘못 되었습니다.");
                return;
            }

            this.total.Clear();

            var list = AP.Alarm.GetAlarmHistoryList(this.StartTime, this.EndTime.AddDays(1).AddSeconds(-1));

            foreach (var alarm in list.ToPageList(MAX_COUNT).Select((t, i) => new { i, Model = t.Select(x => (AlarmHistoryModel)x).ToList() }))
            {
                this.total[alarm.i] = alarm.Model;
            }

            this.curPage = 1;
            this.maxPage = this.total.Count;

            UpdateAlarm(curPage);
        }

        private void UpdateAlarm(int page)
        {
            if (page < 1 || page > maxPage) return;

            this.AlarmList.Clear();

            foreach (var item in total[page - 1])
            {
                this.AlarmList.Add(item);
            }

            this.curPage = page;

            this.Page = $"{this.curPage} / {this.maxPage}";
        }
    }

    public class AlarmHistoryModel
    {
        public string PostTime { get; set; }

        public string ClearTime { get; set; }

        public eAlarm Alarm { get; set; }

        public AlarmLevel Level { get; set; }

        public static implicit operator AlarmHistoryModel(AlarmHistoryEntity item)
        {
            if (item == null) return null;

            return new AlarmHistoryModel()
            {
                Alarm = item.ToType<eAlarm>(),
                Level = (AlarmLevel)item.AlarmLevel,

                PostTime = item.PostTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ClearTime = item.ClearTime.HasValue ? item.ClearTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
            };
        }
    }
}
