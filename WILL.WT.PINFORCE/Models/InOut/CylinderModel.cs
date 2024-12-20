using System;
using TS.FW.Wpf.v2.Core;
using TS.FW;

namespace WILL.WT.PINFORCE.Models.InOut
{
    public class CylinderModel : IModel
    {
        private readonly CyUnit _unit;
        private readonly string _up;
        private readonly string _down;

        public CylinderModel(CyUnit unit, string up = "UP", string down = "DOWN")
        {
            _unit = unit;

            _up = up;
            _down = down;

        }

        public string Status { get => this.GetValue<string>(); set => this.SetValue(value); }

        public bool Up { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public bool Down { get => this.GetValue<bool>(); set => this.SetValue(value); }

        public NormalCommand OnSetCylinderCmd => new NormalCommand(SetCylinderCmd);

        public void Update()
        {
            try
            {
                this.Up = AP.IO.GetCY(_unit, CyState.UP);
                this.Down = AP.IO.GetCY(_unit, CyState.DOWN);

                if (Up && Down)
                {
                    this.Status = "ERROR";
                }
                else if (Up)
                {
                    this.Status = _up;
                }
                else if (Down)
                {
                    this.Status = _down;
                }
                else
                {
                    this.Status = "None";
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        private void SetCylinderCmd(object param)
        {
            try
            {
                switch (param as string)
                {
                    case "UP":
                        {
                            AP.IO.SetCY(this._unit, CyState.UP);
                        }
                        break;
                    case "DOWN":
                        {
                            AP.IO.SetCY(this._unit, CyState.DOWN);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}