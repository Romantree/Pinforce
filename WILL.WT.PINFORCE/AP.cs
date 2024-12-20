using System;
using System.Configuration;
using System.IO;
using System.Linq;
using WILL.WT.PINFORCE.Configs;
using WILL.WT.PINFORCE.Managers;
using TS.FW;
using WILL.WT.PINFORCE.Models;

namespace WILL.WT.PINFORCE
{
    public static class AP
    {
        public static bool IsSim => ConfigurationManager.AppSettings["Simulation"] == "1";

        public static Logger.LogEventLevel LOG_LEVEL => (Logger.LogEventLevel)Enum.Parse(typeof(Logger.LogEventLevel), ConfigurationManager.AppSettings["LOG_LEVEL"]);

        public static string LOG_FILE => ConfigurationManager.AppSettings["LOG_FILE"];

        public static string INOUT_FILE => ConfigurationManager.AppSettings["INOUT_FILE"];

        public static string MOT_FILE => ConfigurationManager.AppSettings["MOT_FILE"];

        public readonly static SystemManager System = new SystemManager();
        public readonly static AlarmManager Alarm = new AlarmManager();
        public readonly static RecipeManager Rcp = new RecipeManager();

        public readonly static DeviceManager Device = new DeviceManager();
        public readonly static InOutManager IO = new InOutManager();
        public readonly static NetManager Net = new NetManager();
        public readonly static CameraManager Cam = new CameraManager();

        public readonly static ProcManager Proc = new ProcManager();

       

        public static bool IsEpcAlarm = true;

        static AP()
        {

        }

        public static void ProcessStop()
        {
            try
            {
                AP.Device.Stop();
                AP.IO.Abort();
                AP.Proc.Abort();
                AP.Net.Abort();
            }
            catch (Exception ex)
            {
                Logger.Write(typeof(AP), ex);
            }
        }
    }

    public static class DB
    {
        private static ConnectionStringsSection section;

        static DB()
        {
            LoadDll();
        }

        // DB 선언
        public static UserDB User { get; private set; }

        public static NetworkDB Network { get; private set; }

        public static WorkParamDB WorkParam { get; private set; }

        public static MotionDB Motion { get; private set; }
        public static LoadcellDB Loadcell { get; private set; }

        public static void LoadDatabase()
        {
            if (section == null) section = ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;

            LoadDatabase("CONFIG_DB");
            LoadDatabase("ALARM_DB");

            User = new UserDB();
            Network = new NetworkDB();
            WorkParam = new WorkParamDB();
            Motion = new MotionDB();
            Loadcell = new LoadcellDB();
        }

        public static void DataCopy(object a, object b)
        {
            try
            {
                var aList = a.GetType().GetProperties().Where(t => t.CanRead && t.CanWrite);
                var bList = b.GetType().GetProperties().Where(t => t.CanRead && t.CanWrite);

                foreach (var aInfo in aList)
                {
                    var bInfo = bList.FirstOrDefault(t => t.Name == aInfo.Name);
                    if (bInfo == null) continue;

                    var value = bInfo.GetValue(b);
                    aInfo.SetValue(a, value);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(typeof(DB), ex);
            }
        }

        public static void DataCopyEx(object a, object b) => DataCopy(b, a);

        private static void LoadDll()
        {
            LoadDll("x86");
            LoadDll("x64");
        }

        private static void LoadDll(string target)
        {
            var dst = Path.Combine(Environment.CurrentDirectory, target, "SQLite.Interop.dll");

            if (File.Exists(dst)) return;

            var src = Path.Combine(Environment.CurrentDirectory, "Assets", target, "SQLite.Interop.dll");

            var dir =Path.GetDirectoryName(dst);
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            File.Copy(src, dst, true);
        }

        private static void LoadDatabase(string databaseName)
        {
            if (section == null) return;

            var config = section.ConnectionStrings[databaseName];
            if (config == null) return;

            var provider = config.ProviderName.Trim();
            if (provider != "System.Data.SQLite") return;

            var dst = config.ConnectionString.Replace("Data Source=", "").Replace(";","");

            if (File.Exists(dst) == true) return;
            
            var src = Path.Combine(Environment.CurrentDirectory, "Assets", Path.GetFileName(dst));

            var dir =Path.GetDirectoryName(dst);
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            File.Copy(src, dst, true);
        }
    }
}
