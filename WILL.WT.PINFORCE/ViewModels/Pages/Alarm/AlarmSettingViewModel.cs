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
using TS.FW.Wpf.v2.Core;
using TS.FW.Wpf.v2.Helpers;
using TS.FW;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Alarm
{
    public class AlarmSettingViewModel : IAlarmViewModel
    {
        private const int MAX_COUNT = 17;

        private readonly FrameworkElement view = new AlarmSettingView();
        private Dictionary<int, List<AlarmSettingModel>> total = new Dictionary<int, List<AlarmSettingModel>>();
        private int curPage = 1;
        private int maxPage = 1;

        public override int No => 2;

        public override string Name => "Setting";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_cogs", "Icons"] as Visual;

        public string Search { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool IsOpen { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string Page { get => this.GetValue<string>(); set => this.SetValue(value); }

        public ObservableCollection<AlarmSettingModel> AlarmList { get; set; } = new ObservableCollection<AlarmSettingModel>();

        public AlarmSettingModel Select { get => this.GetValue<AlarmSettingModel>(); set=>this.SetValue(value); }

        public AlarmUpdateModel Modify { get; set; } = new AlarmUpdateModel();

        public NormalCommand OnSaveCmd => new NormalCommand(SaveCmd);

        public NormalCommand OnCloseCmd => new NormalCommand(CloseCmd);

        public override void Init()
        {
            try
            {
                base.Init();

                this.view.IsVisibleChanged += View_IsVisibleChanged;

                UpdateAlarm();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                this.IsOpen = false;
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
                    case "Modify":
                        {
                            if (this.Select == null) return;

                            this.Modify.Update(this.Select);
                            this.IsOpen = !this.IsOpen;
                        }
                        break;
                    case "Search":
                        {
                            UpdateAlarm(this.Search);
                        }
                        break;
                    case "Refresh":
                        {
                            UpdateAlarm(curPage);
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

        private void SaveCmd(object param)
        {
            try
            {
                var model = param as AlarmUpdateModel;
                if (model == null) return;

                var item = this.total.SelectMany(t=>t.Value).FirstOrDefault(t=>t.Alarm == model.Alarm);
                if (item == null) return;

                item.Update(model);

                var res = AP.Alarm.UpdateAlarm(item);
                if (res == true) return;

                Logger.Write(this, res.Comment, Logger.LogEventLevel.Error);

                AP.System.InterlockMsgEvent("Alarm 데이터 저장에 실패하였습니다.");
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                this.IsOpen = false;
            }
        }

        private void CloseCmd(object param)
        {
            try
            {
                var model = param as AlarmUpdateModel;
                if (model == null) return;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
            finally
            {
                this.IsOpen = false;
            }
        }

        private void UpdateAlarm()
        {
            this.total.Clear();

            var list = AP.Alarm.GetAlarmList();

            foreach (var alarm in list.ToPageList(MAX_COUNT).Select((t, i) => new { i, Model = t.Select(x => (AlarmSettingModel)x).ToList() }))
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

        private void UpdateAlarm(string search)
        {
            if (string.IsNullOrWhiteSpace(search)) return;

            this.AlarmList.Clear();

            var text = search.ToUpper();

            foreach (var item in total.SelectMany(t => t.Value).Where(t => t.Alarm.ToString().ToUpper().Contains(text)))
            {
                this.AlarmList.Add(item);
            }
        }
    }

    public class AlarmSettingModel : IModel
    {
        public int ID { get; set; }

        public eAlarm Alarm { get; set; }

        public AlarmLevel Level { get => this.GetValue<AlarmLevel>(); set => this.SetValue(value); }

        public string Contetns { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string Action { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool Enable { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public void Update(AlarmUpdateModel model)
        {
            try
            {
                this.Level = model.ToAlarmLevel();
                this.Enable = model.Enable;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public static implicit operator AlarmSettingModel(AlarmData<eAlarm> item)
        {
            if (item == null) return null;

            return new AlarmSettingModel()
            {
                ID = (int)item.Alarm,
                Alarm = item.Alarm,
                Level = item.Level,
                Contetns = item.Contetns,
                Action = item.Action,
                Enable = item.Enable,
            };
        }

        public static implicit operator AlarmData<eAlarm>(AlarmSettingModel item)
        {
            if (item == null) return null;

            return new AlarmData<eAlarm>()
            {
                Alarm = item.Alarm,
                Level = item.Level,
                Contetns = item.Contetns,
                Action = item.Action,
                Enable = item.Enable,
            };
        }
    }

    public class AlarmUpdateModel : IModel
    {
        public eAlarm Alarm { get => this.GetValue<eAlarm>(); set => this.SetValue(value); }

        public bool Light { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool CycleStop { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Heavy { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Critical { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Enable { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public void Update(AlarmSettingModel model)
        {
            this.Alarm = model.Alarm;
            this.Enable = model.Enable;

            switch (model.Level)
            {
                case AlarmLevel.Light: this.Light = true; break;
                case AlarmLevel.CycleStop: this.CycleStop = true; break;
                case AlarmLevel.Heavy: this.Heavy = true; break;
                case AlarmLevel.Critical: this.Critical = true; break;
            }
        }

        public AlarmLevel ToAlarmLevel()
        {
            if (this.Light) return AlarmLevel.Light;
            if (this.CycleStop) return AlarmLevel.CycleStop;
            if (this.Heavy) return AlarmLevel.Heavy;

            return AlarmLevel.Critical;
        }
    }
}
