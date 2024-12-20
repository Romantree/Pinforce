using System;
using System.Collections.Generic;
using System.Diagnostics;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models.Axis
{
    public class MotionModel : IModel
    {
        public MotionModel()
        {
            Debug.WriteLine("MotionModel 생성됨");
        }

        public List<AxisModel> Axis { get; set; } = new List<AxisModel>();

        public AxisModel SelectAxis { get => this.GetValue<AxisModel>(); set => this.SetValue(value); }

        public NormalCommand OnSelectAxisCmd => new NormalCommand(SelectAxisCmd);

        public void Update() => this.Axis.ForEach(t => t.Update());

        public void SelectAxisCmd(object param)
        {
            try
            {
                if (this.SelectAxis != null) this.SelectAxis.IsSeleted = false;

                this.SelectAxis = (param as AxisModel);

                if (this.SelectAxis != null)
                {
                    this.SelectAxis.IsSeleted = true;
                    this.SelectAxis.UpdateLimit();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
