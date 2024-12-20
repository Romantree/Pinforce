using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TS.FW;
using TS.FW.Wpf.v2.Helpers;
using WILL.WT.PINFORCE.Models.Setup;
using WILL.WT.PINFORCE.Views.Pages.Setup;

namespace WILL.WT.PINFORCE.ViewModels.Pages.Setup
{
    // Setup (Main)Page -> Loadcell (Sub) Page 정의
    public class SetupLoadcellViewModel : ISetupViewModel
    {
        private readonly FrameworkElement view = new SetupLoadcellView();

        public override int No => 2;

        public override string Name => "Loadcell";

        public override FrameworkElement View => view;

        public override Visual Icon => ResourceHelper.Ins["appbar_scale_unbalanced", "Icons"] as Visual;

        // Binding Data => DataModel

        public LoadcellDataModel LoadcellDataModel_1 { get; set; } = new LoadcellDataModel();
        public LoadcellDataModel LoadcellDataModel_2 { get; set; } = new LoadcellDataModel();
        
        private Parity _selectedParity;
        private StopBits _selectedStopBits;
        private Handshake _selectedHandshake;
        public Parity SelectedParity
        {
            get => _selectedParity;
            set
            {
                _selectedParity = value;
                OnPropertyChanged(nameof(SelectedParity));
            }
        }

        public StopBits SelectedStopBits
        {
            get => _selectedStopBits;
            set
            {
                _selectedStopBits = value;
                OnPropertyChanged(nameof(SelectedStopBits));
            }
        }

        public Handshake SelectedHandshake
        {
            get => _selectedHandshake;
            set
            {
                _selectedHandshake = value;
                OnPropertyChanged(nameof(SelectedHandshake));
            }
        }

        public override void Show()
        {
            try
            {
                base.Show();

                this.Refresh();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void Refresh(bool isSave = false)
        {
            try
            {
                // AP.cs -> DB Class 내에 작성한 DB.cs 작성
                // DB.cs 구조는 Model과 동일하게
                if (isSave)
                {
                    // DB 저장
                    DB.DataCopyEx(this.LoadcellDataModel_1, DB.Loadcell.Loadcell_1);
                    DB.DataCopyEx(this.LoadcellDataModel_2, DB.Loadcell.Loadcell_2);
                }

                // DB 불러오기
                DB.DataCopy(this.LoadcellDataModel_1, DB.Loadcell.Loadcell_1);
                DB.DataCopy(this.LoadcellDataModel_1, DB.Loadcell.Loadcell_2);
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        protected override void OnCommand(object parameter)
        {
            // [Open, Close]_[1, 2]
            string[] param = (parameter as string).Split('_');
            try
            {
                switch (param[0]) // Open, Close
                {
                    case "Open":
                        this.Refresh(true);
                        // OpenSerial(param[1]);
                        break;
                    case "Close":
                        // CloseSerial(param[1]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        // ValueChanged가 있는지?
    }
}
