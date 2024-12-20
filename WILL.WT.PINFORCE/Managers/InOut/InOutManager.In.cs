namespace WILL.WT.PINFORCE.Managers
{
    public partial class InOutManager
    {
        [IOSetting(IN, 0x000, "TEST #1")]
        public bool X_TEST_01 { get => this.ReadX(); set => this.WriteX(value); }

        [IOSetting(IN, 0x001, "TEST #2")]
        public bool X_TEST_02 { get => this.ReadX(); set => this.WriteX(value); }
    }
}
