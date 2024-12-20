using WILL.WT.PINFORCE.Managers;
using System.Runtime.Serialization;
using TS.FW.Wpf.v2.Core;

namespace WILL.WT.PINFORCE.Models.Recipe
{
    [DataContract]
    public class MainRecipeModel : IRecipeModel
    {
        public MainRecipeModel() : base(RecipeType.MAIN) { }

        [DataMember]
        public int Start { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public int Step { get => this.GetValue<int>(); set => this.SetValue(value); }
        [DataMember]
        public int Max { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public int End { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public double ContactTime { get => this.GetValue<double>(); set => this.SetValue(value); }
        [DataMember]
        public double WorkSpeed { get => this.GetValue<double>(); set => this.SetValue(value); }

        [DataMember]
        public int RepeatCount { get => this.GetValue<int>(); set => this.SetValue(value); }

        [DataMember]
        public RepeatType RepeatMethod { get => this.GetValue<RepeatType>(); set => this.SetValue(value); }

        [DataMember]
        public bool ReleaseUse { get => this.GetValue<bool>(); set => this.SetValue(value); }

        [DataMember]
        public double ReleaseTime { get => this.GetValue<double>(); set => this.SetValue(value); }

        [DataMember]
        public int ReleaseDist { get => this.GetValue<int>(); set => this.SetValue(value); }
    }
}