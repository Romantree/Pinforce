using System;
using WILL.WT.PINFORCE.Managers;
using TS.FW;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models
{
    public class NetworkState : IModel
    {
        public void Update()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }
}
