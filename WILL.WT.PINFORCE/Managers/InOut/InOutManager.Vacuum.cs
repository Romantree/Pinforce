namespace WILL.WT.PINFORCE.Managers
{
    public partial class InOutManager
    {
        private VacuumStatus GetVacuum(bool vacuum, bool vac, bool vnt)
        {
            if (vacuum == true) return VacuumStatus.Vacuum;
            if (vnt == true && vacuum == false) return VacuumStatus.Vent;

            if (vac == true) return VacuumStatus.VacuumProcess;
            if (vnt == true) return VacuumStatus.VentProcess;

            return vacuum == false ? VacuumStatus.ATM : VacuumStatus.Error;
        }

        private void SetVacuum(string vacuum, string vac, string vnt, VacuumSetting set)
        {
            switch (set)
            {
                case VacuumSetting.Vacuum:
                    {
                        this.WriteY(false, vnt);
                        this.WriteY(true, vac);

                        if (AP.IsSim) this.WriteX(true, vacuum);
                    }
                    break;
                case VacuumSetting.Vent:
                    {
                        this.WriteY(false, vac);
                        this.WriteY(true, vnt);

                        if (AP.IsSim) this.WriteX(false, vacuum);
                    }
                    break;
                case VacuumSetting.Off:
                    {
                        this.WriteY(false, vac);
                        this.WriteY(false, vnt);

                        if (AP.IsSim) this.WriteX(false, vacuum);
                    }
                    break;
            }
        }
    }
}