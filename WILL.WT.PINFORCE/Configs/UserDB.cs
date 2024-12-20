using TS.FW.Dac.Cfg;
using TS.FW.Wpf.v2.Controls.InPut;

namespace WILL.WT.PINFORCE.Configs
{
    public class UserDB
    {
        public readonly OperatorUser Operator = new OperatorUser();
        public readonly EngineerUser Engineer = new EngineerUser();
        public readonly ManagerUser Manager = new ManagerUser();
        public readonly ProgrammerUser Programmer = new ProgrammerUser();
        public readonly LockUser Lock = new LockUser();

        public IUserData ToUserData(LoginMode mode)
        {
            switch (mode)
            {
                case LoginMode.Operator: return this.Operator;
                case LoginMode.Engineer: return this.Engineer;
                case LoginMode.Manager: return this.Manager;
                case LoginMode.Programmer: return this.Programmer;
                default: return this.Lock;
            }
        }
    }

    public abstract class IUserData : IConfigDb
    {
        public int Password { get => this.GetValueInt(); set => this.SetValue(value); }

        public bool Recipe { get => this.GetValueBool(); set => this.SetValue(value); }

        public bool Service { get => this.GetValueBool(); set => this.SetValue(value); }

        public bool Config { get => this.GetValueBool(); set => this.SetValue(value); }

        public bool Utilty { get => this.GetValueBool(); set => this.SetValue(value); }

        public bool Setup { get => this.GetValueBool(); set => this.SetValue(value); }

        public bool Alarm { get => this.GetValueBool(); set => this.SetValue(value); }
    }

    public class OperatorUser : IUserData { }

    public class EngineerUser : IUserData { }

    public class ManagerUser : IUserData { }

    public class ProgrammerUser : IUserData
    {
        public ProgrammerUser()
        {
            foreach (var info in this.GetType().GetProperties())
            {
                if (info.PropertyType != typeof(bool)) continue;
                info.SetValue(this, true);
            }
        }
    }

    public class LockUser : IUserData
    {
        public LockUser()
        {
            foreach (var info in this.GetType().GetProperties())
            {
                if (info.PropertyType != typeof(bool)) continue;
                info.SetValue(this, false);
            }
        }
    }

}
