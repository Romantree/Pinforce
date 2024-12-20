using System.Runtime.Serialization;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Managers;

namespace WILL.WT.PINFORCE.Models.Recipe
{
    [DataContract]
    public class AutoContectRcpModel : IRecipeModel
    {
        public AutoContectRcpModel() : base(RecipeType.AUTO_CONTACT) 
        {
            this.Data01 = new AutoContectData();
            this.Data02 = new AutoContectData();
            this.Data03 = new AutoContectData();
        }

        /// <summary>
        ///  반복 횟수
        /// </summary>
        [DataMember]
        public int RepeatCount { get => this.GetValue<int>(); set => this.SetValue(value); }

        /// <summary>
        /// 프로세스 실행 여부
        /// </summary>
        [DataMember]
        public bool IsAutoContect { get => this.GetValue<bool>(); set => this.SetValue(value); }

        /// <summary>
        /// 프로세스 종료 후 다시 올릴지 여부
        /// </summary>
        [DataMember]
        public bool IsLastReleaseMove { get => this.GetValue<bool>(); set => this.SetValue(value); }

        /// <summary>
        /// 프로세스 종료 후 올리는 거리
        /// </summary>
        [DataMember]
        public int LastReleaseMoveDist { get => this.GetValue<int>(); set => this.SetValue(value); }

        /// <summary>
        /// 프로세스 초기시작점 거리??
        /// </summary>
        [DataMember]
        public int AutoAdjustmentDist { get => this.GetValue<int>(); set => this.SetValue(value); }

        /// <summary>
        /// Process Timeout
        /// </summary>
        [DataMember]
        public double ProcTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }

        [DataMember]
        public AutoContectData Data01 { get => this.GetValue<AutoContectData>(); set => this.SetValue(value); }

        [DataMember]
        public AutoContectData Data02 { get => this.GetValue<AutoContectData>(); set => this.SetValue(value); }

        [DataMember]
        public AutoContectData Data03 { get => this.GetValue<AutoContectData>(); set => this.SetValue(value); }
    }

    [DataContract]
    public class AutoContectData : IModel
    {
        /// <summary>
        /// 압력 감지 무게 기준
        /// </summary>
        [DataMember]
        public double OriginForce { get => this.GetValue<double>(); set => this.SetValue(value); }

        /// <summary>
        /// 이동 거리
        /// </summary>
        [DataMember]
        public int ContactStepDist { get => this.GetValue<int>(); set => this.SetValue(value); }

        /// <summary>
        /// 도착 후 대기 시간
        /// </summary>
        [DataMember]
        public double MotionDelay { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
