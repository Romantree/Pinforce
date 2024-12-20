namespace WILL.WT.PINFORCE.Managers
{
    public partial class InOutManager
    {
        public const int INOUT_COUNT = 2;
        public const int OA = INOUT_COUNT * 32;

        [IOSetting(OUT, OA + 0x000, "TEST #1")]
        public bool TEST_01 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x001, "TEST #2")]
        public bool TEST_02 { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x002, "TOWER LAMP RED")]
        public bool TOWER_LAMP_RED { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x003, "TOWER LAMP YELLOW")]
        public bool TOWER_LAMP_YELLOW { get => this.ReadY(); set => this.WriteY(value); }

        [IOSetting(OUT, OA + 0x004, "TOWER LAMP GREEN")]
        public bool TOWER_LAMP_GREEN { get => this.ReadY(); set => this.WriteY(value); }
    }
}
