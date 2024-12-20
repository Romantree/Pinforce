using WILL.WT.PINFORCE.Models.InOut;
using System;
using System.Collections.Generic;
using TS.FW;

namespace WILL.WT.PINFORCE.Models
{
    public class EqpControl
    {
        private readonly List<OutControlModel> _updateList = new List<OutControlModel>();

        public OutControlModel TEST_01 { get; set; } = new OutControlModel();
        public OutControlModel TEST_02 { get; set; } = new OutControlModel();

        public void Init()
        {
            try
            {
                foreach (var info in this.GetType().GetProperties())
                {
                    if (info.PropertyType != typeof(OutControlModel)) continue;

                    var item = info.GetValue(this) as OutControlModel;
                    if (item.LoadData(info.Name) == false) continue;

                    _updateList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public void Update()
        {
            try
            {
                _updateList.ForEach(t => t.Update());
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
