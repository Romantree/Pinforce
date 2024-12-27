using System;
using System.Diagnostics;
using System.Windows;
using TS.FW;
using TS.FW.Device;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Controls.Motion;
using WILL.WT.PINFORCE.Models.Setup;

namespace WILL.WT.PINFORCE.Models.Axis
{
    public class AxisModel : IModel
    {
        protected readonly IAxis _axis;
        protected readonly IAxis _axisGantry;
        protected readonly bool _gantryCheck = false;

        private eAxis _AxisType => (eAxis)_axis.No;

        private eAxis _AxisGantryType => (eAxis)_axisGantry.No;

        public JogSpeedType JogSpeed { get => this.GetValue<JogSpeedType>(); set => this.SetValue(value); }

        // 필드를 속성으로 변경
        private int _selectedJogSpeed = 0;
        public int SelectedJogSpeed
        {
            get => _selectedJogSpeed;
            set
            {
                if (_selectedJogSpeed != value)
                {
                    _selectedJogSpeed = value;
                    OnPropertyChanged(nameof(SelectedJogSpeed));  // INotifyPropertyChanged 호출
                }
            }
        }

        public AxisModel(eAxis type)
        {
            this._axis = AP.Device[type];

            this.Name = this.ToName(type);
            this.GantryModel = false;

            this.State.Update(_axis);
            this.Limit.Update(_axis);

            // DB로부터 설정한 속도값 가져오기
            DB.DataCopy(this.speed, DB.Motion.speed);
        }

        public AxisModel(eAxis master, eAxis slave, bool gantryCheck = true) : this(master)
        {
            this._axisGantry = AP.Device[slave];
            this._gantryCheck = gantryCheck;
            this.GantryModel = true;

            // DB로부터 설정한 속도값 가져오기
            DB.DataCopy(this.speed, DB.Motion.speed);
        }

        public bool IsSeleted { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public string Name { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool GantryModel { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool IsGantry { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public AxisStateModel State { get; set; } = new AxisStateModel();

        public AxisLimitModel Limit { get; set; } = new AxisLimitModel();

        public NormalCommand OnSpeedSet => new NormalCommand(SpeedSet);

        public NormalCommand OnAxisCmd => new NormalCommand(AxisCmd);

        public NormalCommand OnAxisMoveCmd => new NormalCommand(AxisMoveCmd);

        public NormalCommand OnAxisStopCmd => new NormalCommand(AxisStopCmd);

        // 설정했던 Speed값 가져오기용
        public SpeedModel speed { get; set; } = new SpeedModel();

        public void Update()
        {
            try
            {
                this.State.Update(_axis);

                if (GantryModel)
                {
                    this.State.IsServoOn = this.State.IsServoOn && _axisGantry.IsServoOn;
                    this.State.IsAlarm = this.State.IsAlarm || _axisGantry.IsAlarm;
                    this.IsGantry = AP.Device.Gantry(_AxisType, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void UpdateLimit() => this.Limit.Update(_axis);

        private void SpeedSet(object param)
        {
            // public NormalCommand OnSpeedSet => new NormalCommand(SpeedSet); 에 의해 호출된다.
            try
            {
                switch (param as string)
                {
                    // NewAxisControl.xaml의 Button Param은 CommandParameter="SpeedSet" 이다.
                    case "SpeedSet":
                        {
                            // SpeedSetting.xaml
                            var view = new SpeedSettingViewModel();
                            if (view.Show() == false) return;

                            // 무언가 저장했었으면 속도값 다시 가져오기
                            Debug.WriteLine("값 다시가져옴");
                            DB.DataCopy(this.speed, DB.Motion.speed);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void AxisCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "Servo":
                        {
                            _axis.IsServoOn = !this.State.IsServoOn;
                            if (GantryModel) _axisGantry.IsServoOn = _axis.IsServoOn;
                        }
                        break;
                    case "Alarm":
                        {
                            _axis.ResetAlarm();
                            if (GantryModel) _axisGantry.ResetAlarm();
                        }
                        break;
                    case "Reset":
                        {
                            _axis.ResetPosition();
                            if (GantryModel) _axisGantry.ResetPosition();
                        }
                        break;
                    case "SetLimit":
                        {
                            this.Limit.SetLimit(_axis);
                        }
                        break;
                    case "UpdateLimit":
                        {
                            UpdateLimit();
                        }
                        break;
                    case "GantryOn":
                        {
                            if (GantryModel == false || State.IsBusy) return;

                            AP.Device.GantryEnable(_AxisType, _AxisGantryType);
                        }
                        break;
                    case "GantryOff":
                        {
                            if (GantryModel == false || State.IsBusy) return;

                            AP.Device.GantryDisable(_AxisType, _AxisGantryType);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void AxisMoveCmd(object param)
        {
            try
            {
                if (this.MoveCheck()) return;

                // 계수 테스트
                // double spd = State.Speed * this.speed.JogLowSpeed * 0.01;
                double jogSpd = DB.Motion.speed.RefSpeed;
                jogSpd =    (SelectedJogSpeed == (int)JogSpeedType.High) ? jogSpd * DB.Motion.speed.JogHighSpeed :
                            (SelectedJogSpeed == (int)JogSpeedType.Middle) ? jogSpd * DB.Motion.speed.JogMidSpeed : jogSpd * DB.Motion.speed.JogLowSpeed;
                jogSpd /= 100; // % 로 변환
                Debug.WriteLine(jogSpd);
                switch (param as string)
                {
                    case "HOME":
                        {
                            _axis.HomeAsync(out HomeAsyncResult result);
                        }
                        break;
                    case "JOG+":
                        {
                            if (this.PMoveCheck()) return;

                            _axis.MoveVEL(eDirection.Plus, jogSpd, State.Speed * 4, State.Speed * 4);
                        }
                        break;
                    case "JOG-":
                        {
                            if (this.NMoveCheck()) return;

                            _axis.MoveVEL(eDirection.Minus, jogSpd, State.Speed * 4, State.Speed * 4);
                        }
                        break;
                    case "REL+":
                        {
                            if (this.PMoveCheck()) return;

                            _axis.MoveREL(State.RelPos, jogSpd, State.Speed * 4, State.Speed * 4);
                        }
                        break;
                    case "REL-":
                        {
                            if (this.NMoveCheck()) return;

                            _axis.MoveREL(-State.RelPos, jogSpd, State.Speed * 4, State.Speed * 4);
                        }
                        break;
                    case "ABS":
                        {
                            if(State.AbsPos > State.ActPos)
                            {
                                if (this.PMoveCheck()) return;
                            }
                            else
                            {
                                if (this.NMoveCheck()) return;
                            }

                            _axis.MoveABS(State.AbsPos, jogSpd, State.Speed * 4, State.Speed * 4);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void AxisStopCmd(object param)
        {
            try
            {
                _axis.Stop();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private bool MoveCheck()
        {
            if (this.State.IsAlarm || this.State.IsBusy) return true;
            if (this.GantryModel && (_gantryCheck && IsGantry == false))
            {
                this.AxisCmd("GantryOn");
                return true;
            }

            return false;
        }

        private bool PMoveCheck()
        {
            return false;
        }

        private bool NMoveCheck()
        {
            return false;
        }

        protected string ToName(eAxis type)
        {
            switch (type)
            {

            }

            return $"{type}".Replace("_", " ");
        }
    }
}
