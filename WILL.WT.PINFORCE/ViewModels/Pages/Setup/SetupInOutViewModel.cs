using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WILL.WT.PINFORCE.Managers;
using WILL.WT.PINFORCE.Models.InOut;
using WILL.WT.PINFORCE.Views.Pages.Setup;
using TS.FW;
using TS.FW.Utils;
using TS.FW.Wpf.v2.Core;
using TS.FW.Wpf.v2.Helpers;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Setup
{
    // I/O 쓰지 않아 주석처리
    /*
    public class SetupInOutViewModel : ISetupViewModel
    {
        private readonly FrameworkElement view = new SetupInOutView();

        private Dictionary<int, InOutModel[]> inList = new Dictionary<int, InOutModel[]>();
        private Dictionary<int, InOutModel[]> outList = new Dictionary<int, InOutModel[]>();

        private int inPage = 0;
        private int outPage = 0;
        private int inMaxPage = 0;
        private int outMaxPage = 0;

        private int INOUT_COUNT => InOutManager.INOUT_COUNT;

        public override int No => 2;

        public override string Name => "I/O";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_source_fork", "Icons"] as Visual;

        public InOutModel[] In { get => this.GetValue<InOutModel[]>(); set => this.SetValue(value); }

        public InOutModel[] Out { get => this.GetValue<InOutModel[]>(); set => this.SetValue(value); }

        public NormalCommand OnTypeChangedCmd => new NormalCommand(OnTypeChanged);

        public NormalCommand OnDataOnOffCmd => new NormalCommand(OnDataOnOff);

        public override void Init()
        {
            try
            {
                base.Init();

                this.InitPage();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Update()
        {
            try
            {
                base.Update();

                this.UpdateIn();
                this.UpdateOut();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnCommand(object commandParameter)
        {
            try
            {
                switch (commandParameter as string)
                {
                    case "IN_P":
                        {
                            if (this.inPage == 0) return;

                            this.inPage--;
                            this.UpdateInPage();
                        }
                        break;
                    case "IN_N":
                        {
                            if (this.inPage == this.inMaxPage) return;

                            this.inPage++;
                            this.UpdateInPage();
                        }
                        break;
                    case "OUT_P":
                        {
                            if (this.outPage == 0) return;

                            this.outPage--;
                            this.UpdateOutPage();
                        }
                        break;
                    case "OUT_N":
                        {
                            if (this.outPage == this.outMaxPage) return;

                            this.outPage++;
                            this.UpdateOutPage();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void InitPage()
        {
            try
            {
                foreach (var item in this.ToList(AP.IO.CreateIOList(eIOType.IN, 0, INOUT_COUNT), AP.IO.inList).ToPageList(32))
                {
                    this.inList.Add(inPage++, item.ToArray());
                }

                this.inMaxPage = inPage - 2;
                this.inPage = 0;
                this.UpdateInPage();

                foreach (var item in this.ToList(AP.IO.CreateIOList(eIOType.OUT, INOUT_COUNT + 1, INOUT_COUNT), AP.IO.outList).ToPageList(32))
                {
                    this.outList.Add(outPage++, item.ToArray());
                }

                this.outMaxPage = outPage - 2;
                this.outPage = 0;
                this.UpdateOutPage();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateInPage()
        {
            try
            {
                lock (this.inList)
                {
                    this.In = this.inList[inPage];
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateOutPage()
        {
            try
            {
                lock (this.outList)
                {
                    this.Out = this.outList[outPage];
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateIn()
        {
            try
            {
                lock (this.inList)
                {
                    foreach (var item in this.In)
                    {
                        item.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void UpdateOut()
        {
            try
            {
                lock (this.outList)
                {
                    foreach (var item in this.Out)
                    {
                        item.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void OnTypeChanged(object param)
        {
            try
            {
                var model = param as InOutModel;
                if (model == null || string.IsNullOrEmpty(model.Name)) return;

                model.data.SignalType = model.IsAType ? eSignalType.B : eSignalType.A;

                AP.IO.CreateFile();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void OnDataOnOff(object param)
        {
            try
            {
                var model = param as InOutModel;
                if (model == null || string.IsNullOrEmpty(model.Name)) return;

                AP.IO.WriteY(!model.OnOff, model.Key);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public IEnumerable<InOutModel> ToList(List<IOData> aList, Dictionary<string, IOData> bList)
        {
            foreach (var item in aList.GroupJoin(bList, a => a.Address, b => b.Value.Address, (a, b) => new { a, b }))
            {
                var b = item.b.FirstOrDefault();
                if (b.Value != null) yield return new InOutModel(b.Key, b.Value);
                else yield return item.a;
            }
        }
    }
    */
}
