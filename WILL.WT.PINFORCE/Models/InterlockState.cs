using WILL.WT.PINFORCE.Models.InOut;
using System;
using System.Collections.Generic;
using TS.FW;

namespace WILL.WT.PINFORCE.Models
{
    public class InterlockState
    {
        private readonly List<InStateModel> _updateList = new List<InStateModel>();

        public InStateModel X_TEST_01 { get; set; } = new InStateModel();
        public InStateModel X_TEST_02 { get; set; } = new InStateModel();

        public void Init()
        {
            try
            {
                foreach (var info in this.GetType().GetProperties())
                {
                    if (info.PropertyType != typeof(InStateModel)) continue;

                    var item = info.GetValue(this) as InStateModel;
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
