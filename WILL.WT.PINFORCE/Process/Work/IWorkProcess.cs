using WILL.WT.PINFORCE.Models.Recipe;
using System;
using TS.FW;
using System.Drawing.Drawing2D;

namespace WILL.WT.PINFORCE.Process.Work
{
    public abstract class IWorkProcess<T> : IUnitProcess<T> where T : struct, IConvertible
    {
        public WorkItem WorkItem { get; private set; } = null;

        protected MainRecipeModel Rcp => WorkItem.Rcp;

        protected IWorkProcess() : base(true) { }

        public void SetWorkItem(WorkItem item)
        {
            lock (this)
            {
                this.WorkItem = item;
            }
        }

        public void ClearWorkItem()
        {
            this.SetWorkItem(null);
        }

        protected override StepResult ExcuteProcess()
        {
            lock (this)
            {
                if (this.WorkItem == null) return StepResult.Pending;

                return base.ExcuteProcess();
            }
        }

        protected override void Finish()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }

        public override void Stop()
        {
            try
            {
                this.ClearWorkItem();
                base.Stop();
            }
            catch (Exception ex)
            {
                Logger.Write(this, ex);
            }
        }
    }

    public class WorkItem
    {
        public WorkMode Mode { get; private set; }

        public MainRecipeModel Rcp { get; set; }

        public WorkItem(MainRecipeModel rcp)
        {
            this.Rcp = rcp;
        }
        public enum WorkMode
        {
            AUTO,
        }
    }
}
