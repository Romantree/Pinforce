namespace WILL.WT.PINFORCE.Managers
{
    public partial class InOutManager
    {
        public bool GetCY(CyUnit unit, CyState state)
        {
            var on = $"X_{unit}_{state}";
            var off = $"X_{unit}_{state.ToRev()}";

            return this.ReadX(on) == true && this.ReadX(off) == false;
        }

        public void SetCY(CyUnit unit, CyState state, bool isProcess = false)
        {
            if (isProcess == false && InterlcokCheck(unit, state)) return;

            var on = $"{unit}_{state}";
            var off = $"{unit}_{state.ToRev()}";

            this.WriteY(false, off);
            this.WriteY(true, on);

            if (AP.IsSim)
            {
                this.WriteX(true, $"X_{on}");
                this.WriteX(false, $"X_{off}");

                System.Threading.Thread.Sleep(20);
            }
        }

        private bool InterlcokCheck(CyUnit unit, CyState state)
        {
            return false;
        }
    }
}
