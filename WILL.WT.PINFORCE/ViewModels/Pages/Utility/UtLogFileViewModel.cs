using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using WILL.WT.PINFORCE.Views.Pages.Utility;
using TS.FW;
using TS.FW.Wpf.v2.Core;
using TS.FW.Wpf.v2.Helpers;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Utility
{
    public class UtLogFileViewModel : IUtilViewModel
    {
        private readonly FrameworkElement view = new UtLogFileView();

        public override int No => 0;

        public override string Name => "Logger";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_calendar_year", "Icons"] as Visual;

        public DateTime StartTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public DateTime EndTime { get => this.GetValue<DateTime>(); set => this.SetValue(value); }

        public string LogMsg { get => this.GetValue<string>(); set => this.SetValue(value); }

        public string FileName { get => this.GetValue<string>(); set => this.SetValue(value); }

        public ObservableCollection<LogFileModel> LogList { get; set; } = new ObservableCollection<LogFileModel>();

        public NormalCommand OnLogSelectedCmd => new NormalCommand(LogSelectedCmd);

        public override void Init()
        {
            try
            {
                base.Init();

                this.StartTime = DateTime.Now.Date;
                this.EndTime = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateLog()
        {
            try
            {
                this.LogList.Clear();
                var total = (int)Math.Ceiling((this.EndTime - this.StartTime).TotalDays + 1);

                for (int i = 0; i < total; i++)
                {
                    var time = this.StartTime.AddDays(i);
                    var path = Path.Combine(Logger.RootPath, time.ToString("yyyy"), time.ToString("MM"), time.ToString("dd"));
                    if (Directory.Exists(path) == false) continue;

                    foreach (var file in Directory.EnumerateFiles(path, "*.log"))
                    {
                        if (File.Exists(file) == false) continue;

                        var name = Path.GetFileNameWithoutExtension(file);
                        var tiem = time.Date.ToString("yyyy-MM-dd");

                        this.LogList.Add(new LogFileModel()
                        {
                            FilePath = file,
                            Name = name,
                            Time = tiem,
                        });
                    }
                }
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
                    case "SAVE":
                        {
                            if (LogList.Count <= 0)
                            {
                                AP.System.InterlockMsgEvent("저장가능한 Log File이 없습니다.");
                                return;
                            }

                            var dig = new FolderBrowserDialog();
                            if (dig.ShowDialog() != DialogResult.OK) return;

                            foreach (var item in LogList)
                            {
                                var dest = Path.Combine(dig.SelectedPath, Path.GetFileName(item.FilePath));
                                if (File.Exists(dest)) File.Delete(dest);

                                File.Copy(item.FilePath, dest);
                            }
                        }
                        break;
                    case "SEARCH":
                        {
                            this.UpdateLog();
                        }
                        break;
                    case "FILTER":
                        {
                            if (string.IsNullOrWhiteSpace(this.FileName))
                            {
                                this.UpdateLog();
                            }
                            else
                            {
                                foreach (var item in this.LogList.ToList())
                                {
                                    if (item.Name.ToUpper().Contains(this.FileName.ToUpper())) continue;

                                    this.LogList.Remove(item);
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void LogSelectedCmd(object param)
        {
            try
            {
                var model = param as LogFileModel;
                if (model == null) return;

                if (File.Exists(model.FilePath) == false) return;

                this.LogMsg = File.ReadAllText(model.FilePath, Encoding.Default);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }

    public class LogFileModel
    {
        public string FilePath { get; set; }

        public string Name { get; set; }

        public string Time { get; set; }
    }
}
