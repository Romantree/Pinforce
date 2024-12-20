using System;
using System.Collections.Generic;
using TS.FW;
using TS.FW.Dac.Alarm;

namespace WILL.WT.PINFORCE
{
    public static class EnumHelper
    {
        public static List<string> SerialPort { get; set; } = new List<string>();

        public static List<AlarmLevel> AlarmLevel { get; set; } = new List<AlarmLevel>();

        public static List<eAxis> Axis { get; set; } = new List<eAxis>();

        // Repeat 방법 정의
        public static List<RepeatType> RepeatMethod { get; set; } = new List<RepeatType>();

        // JogSpeed 정의
        public static List<JogSpeedType> JogSpeed { get; set; } = new List<JogSpeedType>();

        static EnumHelper()
        {
            foreach (var item in System.IO.Ports.SerialPort.GetPortNames()) SerialPort.Add(item);

            InitEnum(AlarmLevel);
            InitEnum(Axis);

            InitEnum(RepeatMethod);
            InitEnum(JogSpeed);
        }

        private static void InitEnum<T>(List<T> list)
        {
            try
            {
                foreach (T item in Enum.GetValues(typeof(T)))
                {
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(typeof(EnumHelper), ex);
            }
        }

        public static CyState ToRev(this CyState state) => state == CyState.UP ? CyState.DOWN : CyState.UP;
    }

    public enum eAxis
    {
        AXIS_Z,
    }

    public enum CyUnit
    {

    }

    public enum CyState
    {
        UP,
        DOWN,
    }

    // Repeat 방법 정의
    public enum RepeatType
    {
        Sequential,
        Zero
    }

    public enum VacuumSetting
    {
        Vacuum,
        Vent,
        Off,
    }

    public enum VacuumStatus
    {
        ATM,

        Vacuum,
        Vent,

        VacuumProcess,
        VentProcess,

        Error,
    }

    public enum eIOType
    {
        IN,
        OUT,
    }

    public enum eSignalType
    {
        A,
        B,
    }

    // JogSpeedType 정의
    public enum JogSpeedType
    {
        Low,
        Middle,
        High
    }
}
