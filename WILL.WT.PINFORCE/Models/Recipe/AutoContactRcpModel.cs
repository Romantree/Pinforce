using System.Runtime.Serialization;
using TS.FW.Wpf.v2.Core;
using WILL.WT.PINFORCE.Managers;

namespace WILL.WT.PINFORCE.Models.Recipe
{
    [DataContract]
    public class AutoContactRcpModel : IRecipeModel
    {
        public AutoContactRcpModel() : base(RecipeType.AUTO_CONTACT) 
        {
            this.Data01 = new AutoContactData();
            this.Data02 = new AutoContactData();
            this.Data03 = new AutoContactData();
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
        public bool IsAutoContact { get => this.GetValue<bool>(); set => this.SetValue(value); }

        /// <summary>
        /// 프로세스 종료 후 다시 올릴지 여부
        /// </summary>
        [DataMember]
        public bool IsLastReleaseMove { get => this.GetValue<bool>(); set => this.SetValue(value); }

        /// <summary>
        /// 프로세스 종료 후 올리는 거리
        /// </summary>
        [DataMember]
        public double LastReleaseMoveDist { get => this.GetValue<double>(); set => this.SetValue(value); }

        /// <summary>
        /// 프로세스 초기 시작점 조점
        /// </summary>
        [DataMember]
        public double AutoAdjustmentDist { get => this.GetValue<double>(); set => this.SetValue(value); }

        /// <summary>
        /// Process Timeout
        /// </summary>
        [DataMember]
        public double ProcTimeout { get => this.GetValue<double>(); set => this.SetValue(value); }

        [DataMember]
        public AutoContactData Data01 { get => this.GetValue<AutoContactData>(); set => this.SetValue(value); }

        [DataMember]
        public AutoContactData Data02 { get => this.GetValue<AutoContactData>(); set => this.SetValue(value); }

        [DataMember]
        public AutoContactData Data03 { get => this.GetValue<AutoContactData>(); set => this.SetValue(value); }
    }

    [DataContract]
    public class AutoContactData : IModel
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
        public double ContactStepDist { get => this.GetValue<double>(); set => this.SetValue(value); }

        /// <summary>
        /// 도착 후 대기 시간
        /// </summary>
        [DataMember]
        public double MotionDelay { get => this.GetValue<double>(); set => this.SetValue(value); }
    }
}
